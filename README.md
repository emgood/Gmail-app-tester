# ![#1589F0](https://placehold.it/15/1589F0/000000?text=+) Project: Gmail-app-tester

# ![#c5f015](https://placehold.it/15/c5f015/000000?text=+) Description:
Automation project for testing Gmail-app functionalities, using Selenium C#.


# ![#c5f015](https://placehold.it/15/c5f015/000000?text=+) Requirements: 
 1. Start script from pipeline play btn.
 
2. Login.
 
3. Write email with current date in subject.
 
4. Send it.
 
5. Make sure it gets to the sent items.

6. Logout.


# ![#f03c15](https://placehold.it/15/f03c15/000000?text=+) miscellaneous:
relevant files:
  - code: GmilAppCode/GmailApp.cs
  - test: UnitTestProject1/UnitTest1.cs

 
 
overloading methods:
- It is possible to change test methods, in order to achieve more flexible test:

A.user account: 
- LoginPage.LogAs();
- LoginPage.LogAs("username");

- LoginPage.WithPassword();
-  LoginPage.WithPassword("password");

 
B. post:
- MenuBar.NewPost.Create();
-  MenuBar.NewPost.Create("mailTo", "message body");
