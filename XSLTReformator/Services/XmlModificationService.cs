using Microsoft.Extensions.Options;
using System.Globalization;
using System.Xml.Linq;
using XSLTReformator.Abstractions;
using XSLTReformator.Configurations;

namespace XSLTReformator.Services
{
    public class XmlModificationService : IXmlModificationService
    {
        private readonly IXmlFileService _xmlFileService;
        private readonly XmlFileSettings _xmlSettings;
        private readonly CultureInfo _cultureInfo;

        public XmlModificationService(IXmlFileService xmlFileService
            , IOptions<XmlFileSettings> xmlSettings)
        {
            _xmlFileService = xmlFileService;
            _xmlSettings = xmlSettings.Value;
            _cultureInfo = CultureInfo.InvariantCulture;
        }
        public async Task<string> WriteTotalAmountToSourceFileAsync(double totalAmount
            , string xmlDataPath
            , CancellationToken cancellationToken = default)
        {
            var xmlDataString = await _xmlFileService.ReadFileAsync(xmlDataPath, cancellationToken);

            var xDoc = XDocument.Parse(xmlDataString, LoadOptions.PreserveWhitespace);

            var payElement = xDoc.Root;

            payElement?.SetAttributeValue("totalAmount", totalAmount);

            return await _xmlFileService.WriteToFileAsync(xDoc, xmlDataPath
                , cancellationToken: cancellationToken);
        }
        public double ExtractTotalAmountFromEmployeesXml(string employeeXML)
        {
            var xDoc = XDocument.Parse(employeeXML);

            return xDoc.Descendants("Employee")
                .Select(employee =>
                {
                    var salary = employee.Attribute("salary")?.Value;
                    return double.Parse(salary ?? "0", _cultureInfo);
                })
                .Sum();
        }
        public async Task<string> AddSalaryToEmployees(string sourceXmlString
            , CancellationToken cancellationToken = default)
        {
            var xDoc = XDocument.Parse(sourceXmlString);

            foreach (var employee in xDoc.Descendants("Employee"))
            {
                var salary = employee.Elements("salary")
                    .Select(s =>
                    {
                        var amount = s.Attribute("amount")?.Value;

                        return double.Parse(amount?.Replace(",", ".") ?? "0", _cultureInfo);
                    })
                    .Sum();

                employee.SetAttributeValue("salary"
                    , Math.Round(salary, 2).ToString("0.00", _cultureInfo));
            }

            var xmlResultPath = Path.Combine(Directory.GetCurrentDirectory()
                , $"{_xmlSettings.BasePath}/Employees.xml");

            return await _xmlFileService.WriteToFileAsync(xDoc, xmlResultPath
                , cancellationToken: cancellationToken);
        }
    }
}
