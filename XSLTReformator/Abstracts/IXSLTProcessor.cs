namespace XSLTReformator.Abstracts
{
    public interface IXSLTProcessor
    {
        Task<string> TransformAsync(string xmlDataPath, string xsltPath, CancellationToken cancellationToken = default);
    }
}