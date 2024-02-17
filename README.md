## appsettings.json

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MyBlogContext" : "Server=localhost,1433; Database=DatabaseEF; Encrypt=true; TrustServerCertificate=true; Integrated Security=true;"
  },
  "MailSettings": {
    "Mail": "",
    "DisplayName": "",
    "Password": "",
    "Host": "smtp.gmail.com",
    "Port": 587
  },

  "Authentication": {
    "Google": {
      "ClientId": "",
      "ClientSecret": ""
    }, 
    "Facebook": {
      "AppId": "",
      "AppSecret": ""
    }
  }
}

```
