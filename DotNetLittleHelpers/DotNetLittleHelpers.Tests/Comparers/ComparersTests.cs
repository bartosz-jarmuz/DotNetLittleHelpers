using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetLittleHelpers.Tests
{
    [TestClass()]
    public class ComparersTests
    {

        bool Evaluate<T>(Func<T, T, bool> @operator, T arg1, T arg2) where T : IComparable<T>
        {
            return (@operator(arg1, arg2));
        }

        [TestMethod()]
        public void TestComparerOperator()
        {
            var smallerDate = new DateTime(1000, 1, 1);
            var largerDate = new DateTime(1999, 9, 9);
            Assert.IsTrue(this.Evaluate(ComparisonOperator.Greater<DateTime>(), largerDate, smallerDate));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Greater<DateTime>(), smallerDate, largerDate));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Greater<DateTime>(), smallerDate, largerDate));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(), 2, 1));
            Assert.IsTrue(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(), 2, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.GreaterOrEqual<int>(), 1, 2));


            Assert.IsTrue(this.Evaluate(ComparisonOperator.Equal<int>(), 2, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Equal<int>(), 1, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Equal<int>(), 2, 1));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.Smaller<int>(), 1, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Smaller<int>(), 2, 1));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.Smaller<int>(), 2, 2));

            Assert.IsTrue(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(), 1, 2));
            Assert.IsTrue(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(), 2, 2));
            Assert.IsFalse(this.Evaluate(ComparisonOperator.SmallerOrEqual<int>(), 2, 1));
        }
    }
}