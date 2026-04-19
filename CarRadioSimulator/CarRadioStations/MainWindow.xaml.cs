using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CarRadioSimulator
{
    // Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        // Primary data container: Dictionary keyed by base-frequency for O(1) dial lookup
        private Dictionary<double, RadioStation> _stations =
            new Dictionary<double, RadioStation>();

        // Flat list of stations ordered by base frequency (used for UI lists)
        private List<RadioStation> _sortedStations = new List<RadioStation>();

        // Frequency mapping constants
        private const double SliderMin  = 0.0;     // potentiometer minimum
        private const double SliderMax  = 200.0;   // potentiometer maximum
        private const double FmMin      = 88.0;    // FM band start  (MHz)
        private const double FmMax      = 108.0;   // FM band end    (MHz)
        private const double SnapWindow = 0.2;     // ±0.2 MHz = "on station"

        // Constructor - initializes the UI and sets initial visibility states
        public MainWindow()
        {
            InitializeComponent();
            LvStations.Visibility    = Visibility.Visible;
            ChartScroller.Visibility = Visibility.Collapsed;
        }

        // Converts potentiometer slider value to FM frequency using linear interpolation
        private double DialToFrequency(double sliderValue)
        {
            double freq = FmMin + (sliderValue - SliderMin) /
                          (SliderMax - SliderMin) * (FmMax - FmMin);
            return Math.Round(freq, 1);
        }

        // Event handler: Slider value changed
        private void FreqSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double freq = DialToFrequency(FreqSlider.Value);
            TxtFrequency.Text = freq.ToString("F1");

            // Try to find a station within the snap window
            RadioStation found = FindStationByFrequency(freq);
            DisplayStationRDS(found);
        }

        // Find station within ±SnapWindow of requested frequency
        private RadioStation FindStationByFrequency(double freq)
        {
            if (_stations.Count == 0) return null;

            // Check base frequencies
            foreach (var kvp in _stations)
            {
                if (Math.Abs(kvp.Key - freq) <= SnapWindow)
                    return kvp.Value;
            }

            // Also check alternative frequencies
            foreach (var station in _sortedStations)
            {
                if (Math.Abs(station.AlternativeFrequency - freq) <= SnapWindow)
                    return station;
            }

            return null;
        }

        // Update the RDS info cards
        private void DisplayStationRDS(RadioStation s)
        {
            if (s == null)
            {
                TxtStationName.Text = "---";
                TxtRadioText.Text   = "---";
                TxtBaseFreq.Text    = "---";
                TxtAltFreq.Text     = "---";
                LedTravel.Fill      = FindResource("LedOff") as SolidColorBrush;
                TxtStatus.Text      = "No station at this frequency.";
            }
            else
            {
                TxtStationName.Text = s.StationName;
                TxtRadioText.Text   = s.RadioText;
                TxtBaseFreq.Text    = $"{s.BaseFrequency:F1}";
                TxtAltFreq.Text     = $"{s.AlternativeFrequency:F1}";
                LedTravel.Fill      = s.HasTravelInfo
                    ? (SolidColorBrush)FindResource("LedGreen")
                    : (SolidColorBrush)FindResource("LedOff");
                TxtStatus.Text      = $"Now tuned: {s.StationName}  |  {s.RadioText}";
            }
        }

        // Button click handlers: Load Stations
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
            => LoadStations();

        private void MenuLoad_Click(object sender, RoutedEventArgs e)
            => LoadStations();

        private void LoadStations()
        {
            try
            {
                _stations       = StationLoader.LoadStations();
                _sortedStations = _stations.Values
                    .OrderBy(s => s.BaseFrequency)
                    .ToList();

                PopulateFilterCombos();
                LvStations.ItemsSource = _sortedStations
                    .Select(s => new StationDisplayRow(s))
                    .ToList();

                ChkFilterActive.IsChecked = false;
                TxtStatus.Text = $"Loaded {_stations.Count} station(s) successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TxtStatus.Text = "Load failed — see error dialog.";
            }
        }

        // Populate From/To combo boxes from loaded station frequencies
        private void PopulateFilterCombos()
        {
            var freqStrings = _sortedStations
                .Select(s => s.BaseFrequency.ToString("F1"))
                .ToList();

            CboFrom.ItemsSource = new List<string>(freqStrings);
            CboTo.ItemsSource   = new List<string>(freqStrings);

            if (CboFrom.Items.Count > 0)
                CboFrom.SelectedIndex = 0;
            if (CboTo.Items.Count > 0)
                CboTo.SelectedIndex = CboTo.Items.Count - 1;
        }

        // Button click handlers: Filter Stations
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
            => ApplyFilter();

        private void MenuFilter_Click(object sender, RoutedEventArgs e)
            => ApplyFilter();

        private void ApplyFilter()
        {
            if (_sortedStations.Count == 0)
            {
                MessageBox.Show("Please load stations first.", "No Data", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (CboFrom.SelectedItem == null || CboTo.SelectedItem == null)
            {
                MessageBox.Show("Please select both From and To frequencies.", "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double from = double.Parse(CboFrom.SelectedItem.ToString());
            double to   = double.Parse(CboTo.SelectedItem.ToString());

            if (from > to)
            {
                MessageBox.Show(
                    $"'From' frequency ({from:F1} MHz) must be ≤ 'To' frequency ({to:F1} MHz).", "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tick the checkbox — DisplayStations will respect it
            ChkFilterActive.IsChecked = true;
            TxtStatus.Text = $"Filter set: {from:F1} – {to:F1} MHz.  Press [Display Stations].";
        }

        // Button click handler: Display Stations (chart)
        private void BtnDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (_sortedStations.Count == 0)
            {
                MessageBox.Show("Please load stations first.", "No Data", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            List<RadioStation> toDisplay = _sortedStations;

            // Apply filter if checkbox is ticked
            if (ChkFilterActive.IsChecked == true &&
                CboFrom.SelectedItem != null && CboTo.SelectedItem != null)
            {
                double from = double.Parse(CboFrom.SelectedItem.ToString());
                double to   = double.Parse(CboTo.SelectedItem.ToString());
                toDisplay = _sortedStations
                    .Where(s => s.BaseFrequency >= from && s.BaseFrequency <= to)
                    .ToList();
            }

            // Update the ListView
            LvStations.ItemsSource = toDisplay
                .Select(s => new StationDisplayRow(s))
                .ToList();

            // Draw the bar chart on the canvas
            DrawBarChart(toDisplay);

            // Show chart canvas below the list
            LvStations.Visibility    = Visibility.Visible;
            ChartScroller.Visibility = Visibility.Visible;

            TxtStatus.Text = $"Displaying {toDisplay.Count} station(s)" +
                (ChkFilterActive.IsChecked == true ? " (filter active)." : ".");
        }

        // Draw frequency bar chart
        private void DrawBarChart(List<RadioStation> stations)
        {
            ChartCanvas.Children.Clear();

            if (stations.Count == 0) return;

            double canvasHeight = 180;
            double barWidth     = 60;
            double gap          = 20;
            double totalWidth   = stations.Count * (barWidth + gap) + gap;
            double maxBarHeight = canvasHeight - 40;

            ChartCanvas.Width  = Math.Max(totalWidth, 600);
            ChartCanvas.Height = canvasHeight + 40;

            double minFreq = stations.Min(s => s.BaseFrequency);
            double maxFreq = stations.Max(s => s.BaseFrequency);
            double freqRange = maxFreq - minFreq == 0 ? 1 : maxFreq - minFreq;

            for (int i = 0; i < stations.Count; i++)
            {
                var s = stations[i];
                double x = gap + i * (barWidth + gap);

                double normalized = (s.BaseFrequency - minFreq) / freqRange;
                double barH = 30 + normalized * (maxBarHeight - 30);
                double y = canvasHeight - barH;

                // Bar gradient via rectangle + overlay
                var bar = new Rectangle
                {
                    Width   = barWidth,
                    Height  = barH,
                    Fill    = new LinearGradientBrush(Color.FromRgb(0xE9, 0x45, 0x60), Color.FromRgb(0x0F, 0x34, 0x60), 90),
                    RadiusX = 3,
                    RadiusY = 3
                };
                Canvas.SetLeft(bar, x);
                Canvas.SetTop(bar,  y);
                ChartCanvas.Children.Add(bar);

                // Frequency label on top of bar
                var lblFreq = new TextBlock
                {
                    Text       = $"{s.BaseFrequency:F1}",
                    Foreground = new SolidColorBrush(Color.FromRgb(0x39, 0xFF, 0x14)),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 9,
                    Width      = barWidth,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(lblFreq, x);
                Canvas.SetTop(lblFreq,  y - 16);
                ChartCanvas.Children.Add(lblFreq);

                // Station name below bar
                var lblName = new TextBlock
                {
                    Text       = s.StationName,
                    Foreground = new SolidColorBrush(Colors.LightGray),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize   = 9,
                    Width      = barWidth,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping  = TextWrapping.NoWrap
                };
                Canvas.SetLeft(lblName, x);
                Canvas.SetTop(lblName,  canvasHeight + 4);
                ChartCanvas.Children.Add(lblName);
            }

            // Baseline
            var baseline = new Line
            {
                X1     = 0,
                Y1     = canvasHeight,
                X2     = ChartCanvas.Width,
                Y2     = canvasHeight,
                Stroke = new SolidColorBrush(Color.FromRgb(0xE9, 0x45, 0x60)),
                StrokeThickness = 1
            };
            ChartCanvas.Children.Add(baseline);
        }

        // Button click handler: Clear filter
        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ChkFilterActive.IsChecked = false;
            LvStations.ItemsSource = _sortedStations
                .Select(s => new StationDisplayRow(s))
                .ToList();
            ChartScroller.Visibility = Visibility.Collapsed;
            TxtStatus.Text = "Filter cleared.  All stations shown.";
        }

        // Menu click handler: Exit
        private void MenuExit_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();
    }

    // Helper class for ListView row presentation (converts RadioStation to display-friendly format)
    public class StationDisplayRow
    {
        public string StationName          { get; }
        public double BaseFrequency        { get; }
        public double AlternativeFrequency { get; }
        public string TravelInfoDisplay    { get; }
        public string RadioText            { get; }

        public StationDisplayRow(RadioStation s)
        {
            StationName          = s.StationName;
            BaseFrequency        = s.BaseFrequency;
            AlternativeFrequency = s.AlternativeFrequency;
            TravelInfoDisplay    = s.HasTravelInfo ? "Yes" : "No";
            RadioText            = s.RadioText;
        }
    }
}
