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