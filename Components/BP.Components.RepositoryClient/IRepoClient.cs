using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.RepositoryClient
{
    public interface IRepoClient
    {
        Task<RepoResponse<TResult>> GetAsync<TResult>(string route);
        Task<RepoResponse<TResult>> PostAsync<T, TResult>(string route, T body);
        Task<RepoResponse<object>> PostAsync<T>(string route, T body);
        Task<RepoResponse<object>> Put<T>(string route, T body);
        string BuildQuery(string route, params object[] args);

    }
}
