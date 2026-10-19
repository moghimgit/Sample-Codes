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