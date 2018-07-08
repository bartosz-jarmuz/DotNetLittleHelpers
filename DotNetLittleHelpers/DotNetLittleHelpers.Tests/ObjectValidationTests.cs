namespace DotNetLittleHelpers.Tests
{
    #region Using
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    #endregion

    [TestFixture]
    public class ObjectValidationTests
    {
        [Test]
        public void CheckArgumentNullTest_NotNull()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject());
            list.CheckArgumentNull("list");
            new TestObject().CheckArgumentNull("obj");
        }

        [Test]
        public void CheckArgumentNullTest_Null()
        {
            List<TestObject> list = null;
            try
            {
                list.CheckArgumentNull("list");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Null parameter passed to method [CheckArgumentNullTest_Null].\r\nParameter name: list", ex.Message);
                return;
            }

            Assert.Fail("Exception expected");
        }

        [Test]
        public void ValidatorExtensionTests_Error()
        {
            string input = "THEVALUE";
            string nullInput = null;
            Assert.That(() =>
                    input.Validate(() => int.TryParse(input, out int _), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));
            Assert.That(() =>
                    nullInput.Validate(() => int.TryParse(nullInput, out int _), () => throw new InvalidOperationException($"Error to throw: [{nullInput}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []"));

            Assert.That(() =>
                    input.Validate(() => int.TryParse(input, out int _), new InvalidOperationException("Error to throw")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw"));
            Assert.That(() =>
                    nullInput.Validate(() => int.TryParse(nullInput, out int _), new InvalidOperationException($"Error to throw: [{nullInput}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []"));

            Assert.That(() =>
                    input.Validate(() => this.MyOwnValidation("asd", 3, 2M, null), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));
        }

        [Test]
        public void ValidatorExtensionTests_TestOutput()
        {
            string input = "3";
            int output = 0;
            input.Validate(() => int.TryParse(input, out output), () => throw new InvalidOperationException("Error to throw"));
            Assert.AreEqual(3, output);

            input = "1";
            input.Validate(() => int.TryParse(input, out output), new InvalidOperationException("Error to throw"));
            Assert.AreEqual(1, output);
        }

        [Test]
        public void ValidatorTests_Error()
        {
            string input = "THEVALUE";
            string nullInput = null;
            Assert.That(() =>
                    ObjectValidator.Validate(() => int.TryParse(input, out int _), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));
            Assert.That(() =>
                    ObjectValidator.Validate(() => int.TryParse(nullInput, out int _), () => throw new InvalidOperationException($"Error to throw: [{nullInput}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []"));

            Assert.That(() =>
                    ObjectValidator.Validate(() => int.TryParse(input, out int _), new InvalidOperationException("Error to throw")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw"));
            Assert.That(() =>
                    ObjectValidator.Validate(() => int.TryParse(nullInput, out int _), new InvalidOperationException($"Error to throw: [{nullInput}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []"));

            Assert.That(() =>
                    ObjectValidator.Validate(() => this.MyOwnValidation("asd", 3, 2M, null), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));
        }

        [Test]
        public void ValidatorTests_TestOutput()
        {
            string input = "3";
            int output = 0;
            ObjectValidator.Validate(() => int.TryParse(input, out output), () => throw new InvalidOperationException("Error to throw"));
            Assert.AreEqual(3, output);

            input = "1";
            ObjectValidator.Validate(() => int.TryParse(input, out output), new InvalidOperationException("Error to throw"));
            Assert.AreEqual(1, output);
        }

        private bool MyOwnValidation(string paramOne, int param2, decimal param3, Dictionary<bool, int> pram4)
        {
            return pram4 != null;
        }

        private class TestObject
        {
        }
    }
}