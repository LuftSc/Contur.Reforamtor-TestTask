namespace XSLTReformator.Abstractions
{
    public interface IXmlModificationService
    {
        Task<string> AddSalaryToEmployees(string sourceXmlString
            , CancellationToken cancellationToken = default);
        double ExtractTotalAmountFromEmployeesXml(string employeeXML);
        Task<string> WriteTotalAmountToSourceFileAsync(double totalAmount
            , string xmlDataPath
            , CancellationToken cancellationToken = default);
    }
}