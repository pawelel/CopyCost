

namespace CopyCost.Extensions;

public class OperationResult
{
    public bool IsSuccess { get; private init; }
    public Dictionary<string, string> Errors { get; set; } = new();
    public string Message { get; private set; } = string.Empty;

    public static OperationResult Failed(string key, string value)
        => new() { IsSuccess = false, Errors = new Dictionary<string, string> { { key, value } } };

    public static OperationResult Failed(Dictionary<string, string> errors)
        => new() { IsSuccess = false, Errors = errors };

    public static OperationResult Success(string message)
        => new() { IsSuccess = true , Message = message };
}