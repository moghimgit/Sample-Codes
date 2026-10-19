public static async Task SendPatternSmsAsync(string recipient, string patternSlug, Dictionary<string, string> variables, string sourceNumber = "10001")
{
    var requestBody = new
    {
        patternSlug = patternSlug,
        variables = variables,
        recipient = recipient,
        sourceNumber = sourceNumber
    };

    var json = JsonSerializer.Serialize(requestBody);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/sms/pattern");
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
    request.Content = content;

    var response = await _httpClient.SendAsync(request);
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}