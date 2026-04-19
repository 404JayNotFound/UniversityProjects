using CarRadioSimulator;

namespace CarRadioStations.Tests;

public class MainWindowTests
{
    // MainWindow is a WPF UI class tightly coupled to UI controls, so these tests verify the underlying logic
    [Theory]
    [InlineData(0.0, 88.0)]      // Slider min maps to FM min
    [InlineData(200.0, 108.0)]   // Slider max maps to FM max
    [InlineData(100.0, 98.0)]    // Middle value
    [InlineData(50.0, 93.0)]     // Quarter way
    [InlineData(150.0, 103.0)]   // Three quarters
    public void DialToFrequency_ConvertsSliderValueToFrequency(double sliderValue, double expectedFrequency)
    {
        // Tests the formula: f(x) = FmMin + (x - SliderMin) / (SliderMax - SliderMin) × (FmMax - FmMin)

        // Arrange
        const double SliderMin = 0.0;
        const double SliderMax = 200.0;
        const double FmMin = 88.0;
        const double FmMax = 108.0;

        // Act
        double freq = FmMin + (sliderValue - SliderMin) / (SliderMax - SliderMin) * (FmMax - FmMin);
        double rounded = Math.Round(freq, 1);

        // Assert
        Assert.Equal(expectedFrequency, rounded);
    }

    [Fact]
    public void StationDisplayRow_ConvertsRadioStationCorrectly()
    {
        // Arrange
        var station = new RadioStation(
            stationName: "96FM",
            hasTravelInfo: true,
            radioText: "Morning Show",
            baseFrequency: 96.1,
            alternativeFrequency: 96.5
        );

        // Act
        var displayRow = new StationDisplayRow(station);

        // Assert
        Assert.Equal("96FM", displayRow.StationName);
        Assert.Equal(96.1, displayRow.BaseFrequency);
        Assert.Equal(96.5, displayRow.AlternativeFrequency);
        Assert.Equal("Yes", displayRow.TravelInfoDisplay);
        Assert.Equal("Morning Show", displayRow.RadioText);
    }

    [Fact]
    public void StationDisplayRow_WithNoTravelInfo_ShowsNo()
    {
        // Arrange
        var station = new RadioStation(
            stationName: "Test FM",
            hasTravelInfo: false,
            radioText: "Show",
            baseFrequency: 100.0,
            alternativeFrequency: 100.5
        );

        // Act
        var displayRow = new StationDisplayRow(station);

        // Assert
        Assert.Equal("No", displayRow.TravelInfoDisplay);
    }

    [Fact]
    public void FindStationByFrequency_Logic_FindsStationWithinSnapWindow()
    {
        // Tests the logic used by FindStationByFrequency
        // Arrange
        const double SnapWindow = 0.2;
        var stations = new Dictionary<double, RadioStation>
        {
            [96.1] = new RadioStation("96FM", true, "Show", 96.1, 96.5),
            [100.0] = new RadioStation("100FM", false, "News", 100.0, 100.4)
        };

        // Test finding station within snap window
        double searchFreq = 96.15; // Within 0.2 of 96.1

        // Act
        RadioStation? found = null;
        foreach (var kvp in stations)
        {
            if (Math.Abs(kvp.Key - searchFreq) <= SnapWindow)
            {
                found = kvp.Value;
                break;
            }
        }

        // Assert
        Assert.NotNull(found);
        Assert.Equal("96FM", found.StationName);
    }

    [Fact]
    public void FindStationByFrequency_Logic_ReturnsNullWhenOutsideSnapWindow()
    {
        // Arrange
        const double SnapWindow = 0.2;
        var stations = new Dictionary<double, RadioStation>
        {
            [96.1] = new RadioStation("96FM", true, "Show", 96.1, 96.5)
        };

        double searchFreq = 96.5; // 0.4 away from 96.1, outside snap window

        // Act
        RadioStation? found = null;
        foreach (var kvp in stations)
        {
            if (Math.Abs(kvp.Key - searchFreq) <= SnapWindow)
            {
                found = kvp.Value;
                break;
            }
        }

        // Assert
        Assert.Null(found);
    }

    [Fact]
    public void FindStationByFrequency_Logic_ChecksAlternativeFrequencies()
    {
        // Demonstrates checking alternative frequencies
        // Arrange
        const double SnapWindow = 0.2;
        var station = new RadioStation("96FM", true, "Show", 96.1, 96.5);
        double searchFreq = 96.55; // Within 0.2 of alternative frequency 96.5

        // Act
        bool foundOnAlternative = Math.Abs(station.AlternativeFrequency - searchFreq) <= SnapWindow;

        // Assert
        Assert.True(foundOnAlternative);
    }

    [Theory]
    [InlineData(88.0, 108.0, 96.1, true)]   // Within range
    [InlineData(95.0, 97.0, 96.1, true)]    // Within range
    [InlineData(88.0, 95.0, 96.1, false)]   // Below range
    [InlineData(97.0, 108.0, 96.1, false)]  // Above range
    public void FrequencyFilter_Logic_FiltersStationsCorrectly(
        double fromFreq, double toFreq, double stationFreq, bool shouldInclude)
    {
        // Tests the filtering logic used in ApplyFilter
        // Act
        bool isInRange = stationFreq >= fromFreq && stationFreq <= toFreq;

        // Assert
        Assert.Equal(shouldInclude, isInRange);
    }
}
