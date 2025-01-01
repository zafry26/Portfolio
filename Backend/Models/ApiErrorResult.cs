using System.Collections.Generic;

namespace Backend.Models;

public class ApiErrorResult
{
    public IEnumerable<ApiError> Errors { get; set; }

    public ApiErrorResult(int code, string header, string message)
    {
        Errors = new List<ApiError>
        {
            new()
            {
                Code = code,
                Header = header,
                Message = message,
            },
        };
    }
}