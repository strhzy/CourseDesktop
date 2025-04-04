using System.Text.Json;
using DnDPartyManager.M;
using LiteDB;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DnDPartyManager.S;

public static class DBHelper
{
    public static LiteDatabase DB = new LiteDatabase(@"data.db");
    private static readonly HttpClient client = new HttpClient();
    private static EnemyResponse lastResponse = null;

    public static List<string> GetSlugs()
    {
        List<string> allSlugs = new List<string>();
        string baseUrl = "https://api.open5e.com/v1/monsters/";
        using HttpClient client = new HttpClient();
    
        string url = baseUrl;
    
        while (url != null)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            string json = response.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<ApiResponse>(json, options);

            if (data?.Results != null)
                allSlugs.AddRange(data.Results.ConvertAll(r => r.Slug));

            url = data?.Next; // Переход на следующую страницу (если есть)
        }

        return allSlugs;
    }

    public static bool CheckSlugUpdate(int slug)
    {
        string url = "https://api.open5e.com/v1/monsters/";
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;
        response.EnsureSuccessStatusCode();

        string json = response.Content.ReadAsStringAsync().Result;
        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<SlugCount>(json, options);

        return slug == data.Count;
    }

    public static async Task<ObservableCollection<Enemy>> GetEnemiesAsync(int page = 1, string filter = "")
    {
        if (lastResponse != null)
        {
            if (lastResponse.Previous == null && page != 2)
            {
                page = 1;
            }
            else if (lastResponse.Next == null)
            {
                page--;
            }
        }
        // TODO: обдумать поиск и перевод на русский
        string url = $"https://api.open5e.com/v1/monsters/?page={page}&name__icontains={filter}";
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        EnemyResponse enemyResponse;
        string json = await response.Content.ReadAsStringAsync();
        enemyResponse = JsonSerializer.Deserialize<EnemyResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        lastResponse = enemyResponse;
        
        return enemyResponse?.Results ?? new ObservableCollection<Enemy>();
    }
}


public class ApiResponse
{
    public List<ResultItem> Results { get; set; }
    public string Next { get; set; }
}

public class SlugCount
{
    public int Count { get; set; }
}

public class ResultItem
{
    public string Slug { get; set; }
}

public class EnemyResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("next")]
    public string Next { get; set; }

    [JsonPropertyName("previous")]
    public string Previous { get; set; }

    [JsonPropertyName("results")]
    public ObservableCollection<Enemy> Results { get; set; }
}