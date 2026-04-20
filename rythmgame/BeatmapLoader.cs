using System.Text.Json;

namespace rythmgame
{
    public static class BeatmapLoader
    {
        public static Beatmap Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            Beatmap beatmap = JsonSerializer.Deserialize<Beatmap>(json);
            return beatmap;
        }

    }
}
