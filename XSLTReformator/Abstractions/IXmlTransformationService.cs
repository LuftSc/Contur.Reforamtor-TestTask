using XSLTReformator.Models;

namespace XSLTReformator.Abstractions
{
    public interface IXmlTransformationService
    {
        Task<TransformationResult> TransformAndUpdateSourceAsync(string xmlFileName, CancellationToken cancellationToken = default);
    }
}