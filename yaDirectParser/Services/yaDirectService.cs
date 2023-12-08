using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using yaDirectParser.Models;

namespace yaDirectParser.Middlewares;

public class yaDirectService
{
    private readonly HttpClient _httpClient;
    public static List<Ad>? Result;
    private static DateTime _dateOfRequest;
    public yaDirectService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var clientLogin = context.Session.GetString("ClientLogin");
        var token = context.Session.GetString("AccessToken");
        if (DateTime.Now.Date != _dateOfRequest)
        {
            _dateOfRequest = DateTime.Now.Date;
            await RequestToCampaigns(token, clientLogin);
        }
    }

    public async Task RequestToCampaigns(string token, string clientLogin)
    {
        const string campaignsUrl = "https://api-sandbox.direct.yandex.com/json/v5/campaigns";
        var request = new HttpRequestMessage(HttpMethod.Post, campaignsUrl);
        
        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Client-Login", clientLogin);
        request.Headers.Add("Accept-Language", "ru");
        
        var body = new
        {
            method = "get",
            @params = new
            {
                SelectionCriteria = new {},
                FieldNames = new string[] { "Id", "Name" }
            }
        };
        
        var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        
        try
        {
            var response = await _httpClient.SendAsync(request);
        
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Произошла ошибка при обращении к серверу API Директа.");
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Успешный запрос!");
                var resultObjects = JsonConvert.DeserializeObject<ResponseObject>(responseBody);
                await RequestToCampaignAdGroups(resultObjects.result.Campaigns, token, clientLogin);
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Произошла ошибка HTTP-запроса: {e.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
        }
    }

    public async Task RequestToCampaignAdGroups(IEnumerable<Campaign> campaigns, string token, string clientLogin)
    {
        List<AdGroup> campaignAdGroups = new List<AdGroup>();
        const string adGroupUrl = "https://api-sandbox.direct.yandex.com/json/v5/adgroups";
        var request = new HttpRequestMessage(HttpMethod.Post, adGroupUrl);

        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Client-Login", clientLogin);
        request.Headers.Add("Accept-Language", "ru");
            var body = new
            {
                method = "get",
                @params = new
                {
                    SelectionCriteria = new { CampaignIds = campaigns.Select(value => value.Id).ToArray() },
                    FieldNames = new string[] {"Id", "Name", "Status", "Type"}
                }
            };

            var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Произошла ошибка при обращении к серверу API Директа.");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Успешный запрос!");
                    var resultObjects = JsonConvert.DeserializeObject<ResponseObject>(responseBody);
                    campaignAdGroups.AddRange(resultObjects.result.AdGroups);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Произошла ошибка HTTP-запроса: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
        await RequestToAdGroups(campaignAdGroups, token, clientLogin);
        
    }

    public async Task RequestToAdGroups(IEnumerable<AdGroup> adGroups, string token, string clientLogin)
    {
        List<Ad> ads = new List<Ad>();
        const string adGroupUrl = "https://api-sandbox.direct.yandex.com/json/v5/ads";
        var request = new HttpRequestMessage(HttpMethod.Post, adGroupUrl);

        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Client-Login", clientLogin);
        request.Headers.Add("Accept-Language", "ru");
        var body = new
            {
                method = "get",
                @params = new
                {
                    SelectionCriteria = new { AdGroupIds = adGroups.Select(value => value.Id).ToArray() },
                    FieldNames = new string[] { "Id" }
                }
            };

            var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Произошла ошибка при обращении к серверу API Директа.");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Успешный запрос!");
                    var resultObjects = JsonConvert.DeserializeObject<ResponseObject>(responseBody);
                    ads.AddRange(resultObjects.result.Ads);
                }

                await RequestToAds(ads, token, clientLogin);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Произошла ошибка HTTP-запроса: {e.Message}");
            }
            catch (Exception ex)
            {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
    }
    public async Task RequestToAds(IEnumerable<Ad> adGroups, string token, string clientLogin)
    {
        const string adGroupUrl = "https://api-sandbox.direct.yandex.com/json/v5/ads";
        var request = new HttpRequestMessage(HttpMethod.Post, adGroupUrl);

        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Client-Login", clientLogin);
        request.Headers.Add("Accept-Language", "ru");
            var body = new
            {
                method = "get",
                @params = new
                {
                    SelectionCriteria = new { Ids = adGroups.Select(value => value.Id).ToArray() },
                    FieldNames = new string[] { "Id" },
                    TextAdFieldNames = new string[] {"Text", "Title", "Href", "VCardId"}
                }
            };

            var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Произошла ошибка при обращении к серверу API Директа.");
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Успешный запрос!");
                    var resultObjects = JsonConvert.DeserializeObject<ResponseObject>(responseBody);
                    Result = resultObjects.result.Ads;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Произошла ошибка HTTP-запроса: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
        }
    }