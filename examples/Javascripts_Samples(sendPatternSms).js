async function sendPatternSms(recipient, patternSlug, variables, sourceNumber = '10001') {
  const response = await fetch(`${BASE_URL}/sms/pattern`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${ACCESS_TOKEN}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      patternSlug,
      variables,      // مثال: { code: '12345' }
      recipient,
      sourceNumber
    })
  });

  console.log(await response.json());
}