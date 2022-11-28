using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Reactive
{
    public class Promise<T, TException> where TException : Exception //: IPromise<T, TException>
    {
        private TaskCompletionSource<T> _promise;
        private T _result;
        private TException _resultException;
        private readonly Action<T> resolve;
        private readonly Action<TException> reject;
        private PromiseState _state = PromiseState.Pending;


        public Promise(Action<Action<T>, Action<TException>> callback)
        {
            _state = PromiseState.Pending;
            _promise = new TaskCompletionSource<T>();

            resolve = result =>
            {
                _result = result;
                _state = PromiseState.Fullfill;
                _promise.TrySetResult(_result);
            };
            reject = exception =>
            {
                _resultException = exception;
                _state = PromiseState.Reject;
                _promise.TrySetException(_resultException);
            };

            try
            {
                callback.Invoke(resolve, reject);
            }
            catch (TException ex)
            {
                reject.Invoke(ex);
            }
        }

        public Promise()
        {
            _promise = new TaskCompletionSource<T>();
            resolve = result =>
            {
                _result = result;
                _state = PromiseState.Fullfill;
                _promise.TrySetResult(_result);
            };
            reject = exception =>
            {
                _resultException = exception;
                _state = PromiseState.Reject;
                _promise.TrySetException(_resultException);
            };
        }

        public Promise<T, TException> Resolve(T result)
        {
            resolve.Invoke(result);
            return this;
        }

        public Promise<T, TException> Reject(TException exception)
        {
            reject.Invoke(exception);
            return this;
        }


        public Promise<TResult, TResultException> Then<TResult, TResultException>(Action<T, Promise<TResult, TResultException>> callback)
            where TResultException : Exception
        {
            try
            {
                _promise.Task.Wait();
                var result = _promise.Task.Result;
                var promise = new Promise<TResult, TResultException>();
                callback.Invoke(result, promise);
                return promise;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Task<T> Task => _promise.Task;

        public T Result => _promise.Task.Result;
        public TException Exception => _resultException;



    }


    public class Promise<T> : Promise<T, Exception>
    {
        public Promise(Action<Action<T>, Action<Exception>> callback) : base(callback) { }
        //public Promise(Action<Action<Task<T>>, Action<Task<Exception>>> callback) : base(callback) { }
    }

    public static class Promise
    {
        public static Promise<T> Create<T>(Action<Action<T>, Action<Exception>> callback)
        {
            return new Promise<T>(callback);
        }
    }
}
