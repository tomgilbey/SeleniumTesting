using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting.Tests
{
    [TestFixture]
    class SeleniumTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            // Initialize the Firefox driver and WebDriverWait
            driver = new FirefoxDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CreateUserAccount()
        {
            // Navigate to the home page
            driver.Navigate().GoToUrl("https://w22002938.nuwebspace.co.uk/SellerScore/index.php");

            // Verify the title of the home page
            Assert.That(driver.Title, Is.EqualTo("Home Page"));

            // Click on the Register button
            IWebElement registerButton = driver.FindElement(By.LinkText("Register"));
            registerButton.Click();

            // Verify the title of the registration page
            Assert.That(driver.Title, Is.EqualTo("Register an Account"));

            // Fill in the registration form
            IWebElement firstname = driver.FindElement(By.Id("inputFirstName"));
            IWebElement surname = driver.FindElement(By.Id("inputSurname"));
            IWebElement username = driver.FindElement(By.Id("inputUsername"));
            IWebElement email = driver.FindElement(By.Id("inputEmail"));
            IWebElement dob = driver.FindElement(By.Id("inputDOB"));
            IWebElement password = driver.FindElement(By.Id("inputPassword"));
            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(), 'Register')]"));
            firstname.SendKeys("John");
            surname.SendKeys("Smith");
            // Use unique username and email to avoid conflicts
            username.SendKeys("UserName12345");
            email.SendKeys("user@email.com");
            dob.SendKeys("2000-01-01");
            password.SendKeys("Password123");
            submit.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
                // Check for error messages
                IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector(".alert.alert-danger")));
                string errorText = errorMessage.Text;
                Console.WriteLine("Error message displayed: " + errorText);

                // Handle specific error messages
                if (errorText.Contains("Password must be at least 6 characters"))
                {
                    Console.WriteLine("Weak password detected.");
                }
                if (errorText.Contains("Email is already registered"))
                {
                    Console.WriteLine("Existing email detected.");
                }
                if (errorText.Contains("Username already exists"))
                {
                    Console.WriteLine("Existing username detected.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Verify successful registration by checking the title of the login page
                Assert.That(driver.Title, Is.EqualTo("Login"));
                Console.WriteLine("No error message displayed.");
            }
            driver.Quit();
        }

        private void PerformLogin()
        {
            // Navigate to the home page
            driver.Navigate().GoToUrl("https://w22002938.nuwebspace.co.uk/SellerScore/index.php");
            Assert.That(driver.Title, Is.EqualTo("Home Page"));

            // Click on the Login button
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));
            loginButton.Click();
            Assert.That(driver.Title, Is.EqualTo("Login"));

            // Fill in the login form
            IWebElement username = driver.FindElement(By.Id("inputUsername"));
            IWebElement password = driver.FindElement(By.Id("inputPassword"));
            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(), 'Sign in')]"));
            username.SendKeys("JohnSmith123");
            password.SendKeys("cram");
            submit.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                // Check for error messages
                IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector(".alert.alert-danger")));
                string errorText = errorMessage.Text;
                Console.WriteLine("Error message displayed: " + errorText);

                // Handle specific error messages
                if (errorText.Contains("Login Details are incorrect."))
                {
                    Console.WriteLine("Login Details are incorrect.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Verify successful login by checking the title of the home page
                Assert.That(driver.Title, Is.EqualTo("Home Page"));
                Console.WriteLine("✅ Successfully logged in.");
            }
        }

        [Test]
        public void Login()
        {
            // Perform login and quit the driver
            PerformLogin();
            driver.Quit();
        }

        [Test]
        public void Search()
        {
            // Navigate to the home page
            driver.Navigate().GoToUrl("https://w22002938.nuwebspace.co.uk/SellerScore/index.php");
            Assert.That(driver.Title, Is.EqualTo("Home Page"));

            // Perform a search
            IWebElement searchButton = driver.FindElement(By.Id("searchInputNavbar"));
            IWebElement submitSearch = driver.FindElement(By.XPath("//button[strong[contains(text(), 'Search')]]"));
            searchButton.SendKeys("JohnSmith123");
            submitSearch.Click();
            Assert.That(driver.Title, Is.EqualTo("Search Results"));

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                // Check for no results message
                IWebElement noResultsMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'No results found for')]")));
                string errorText = noResultsMessage.Text;
                Console.WriteLine("Message displayed: " + errorText);

                if (errorText.Contains("No results found"))
                {
                    Console.WriteLine("✅ No search results found.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Verify search results and navigate to profile
                Console.WriteLine("✅ Search returned results.");
                IWebElement profileLink = driver.FindElement(By.XPath("//a[contains(@href, 'profile.php?user=JohnSmith123')]"));
                profileLink.Click();
                Assert.That(driver.Title, Is.EqualTo("Profile"));
            }

            driver.Quit();
        }

        [Test]
        public void ViewReputation()
        {
            // Perform login
            PerformLogin();

            // Navigate to My Reputation page
            wait.Until(d => d.FindElement(By.LinkText("My Reputation")));
            IWebElement myReputation = driver.FindElement(By.LinkText("My Reputation"));
            myReputation.Click();

            // Verify the title of the My Reputation page
            Assert.That(driver.Title, Is.EqualTo("My Reputation"));
            Console.WriteLine("✅ Successfully navigated to My Reputation page.");
        }

        [Test]
        public void LinkNewAccount()
        {
            // Perform login
            PerformLogin();
            wait.Until(d => d.FindElement(By.LinkText("Manage Account")));

            // Navigate to Manage Account page
            IWebElement manageAccount = driver.FindElement(By.LinkText("Manage Account"));
            manageAccount.Click();
            Assert.That(driver.Title, Is.EqualTo("Manage your Account"));

            // Link a new marketplace account
            IWebElement marketplaceSelect = driver.FindElement(By.Id("MarketplaceID"));
            IWebElement inputUsername = driver.FindElement(By.Id("MarketplaceUsername"));
            IWebElement linkAccount = driver.FindElement(By.XPath("//button[contains(text(), 'Link Account')]"));
            SelectElement selectMarketplace = new SelectElement(marketplaceSelect);
            selectMarketplace.SelectByValue("2"); // Select eBay
            inputUsername.SendKeys("TestUser123");
            linkAccount.Click();

            try
            {
                // Check for error messages
                IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector(".alert.alert-danger")));
                string errorText = errorMessage.Text;
                Console.WriteLine("Error message displayed: " + errorText);

                // Handle specific error messages
                if (errorText.Contains("You have already added this marketplace."))
                {
                    Console.WriteLine("❌ Marketplace already linked.");
                }
                else if (errorText.Contains("All fields are required."))
                {
                    Console.WriteLine("❌ Missing required fields.");
                }
                else if (errorText.Contains("Marketplace account could not be added."))
                {
                    Console.WriteLine("❌ Failed to link marketplace account.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                try
                {
                    // Check for success messages
                    IWebElement successMessage = wait.Until(d => d.FindElement(By.CssSelector(".alert.alert-success")));
                    string successText = successMessage.Text;
                    Console.WriteLine("Success message displayed: " + successText);

                    if (successText.Contains("Marketplace account added successfully."))
                    {
                        Console.WriteLine("✅ Successfully linked marketplace account.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Verify successful navigation to Manage Account page
                    Assert.That(driver.Title, Is.EqualTo("Manage your Account"));
                    Console.WriteLine("✅ Successfully navigated to Manage Account page.");
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Quit the driver
            driver.Quit();
        }
    }
}
