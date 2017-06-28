using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;

namespace Player.Test
{
    /// <summary>
    /// Summary description for ProgramTest
    /// </summary>
    [TestClass]
    public class ProgramTest
    {

        [TestMethod]
        public void ProgramHandlesExceptionsThrownByConnectionMethod()
        {
            //  Arrange
            Program program = new Program();
            var connection = A.Fake<Connection>();
            A.CallTo(() => connection.Connect()).Throws(new FakeException());
            //  Act
            
            //  Assert

        }
    }
}
