using PassportMD.Models;

namespace PassportMD.Services
{
    public class CsvRepository
    {
        private readonly string _folderPath;

        public CsvRepository()
        {
            // Use app data folder + "passports" subfolder — auto-creates if missing
            _folderPath = Path.Combine(FileSystem.AppDataDirectory, "passports");
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
        }

        public string FolderPath => _folderPath;

        /// <summary>
        /// Saves a HealthPassport to a CSV file named after the profile's name.
        /// Line 1: Header
        /// Line 2: Profile + Vitals data
        /// </summary>
        public void Save(HealthPassport passport)
        {
            string fileName = SanitizeFileName(passport.Profile.Name) + ".csv";
            string filePath = Path.Combine(_folderPath, fileName);

            using StreamWriter writer = new StreamWriter(filePath);

            // Header row
            writer.WriteLine("Name,Age,Height,Weight,Systolic,Diastolic,HeartRate");

            // Data row
            string vitalsData = passport.HasVitals()
                ? $"{passport.Vitals.Systolic},{passport.Vitals.Diastolic},{passport.Vitals.HeartRate}"
                : ",,";

            writer.WriteLine($"{EscapeCsv(passport.Profile.Name)},{passport.Profile.Age},{passport.Profile.Height},{passport.Profile.Weight},{vitalsData}");
        }

        /// <summary>
        /// Loads a HealthPassport from a CSV file.
        /// Returns null if the file doesn't exist or can't be parsed.
        /// </summary>
        public HealthPassport? Load(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
                return null;

            // Skip header (line 0), parse data (line 1)
            string[] fields = ParseCsvLine(lines[1]);
            if (fields.Length < 4)
                return null;

            string name = fields[0];
            if (!int.TryParse(fields[1], out int age)) return null;
            if (!double.TryParse(fields[2], out double height)) return null;
            if (!double.TryParse(fields[3], out double weight)) return null;

            var profile = new PersonProfile(name, age, height, weight);
            var passport = new HealthPassport(profile);

            // Load vitals if present
            if (fields.Length >= 7
                && int.TryParse(fields[4], out int systolic) && systolic > 0
                && int.TryParse(fields[5], out int diastolic) && diastolic > 0
                && int.TryParse(fields[6], out int heartRate) && heartRate > 0)
            {
                passport.RecordVitals(systolic, diastolic, heartRate);
            }

            return passport;
        }

        /// <summary>
        /// Returns all saved passport file paths.
        /// </summary>
        public string[] GetSavedFiles()
        {
            if (!Directory.Exists(_folderPath))
                return Array.Empty<string>();

            return Directory.GetFiles(_folderPath, "*.csv");
        }

        /// <summary>
        /// Deletes a saved passport CSV file.
        /// </summary>
        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private static string SanitizeFileName(string name)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            foreach (char c in invalid)
                name = name.Replace(c, '_');
            return name;
        }

        private static string EscapeCsv(string field)
        {
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            return field;
        }

        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            bool inQuotes = false;
            string current = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current += '"';
                        i++;
                    }
                    else if (c == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        current += c;
                    }
                }
                else
                {
                    if (c == '"')
                        inQuotes = true;
                    else if (c == ',')
                    {
                        fields.Add(current);
                        current = "";
                    }
                    else
                        current += c;
                }
            }
            fields.Add(current);
            return fields.ToArray();
        }
    }
}
