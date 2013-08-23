using System;
using System.Diagnostics;
using System.Windows.Automation;
using System.Threading;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CalcWpfNunitTests.Tests
{
    abstract public class CalcWpfTestsAbstract
    {

        #region Properties
        public static PropertiesReader _propertiesReader = new PropertiesReader(@"..\..\..\properties.properties");
        public static string _appPath = _propertiesReader.GetProperty("app_path", @"..\..\..\CalcWpf.exe");
        public static string _appName = _propertiesReader.GetProperty("app_name", "CalcViewTitle");
        public static bool _screenShotsFolderOveride = bool.Parse(_propertiesReader.GetProperty("screenshots_folder_overide", "false"));
        public static string _screenshotsFolder = SetScreenshotFolder(_propertiesReader.GetProperty("screenshots_folder", @"..\..\..\Screenshots"));
        #endregion

        public Process _p = null;
        public AutomationElement _aMainWindow = null;

        /// <summary>
        /// TestFixture SetUp
        /// Will be done before each TestFixture (usually a class)
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // I'm not setting anything important here, but leaving this method just to show how TestFixtureSetUp works
            Log("TestFixtureSetUp");
        }

        /// <summary>
        /// TestFixture TearDown
        /// Will be done after each TestFixture (usually a class)
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            // I'm not setting anything important here, but leaving this method just to show how TestFixtureTearDown works
            Log("TestFixtureTearDown");
        }

        /// <summary>
        /// Test SetUp method
        /// Will be done before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            GetTestAppMainWindow();
        }

        /// <summary>
        /// Test TearDown method
        /// Will be done after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            /* 
             * Take screenshot on fail, but only if the test app was open
             * */
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed && _aMainWindow != null)
            {
                _aMainWindow.SetFocus();
                TakeScreenShot();
            }

            CloseTestAppMainWindow();
        }

        /// <summary>
        /// Will find and open test app
        /// </summary>
        public void GetTestAppMainWindow()
        {
            try
            {
                Log("Begin WPF UIAutomation test run");

                // Start the test app
                if (File.Exists(_appPath))
                {
                    _p = Process.Start(_appPath);
                }
                else
                {
                    throw new Exception("Failed to find application: " + _appPath);
                }

                // Wait for the test app to load
                int count = 0;
                do
                {
                    Log("Looking for process...");
                    count++;
                    Thread.Sleep(1000);
                } while (_p == null && count < 60);

                if (_p == null)
                {
                    throw new Exception("Failed to find process");
                }
                else
                {
                    Log("Found process, " + _p.Id + ", " + _p.MainWindowTitle);
                }

                // No need to wait for the Desktop ;)
                Log("Getting Desktop...");
                AutomationElement aeDesktop = AutomationElement.RootElement;
                if (aeDesktop == null)
                {
                    throw new Exception("Failed to find desktop");
                }
                else
                {
                    Log("Found desktop");
                }

                // Look for main window of the test app, look by the main window name (title in xaml) attribute
                count = 0;
                do
                {
                    Log("Looking for main window...");
                    _aMainWindow = aeDesktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, _appName));
                    count++;
                    Thread.Sleep(1000);
                } while (_aMainWindow == null && count < 60);
                if (_aMainWindow == null)
                {
                    throw new Exception("Failed to find element with name " + _appName);
                }
                else
                {
                    Log("Found element with name " + _appName);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                /*
                 * If you don't want for Visual Studio to stop on each exception while debugging, you need to "disable" AssertionException exceptions:
                 * 1) Go to the Debug -> Exceptions...
                 * 2) Click Add..., as Type select Common Language Runtime Exceptions, as Name write NUnit.Framework.AssertionException
                 * 3) Click Ok, and uncheck Thrown and User-unhandled
                 * 4) Click OK
                 * Credits goeas to Saurabh Arora, solution found here: http://social.msdn.microsoft.com/Forums/en-US/f7e89111-5cdc-440d-84c8-960c34034b85/how-to-make-a-coded-ui-test-be-set-to-failed-when-an-exception-is-caught-using-trycatch-blocks
                 * */
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Will close main process of the test app
        /// </summary>
        public void CloseTestAppMainWindow()
        {
            if (_p != null)
            {
                _p.CloseMainWindow();
            }
        }

        /// <summary>
        /// Assert that two strings are equal, log info
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public void AsserEquals(string expected, string actual)
        {
            Log("Expected = " + expected + ", actual = " + actual);
            Assert.AreEqual(expected, actual);
        }

        public string GetAddButtonName { get { return "button_op_add"; } }
        public string GetSubButtonName { get { return "button_op_sub"; } }
        public string GetMulButtonName { get { return "button_op_mul"; } }
        public string GetDivButtonName { get { return "button_op_div"; } }
        public string GetPow2ButtonName { get { return "button_op_pow_to_2"; } }
        public string GetPowxButtonName { get { return "button_op_pow_to_x"; } }
        public string GetRoot2ButtonName { get { return "button_op_sqrt"; } }
        public string GetRootxButtonName { get { return "button_op_root"; } }
        public string GetResButtonName { get { return "button_op_result"; } }
        public string GetClearButtonName { get { return "button_op_c"; } }
        public string GetBackButtonName { get { return "button_op_back"; } }
        public string GetNegativeButtonName { get { return "button_negative"; } }

        public string GetResTextBoxName { get { return "textBox_result"; } }
        public string GetUserInputTextBoxName { get { return "textBox_userInput"; } }

        public string GetNumberButtonName(string number)
        {
            return "button_number_" + number;
        }

        /// <summary>
        /// Will click button with given AutomationId (name in xaml) inside main window
        /// </summary>
        /// <param name="id"></param>
        public void ClickButtonWithId(string id)
        {
            ClickButtonWithId(id, _aMainWindow);
        }

        /// <summary>
        /// Will click button with given AutomationId (name in xaml) inside given direct parent window
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        public void ClickButtonWithId(string id, AutomationElement parent)
        {
            ((InvokePattern)FindElementWithId(id, parent).GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }

        /// <summary>
        /// Will find button with given AutomationId (name in xaml) inside main window
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutomationElement FindElementWithId(string id)
        {
            return FindElementWithId(id, _aMainWindow);
        }

        /// <summary>
        /// Will find element with given AutomationId (name in xaml) inside given parent window
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public AutomationElement FindElementWithId(string id, AutomationElement parent)
        {
            AutomationElement element = parent.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, id));
            try
            {
                if (element == null)
                {
                    throw new Exception("Failed to find element with id " + id);
                }
                else
                {
                    Log("Found element with id " + id);
                }
            }
            catch (Exception ex)
            {
                // Fail test in case of not finding the element
                Log(ex.Message);
                Assert.Fail(ex.Message);
            }
            return element;
        }

        /// <summary>
        /// Will return value of the result text box
        /// </summary>
        /// <returns></returns>
        public string GetResultValue()
        {
            return (string)FindElementWithId(GetResTextBoxName).GetCurrentPropertyValue(ValuePattern.ValueProperty);
        }

        /// <summary>
        /// Will write line in log file
        /// </summary>
        /// <param name="messageToLog"></param>
        public void Log(string messageToLog)
        {
            // As I didn't want to add to much unnecessary code, this "log" it just outputting to console prompt
            Console.WriteLine(messageToLog);
        }

        public static string SetScreenshotFolder(string screenshotsFolder)
        {
            // If screen_shots_folder property ends with \, remove last character
            if (screenshotsFolder.EndsWith("\\"))
            {
                screenshotsFolder = screenshotsFolder.Substring(0, screenshotsFolder.Length - 1);
            }

            // If screen_shots_folder_overide is set to true, delate whole existing ScreenShots folder with everyting inside
            if (_screenShotsFolderOveride)
            {
                Directory.Delete(screenshotsFolder, true);
            }
            // If screen_shots_folder_overide is set to false, create new ScreenShots folder with timestamp in name
            else
            {
                screenshotsFolder = screenshotsFolder + DateTime.Now.ToString("_dd-MM-yyyy_HH-mm-ss");
            }

            Directory.CreateDirectory(screenshotsFolder);

            return screenshotsFolder;
        }

        /// <summary>
        /// Will take a screenshot and save it to folder set by screen_shots_folder property
        /// </summary>
        public void TakeScreenShot()
        {
            // Gets main test window bounds in form: X;Y;Width;Height
            System.Windows.Rect aMainWindowBounds = _aMainWindow.Current.BoundingRectangle;

            // Creates new bounds based on aMainWindowBounds in form: {X=X,Y=Y,Width=Width,Height=Height}
            Rectangle bounds = new Rectangle(Convert.ToInt32(aMainWindowBounds.X), Convert.ToInt32(aMainWindowBounds.Y), Convert.ToInt32(aMainWindowBounds.Width), Convert.ToInt32(aMainWindowBounds.Height));

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                // Save the screenshot as ClassName_MethodName.jpgl
                string[] tmp = TestContext.CurrentContext.Test.FullName.Split('.');
                bitmap.Save(_screenshotsFolder + "\\" + tmp[tmp.Length - 2] + "_" + tmp[tmp.Length - 1] + ".jpg", ImageFormat.Jpeg);
            }
        }
    }

}