using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WindowsInput;

class Program
{

    static void Main()
    {
        // Set up the driver
        IWebDriver driver = new ChromeDriver("./");

        // Navigate to the reaction speed website
        driver.Navigate().GoToUrl("https://humanbenchmark.com/tests/reactiontime");

        // Give some time for the page to fully load
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        driver.Manage().Window.Maximize(); // Maximizes the window to full screen

        #region 
        IWebElement loginButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[3]/div/div[2]/a[2]"));
        loginButton.Click();

        IWebElement inputUser = WaitForElement(driver, By.CssSelector("#root > div > div.css-1f554t4.e19owgy74 > div > div > form > p:nth-child(1) > input[type=text]"), TimeSpan.FromSeconds(10));
        inputUser.SendKeys("Made by Layth Hammad");
        Thread.Sleep(1000);
        inputUser.Clear();
        inputUser.SendKeys("BOT49");

        IWebElement inputKey = WaitForElement(driver, By.CssSelector("#root > div > div.css-1f554t4.e19owgy74 > div > div > form > p:nth-child(2) > input[type=password]"), TimeSpan.FromSeconds(10));
        inputKey.SendKeys("Layth12+");

        IWebElement confirmButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/form/p[3]/input"));
        confirmButton.Click();

        IWebElement reactionButton = WaitForElement(driver, By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/div[1]/div/div[1]"), TimeSpan.FromSeconds(10));
        reactionButton.Click();

        IWebElement startButton = WaitForElement(driver, By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/div[2]/div[2]/div/div[4]/a"), TimeSpan.FromSeconds(10));
        startButton.Click();

        IWebElement startTest = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div"));
        Thread.Sleep(1000);
        startTest.Click();
        #endregion


        // Set the coordinates of the specific pixel to monitor
        //int pixelX = 400;
        //int pixelY = -700;
        int pixelX = 1300;
        int pixelY = 230;

        // Start the color monitoring thread
        MonitorPixelColor(pixelX, pixelY);
    }

    // Method to monitor pixel color in a separate thread
    static void MonitorPixelColor(int x, int y)
    {
        Color pixelColor;
        while (true)
        {
            do
            {
                // Get the RGB values of the specified pixel
                pixelColor = GetPixelColor(x, y);
            } while (!IsColorChangedFromRedToGreen(pixelColor));

            Thread clickThread = new Thread(() => ClickAsFastAsPossible());
            clickThread.Start();
            Thread.Sleep(500);

        }
    }

    // Method to simulate rapid mouse clicks
    static void ClickAsFastAsPossible()
    {
        InputSimulator simulator = new InputSimulator();

        // Simulate a left mouse button down
        simulator.Mouse.LeftButtonClick();
        Thread.Sleep(500);
        simulator.Mouse.LeftButtonClick();
    }

    // Method to get pixel color at specified coordinates
    static Color GetPixelColor(int x, int y)
    {
        using (Bitmap screenCapture = new Bitmap(1, 1))
        {
            using (Graphics g = Graphics.FromImage(screenCapture))
            {
                g.CopyFromScreen(x, y, 0, 0, screenCapture.Size);
            }
            return screenCapture.GetPixel(0, 0);
        }
    }

    // Method to check if color changes from red to green
    static bool IsColorChangedFromRedToGreen(Color pixelColor)
    {
        // Define the exact RGB values for red
        Color redColor = Color.FromArgb(206, 38, 54);
        Color greenColor = Color.FromArgb(75, 219, 106);


        // Check if the pixel color changes from red to green
        return pixelColor.ToArgb() != redColor.ToArgb() && pixelColor.ToArgb() == greenColor.ToArgb();

    }

    static IWebElement WaitForElement(IWebDriver driver, By by, TimeSpan timeout)
    {
        WebDriverWait wait = new WebDriverWait(driver, timeout);
        return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
    }
}
