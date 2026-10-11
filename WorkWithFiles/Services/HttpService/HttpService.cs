using Microsoft.AspNetCore.WebUtilities;
using WorkWithFiles.Models;

namespace WorkWithFiles.Services.HttpService
{
    public class HttpService<T> : IHttpService<T>
    {
        private HttpClient _httpClient;

        public HttpService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public async Task<List<T>?> GetAll(string url, CancellationToken cancellationToken, PhotoFilter? filter = null)
        {
            Dictionary<string, string> query = new();
            
            if (filter != null)
            {
                query["_page"] = filter.Page.ToString();
                query["_limit"] = filter.Limit.ToString();
                query["search"] = filter.Search;
            }

            var requestUrl = QueryHelpers.AddQueryString(url, query); 

            var reponse = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (reponse.IsSuccessStatusCode)
            {
                return await reponse.Content.ReadFromJsonAsync<List<T>>(cancellationToken);
            }

            return null;
        }
    }
}
