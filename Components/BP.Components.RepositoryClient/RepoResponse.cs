namespace BP.Components.RepositoryClient
{
    public class RepoResponse<T>
    {
        public RepoResponse(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            Success = success;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Success { get; }
        public T Response { get; }
        public HttpResponseMessage HttpResponseMessage { get; }

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
