using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace DotNetCypressTestsExample.Selenium.Tests.Setup
{
    public abstract class UITest
    {
        private int _iisPort
        {
            get
            {
                if (int.TryParse(ConfigurationManager.AppSettings["application-port"], out var value))
                {
                    return value;
                }

                throw new InvalidOperationException("Port number incorrect");
            }
        }

        public ChromeDriver _chromeDriver;
        private static Process _iisProcess;

        [OneTimeSetUp]
        public void Initialize()
        {
            StartWebserver();
            StartChromeWebDriver();
        }

        [OneTimeTearDown]
        public void Destroy()
        {
            StopWebserver();
            StopChromeWebDriver();
        }

        private void StopChromeWebDriver()
        {
            _chromeDriver.Dispose();
        }

        private void StopWebserver()
        {
            if (!_iisProcess.HasExited)
            {
                _iisProcess.Kill();
            }
        }

        private void StartChromeWebDriver()
        {
            ChromeOptions option = new ChromeOptions();

            var runHeadless = Convert.ToBoolean(ConfigurationManager.AppSettings["run-chrome-headless"]);

            if (runHeadless)
            {
                option.AddArgument("--headless");
            }

            _chromeDriver = new ChromeDriver(option);
            _chromeDriver.Manage().Window.Maximize();
        }

        private void StartWebserver()
        {
            var applicationName = ConfigurationManager.AppSettings["application-name"];
            var applicationPath = GetApplicationPath(applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = $"/path:\"{applicationPath}\" /port:{_iisPort}";
            _iisProcess.Start();
        }

        private string GetApplicationPath(string applicationName)
        {
            var projectFolder =
                    Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
            var parent = Directory.GetParent(projectFolder);
            return Path.Combine(parent.FullName, applicationName);
        }
    }
}