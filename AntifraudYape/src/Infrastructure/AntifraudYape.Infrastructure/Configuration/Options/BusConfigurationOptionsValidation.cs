using Microsoft.Extensions.Options;

namespace AntifraudYape.Infrastructure.Configuration.Options;

public class BusConfigurationOptionsValidation : IValidateOptions<BusConfigurationOptions>
{
    public ValidateOptionsResult Validate(string? name, BusConfigurationOptions options)
    {
        string? validationError = null;
        if (string.IsNullOrEmpty(options.RabbitMQConnectionString))
        {
            validationError = $"Error Settings in: {nameof(BusConfigurationOptions)} {nameof(options.RabbitMQConnectionString)} is required";
        }
        return validationError is not null ? ValidateOptionsResult.Fail(validationError) : ValidateOptionsResult.Success;
    }
}