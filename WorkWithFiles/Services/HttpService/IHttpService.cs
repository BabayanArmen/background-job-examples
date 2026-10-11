using WorkWithFiles.Models;

namespace WorkWithFiles.Services.HttpService
{
    public interface IHttpService<T>
    {
        public Task<List<T>?> GetAll(string url, CancellationToken cancellationToken, PhotoFilter? filter = null);
    }
}
