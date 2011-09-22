using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SF.Selenium.MsTests
{
    [TestClass]
    public class GoogleSearchTest
    {
        #region Fields

        private static IWebDriver _driver;

        #endregion

        #region Fixture

        /// <summary>
        /// Setup the Selenium Driver and test fixture.
        /// </summary>
        /// <remarks>
        /// This method is executed once before any of the 
        /// tests in the fixture are executed.
        /// </remarks>
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _driver = new InternetExplorerDriver();
        }

        /// <summary>
        /// Cleanup the Selenium Driver and test fixture.
        /// </summary>
        /// <remarks>
        /// This method is executed when all the tests in the
        /// fixture have been executed.
        /// </remarks>
        [ClassCleanup]
        public static void CleanUp()
        {
            _driver.Quit();
        }

        /// <summary>
        /// Every test starts at the google homepage, so
        /// we'll do this in the test setup rather than in each test.
        /// </summary>
        /// <remarks>
        /// This method is executed before each test in the fixture.
        /// </remarks>
        [TestInitialize]
        public void TestInitialize()
        {
            _driver.Navigate().GoToUrl("http://www.google.co.uk");
        }

        #endregion

        #region Tests

        [TestMethod]
        public void GooglePageTitle()
        {
            //the page is open, let just verify the title
            Assert.AreEqual("Google", _driver.Title);
        }

        [TestMethod]
        public void NavigateToImageSearch()
        {
            //find the link that contains the text images and click it.
            var imageLink = _driver.FindElement(By.LinkText("Images"));
            imageLink.Click();

            //verify that the page title is now for google images
            Assert.AreEqual("Google Images", _driver.Title);
        }

        [TestMethod]
        public void SearchForSelenium()
        {
            var searchbox = _driver.FindElement(By.Name("q"));
            searchbox.SendKeys("Selenium");
            searchbox.SendKeys(Keys.Enter); //submits the form that the control is on.

            //google ajax's the search results in, so we need to wait on
            //something on the page that we know should exist. In this case we
            //wait on a link containing "Selenium - Web Browser Automation".
            //This test is extremely fragile because of this wait, what if the
            //link text changed, the test would fail.
            var waitDriver = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
            waitDriver.Until( driver => driver.FindElement(By.LinkText("Selenium - Web Browser Automation")));

            //now we can assert the title.
            Assert.AreEqual("selenium - Google Search", _driver.Title);
        }
        
        #endregion

    }
}
