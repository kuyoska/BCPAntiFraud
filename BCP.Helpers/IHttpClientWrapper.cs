namespace BCP.Helpers
{
    public interface IHttpClientWrapper
    {
        Task<T> Get<T>(string url);
        Task<T> PostRequest<T>(string apiUrl, T postObject);
        Task PutRequest<T>(string apiUrl, T putObject);
    }
}
