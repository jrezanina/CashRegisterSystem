using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Models;
using System.Diagnostics;

namespace PokladniSystem.Controllers
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
        public ActionResult Print(string inputValue)
        {
            string contentToPrint = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <style>\r\n    body {{\r\n      margin: 0;\r\n      padding: 0;\r\n      font-family: Arial, sans-serif;\r\n    }}\r\n\r\n    .receipt {{\r\n      display: flex;\r\n      flex-direction: column;\r\n    }}\r\n\r\n    h1, p {{\r\n      margin: 5px 0;\r\n    }}\r\n\r\n    @media print {{\r\n      .receipt {{\r\n        max-width: 100%;\r\n      }}\r\n    }}\r\n  </style>\r\n  <script>\r\n    // Funkce pro dynamické nastavení velikosti písma na základě délky textu\r\n    function adjustFontSize() {{\r\n      var receipt = document.querySelector('.receipt');\r\n      var textElements = Array.from(receipt.querySelectorAll('h1, p'));\r\n\r\n      textElements.forEach(function(element) {{\r\n        var contentLength = element.textContent.length;\r\n        var baseFontSize = 16; // Výchozí velikost písma\r\n        var scaleFactor = Math.min(1, 50 / contentLength); // Změna měřítka (upravte podle potřeby)\r\n\r\n        var fontSize = baseFontSize * scaleFactor + 'px';\r\n        element.style.fontSize = fontSize;\r\n      }});\r\n    }}\r\n\r\n    // Volání funkce při načítání stránky a při změně velikosti okna\r\n    window.addEventListener('load', adjustFontSize);\r\n    window.addEventListener('resize', adjustFontSize);\r\n  </script>\r\n</head>\r\n<body>\r\n  <div class=\"receipt\">\r\n    <h1>Pokus</h1>\r\n    <p>Datum: {DateTime.Now.ToString("dd.MM.yyyy")}</p>\r\n    <p>Čas: {DateTime.Now.ToString("HH:mm:ss")}</p>\r\n    <p>EAN: {inputValue}</p>\r\n  </div>\r\n</body>\r\n</html>";

            return Content(contentToPrint, "text/html");
        }
    }
}
