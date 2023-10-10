using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace wkhtmlToPdfDemo.Controllers;
public class ApiController : Controller
{
    private readonly IConverter _converter;

    public ApiController(IConverter converter)
    {
        _converter = converter;
    }

    [HttpGet]
    [Route("/api")]
    public IActionResult Index(string url)
    {

        if (url.Contains("?"))
        {
            url = url.Split("?")[0];
        }

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4Plus,

            },
            Objects =
            {
                new ObjectSettings()
                {
                    PagesCount = true,
                    UseExternalLinks = true,
                    WebSettings =
                    {
                        DefaultEncoding = "utf-8", PrintMediaType = true
                    },

                    HeaderSettings = new HeaderSettings(),
                    Page = url,

                }
            }
        };

        return File(_converter.Convert(doc), "application/pdf");
    }
}
