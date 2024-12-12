using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using System.Security.Cryptography;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;
using System.Text;

namespace wkhtmlToPdfDemo.Controllers;
public class ApiController : Controller
{
    private readonly IConverter _converter;
    private readonly string _secret;

    public ApiController(IConverter converter, IConfiguration configuration)
    {
        _converter = converter;
        _secret = configuration["secret"];
    }

    [HttpGet]
    [Route("/api")]
    public IActionResult Index(string url,string requestTimeUtc, string checksum)
    {
        var requestDate = DateTime.ParseExact(requestTimeUtc,"O", null);

        if (DateTime.UtcNow - requestDate > TimeSpan.FromMinutes(1))
        {
            throw new Exception("Request is too old");
        }

        if (!VerifyChecksum(checksum, $"{_secret}{url}{requestTimeUtc}"))
        {
            throw new Exception("Checksum incorrect");
        }

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

    private bool VerifyChecksum(string checksum, string body)
    {
        var computedSecret = Sha256(body);
        return computedSecret.Equals(checksum, StringComparison.OrdinalIgnoreCase);
    }

    static string Sha256(string randomString)
    {
        var crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
        foreach (byte theByte in crypto)
        {
            hash += theByte.ToString("x2");
        }
        return hash;
    }
}
