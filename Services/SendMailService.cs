using System;
using System.Threading.Tasks;
using MailKit;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using WebTN.Models;

//namespace WebTN.Services;

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}

// Identity
// public class SendMailService : IEmailSender<AppUser>
// {
//     private readonly MailSettings mailSettings;

//     private readonly ILogger<SendMailService> logger;



//     // mailSetting được Inject qua dịch vụ hệ thống
//     // Có inject Logger để xuất log
//     public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
//     {
//         mailSettings = _mailSettings.Value;
//         logger = _logger;
//         logger.LogInformation("Create SendMailService");
//     }

//     public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
//     {
//         await SendEmailAsync(email,subject:"Kích Hoạt Tài Khoản",confirmationLink);
//     }

//     // Gửi email, theo nội dung trong mailContent
//     public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//     {
//         var message = new MimeMessage();
//         message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
//         message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
//         message.To.Add(MailboxAddress.Parse(email));
//         message.Subject = subject;


//         var builder = new BodyBuilder();
//         builder.HtmlBody = htmlMessage;
//         message.Body = builder.ToMessageBody();

//         // dùng SmtpClient của MailKit
//         using var smtp = new MailKit.Net.Smtp.SmtpClient();

//         try
//         {
//             smtp.CheckCertificateRevocation = false; // user if error occurred while attempting to establish an SSL or TLS connection
//             await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
//             await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
//             await smtp.SendAsync(message);
//         }
//         catch (Exception ex)
//         {
//             // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
//             System.IO.Directory.CreateDirectory("mailssave");
//             var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
//             await message.WriteToAsync(emailsavefile);

//             logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
//             logger.LogError(ex.Message);
//         }

//         await smtp.DisconnectAsync(true);

//         logger.LogInformation("send mail to " + email);
//     }

//     public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
//     {
//         await SendEmailAsync(email,subject:"Quen Mật Khẩu",resetCode);
//     }

//     public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
//     {
//         await SendEmailAsync(email,subject:"Quen Mật Khẩu",resetLink);
//     }
// }

//Identity UI
public class SendMailService : IEmailSender
{
    private readonly MailSettings mailSettings;

    private readonly ILogger<SendMailService> logger;



    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
    {
        mailSettings = _mailSettings.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

    // Gửi email, theo nội dung trong mailContent
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
        message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = htmlMessage;
        message.Body = builder.ToMessageBody();

        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            smtp.CheckCertificateRevocation = false; // user if error occurred while attempting to establish an SSL or TLS connection
            await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(message);
        }
        catch (Exception ex)
        {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            System.IO.Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);

            logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
            logger.LogError(ex.Message);
        }

        await smtp.DisconnectAsync(true);

        logger.LogInformation("send mail to " + email);
    }

}