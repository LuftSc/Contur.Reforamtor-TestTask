namespace XSLTReformator.Abstracts
{
    public interface IXMLService
    {
        Task<string> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}