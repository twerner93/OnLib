using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnLib;
using OnLib.Controllers;

namespace OnLib.Tests.Controllers
{
    [TestClass]
    public class TitelControllerTest
    {
        [TestMethod]
        public void TitelIndexAllTest()
        {
            TitelController controller = new TitelController();
            ViewResult result = controller.Index("") as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
