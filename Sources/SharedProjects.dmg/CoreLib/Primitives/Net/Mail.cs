using System.Net;
using System.Net.Mail;

namespace Mohammad.Net
{
    public class Mail
    {
        public const string HOST_HOTMAIL_ADDESS = "smtp.live.com";
        public const int HOST_HOTMAIL_PORT = 587;
        public const string HOST_GMAIL_ADDESS = "smtp.gmail.com";
        public const int HOST_GMAIL_PORT = 587;
        public const string HOST_YAHOO_ADDESS = "smtp.mail.yahoo.com";
        public const int HOST_YAHOO_PORT = 587;
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
        public string Domain { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
        public int? Port { get; set; }
        public Mail() { }

        public Mail(string host, int? port, string userName, string password, string sender, string reciever = null, string subject = null, string content = null,
            string senderName = null, string domain = null, bool enableSsl = true, bool isBodyHtml = true)
        {
            this.Sender = sender;
            this.Reciever = reciever;
            this.Subject = subject;
            this.Content = content;
            this.Host = host;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;
            this.SenderName = senderName;
            this.Domain = domain;
            this.EnableSsl = enableSsl;
            this.IsBodyHtml = isBodyHtml;
        }

        public void Send()
        {
            Send(this.Subject,
                this.Content,
                this.Reciever,
                this.Sender,
                this.SenderName,
                this.Host,
                this.Port,
                this.UserName,
                this.Password,
                this.Domain,
                this.EnableSsl,
                this.IsBodyHtml);
        }

        public static void Send(string subject, string content, string reciever, string sender, string senderName, string host, int? port, string username,
            string password, string domain, bool enableSsl, bool isBodyHtml)
        {
            using (
                var message = new MailMessage
                              {
                                  From = new MailAddress(sender, senderName),
                                  Subject = subject,
                                  Body = content,
                                  Sender = new MailAddress(sender),
                                  IsBodyHtml = isBodyHtml
                              })
            {
                message.To.Add(reciever);
                using (var smtp = new SmtpClient {Host = host, Credentials = new NetworkCredential(username, password, domain), EnableSsl = enableSsl})
                {
                    if (port.HasValue)
                        smtp.Port = port.Value;
                    smtp.Send(message);
                }
            }
        }
    }
}