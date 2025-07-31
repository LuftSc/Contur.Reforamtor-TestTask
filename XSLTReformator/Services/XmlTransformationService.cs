using Microsoft.Extensions.Options;
using XSLTReformator.Abstractions;
using XSLTReformator.Configurations;
using XSLTReformator.Models;

namespace XSLTReformator.Services
{
    public class XmlTransformationService : IXmlTransformationService
    {
        private readonly IXsltProcessor _processor;
        private readonly IXmlModificationService _xmlService;
        private readonly XsltFileSettings _xsltSettings;
        private readonly XmlFileSettings _xmlSettings;

        public XmlTransformationService(IXsltProcessor processor
            , IXmlModificationService xmlService
            , IOptions<XmlFileSettings> xmlSettings
            , IOptions<XsltFileSettings> xsltSettings)
        {
            _processor = processor;
            _xmlService = xmlService;
            _xsltSettings = xsltSettings.Value;
            _xmlSettings = xmlSettings.Value;
        }
        public async Task<TransformationResult> TransformAndUpdateSourceAsync(string xmlFileName
            , CancellationToken cancellationToken = default)
        {
            var xmlDataPath = Path.Combine(Directory.GetCurrentDirectory()
                , $"{_xmlSettings.BasePath}/{xmlFileName}");
            var xsltPath = Path.Combine(Directory.GetCurrentDirectory()
                , $"{_xsltSettings.BasePath}/transform-employee-data.xslt");

            var transformedXML = await _processor.TransformAsync(xmlDataPath, xsltPath, cancellationToken);

            var modifiedXML = await _xmlService.AddSalaryToEmployees(transformedXML, cancellationToken);
            var totalEmployeesAmount = _xmlService.ExtractTotalAmountFromEmployeesXml(modifiedXML);
            var updatedSourceXML = await _xmlService.WriteTotalAmountToSourceFileAsync
                ( totalEmployeesAmount
                , xmlDataPath
                , cancellationToken:cancellationToken);

            return new TransformationResult
            {
                ResultXml = modifiedXML,
                SourceXml = updatedSourceXML
            };
        }
    }
}
