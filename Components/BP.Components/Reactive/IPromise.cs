using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Reactive
{
    /// <summary>
    /// Represents the eventual completition (or failure) of an asynchronous operation and its resulting value.
    /// <br/><br/>
    /// This interface is based on <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise">JavaScript Promises</see><br/>
    /// Essentially, this implementation of promises allow to await the result of an asynchronous operation passed via callback. It not allow neither Chaining nor
    /// nesting due to strong typed system type of C# unlike JS
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    public interface IPromise<T, TException> where TException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TResultException"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IPromise<TResult, TResultException> Then<TResult, TResultException>(Action<Action<TResult>, Action<TResultException>> callback)
            where TResultException : Exception;


        /// <summary>
        /// Gets the <see cref="Task{T}"/> returned by the internal <see cref="TaskCompletionSource{T}"/>
        /// </summary>
        public Task<T> Task { get; }

        /// <summary>
        /// Gets the result value of the internal <see cref="TaskCompletionSource{T}.Task"/>
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Gets the <see cref="System.Exception"/> resulted of rejected the current <see cref="IPromise{T, TException}"/>
        /// </summary>
        public TException Exception { get; }
    }

    public interface IPromise<T> : IPromise<T, Exception>
    {

    }
}
