using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PES.Application.Helper.EmailHandler
{
    public  class SendMailHandler
    {
        private readonly IConfiguration _config;

        public SendMailHandler(IConfiguration config)
        {
            _config = config;
        }


        public async Task SendMail(string? subject = "Fotget Password", string? Email = "mandayngu@gmail.com", string OTP = "XXX_02",string UserName = "vui ve")
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(Email);
            mail.From = new MailAddress(_config["SendMailAccount:UserName"].ToString(), "From PlantEcommerceSystem", Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            using (StreamReader reader = new StreamReader("../PES.Presentation/Template/forgetPassword.html"))
            //C:\Code\Assignment_NashTech\src\PES.Presentation\Template\forgerPassword.html
            {
                mail.Body = reader.ReadToEnd();

            }
            mail.Body = mail.Body.Replace("[Customer Name]", UserName);
            mail.Body = mail.Body.Replace("[OTP]", OTP);
        

            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            var client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_config["SendMailAccount:UserName"], _config["SendMailAccount:AppPassword"]);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(mail);
        }

        public async Task SendOTP(string? Email = "mandayngu@gmail.com", int Otp = 0)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(Email);
            mail.From = new MailAddress(_config["SendMailAccount:UserName"].ToString(), "From FireDetection_FPT", Encoding.UTF8);
            mail.Subject = "Reset Password";
            mail.SubjectEncoding = Encoding.UTF8;
            using (StreamReader reader = new StreamReader("../wwwroot/Template/sendotp.html"))
            {
                mail.Body = reader.ReadToEnd();

            }
            mail.Body = mail.Body.Replace("{otp}", Otp.ToString());
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            var client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_config["SendMailAccount:UserName"], _config["SendMailAccount:AppPassword"]);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(mail);
        }

    }
}
