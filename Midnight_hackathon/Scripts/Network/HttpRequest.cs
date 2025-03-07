using System;

public class HttpRequest<T>
{
    public string Url { get; set; }
    public string ContentType { get; set; }
    public string RequestBody { get; set; }
    public Action<T> OnComplete { get; set; }
}