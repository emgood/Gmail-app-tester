using GmailAppCode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitBrowser()
        {
            Driver.Init();
        }

        //Note: possibility to pass username and password strings parameters,  into logAs and WithPassword methods, in respectively.
        [TestMethod]
        public void CanLoginIntoGmail()
        {
            LoginPage.GoTo();
            LoginPage.LogAs();
            LoginPage.WithPassword();
            LoginPage.Login();

            Assert.IsTrue(MenuBar.Exsist, "failed to login");
        }

        [TestMethod]
        //Note:possibility to pass email address, and message body string parameters into Create.
        public void CanCreateNewPost()
        {
            MenuBar.NewPost.Create();
            Assert.IsTrue(MenuBar.NewPost.sentPost, "failed to sent a new post");
        }

        [TestMethod]
        public void CanLogoutFromGmail()
        {
            MenuBar.LogOut.Submit();
        }

        [TestMethod]
        public void CloseBrowser()
        {
            Driver.Close();
        }
    }
}