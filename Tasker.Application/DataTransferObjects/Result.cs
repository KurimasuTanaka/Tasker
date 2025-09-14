using System;

namespace Tasker.Application;

public class Result<T>
{
    public T Value = default!;
    public string ErrorMessage = string.Empty;
    public bool IsSuccess = true;

    public Result(T value, bool isSuccess = true, string errorMessage = "")
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new Result<T>(value);

    public static Result<T> Failure<T>(string errorMessage) => new Result<T>(default!, false, errorMessage);
}