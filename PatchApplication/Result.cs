using System.Text;

namespace PatchApplication;

public class Result
{
    public bool IsSucceed { get; private init; }
    public string[] Errors { get; private init; } = [];
    public string Message { get; private init; } = string.Empty;

    public static Result Success(string message) => new()
    {
        IsSucceed = true,
        Message = message
    };

    public static Result Error(params string[] errors) => new()
    {
        Errors = errors,
        IsSucceed = false
    };

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        if (IsSucceed)
        {
            stringBuilder.AppendLine($"SUCCESS: {Message}");
        }
        else
        {
            foreach (var error in Errors)
            {
                stringBuilder.AppendLine($"ERROR: {error}");       
            }
        }

        return stringBuilder.ToString();
    }
}