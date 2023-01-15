using System.Diagnostics;
using System.Net;

namespace BP.Components.RepositoryClient
{
    [DebuggerDisplay("{StatusCode}; {Response}")]
    public class RepoResponse<T>
    {
        public RepoResponse()
        {

        }
        public RepoResponse(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            Success = success;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Success { get; }
        public T Response { get; }
        public HttpResponseMessage HttpResponseMessage { get; }
        public HttpStatusCode StatusCode => HttpResponseMessage.StatusCode;

        /// <summary>
        /// Equivale a <see cref="HttpResponseMessage.Content.ReadAsStringAsync()"/>
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadAsStringAsync()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
