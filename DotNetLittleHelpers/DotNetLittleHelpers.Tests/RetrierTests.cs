using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
// ReSharper disable ConsiderUsingConfigureAwait

namespace DotNetLittleHelpers.Tests
{
    [TestFixture]
    public class RetrierTests
    {
        public class ErrorThrower
        {
            public int AttemptNumber => this.attemptNumber;
            private int attemptNumber;

            private readonly int numberOfAttemptsToSuccess;
            private readonly Action exceptionAction;

            public ErrorThrower(int numberOfAttemptsToSuccess, Action exceptionAction)
            {
                this.numberOfAttemptsToSuccess = numberOfAttemptsToSuccess;
                this.exceptionAction = exceptionAction;
            }
            public string Work()
            {
                this.attemptNumber++;
                if (this.attemptNumber < this.numberOfAttemptsToSuccess)
                {
                    this.exceptionAction();
                }

                return "ok";
            }

            public async Task<string> WorkAsync()
            {
                await Task.Delay(0);
                this.attemptNumber++;
                if (this.attemptNumber < this.numberOfAttemptsToSuccess)
                {
                    this.exceptionAction();
                }

                return "ok";
            }

            public async Task WorkVoidAsync()
            {
                await Task.Delay(0);
                this.attemptNumber++;
                if (this.attemptNumber < this.numberOfAttemptsToSuccess)
                {
                    this.exceptionAction();
                }

            }

            public void WorkVoid()
            {
                this.attemptNumber++;
                if (this.attemptNumber < this.numberOfAttemptsToSuccess)
                {
                    this.exceptionAction();
                }

            }


        }



        [Test]
        public async Task Test_Async_AllOk()
        {
            var thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            var value = await Retrier.RetryAsync(() => thrower.Work(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual("ok", value);

            thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            await Retrier.RetryAsync(() => thrower.WorkVoid(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, thrower.AttemptNumber);


            thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            await Retrier.RetryAsync(() => thrower.WorkVoid(), ex => ex.Message == "Boo", TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, thrower.AttemptNumber);
        }

        [Test]
        public void Test_Sync_AllOk()
        {
            var thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            var value = Retrier.Retry(() => thrower.Work(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual("ok", value);

            thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            Retrier.Retry(() => thrower.WorkVoid(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, thrower.AttemptNumber);

            thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            Retrier.Retry(() => thrower.WorkVoid(), ex => ex.Message == "Boo", TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, thrower.AttemptNumber);
        }

        [Test]
        public async Task Test_Async_ExceptionNotAllowedOk()
        {
            var thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            try
            {
                await Retrier.RetryAsync(() => thrower.WorkVoid(), ex => ex.Message != "Boo", TimeSpan.FromMilliseconds(10));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(InvalidOperationException));
                Assert.IsTrue(ex.Message == "Boo");
                Assert.AreEqual(1, thrower.AttemptNumber);

            }
        }

        [Test]
        public void Test_Sync_ExceptionNotAllowedOk()
        {
            var thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            try
            {
                Retrier.Retry(() => thrower.WorkVoid(), ex => ex.Message != "Boo", TimeSpan.FromMilliseconds(10));
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(InvalidOperationException));
                Assert.IsTrue(ex.Message == "Boo");
                Assert.AreEqual(1, thrower.AttemptNumber);
            }
        }

        [Test]
        public async Task Test_Task_AllOkAsync()
        {
            var thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            string value = await Retrier.RetryTaskAsync(() => thrower.WorkAsync(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual("ok", value);

            thrower = new ErrorThrower(3, () => throw new InvalidOperationException("Boo"));
            Assert.AreEqual(0, thrower.AttemptNumber);
            await Retrier.RetryTaskAsync(() => thrower.WorkVoidAsync(), TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, thrower.AttemptNumber);
        }


        [Test]
        public async Task Test_Task_TooManyErrorsAsync()
        {
            var thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));

            try
            {
                await Retrier.RetryTaskAsync(() => thrower.WorkAsync(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

            try
            {
                thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));
                Assert.AreEqual(0, thrower.AttemptNumber);

                await Retrier.RetryTaskAsync(() => thrower.WorkVoidAsync(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

        }

        [Test]
        public async Task Test_Async_TooManyErrors()
        {
            var thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));

            try
            {
                await Retrier.RetryAsync(() => thrower.Work(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

            try
            {
                thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));
                Assert.AreEqual(0, thrower.AttemptNumber);

                await Retrier.RetryAsync(() => thrower.WorkVoid(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

        }


        [Test]
        public void Test_Sync_TooManyErrors()
        {
            var thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));

            try
            {
                 Retrier.Retry(() => thrower.Work(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

            try
            {
                thrower = new ErrorThrower(4, () => throw new InvalidOperationException("Boo"));
                Assert.AreEqual(0, thrower.AttemptNumber);

                Retrier.Retry(() => thrower.WorkVoid(), TimeSpan.FromMilliseconds(10));
                Assert.Fail("Error expected");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), typeof(AggregateException));
                Assert.AreEqual(3, ((AggregateException)ex).InnerExceptions.Count);
                Assert.IsTrue(((AggregateException)ex).InnerExceptions.All(x => x.Message == "Boo"));

            }

        }
    }
}
