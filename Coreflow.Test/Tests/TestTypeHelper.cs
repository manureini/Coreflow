using Coreflow.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreflow.Test.Tests
{
    [TestClass]
    public class TestTypeHelper
    {


        [TestMethod]
        public void FindStringType()
        {
            var type = TypeHelper.SearchType("System.String");
            Assert.AreEqual(typeof(string), type);
            type = TypeHelper.SearchType("String");
            Assert.AreEqual(typeof(string), type);
        }

        [TestMethod]
        public void FindSimpleTypeStrArray()
        {
            var type = TypeHelper.SearchType("string[]");
            Assert.AreEqual(typeof(string[]), type);
        }

        [TestMethod]
        public void FindSimpleTypeStrNullable()
        {
            var type = TypeHelper.SearchType("int?");
            Assert.AreEqual(typeof(int?), type);
        }

        [TestMethod]
        public void FindListString() { 
            var type = TypeHelper.SearchType("System.Collections.Generic.List`1[[System.String]]");
            Assert.AreEqual(typeof(List<string>), type);
        }

        [TestMethod]
        public void FindListStringCode()
        {
            var type = TypeHelper.SearchType("System.Collections.Generic.List<System.String>");
            Assert.AreEqual(typeof(List<string>), type);
        }

        [TestMethod]
        public void FindListStringCodeSimple()
        {
            var type = TypeHelper.SearchType("List<string>");
            Assert.AreEqual(typeof(List<string>), type);
        }

        [TestMethod]
        public void FindIDictStringObj()
        {
            var name = typeof(IDictionary<string, object>).FullName;
            var type = TypeHelper.SearchType(name);
            Assert.AreEqual(typeof(IDictionary<string, object>), type);
        }

        [TestMethod]
        public void FindIDictStringObj2()
        {
            string fullname = typeof(IDictionary<,>).FullName;
            var type = TypeHelper.SearchType(fullname);
            Assert.AreEqual(typeof(IDictionary<,>), type);
        }


        [TestMethod]
        public void TypeStringToCode()
        {
            var type = TypeHelper.TypeNameToCode(typeof(string));
            Assert.AreEqual("global::System.String", type);
        }

        [TestMethod]
        public void TypeDictToCode()
        {
            var type = TypeHelper.TypeNameToCode(typeof(IDictionary<string, object>));
            Assert.AreEqual("global::System.Collections.Generic.IDictionary<global::System.String, global::System.Object>", type);
        }



    }
}
