using CarRadioSimulator;
using System.IO;

namespace CarRadioStations.Tests;

public class StationLoaderTests
{
    private const string TestFilePath = @"C:\StationsInfo\StationsInformation.txt";

    [Fact]
    public void LoadStations_WhenFileNotFound_ThrowsFileNotFoundException()
    {
        // Skip test if file exists (can't test file-not-found when file exists)
        if (File.Exists(TestFilePath))
        {
            return;
        }

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => StationLoader.LoadStations());
        Assert.Contains("Station data file not found", exception.Message);
        Assert.Contains(TestFilePath, exception.Message);
    }

    [Fact]
    public void LoadStations_WhenFileExists_ReturnsDictionary()
    {
        // Skip if file doesn't exist
        if (!File.Exists(TestFilePath))
        {
            return;
        }

        // Act
        var stations = StationLoader.LoadStations();

        // Assert
        Assert.NotNull(stations);
        Assert.IsType<Dictionary<double, RadioStation>>(stations);
    }

    [Fact]
    public void LoadStations_WhenFileExists_KeysByBaseFrequency()
    {
        // Skip if file doesn't exist
        if (!File.Exists(TestFilePath))
        {
            return;
        }

        // Act
        var stations = StationLoader.LoadStations();

        // Assert
        foreach (var kvp in stations)
        {
            Assert.Equal(kvp.Key, kvp.Value.BaseFrequency);
        }
    }

    [Fact]
    public void LoadStations_WhenFileExists_AllStationsHaveValidFrequencies()
    {
        // Skip if file doesn't exist
        if (!File.Exists(TestFilePath))
        {
            return;
        }

        // Act
        var stations = StationLoader.LoadStations();

        // Assert
        foreach (var station in stations.Values)
        {
            Assert.InRange(station.BaseFrequency, 88.0, 108.0);
            Assert.InRange(station.AlternativeFrequency, 88.0, 108.0);
        }
    }

    [Fact]
    public void LoadStations_WhenFileExists_AllStationsHaveNames()
    {
        // Skip if file doesn't exist
        if (!File.Exists(TestFilePath))
        {
            return;
        }

        // Act
        var stations = StationLoader.LoadStations();

        // Assert
        foreach (var station in stations.Values)
        {
            Assert.False(string.IsNullOrWhiteSpace(station.StationName));
        }
    }

    [Fact]
    public void LoadStations_WhenFileExists_ReturnsNonEmptyDictionary()
    {
        // Skip if file doesn't exist
        if (!File.Exists(TestFilePath))
        {
            return;
        }

        // Act
        var stations = StationLoader.LoadStations();

        // Assert
        Assert.NotEmpty(stations);
    }

    // Testing malformed file data would require creating test files or mocking File operations
}
