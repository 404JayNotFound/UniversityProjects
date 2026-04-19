using CarRadioSimulator;

namespace CarRadioStations.Tests;

public class RadioStationTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesStation()
    {
        // Arrange & Act
        var station = new RadioStation(
            stationName: "96FM",
            hasTravelInfo: true,
            radioText: "Morning Show",
            baseFrequency: 96.1,
            alternativeFrequency: 96.5
        );

        // Assert
        Assert.Equal("96FM", station.StationName);
        Assert.True(station.HasTravelInfo);
        Assert.Equal("Morning Show", station.RadioText);
        Assert.Equal(96.1, station.BaseFrequency);
        Assert.Equal(96.5, station.AlternativeFrequency);
    }

    [Fact]
    public void DefaultConstructor_CreatesEmptyStation()
    {
        // Arrange & Act
        var station = new RadioStation();

        // Assert
        Assert.NotNull(station);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void StationName_WithInvalidValue_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var station = new RadioStation();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => station.StationName = invalidName);
    }

    [Fact]
    public void StationName_WithWhitespace_TrimsValue()
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.StationName = "  96FM  ";

        // Assert
        Assert.Equal("96FM", station.StationName);
    }

    [Theory]
    [InlineData(87.9)]  // Below minimum
    [InlineData(108.1)] // Above maximum
    [InlineData(50.0)]  // Way below
    [InlineData(150.0)] // Way above
    public void BaseFrequency_OutOfRange_ThrowsArgumentOutOfRangeException(double invalidFrequency)
    {
        // Arrange
        var station = new RadioStation();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => station.BaseFrequency = invalidFrequency);
    }

    [Theory]
    [InlineData(88.0)]   // Minimum valid
    [InlineData(108.0)]  // Maximum valid
    [InlineData(96.5)]   // Mid-range
    [InlineData(100.0)]  // Mid-range
    public void BaseFrequency_ValidRange_AcceptsValue(double validFrequency)
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.BaseFrequency = validFrequency;

        // Assert
        Assert.Equal(validFrequency, station.BaseFrequency);
    }

    [Theory]
    [InlineData(87.9)]  // Below minimum
    [InlineData(108.1)] // Above maximum
    public void AlternativeFrequency_OutOfRange_ThrowsArgumentOutOfRangeException(double invalidFrequency)
    {
        // Arrange
        var station = new RadioStation();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => station.AlternativeFrequency = invalidFrequency);
    }

    [Theory]
    [InlineData(88.0)]
    [InlineData(108.0)]
    [InlineData(96.5)]
    public void AlternativeFrequency_ValidRange_AcceptsValue(double validFrequency)
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.AlternativeFrequency = validFrequency;

        // Assert
        Assert.Equal(validFrequency, station.AlternativeFrequency);
    }

    [Fact]
    public void BaseFrequency_RoundsToOneDecimalPlace()
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.BaseFrequency = 96.123456;

        // Assert
        Assert.Equal(96.1, station.BaseFrequency);
    }

    [Fact]
    public void AlternativeFrequency_RoundsToOneDecimalPlace()
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.AlternativeFrequency = 96.789;

        // Assert
        Assert.Equal(96.8, station.AlternativeFrequency);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("  Morning Show  ", "Morning Show")]
    public void RadioText_WithVariousInputs_HandlesCorrectly(string input, string expected)
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.RadioText = input;

        // Assert
        Assert.Equal(expected, station.RadioText);
    }

    [Fact]
    public void HasTravelInfo_CanBeSetAndRetrieved()
    {
        // Arrange
        var station = new RadioStation();

        // Act
        station.HasTravelInfo = true;

        // Assert
        Assert.True(station.HasTravelInfo);

        // Act
        station.HasTravelInfo = false;

        // Assert
        Assert.False(station.HasTravelInfo);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
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
        string result = station.ToString();

        // Assert
        Assert.Contains("96FM", result);
        Assert.Contains("96.1 MHz", result);
        Assert.Contains("96.5 MHz", result);
        Assert.Contains("Travel:Yes", result);
        Assert.Contains("Morning Show", result);
    }

    [Fact]
    public void ToString_WithNoTravelInfo_ShowsNo()
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
        string result = station.ToString();

        // Assert
        Assert.Contains("Travel:No", result);
    }
}
