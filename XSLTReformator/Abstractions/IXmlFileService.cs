using System.Xml.Linq;
using XSLTReformator.Models;

namespace XSLTReformator.Abstractions
{
    public interface IXmlFileService
    {
        Task<XmlSourceFileModel> GetSourceFilesAsync();
        Task<string> ReadFileAsync(string filePath
            , CancellationToken cancellationToken = default);
        Task<string> WriteToFileAsync(XDocument document
            , string filePath
            , CancellationToken cancellationToken = default);
    }
}