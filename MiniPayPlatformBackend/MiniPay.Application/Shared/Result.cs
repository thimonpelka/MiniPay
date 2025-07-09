public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
	public int ErrorCode { get; set; }
    public T? Data { get; set; }

    public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
    public static Result<T> Fail(string errorMessage, int errorCode) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage, ErrorCode = errorCode };
}
