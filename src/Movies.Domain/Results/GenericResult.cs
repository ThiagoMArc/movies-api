using System.Net;

namespace Movies.Domain.Results;
public class GenericResult
{
    public GenericResult(bool success, HttpStatusCode status, object? data)
    {
        Success = success;
        Status = status;
        Data = data;
    }
    public bool Success { get; private set; }
    public HttpStatusCode Status { get; private set; }  
    public object? Data { get; private set; }   
}
