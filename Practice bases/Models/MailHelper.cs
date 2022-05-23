using System.Net;
using System.Net.Mail;

namespace Practice_bases.Models;

public class MailHelper
{
    
    
    public static async Task SendEmailAsync(string email, string key)
    {
        MailAddress from = new MailAddress("OrexKashtan@gmail.com", "MPT practice assistant");
         MailAddress to = new MailAddress(email);
         MailMessage m = new MailMessage(from, to);
         m.Subject = "подтверждение адреса электронной почты";
         m.Body = $"Для подтверждения адреса электронной почты нажмите на ссылку: http://localhost:5000/Account/Activation?key={key}";
         SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
         smtp.Credentials = new NetworkCredential("OrexKashtan@gmail.com", "HXJ4p65COnfW");
         smtp.EnableSsl = true;
         await smtp.SendMailAsync(m);
    }
    
    private static readonly Random random = new Random();
    private static readonly string alphabet = "0123456789ABCDEF";
    public static string Generate()
    {
        var buffer = new char[32];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = alphabet[random.Next(alphabet.Length)];
        }
        return new string(buffer);
    }
}