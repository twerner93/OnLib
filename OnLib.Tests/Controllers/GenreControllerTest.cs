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
    public class GenreControllerTest
    {
        [TestMethod]
        public void GenreIndex()
        {
            GenreController controller = new GenreController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GenreCreate()
        {
            GenreController controller = new GenreController();
            controller.Create(new Models.Genre { Name = "TestGenre" });
            Assert.IsTrue(controller.Exists("TestGenre"));
        }
    }
}
