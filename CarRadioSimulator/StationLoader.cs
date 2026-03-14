using System;
using System.Collections.Generic;
using System.IO;

namespace CarRadioSimulator
{
    // Class to load radio station information from the .txt file
    public static class StationLoader
    {
        // Path to StationsInformation.txt: Update if necessary.
        private const string FilePath = @"C:\StationsInfo\StationsInformation.txt";

        public static Dictionary<double, RadioStation> LoadStations()
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"StationsInformation.txt not found:\n{FilePath}\n");

            string[] lines = File.ReadAllLines(FilePath);

            // Parse the sections listed in the "StationsInformation.txt" file
            var names = ParseSection(lines, "[Station Name]");
            var travel = ParseSection(lines, "[Travel Information]");
            var radioTexts = ParseSection(lines, "[Radio Text]");
            var baseFreqs = ParseSection(lines, "[Base Frequency]");
            var altFreqs = ParseSection(lines, "[Alternative Frequency]");

            int count = names.Count;
            if (travel.Count != count || radioTexts.Count != count || baseFreqs.Count != count || altFreqs.Count != count)
                throw new InvalidDataException("All sections dont have the same number of entries. Check the file format");

            // Build dictionary keyed by base frequency for quick lookup in the radio simulator
            var stations = new Dictionary<double, RadioStation>();

            for (int i = 0; i < count; i++)
            {
                bool hasTravelInfo = travel[i].Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase);

                if (!double.TryParse(baseFreqs[i], out double baseMHz))
                    throw new InvalidDataException($"Cant parse the base frequency: '{baseFreqs[i]}'");

                if (!double.TryParse(altFreqs[i], out double altMHz))
                    throw new InvalidDataException($"Cant parse the alternative frequency: '{altFreqs[i]}'");

                var station = new RadioStation(names[i], hasTravelInfo, radioTexts[i], baseMHz, altMHz);

                stations[baseMHz] = station;
            }

            return stations;
        }

        // Parses each section and returns a list of entries.
        private static List<string> ParseSection(string[] lines, string header)
        {
            var result = new List<string>();
            bool inSection = false;

            foreach (string raw in lines)
            {
                string line = raw.Trim();
                if (line.Equals(header, StringComparison.OrdinalIgnoreCase))
                {
                    inSection = true;
                    continue;
                }
                if (inSection)
                {
                    if (line.StartsWith("[")) break;   // Moves to next section header
                    if (line.Length > 0)
                        result.Add(line);
                }
            }
            return result;
        }
    }
}
