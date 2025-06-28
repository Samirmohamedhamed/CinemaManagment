using System.Text.Json;

namespace MoviePoster
{

    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static readonly string apiKey = ""; // استبدل بمفتاحك
        static readonly string folderPath = @""; // مسار حفظ الصور
        static readonly string moviesFilePath = @"\movies.txt"; // ملف أسماء الأفلام

        static async Task Main(string[] args)
        {
            if (!File.Exists(moviesFilePath))
            {
                Console.WriteLine($"❌ ملف {moviesFilePath} غير موجود.");
                return;
            }

            Directory.CreateDirectory(folderPath);

            var movies = await File.ReadAllLinesAsync(moviesFilePath);

            foreach (var rawTitle in movies)
            {
                var title = rawTitle.Trim();
                if (string.IsNullOrWhiteSpace(title)) continue;

                Console.WriteLine($"🎬 Fetching poster for: {title}");

                var url = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={apiKey}";
                var json = await client.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("Poster", out var posterElement))
                {
                    var posterUrl = posterElement.GetString();

                    if (!string.IsNullOrEmpty(posterUrl) && posterUrl != "N/A")
                    {
                        var bytes = await client.GetByteArrayAsync(posterUrl);
                        var safeTitle = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));
                        var filePath = Path.Combine(folderPath, $"{safeTitle}.jpg");

                        await File.WriteAllBytesAsync(filePath, bytes);
                        Console.WriteLine($"✅ Saved: {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ No poster found for: {title}");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Failed to fetch data for: {title}");
                }
            }

            Console.WriteLine("🏁 Finished downloading all posters.");
        }
    }
}
