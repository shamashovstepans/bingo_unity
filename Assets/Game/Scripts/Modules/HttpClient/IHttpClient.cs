using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scenes.HttpClient
{
    public interface IHttpClient
    {
        UniTask<TResponse> Get<TResponse>(
            string url,
            CancellationToken cancellationToken)
            where TResponse : Response;

        UniTask<TResponse> Post<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken)
            where TResponse : Response
            where TRequest : Request;
    }
}