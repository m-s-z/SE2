using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.Integration
{
    [TestClass]
    public class EndpointValidationTest
    {
        [TestMethod]
        public void EndpointValidationTestAcceptsValidIpAddress()
        {
            //before
            string address = "127.0.0.1";
            string port = "8000";
            Tuple<string,int> ans = Messages.EndpointValidation.validate(address, port);
            Assert.AreEqual(address, ans.Item1);
            Assert.AreEqual(8000, ans.Item2);
        }
        [TestMethod]
        public void EndpointValidationTestCorrectsToDefaultEndpointWhenIpAddressIsWrong()
        {
            //before
            string address = "wrong string";
            string port = "8000";
            Tuple<string, int> ans = Messages.EndpointValidation.validate(address, port);
            Assert.AreEqual("127.0.0.1", ans.Item1);
            Assert.AreEqual(8000, ans.Item2);
        }
        [TestMethod]
        public void EndpointValidationTestCorrectsToDefaultEndpointWhenPortIsAString()
        {
            //before
            string address = "wrong string";
            string port = "this is not a number";
            Tuple<string, int> ans = Messages.EndpointValidation.validate(address, port);
            Assert.AreEqual("127.0.0.1", ans.Item1);
            Assert.AreEqual(8000, ans.Item2);
        }
    }
}
