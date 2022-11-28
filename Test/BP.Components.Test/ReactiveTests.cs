using BP.Components.Reactive;
using System.Diagnostics;

namespace BP.Components.Test
{
    public class ReactiveTests
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private const int _defaultDelay = 100;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task AsynchronousResolve()
        {
            var promise = new Promise<int>(async (resolve, reject) =>
            {
                await Task.Delay(_defaultDelay);
                resolve(0);
            });

            Assert.Multiple(() =>
            {
                Assert.That(promise.Task.Status, Is.LessThanOrEqualTo(TaskStatus.Running));
                Assert.That(promise.Task.IsCompleted, Is.False);
            });

            await promise.Task;
            Assert.That(promise.Task.IsCompleted, Is.True);
        }

        [Test]
        public void SynchronousReject()
        {
            var promise = new Promise<int>((resolve, reject) =>
            {
                Task.Delay(_defaultDelay).Wait();
                reject(new Exception());
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        public void AsynchronousReject()
        {
            var promise = new Promise<int>(async (resolve, reject) =>
            {
                await Task.Delay(_defaultDelay);
                reject(new Exception());
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        public void SynchronousUnhandledException()
        {
            Promise<int> promise = null;
            Assert.DoesNotThrow(() =>
            {
                promise = new Promise<int>((resolve, reject) =>
                {
                    Task.Delay(_defaultDelay).Wait();
                    throw new Exception("Unhandled exception");
                });
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        public void AsynchronousUnhandledException()
        {
            Promise<int> promise = null;
            Assert.DoesNotThrowAsync(async () =>
            {
                promise = new Promise<int>(async (resolve, reject) =>
                {
                    try
                    {
                        await Task.Delay(_defaultDelay);
                        throw new Exception("Unhandled exception");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        public void SynchronousRejectOnCatch()
        {
            Promise<int> promise = null;
            Assert.DoesNotThrow(() =>
            {
                promise = new Promise<int>((resolve, reject) =>
                {
                    try
                    {
                        Task.Delay(_defaultDelay).Wait();
                        throw new Exception("Unhandled exception");
                    }
                    catch (Exception ex)
                    {
                        reject(ex);
                    }

                });
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        public void AsynchronousRejectOnCatch()
        {
            Promise<int> promise = null;
            Assert.DoesNotThrow(() =>
            {
                promise = new Promise<int>(async (resolve, reject) =>
                {
                    try
                    {
                        await Task.Delay(_defaultDelay);
                        throw new Exception("Unhandled exception");
                    }
                    catch (Exception ex)
                    {
                        reject(ex);
                    }

                });
            });

            Assert.ThrowsAsync<Exception>(async () =>
            {
                await promise.Task;
            });
        }

        [Test]
        [TestCase(2, 5)]
        public async Task AsynchronousResolveThenResolveSum(int firstOperand, int secondOperand)
        {
            int value = 0;
            var promise = new Promise<int>(async (resolve, reject) =>
            {
                await Task.Delay(100);
                resolve(firstOperand);
            });

            //Check first promise is not finished
            Assert.Multiple(() =>
            {
                Assert.That(promise.Task.Status, Is.LessThanOrEqualTo(TaskStatus.Running));
                Assert.That(promise.Task.IsCompleted, Is.False);
            });

            var promise2 = promise.Then<int, Exception>(async (result, then) =>
            {
                await Task.Delay(100);
                result += secondOperand;
                then.Resolve(result);
            });

            //Check second promise is not finished
            Assert.Multiple(() =>
            {
                Assert.That(promise2.Task.Status, Is.LessThanOrEqualTo(TaskStatus.Running));
                Assert.That(promise2.Task.IsCompleted, Is.False);
            });

            value = await promise2.Task;
            // Both task must be completed and the result is correct
            Assert.Multiple(() =>
            {
                Assert.That(promise.Task.IsCompleted, Is.True);
                Assert.That(promise2.Task.IsCompleted, Is.True);
                Assert.That(value, Is.EqualTo(firstOperand + secondOperand));
            });
        }


    }
}