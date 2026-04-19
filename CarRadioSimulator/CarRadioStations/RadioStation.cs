using System;

namespace CarRadioSimulator
{
    // Represents the details of the Radio station as read from the text file.
    public class RadioStation
    {
        // Backing fields for properties
        private string _stationName;
        private bool   _hasTravelInfo;
        private string _radioText;
        private double _baseFrequency;
        private double _alternativeFrequency;

        // Constructors for creating instances of RadioStation
        public RadioStation() { }

        public RadioStation(string stationName, bool hasTravelInfo, string radioText, double baseFrequency, double alternativeFrequency)
        {
            StationName          = stationName;
            HasTravelInfo        = hasTravelInfo;
            RadioText            = radioText;
            BaseFrequency        = baseFrequency;
            AlternativeFrequency = alternativeFrequency;
        }


        // Properties / Getters & Setters
        public string StationName
        {
            get => _stationName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Station name cannot be empty."); // Catch empty or whitespace-only names
                _stationName = value.Trim();
            }
        }

        // Indicates if the station provides travel information (e.g., traffic updates)
        public bool HasTravelInfo
        {
            get => _hasTravelInfo;
            set => _hasTravelInfo = value;
        }

        // Current RDS Radio Text (programme/show name)
        public string RadioText
        {
            get => _radioText;
            set => _radioText = value?.Trim() ?? string.Empty;
        }

        // Primary broadcast frequency in MHz (88.0 – 108.0)
        public double BaseFrequency
        {
            get => _baseFrequency;
            set
            {
                if (value < 88.0 || value > 108.0)
                    throw new ArgumentOutOfRangeException(nameof(BaseFrequency), "FM frequency must be between 88.0 and 108.0 MHz.");
                _baseFrequency = Math.Round(value, 1);
            }
        }

        // Alternative/secondary frequency in MHz (88.0 – 108.0)
        public double AlternativeFrequency
        {
            get => _alternativeFrequency;
            set
            {
                if (value < 88.0 || value > 108.0)
                    throw new ArgumentOutOfRangeException(nameof(AlternativeFrequency), "FM frequency must be between 88.0 and 108.0 MHz.");
                _alternativeFrequency = Math.Round(value, 1);
            }
        }

        // For  debugging; Display station details in a readable format
        public override string ToString() =>
            $"{StationName} [{BaseFrequency:F1} MHz / {AlternativeFrequency:F1} MHz]  " +
            $"Travel:{(HasTravelInfo ? "Yes" : "No")}  \"{RadioText}\"";
    }
}
