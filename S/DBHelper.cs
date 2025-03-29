using System.Text.Json;
using DnDPartyManager.M;
using LiteDB;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DnDPartyManager.S;

public static class DBHelper
{
    public static LiteDatabase DB = new LiteDatabase(@"data.db");

    public static List<string> GetSlugs()
    {
        List<string> allSlugs = new List<string>();
        string baseUrl = "https://api.open5e.com/v1/monsters/";
        using HttpClient client = new HttpClient();
    
        string url = baseUrl;
    
        while (url != null) // Пока есть следующая страница
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

    public static Enemy GetEnemy(string slug)
    {
        string url = $"https://api.open5e.com/v1/monsters/{slug}/";
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;
        response.EnsureSuccessStatusCode();

        string json = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<Enemy>(json);
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