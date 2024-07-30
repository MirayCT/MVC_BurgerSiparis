using Microsoft.AspNetCore.Mvc;
using MvcBurger.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace MvcBurger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult SugAndCom()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SugAndCom(string subject, string content, string mail) 
        {
            // kyzymycjmzgfgkcf
            try
            {
                MailMessage message = new MailMessage();

                SmtpClient smtpClient = new SmtpClient();
                //kimden gideceğini ata
                message.From = new MailAddress(mail);

                //kimlere gideceğini ata
                message.To.Add(new MailAddress("MVCProjesiGrup1@gmail.com"));

                //içeriği ata
                message.Body = content;

                //başlığı ata
                message.Subject = subject;

                //geriye mail ile ilgili ayarlar kaldı

                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;

                //gönderen  ile ilgili bilgiler
                smtpClient.Credentials = new NetworkCredential("MVCProjesiGrup1@gmail.com", "kyzymycjmzgfgkcf");

                //nasıl dağıtıma çıkacağını belirleme
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                //maili gönder
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
    
}
