using XSLTReformator.Abstracts;

namespace XSLTReformator.Services
{
    public class XMLService : IXMLService
    {
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
    }
}
