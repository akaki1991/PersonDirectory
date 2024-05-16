namespace PersonDirectory.Shared;

public class AppException(ErrorCodes code) : Exception(code.ToString())
{
    public ErrorCodes ErrorCode { get; set; } = code;
}
