using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace BingoGame.Common
{
    public class UniTaskWithCancellationToken<T>
    {
        private UniTaskCompletionSource<T> _source;
        private CancellationTokenRegistration? _tokenRegistration;
        private T _result;
        public bool IsDisposed { get; private set; }

        public UniTaskWithCancellationToken(CancellationToken token)
        {
            _source = new UniTaskCompletionSource<T>();
            _tokenRegistration = token.Register(Cancel, true);
        }

        public void Complete(T result)
        {
            if (IsDisposed)
            {
                return;
            }

            _result = result;
            _source.TrySetResult(_result);
            Dispose();
        }

        public void Cancel()
        {
            if (IsDisposed)
            {
                return;
            }

            _source.TrySetCanceled();
            Dispose();
        }

        public void TrySetException(Exception exception)
        {
            if (IsDisposed)
            {
                return;
            }

            _source.TrySetException(exception);
            Dispose();
        }

        public UniTask<T> GetTask()
        {
            return IsDisposed
                ? UniTask.FromResult(_result)
                : _source.Task;
        }

        public UniTask<T>.Awaiter GetAwaiter()
        {
            return GetTask().GetAwaiter();
        }

        private void Dispose()
        {
            _source = null;
            _tokenRegistration?.Dispose();
            _tokenRegistration = null;
            IsDisposed = true;
        }
    }
}