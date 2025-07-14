namespace TransferYape.Application.Utils;
public sealed class CustomValidationDetails
{
    public string Message { get; set; } = "Validation failed.";
    public List<ValidationError> Errors { get; set; } = new();
}

public sealed class ValidationError
{
    public string Field { get; set; }
    public string Error { get; set; }
}