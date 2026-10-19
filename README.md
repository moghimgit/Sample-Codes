# Panelchi SMS Samples

نمونه‌کدهای آماده برای ارسال پیامک با استفاده از **API سرویس پنلچی** در زبان‌های C#، Java و JavaScript.

> **توجه:** برای استفاده از این نمونه‌کدها، باید مقدار `ACCESS_TOKEN` را با توکن واقعی دریافتی از پنل کاربری خود جایگزین کنید. همچنین شماره فرستنده (`sourceNumber`) باید یکی از خطوط فعال حساب شما باشد.

---

## پیش‌نیازها

- دسترسی به API پنلچی (توکن Bearer)
- آشنایی مقدماتی با یکی از زبان‌های C#، Java یا JavaScript
- (اختیاری) آشنایی با مفهوم **ارسال الگو (Pattern)** در سرویس‌های پیامکی

---

## ساختار مخزن

```
examples/
├── csharp/          # نمونه‌های C# (.NET)
├── java/            # نمونه‌های Java
└── javascript/      # نمونه‌های JavaScript (Fetch API)
```

---

## ۱. ارسال پیامک ساده (Simple SMS)

ارسال یک متن دلخواه به شماره مشخص.

### C# (SendSimpleSms.cs)

```csharp
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
    private const string AccessToken = "YOUR_ACCESS_TOKEN";

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

    public static async Task Main()
    {
        await SendSimpleSmsAsync("09121234567", "سلام، این یک پیام تست از طرف آسانیتو است.");
    }
}
```

### Java (SendSimpleSms.java)

```java
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;

public class PanelchiSmsSender {
    private static final String BASE_URL = "https://api.panelchi.com";
    private static final String ACCESS_TOKEN = "YOUR_ACCESS_TOKEN";

    public static void sendSimpleSms(String recipient, String message, String sourceNumber) throws Exception {
        String jsonBody = String.format("""
            {
                "recipient": "%s",
                "message": "%s",
                "sourceNumber": "%s"
            }
            """, recipient, message, sourceNumber);

        HttpClient client = HttpClient.newHttpClient();
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/sms/send"))
                .header("Authorization", "Bearer " + ACCESS_TOKEN)
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(jsonBody, StandardCharsets.UTF_8))
                .build();

        HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
        System.out.println("Status: " + response.statusCode());
        System.out.println("Response: " + response.body());
    }

    public static void main(String[] args) throws Exception {
        sendSimpleSms("09121234567", "سلام، این یک پیام تست از طرف آسانیتو است.", "10001");
    }
}
```

### JavaScript (sendSimpleSms.js)

```javascript
const BASE_URL = 'https://api.panelchi.com';
const ACCESS_TOKEN = 'YOUR_ACCESS_TOKEN';

async function sendSimpleSms(recipient, message, sourceNumber = '10001') {
  const response = await fetch(`${BASE_URL}/sms/send`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${ACCESS_TOKEN}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      recipient,
      message,
      sourceNumber
    })
  });

  const data = await response.json();
  console.log('Status:', response.status);
  console.log('Response:', data);
}

sendSimpleSms('09121234567', 'سلام، این یک پیام تست از طرف آسانیتو است.');
```

---

## ۲. ارسال پیامک با الگو (Pattern SMS)

ارسال پیامک مبتنی بر الگوی از پیش تعریف‌شده در پنل، همراه با متغیرهای جایگزین.

### C# (SendPatternSms.cs)

```csharp
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
```

### Java (SendPatternSms.java)

```java
public static void sendPatternSms(String recipient, String patternSlug,
                                  Map<String, String> variables, String sourceNumber) throws Exception {
    String variablesJson = variables.entrySet().stream()
            .map(e -> String.format("\"%s\":\"%s\"", e.getKey(), e.getValue()))
            .collect(Collectors.joining(","));

    String jsonBody = String.format("""
        {
            "patternSlug": "%s",
            "variables": {%s},
            "recipient": "%s",
            "sourceNumber": "%s"
        }
        """, patternSlug, variablesJson, recipient, sourceNumber);

    // ارسال مشابه نمونهٔ قبل با HttpClient
}
```

### JavaScript (sendPatternSms.js)

```javascript
async function sendPatternSms(recipient, patternSlug, variables, sourceNumber = '10001') {
  const response = await fetch(`${BASE_URL}/sms/pattern`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${ACCESS_TOKEN}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      patternSlug,
      variables,
      recipient,
      sourceNumber
    })
  });

  console.log(await response.json());
}
```

---

## ۳. نمونه پاسخ API (Response Sample)

پس از ارسال موفق، پاسخی مشابه ساختار زیر دریافت می‌کنید:

```json
{
  "data": {
    "cost": "1500.00",
    "message": "کد تایید شما: 12345",
    "uid": "17765023114093517",
    "status": "CREATED",
    "blacklisted_count": 0
  },
  "meta": {
    "message": "عملیات با موفقیت انجام شد.",
    "requestId": "d9647fb8-4e9e-4709-9ba4-dfe9927b05a7",
    "serverTime": "2026-05-11T11:01:36.152123Z"
  }
}
```

---

## نکات مهم

- **Base URL** تمام درخواست‌ها: `https://api.panelchi.com`
- **هدر احراز هویت:** `Authorization: Bearer <YOUR_ACCESS_TOKEN>`
- **Content-Type:** `application/json`
- برای ارسال الگو، باید `patternSlug` از پیش در پنل پنلچی ایجاد و تأیید شده باشد.
- در صورت نیاز به ارسال گروهی یا استفاده از سایر امکانات، به مستندات رسمی پنلچی مراجعه کنید.

---

## مشارکت

اگر نمونه‌کد زبان دیگری دارید یا بهبودی در کدها پیشنهاد می‌دهید، خوشحال می‌شویم از طریق Issue یا Pull Request مشارکت کنید. لطفاً قبل از ارسال PR، فایل [CONTRIBUTING.md](CONTRIBUTING.md) را مطالعه کنید.

---

## لایسنس

این پروژه تحت لایسنس **MIT** منتشر شده است. برای جزئیات بیشتر فایل [LICENSE](LICENSE) را ببینید.