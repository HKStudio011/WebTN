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
<<<<<<< HEAD:appsettings.json
    "Mail": "hkstudioentertainment@gmail.com",
    "DisplayName": "HKStudio",
    "Password": "uzdt bqfv jcay hqhs",
=======
    "Mail": "",
    "DisplayName": "",
    "Password": "",
>>>>>>> abe54367bf1eac3d6b51fc5589e2ef88017fc66a:README.md
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
