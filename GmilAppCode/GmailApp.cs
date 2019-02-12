using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace GmailAppCode
{
    public class Driver
    {
        static internal IWebDriver instance;

        static public void Init()
        {
       
            instance = new ChromeDriver();
            instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public static void GoTo()
        {
            instance.Navigate().GoToUrl("http://www.google.co.il");
            var userNameField = instance.FindElement(By.CssSelector("input[name=\"q\"]"));
            userNameField.SendKeys("gmail sign in");
            userNameField.SendKeys(Keys.Enter);

            var gmailLoginPage = Driver.instance.FindElement(By.CssSelector("a[href=\"https://accounts.google.com/ServiceLogin\"]"));
            gmailLoginPage.Click();
        }



        static public void Close()
        {
            instance.Close();
        }

        static internal bool IsElementExsist(By by)
        {
            try
            {
                instance.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

    static public class LoginPage
    {
        //defult value
        static private string userName = "ozozozozozoz945";

        static private string password = "ct,h kvmkhj1234";

        public static void GoTo()
        {
            Driver.instance.Navigate().GoToUrl("http://www.google.co.il");
            var userNameField = Driver.instance.FindElement(By.CssSelector("input[name=\"q\"]"));
            userNameField.SendKeys("gmail sign in");
            userNameField.SendKeys(Keys.Enter);

            var gmailLoginPage = Driver.instance.FindElement(By.CssSelector("a[href=\"https://accounts.google.com/ServiceLogin\"]"));
            gmailLoginPage.Click();

        }

        public static void LogAs()
        { }

        public static void LogAs(string userName)
        {
            LoginPage.userName = userName;
        }

        public static void WithPassword()
        { }

        public static void WithPassword(string password)
        {
            LoginPage.password = password;
        }

        public static void Login()
        {
            //entering user name
            var userNameField = Driver.instance.FindElement(By.Id("identifierId"));
            userNameField.SendKeys(userName);

            var next = Driver.instance.FindElement(By.Id("identifierNext"));
            next.Click();

            //entering password
            var passwordField = Driver.instance.FindElement(By.Name("password"));
            passwordField.SendKeys(password);

            var loginButttonPassword = Driver.instance.FindElement(By.Id("passwordNext"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
            ex.ExecuteScript("arguments[0].click();", loginButttonPassword);

            //valdiate
            By menuBarElement = By.CssSelector(".gb_zc");
            MenuBar.Exsist = Driver.IsElementExsist(menuBarElement);
        }
    }

    static public class MenuBar
    {
        static public bool Exsist { get; internal set; }

        static public class NewPost
        {
            //defult value
            static private string mailTo = "ozozozozozoz945@gmail.com";

            static private string msgBody = "Hello ZoomD";

            static private TimeSpan subjectTime;
            static public bool sentPost { get; internal set; }

            public static void Create()
            {
                Create(mailTo, msgBody);
            }

            public static void Create(string mailTo, string msgbody)
            {
                //Create a new post
                var newPostButton = Driver.instance.FindElement(By.ClassName("z0"));
                newPostButton.Click();

                //post data
                FilleMailAddress(mailTo);
                FillSubjectAddressWithCurrentDate();
                FillMessageBody(msgbody);

                //submit new post
                var submitNewPost = Driver.instance.SwitchTo().ActiveElement();
                submitNewPost.SendKeys(Keys.Enter);

                //validate sent item
                ValidateSentPost();
            }

            private static void FilleMailAddress(string mailTo)
            {
                var postForm = Driver.instance.FindElement(By.CssSelector("textarea[name=\"to\""));
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
                ex.ExecuteScript("arguments[0].click();", postForm);
                var to = Driver.instance.SwitchTo().ActiveElement();
                to.SendKeys(mailTo);
                to.SendKeys(Keys.Tab);
            }

            private static void FillSubjectAddressWithCurrentDate()
            {
                var subjectBox = Driver.instance.FindElement(By.CssSelector("input[name=\"subjectbox\"]"));
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
                ex.ExecuteScript("arguments[0].click();", subjectBox);
                subjectBox.Click();
                subjectTime = DateTime.Now.TimeOfDay;
                subjectBox.SendKeys(subjectTime.ToString());
            }

            private static void FillMessageBody(string msgBody)
            {
                var messageTextBox = Driver.instance.FindElement(By.CssSelector("div[role=\"textbox\"]"));
                messageTextBox.Click();
                messageTextBox.SendKeys(msgBody);
                messageTextBox.SendKeys(Keys.Tab);
            }

            //until here done -
             //C. code in github D. Azure pipeline

            private static void ValidateSentPost()
            {
                //case: try to access directly by view message popup link,
                //      else - searching on sent item (assumption: first sent post).

                By viewMessageLinkElement = By.Id("link_vsm");
                if (Driver.IsElementExsist(viewMessageLinkElement))
                {
                    var viewMessage = Driver.instance.FindElement(By.Id("link_vsm"));
                    viewMessage.Click();
                    sentPost = true;
                }
                else
                {
                    sentPost = SentItem.IsFirstSentPostSubjectContain(subjectTime);
                }
            }
        }

        public static class SentItem
        {
            internal static bool IsFirstSentPostSubjectContain(TimeSpan subjectTime)
            {
                var menuBar = Driver.instance.FindElement(By.CssSelector(".gb_zc"));
                menuBar.Click();

                var sentItem = Driver.instance.FindElement(By.CssSelector("div[data-tooltip=\"Sent\"]"));
                sentItem.Click();

                var firstSentPost = Driver.instance.FindElement(By.CssSelector("td[tabindex=\"-1\"]"));
                firstSentPost.Click();

                IWebElement BodySrcCode = Driver.instance.FindElement(By.TagName("body"));
                return BodySrcCode.Text.Contains(subjectTime.ToString()) ? true : false;
            }
        }

        public static class LogOut
        {
            public static void Submit()
            {
                //getting the Signout option button, using partial href link (end with: '/SignOutOptions?')
                string PartialLinkHref = "https://accounts.google.com/SignOutOptions?";
                var signOutOption = Driver.instance.FindElement(By.XPath("//a[contains(@href,'" + PartialLinkHref + "')]"));
                signOutOption.Click();

                //logout button
                string logout = "https://accounts.google.com/Logout?";
                var logoutButton = Driver.instance.FindElement(By.XPath("//a[contains(@href,'" + logout + "')]"));
                logoutButton.Click();

                // case: some gmail account pop up an alert message, while try to logout.
                try
                {
                    Driver.instance.SwitchTo().Alert().Accept();
                    Console.WriteLine("successful logout from gmail -  alert logout popup was handled");
                }
                catch (NoAlertPresentException)
                {
                    Console.WriteLine("successful logout from gmail - without alert  logout popup");
                }
            }
        }
    }
}