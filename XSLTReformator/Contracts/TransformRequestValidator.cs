using FluentValidation;

namespace XSLTReformator.Contracts
{
    public class TransformRequestValidator : AbstractValidator<TransformRequest>
    {
        public TransformRequestValidator()
        {
            RuleFor(x => x.XmlFileName)
                .NotEmpty()
                .Must(BeValidXmlFileName).WithMessage("Invalid XML file name");
        }
        private bool BeValidXmlFileName(string fileName)
            => !string.IsNullOrEmpty(fileName) 
            && fileName.EndsWith(".xml") 
            && !fileName.Contains("..");
    }
}
