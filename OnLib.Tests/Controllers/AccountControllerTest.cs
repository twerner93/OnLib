using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnLib;
using OnLib.Controllers;
using System.Threading.Tasks;

namespace OnLib.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        //[ClassCleanup]
        //public void RemoveTestUsers()
        //{

        //}

        [TestMethod]
        public void RegisterTest()
        {
            AccountController controller = new AccountController();

            Assert.IsTrue(controller.UserExists("TestBenutzer123"));
        }
    }
}
