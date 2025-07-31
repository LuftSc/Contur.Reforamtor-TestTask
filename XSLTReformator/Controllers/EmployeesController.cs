using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XSLTReformator.Abstractions;
using XSLTReformator.Contracts;
using XSLTReformator.Models;

namespace XSLTReformator.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IXmlTransformationService _transformationService;
        private readonly IXmlFileService _xmlFileService;

        public EmployeesController(IXmlTransformationService transformationService
            , IXmlFileService xmlFileService)
        {
            _transformationService = transformationService;
            _xmlFileService = xmlFileService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Transform(
            [FromBody] TransformRequest request
            , [FromServices] IValidator<TransformRequest> validator
            , CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            try
            {
                var result = await _transformationService
                    .TransformAndUpdateSourceAsync(request.XmlFileName, cancellationToken);

                return Json(result);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        public async Task<IActionResult> Index()
        {
            var model = await _xmlFileService.GetSourceFilesAsync();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
