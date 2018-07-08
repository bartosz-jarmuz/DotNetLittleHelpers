namespace DotNetLittleHelpers.Tests
{
    #region Using
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    #endregion

    [TestFixture]
    public class ObjectValidationTests_Try
    {
        private bool CustomValidationMethod(string param, bool? secondArgument)
        {
            if (secondArgument == null)
            {
                throw new InvalidOperationException("Expected error");
            }

            return true;
        }

        [Test]
        public void ValidatorExtension_NoError()
        {
            string input = "3";
            input.ValidateTry(() => this.CustomValidationMethod(input, true), () => throw new InvalidOperationException("Error to throw"));
            input.ValidateTry(() => this.CustomValidationMethod(input, true), (ex) => throw new InvalidOperationException("Error to throw", ex));
            input.ValidateTry(() => this.CustomValidationMethod(input, true), new InvalidOperationException("Error to throw"));
        }

        [Test]
        public void ValidatorTests_NoError()
        {
            string input = "3";
            ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, true), () => throw new InvalidOperationException("Error to throw"));
            ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, true), (ex) => throw new InvalidOperationException("Error to throw", ex));
            ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, true), new InvalidOperationException("Error to throw"));
        }

        [Test]
        public void ValidatorExtensionTests_Error()
        {
            string input = "THEVALUE";
            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));

            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), new InvalidOperationException("Error to throw")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw"));

            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), (ex) => throw new InvalidOperationException($"Error to throw: [{input}]", ex)),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]")
                    .And.InnerException.With.Message.EqualTo("Expected error"));
        }

        [Test]
        public void ValidatorExtensionTests_NullInput_Error()
        {
            string input = null;
            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []"));

            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), new InvalidOperationException("Error to throw")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw"));

            Assert.That(() =>
                    input.ValidateTry(() => this.CustomValidationMethod(input, null), (ex) => throw new InvalidOperationException($"Error to throw: [{input}]", ex)),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: []")
                    .And.InnerException.With.Message.EqualTo("The source object is null"));
        }

        [Test]
        public void ValidatorTests_Error()
        {
            string input = "THEVALUE";
            Assert.That(() =>
                    ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, null), () => throw new InvalidOperationException($"Error to throw: [{input}]")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]"));

            Assert.That(() =>
                    ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, null), new InvalidOperationException("Error to throw")),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw"));

            Assert.That(() =>
                    ObjectValidator.ValidateTry(() => this.CustomValidationMethod(input, null), (ex) => throw new InvalidOperationException($"Error to throw: [{input}]", ex)),
                Throws.InvalidOperationException.With.Message.EqualTo("Error to throw: [THEVALUE]")
                    .And.InnerException.With.Message.EqualTo("Expected error"));
        }



       
      
    }
}