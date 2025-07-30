using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XSLTReformator.Abstracts;
using XSLTReformator.Contracts;
using XSLTReformator.Models;

namespace XSLTReformator.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IXSLTProcessor _processor;
        private readonly IXMLService _xmlService;

        public EmployeesController(IXSLTProcessor processor, IXMLService xmlService)
        {
            _processor = processor;
            _xmlService = xmlService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Transform([FromBody] TransformRequest request
            , CancellationToken cancellationToken = default)
        {
            try
            {
                var xmlDataPath = Path.Combine(Directory.GetCurrentDirectory(), $"XML/{request.XmlFileName}");
                var xsltPath = Path.Combine(Directory.GetCurrentDirectory(), "XSLT/transform-employee-data.xslt");

                var resultXML = await _processor.TransformAsync(xmlDataPath, xsltPath, cancellationToken);

                return Content(resultXML, "text/xml");
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
           
        }

        public async Task<IActionResult> Index()
        {
            var xmlData1Path = Path.Combine(Directory.GetCurrentDirectory(), "XML/Data1.xml");
            var xmlData2Path = Path.Combine(Directory.GetCurrentDirectory(), "XML/Data2.xml");

            var readFile1Task = _xmlService.ReadFileAsync(xmlData1Path);
            var readFile2Task = _xmlService.ReadFileAsync(xmlData2Path);

            await Task.WhenAll(readFile1Task, readFile2Task);

            var model = new XmlSourceFileModel()
            {
                Data1Xml = await readFile1Task,
                Data2Xml = await readFile2Task
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
