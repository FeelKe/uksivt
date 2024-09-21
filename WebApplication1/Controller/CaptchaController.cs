using Microsoft.AspNetCore.Mvc;
using WebApplication1.Classes;
using System;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptchaController : ControllerBase
    {
        private static string _captchaText;

        [HttpGet("image")]
        public IActionResult GetCaptchaImage()
        {
            var captcha = new CaptchaGenerate();
            _captchaText = captcha.Text;
            var imageBytes = captcha.ToByteArray();
            return File(imageBytes, "image/png");
        }

        [HttpPost("verify")]
        public IActionResult VerifyCaptcha([FromBody] string userInput)
        {
            if (string.IsNullOrEmpty(_captchaText))
            {
                return BadRequest("Captcha has expired or is not available.");
            }

            if (userInput == _captchaText)
            {
                _captchaText = null; 
                return Ok("Captcha is correct");
            }
            return BadRequest("Invalid captcha");
        }
    }
}
