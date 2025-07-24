using System.IO;
using System.Collections.Generic;

namespace cslogo
{
    public static class TilemapCsvParser
    {
        public static List<TilemapElement> Load(string path)
        {
            var elements = new List<TilemapElement>();
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 4) continue;

                string type = parts[0];
                int row = int.Parse(parts[1]);
                int col = int.Parse(parts[2]);
                string value = parts[3];
                int width = parts.Length > 4 ? int.Parse(parts[4]) : 1;
                int height = parts.Length > 5 ? int.Parse(parts[5]) : 1;
                string tag = parts.Length > 6 ? parts[6] : "";

                elements.Add(new TilemapElement(type, row, col, value, width, height, tag));
            }

            return elements;
        }
    }
}