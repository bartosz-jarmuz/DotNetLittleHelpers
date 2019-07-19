using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DotNetLittleHelpers.Tests
{
    public class BaseClass
    {
        private int PrivateIntProp { get; set; } = 1;
        internal int InternalIntProp { get; set; } = 2;
        public int PublicIntProp { get; set; } = 3;

#pragma warning disable 414
        private double privateDoubleField = 1.1;
#pragma warning restore 414
        internal double internalDoubleField =2.2;
        public double publicDoubleField =3.3;
    }

    public class OtherClass
    {
        private string PrivateStringProp { get; set; } = "a";
        internal string InternalStringProp { get; set; } = "b";
        public string PublicStringProp { get; set; } = "c";
    }

    public class DerivedClass : BaseClass
    {
       

        private OtherClass privateSubObj = new OtherClass();

        internal OtherClass InternalSubObj = new OtherClass();

        public OtherClass PublicSubObj = new OtherClass();

        private void ThrowingMethod(int i)
        {
            throw new DivideByZeroException($"I don't care about your param [{i}]");
        }

        private int MathMethod(int a, int b)
        {
            return a - b;
        }

        private int MathMethod(int a, int b, string dummy)
        {
            return a - b;
        }

        private int MathMethod(int a, string dummy, int b)
        {
            return a + b;
        }

        private void AVoidMethod(OtherClass oth)
        {
            oth.PublicStringProp = "done";
        }

    }


    [TestFixture]
    public class HiddenMembersAccessTests
    {
        [Test]
        public void TestProps_MethodAccess_ComplexOverloads_Ok()
        {
            var derived = new DerivedClass();

            int value = derived.InvokeMethod<int>("MathMethod", 10, 5, "boo");
            Assert.AreEqual(5, value);

            value = derived.InvokeMethod<int>("MathMethod", 10, "boo", 5);
            Assert.AreEqual(15, value);
        }


        [Test]
        public void TestProps_MethodAccess_Ok()
        {
            var derived = new DerivedClass();

            int value = derived.InvokeMethod<int>("MathMethod", 10, 5);
            Assert.AreEqual(5,value);

            value = derived.InvokeMethod<int>("MathMethod", 5,10);
            Assert.AreEqual(-5, value);
        }

        [Test]
        public void TestProps_MethodAccess_Throw()
        {
            var derived = new DerivedClass();
            try
            {

                int value = derived.InvokeMethod<int>("ThrowingMethod", 12);
                Assert.Fail("Error expected.");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DivideByZeroException));
                Assert.AreEqual("I don't care about your param [12]", ex.Message);
            }
        }

        [Test]
        public void TestProps_MethodAccess_WrongParams()
        {
            var derived = new DerivedClass();
            try
            {

                int value = derived.InvokeMethod<int>("MathMethod", 12, "aaa", true);
                Assert.Fail("Error expected. ");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Found 3 [MathMethod] method(s) on [DotNetLittleHelpers.Tests.DerivedClass], however none matches the provided arguments: [Int32,String,Boolean]", ex.Message);
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));

            }

            try
            {

                 derived.InvokeMethod("AVoidMethod", 12, "aaa", true);
                Assert.Fail("Error expected. ");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Found 1 [AVoidMethod] method(s) on [DotNetLittleHelpers.Tests.DerivedClass], however none matches the provided arguments: [Int32,String,Boolean]", ex.Message);
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));

            }
        }


        [Test]
        public void TestProps_Void_Ok()
        {
            var derived = new DerivedClass();
            var param = new OtherClass();

            Assert.AreNotEqual("done", param.PublicStringProp);

            derived.InvokeMethod("AVoidMethod", param);

            Assert.AreEqual("done", param.PublicStringProp);
        }



        [Test]
        public void TestProps_GetValue_WrongType()
        {
            var baseClass = new BaseClass();
            try
            {

                string str = baseClass.GetPropertyValue<string>("PrivateIntProp");
                Assert.Fail("Error expected. Wrong cast, duh");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Unable to cast object of type 'System.Int32' to type 'System.String'.", ex.Message);
            }
        }


        [Test]
        public void TestChaining_NestedType()
        {
            var derived = new DerivedClass();
            string str = derived.GetFieldValue<OtherClass>("privateSubObj").GetPropertyValue<string>("PrivateStringProp");
            Assert.AreEqual("a", str);

            var oth = new OtherClass();
            oth.SetPropertyValue("PrivateStringProp", "aaa");
            derived.SetFieldValue("privateSubObj", oth);
            
            
            str = derived.GetFieldValue<OtherClass>("privateSubObj").GetPropertyValue<string>("PrivateStringProp");
            Assert.AreEqual("aaa", str);


        }

        [Test]
        public void TestProps_SetValue()
        {
            var baseClass = new BaseClass();
            baseClass.SetPropertyValue("PrivateIntProp", 44);
            baseClass.SetPropertyValue("InternalIntProp", 55);
            baseClass.SetPropertyValue("PublicIntProp", 66);

            int number = baseClass.GetPropertyValue<int>("PrivateIntProp");

            Assert.AreEqual(44, number);
            Assert.AreEqual(55, baseClass.GetPropertyValue<int>("InternalIntProp"));
            Assert.AreEqual(66, baseClass.GetPropertyValue<int>("PublicIntProp"));

        }

        [Test]
        public void TestFields_SetValue()
        {
            var baseClass = new BaseClass();
            baseClass.SetFieldValue("privateDoubleField", 4.4);
            baseClass.SetFieldValue("internalDoubleField", 5.5);
            baseClass.SetFieldValue("publicDoubleField", 6.6);

            double number = baseClass.GetFieldValue<double>("privateDoubleField");

            Assert.AreEqual(4.4, number);
            Assert.AreEqual(5.5, baseClass.GetFieldValue<double>("internalDoubleField"));
            Assert.AreEqual(6.6, baseClass.GetFieldValue<double>("publicDoubleField"));

        }

    }
}
