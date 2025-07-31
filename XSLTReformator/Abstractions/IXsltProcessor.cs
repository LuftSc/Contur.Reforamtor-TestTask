namespace XSLTReformator.Abstractions
{
    public interface IXsltProcessor
    {
        Task<string> TransformAsync(string xmlDataPath, string xsltPath, CancellationToken cancellationToken = default);
    }
}