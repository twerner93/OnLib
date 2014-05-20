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
        //private static AccountController controller = new AccountController();

        [TestMethod]
        public void RegisterTest()
        {
            //arrange
            Models.RegisterViewModel model = new Models.RegisterViewModel
            {
                UserName = "TestBenutzer",
                Vorname = "Test",
                Nachname = "Benutzer",
                Email = "test@benutzer.de",
                Geburtstag = new DateTime(1800, 1, 1),
                Password = "Test123",
                ConfirmPassword = "Test123"
            };

           //act
            var controller = new AccountController();
            var result = controller.Register(model);


            
            //assert
            Assert.IsTrue(controller.UserExists("TestBenutzer123"));
        }

         [TestMethod]
         public void UserExists()
         {
             //arrange
             string UserName = "TestBenutzer";

             //act
             //bool exists = controller.UserExists(UserName);

             //assert
             Assert.IsTrue(false);
        }
    }
}
