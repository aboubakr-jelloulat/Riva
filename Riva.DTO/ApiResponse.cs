using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Riva.DTO;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    private static ApiResponse<T> _Create(bool succes, int statusCode, string message, 
        T? data = default, object? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = succes, StatusCode = statusCode, Message = message,
            Data = data, Errors = errors, Timestamp = DateTime.UtcNow
        };
    }


    public static ApiResponse<T> Ok(T data, string message) => 
        _Create(true, (int)HttpStatusCode.OK, message, data);

    public static ApiResponse<T> CreatedAt(T data, string message) =>
        _Create(true, (int)HttpStatusCode.Created, message, data);

    public static ApiResponse<T> NoContent(string message = "Operation completed successfully") =>
        _Create(true, (int)HttpStatusCode.NoContent, message);



    public static ApiResponse<T> NotFound(string message = "Resource not found") =>
        _Create(false, (int)HttpStatusCode.NotFound, message);

    public static ApiResponse<T> BadRequest(string message, object? errors = null) =>
        _Create(false, (int)HttpStatusCode.BadRequest, message, errors: errors);

    public static ApiResponse<T> Conflict(string message) =>
        _Create(false, (int)HttpStatusCode.Conflict, message);

    /*
        * Conflict() :  The request is valid but it can’t be completed because it conflicts with the current state of the server.
        * The client tries to create or update something BUT that action violates a rule or already exists
    */

    public static ApiResponse<T> Error(int statusCode, string message, object? errors = null) =>
        _Create(false, statusCode, message, errors:errors);

}
