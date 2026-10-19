using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class PanelchiSmsSender
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private const string BaseUrl = "https://api.panelchi.com";
    private const string AccessToken = "YOUR_ACCESS_TOKEN"; // توکن دریافتی از پنل

    public static async Task SendSimpleSmsAsync(string recipient, string message, string sourceNumber = "10001")
    {
        var requestBody = new
        {
            recipient = recipient,
            message = message,
            sourceNumber = sourceNumber
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/sms/send");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Response: {responseBody}");
    }

    // مثال فراخوانی
    public static async Task Main()
    {
        await SendSimpleSmsAsync("09121234567", "سلام، این یک پیام تست از طرف آسانیتو است.");
    }
}