namespace XSLTReformator.Configurations
{
    public class XmlFileSettings
    {
        public string BasePath { get; set; } = string.Empty;
        public string BaseDeclarationString { get; set; } = string.Empty;
        public string[] AllowedFiles { get; set; } = [];
    }
}
