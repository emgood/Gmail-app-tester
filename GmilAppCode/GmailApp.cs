using GmilAppCode;
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
        static private string userName = Constants.DEFULT_USER_NAME_ACCOUNT_VALUE;

        static private string password = Constants.DEFULT_PASSWORD_ACCOUNT_VALUE;

        public static void GoTo()
        {
            Driver.instance.Navigate().GoToUrl(Constants.GMAIL_URL);
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
            //login page
            //entering user name
            var userNameField = Driver.instance.FindElement(By.Id(Constants.LOGIN_ID));
            userNameField.SendKeys(userName);

            var next = Driver.instance.FindElement(By.Id(Constants.LOGIN_NEXT));
            next.Click();

            //entering password
            var passwordField = Driver.instance.FindElement(By.Name(Constants.PASSWORD));
            passwordField.SendKeys(password);

            var loginButttonPassword = Driver.instance.FindElement(By.Id(Constants.PASSWORD_NEXT));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
            ex.ExecuteScript("arguments[0].click();", loginButttonPassword);

            //validate login
            By menuBarElement = By.CssSelector(Constants.MENU_BUTTON);
            MenuBar.Exsist = Driver.IsElementExsist(menuBarElement);
        }
    }

    static public class MenuBar
    {
        static public bool Exsist { get; internal set; }

        static public class NewPost
        {
            //defult value
            static private string mailTo = Constants.DEFULT_MAILTO_VALUE;

            static private string msgBody = Constants.DEFULT_MSGBODY_VALUE;

            static private TimeSpan subjectTime;
            static public bool sentPost { get; internal set; }

            public static void Create()
            {
                Create(mailTo, msgBody);
            }

            public static void Create(string mailTo, string msgbody)
            {
                //Create a new post
                var newPostButton = Driver.instance.FindElement(By.ClassName(Constants.NEW_POST_BUTTON));
                newPostButton.Click();

                //fill post data
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
                var postForm = Driver.instance.FindElement(By.CssSelector(Constants.NEW_POST_MAILTO));
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
                ex.ExecuteScript("arguments[0].click();", postForm);
                var to = Driver.instance.SwitchTo().ActiveElement();
                to.SendKeys(mailTo);
                to.SendKeys(Keys.Tab);
            }

            private static void FillSubjectAddressWithCurrentDate()
            {
                var subjectBox = Driver.instance.FindElement(By.CssSelector(Constants.NEW_POST_SUBJECT));
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver.instance;
                ex.ExecuteScript("arguments[0].click();", subjectBox);
                subjectBox.Click();
                subjectTime = DateTime.Now.TimeOfDay;
                subjectBox.SendKeys(subjectTime.ToString());
            }

            private static void FillMessageBody(string msgBody)
            {
                var messageTextBox = Driver.instance.FindElement(By.CssSelector(Constants.NEW_POST_BODY));
                messageTextBox.Click();
                messageTextBox.SendKeys(msgBody);
                messageTextBox.SendKeys(Keys.Tab);
            }

            private static void ValidateSentPost()
            {
                //case: first try to access directly by view message popup link,
                //  if it doesn't exsist, searching on sent item (assumption: first sent post item).

                By viewMessageLinkElement = By.Id(Constants.NEW_POST_SENT_LINK);
                if (Driver.IsElementExsist(viewMessageLinkElement))
                {
                    var viewMessage = Driver.instance.FindElement(By.Id(Constants.NEW_POST_SENT_LINK));
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
                var menuBar = Driver.instance.FindElement(By.CssSelector(Constants.MENU_BUTTON));
                menuBar.Click();

                var sentItem = Driver.instance.FindElement(By.CssSelector(Constants.SENT_ITEMS));
                sentItem.Click();

                var firstSentPost = Driver.instance.FindElement(By.CssSelector(Constants.FIRST_SENT_MAIL));
                firstSentPost.Click();

                IWebElement BodySrcCode = Driver.instance.FindElement(By.TagName(Constants.BODY));
                return BodySrcCode.Text.Contains(subjectTime.ToString()) ? true : false;
            }
        }

        public static class LogOut
        {
            public static void Submit()
            {
                //getting the Signout option button, using partial href link (end with: '/SignOutOptions?')
                var signOutOption = Driver.instance.FindElement(By.XPath(Constants.SIGN_OUT_OPTIONS_XP));
                signOutOption.Click();

                //logout button
                var logoutButton = Driver.instance.FindElement(By.XPath(Constants.LOGOUT_XP));
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