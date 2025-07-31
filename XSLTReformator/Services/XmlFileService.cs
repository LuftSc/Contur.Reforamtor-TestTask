using Microsoft.Extensions.Options;
using System.Text;
using System.Xml.Linq;
using XSLTReformator.Abstractions;
using XSLTReformator.Configurations;
using XSLTReformator.Models;

namespace XSLTReformator.Services
{
    public class XmlFileService : IXmlFileService
    {
        private readonly XmlFileSettings _xmlSettings;

        public XmlFileService(IOptions<XmlFileSettings> xmlSettings)
        {
            _xmlSettings = xmlSettings.Value;
        }
        public async Task<string> ReadFileAsync(string filePath
           , CancellationToken cancellationToken = default)
        {
            try
            {
                return await File.ReadAllTextAsync(filePath, cancellationToken);
            }
            catch (Exception error)
            {
                return $"<!-- Error reading file: {error.Message} -->";
            }
        }

        public async Task<XmlSourceFileModel> GetSourceFilesAsync()
        {
            var fileNames = _xmlSettings.AllowedFiles;

            var readTasks = fileNames
                .Select(fileName =>
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory()
                        , _xmlSettings.BasePath
                        , fileName);
                    return ReadFileAsync(filePath);
                })
                .ToList();

            await Task.WhenAll(readTasks);

            return new XmlSourceFileModel
            {
                Data1Xml = await readTasks[0],
                Data2Xml = await readTasks[1],
            };
        }

        public async Task<string> WriteToFileAsync(XDocument document
            , string filePath
            , CancellationToken cancellationToken = default)
        {
            var resultXml = _xmlSettings.BaseDeclarationString + "\n" +
                string.Join("\n",
                    document.ToString()
                        .Split('\n')
                        .Where(line => !string.IsNullOrWhiteSpace(line)));

            await File.WriteAllTextAsync(filePath, resultXml, Encoding.UTF8, cancellationToken);

            return resultXml;
        }
    }
}
