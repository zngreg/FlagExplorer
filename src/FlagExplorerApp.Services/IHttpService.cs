namespace FlagExplorerApp.FlagExplorerApp.Services
{
    public interface IHttpService
    {
        Task<T?> GetAsync<T>(string url);
    }
}