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

// مثال فراخوانی
sendSimpleSms('09121234567', 'سلام، این یک پیام تست از طرف آسانیتو است.');