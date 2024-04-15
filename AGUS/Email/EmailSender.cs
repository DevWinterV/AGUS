using System.Net.Mail;
using System.Net;

namespace AGUS.Email
{
    public class EmailSender
    {
        public EmailSender() { }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            // Thông tin cấu hình SMTP (điều này phụ thuộc vào nhà cung cấp email của bạn)
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com", // Thay thế bằng host SMTP của bạn
                Port = 587, // Thay thế bằng port SMTP của bạn
                Credentials = new NetworkCredential("chauvanrangdong4440@gmail.com", "wbswwxmptbmwfqpg"), // Thay thế bằng thông tin đăng nhập email của bạn
                EnableSsl = true // Sử dụng SSL (thông thường là true)
            };

            // Tạo đối tượng MailMessage
            var mailMessage = new MailMessage
            {
                From = new MailAddress("chauvanrangdong4440@gmail.com"), // Thay thế bằng địa chỉ email của bạn
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            // Thêm địa chỉ email nhận vào danh sách người nhận
            mailMessage.To.Add(toEmail);

            // Gửi email
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
