using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BP.Components.RepositoryClient
{
    public class RepoClient : IRepoClient
    {
        private readonly HttpClient httpClient;

        public RepoClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RepoResponse<TResult>> GetAsync<TResult>(string route)
        {
            var response = await httpClient.GetAsync(route);
            TResult result = default(TResult);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<TResult>();

            return new RepoResponse<TResult>(result, response.IsSuccessStatusCode, response);
        }

        public async Task<RepoResponse<TResult>> PostAsync<T, TResult>(string route, T body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(route, content);
            TResult result = default(TResult);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<TResult>();

            return new RepoResponse<TResult>(result, response.IsSuccessStatusCode, response);
        }

        public async Task<RepoResponse<object>> PostAsync<T>(string route, T body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(route, content);
            return new RepoResponse<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<RepoResponse<object>> Put<T>(string route, T body)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PutAsync(route, content);
            return new RepoResponse<object>(null, response.IsSuccessStatusCode, response);
        }

        public string BuildQuery(string route, params object[] args)
        {
            if (args.Length % 2 != 0)
                throw new ArgumentException($"Se debe proporcionar un número par de args, ya que se consideran key-value pairs");
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i = i+2)
            {
                queryParams.Add(args[i].ToString(), args[i + 1]?.ToString());
            }
            var query = QueryHelpers.AddQueryString(route, queryParams);
            return query;
        }
    }
}
