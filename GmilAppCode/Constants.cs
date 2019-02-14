using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmilAppCode
{
    public static class Constants
    {
        public const string GMAIL_URL = "http://www.gmail.com";

        public  const string LOGIN_ID = "identifierId";
        public  const string LOGIN_NEXT = "identifierNext";
        public  const string PASSWORD = "password";
        public  const string PASSWORD_NEXT = "passwordNext";

        public  const string MENU_BUTTON = ".gb_zc";
        public const string NEW_POST_BUTTON = "z0";
        public const string SENT_ITEMS = "div[data-tooltip=\"Sent\"]";
        public  const string SIGN_OUT_OPTIONS_XP = "//a[contains(@href,'" + "https://accounts.google.com/SignOutOptions?" + "')]";
        public  const string LOGOUT_XP = "//a[contains(@href,'" + "https://accounts.google.com/Logout?" + "')]";

        public const string NEW_POST_MAILTO = "input[name=\"subjectbox\"]";
        public const string NEW_POST_SUBJECT = "input[name=\"subjectbox\"]";
        public  const string NEW_POST_BODY = "div[role=\"textbox\"]";
        public  const string NEW_POST_SENT_LINK = "link_vsm";
        
        public const string FIRST_SENT_MAIL = "td[tabindex=\"-1\"]";
        public const string BODY = "body";

        public const string DEFULT_USER_NAME_ACCOUNT_VALUE = "apptesterselenuim";
        public const string DEFULT_PASSWORD_ACCOUNT_VALUE = "ct,h kvmkhj";
        public const string DEFULT_MAILTO_VALUE = "apptesterselenuim@gmail.com";
        public const string DEFULT_MSGBODY_VALUE = "Hello World";
    }
}
