using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PALM.Data;
using PALM.Models;
using PALM.Services;
using PALM.Services.MediaStore;

namespace PALM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MediaStoreService _mediaStore;
        private readonly MailService _mailService;

        public HomeController(ILogger<HomeController> logger, 
            IWebHostEnvironment env,
            MediaStoreService mediaStore,
            MailService mailService)
        {
            _logger = logger;
            _mediaStore = mediaStore;
            _mailService = mailService;

            //_contentProvider = new ContentProvider(Path.Combine(env.WebRootPath, "img", "content"));
        }

        public IActionResult Index()
        {
            //return RedirectToAction("Login", "Account");
            return View();
        }

        public IActionResult PhotoBooks()
        {

            return View(_mediaStore.Photobooks.Get().OrderByDescending(p => p.Priority));
        }
        public PartialViewResult PhotoBookCalculator()
        {
            return PartialView();
        }


        public IActionResult CanvasPictures()
        {
            return View(_mediaStore.CanvasPictures.Get().OrderByDescending(c => c.Priority));
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(MailModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendEmail(
                       from: "noreply@artpalm.ro",
                       to: model.Email,
                       subject: "Art PALM: Am primit mesajul tău!",
                       isBodyHtml: true,
                       body: System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine("Data", "MailTemplateHtml.txt")))
                   );


                string interestedIn = string.Empty;
                if (model.PhotobooksCheck) interestedIn += "- Photobooks\n";
                if (model.CanvasCheck) interestedIn += "- Canvas Pictures\n";
                if (model.AudioVideoCheck) interestedIn += "- Audio-Video Transfer\n";
                if (model.OtherSvcCheck) interestedIn += "- Other Services\n";

                _mailService.SendEmail(
                    from: "noreply@artpalm.ro",
                    to: "artpalmro@gmail.com",
                    subject: $"Website message from {model.Name}",
                    isBodyHtml: false,
                    body: $"Name: {model.Name}\nPhone No.: {model.Phone}\nEmail address: {model.Email}\n\nMessage:\n{model.Message}\n\nInterested in:\n{interestedIn}"
                );

                return PartialView("Sent", "MailSuccessfullySent");
            }
            else
            {
                return PartialView(model);
            }
        }

        public PartialViewResult Sent(String s)
        {
            return PartialView(s);
        }



        public IActionResult Magnets()
        {
            return View();
        }


        public IActionResult AudioVideoTransfer()
        {
            return View();
        }

        public IActionResult OtherServices()
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
    }
}
