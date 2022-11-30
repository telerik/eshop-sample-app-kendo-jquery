using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using Telerik.Web.Captcha;
using Models.InputModels;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class ContactsController : Controller
    {
        //protected readonly IWebHostEnvironment HostingEnvironment;
        //protected readonly string CaptchaPath;

        //public ContactsController(IWebHostEnvironment hostingEnvironment)
        //{
        //    HostingEnvironment = hostingEnvironment;            
        //}

        public ActionResult Index()
        {
            return View();
        }        

        [HttpPost]
        public IActionResult Submit(string captchaId, string captcha)
        {
            string? captchaSerialized = HttpContext.Session.GetString("captcha" + captchaId);
            CaptchaImage? captchaImage = GetCaptcha(captchaSerialized);

            if (captchaImage is not null && CaptchaHelper.Validate(captchaImage, captcha.ToUpperInvariant()))
            {
                return RedirectToRoute("/Contacts/Index", new { section = "captcha", example = "success" });
            }
            else
            {
                return RedirectToRoute("/Contacts/Index", new { section = "captcha", example = "fail" });
            }
        }

        [HttpGet]
        public IActionResult Reset()
        {
            CaptchaImage newCaptcha = CaptchaHelper.GetNewCaptcha();

            HttpContext.Session.SetString("captcha" + newCaptcha.UniqueId, SerializeCaptcha(newCaptcha));

            return Json(new
            {
                captcha = Url.Action("image", "Contacts", new { captchaId = newCaptcha.UniqueId }),
                captchaId = newCaptcha.UniqueId
            });
        }

        public IActionResult Image(string captchaId)
        {
            string? captchaSerialized = HttpContext.Session.GetString("captcha" + captchaId);
            CaptchaImage? captcha = GetCaptcha(captchaSerialized);
            var image = CaptchaHelper.RenderCaptcha(captcha);
            byte[] bmpBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                bmpBytes = ms.ToArray();
            }

            return File(bmpBytes, "image/png");
        }
        public IActionResult AudioHandler(string captchaId)
        {
            return Content("./audio?captchaId=" + captchaId);
        }
        public IActionResult Audio(string captchaId)
        {
            string? captchaSerialized = HttpContext.Session.GetString("captcha" + captchaId);
            CaptchaImage? captcha = GetCaptcha(captchaSerialized);
            byte[] bmpBytes;

            using (MemoryStream audio = CaptchaHelper.SpeakText(captcha))
            {
                bmpBytes = audio.ToArray();
            }

            return File(bmpBytes, "audio/wav");
        }

        public IActionResult Validate(string captchaId, string captcha)
        {
            captcha = captcha ?? string.Empty;
            string? captchaSerialized = HttpContext.Session.GetString("captcha" + captchaId);
            CaptchaImage? captchaImage = GetCaptcha(captchaSerialized);

            return Json(CaptchaHelper.Validate(captchaImage, captcha.ToUpperInvariant()));
        }

        private CaptchaImage? GetCaptcha(string? captchaSerialized)
        {
            if (captchaSerialized is null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<CaptchaImage>(captchaSerialized);
        }

        private string SerializeCaptcha(CaptchaImage captcha)
        {
            return JsonConvert.SerializeObject(captcha);
        }        
    }
}
