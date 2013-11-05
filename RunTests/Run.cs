using System;
using System.IO;
using System.Reflection;

namespace RunTests
{
    /*
     * For this solution credits goes to: http://www.damirscorner.com/UnitTestingWithNUnitInVisualC2010Express.aspx
     * Thanks to this class, and the fact that it is set as single startup project, you can run your test project straight to NUnit with just one f5 click.
     * */
    class Run
    {
        // path to latest NUnit bin folder
        // TODO - this should be changed, so it would automatically run latest version without using hard-coded path
        private static readonly string nunitFolder = @"C:\Program Files (x86)\NUnit 2.6.3\bin";
        // if you prefer you can set "NUNIT" environment variable with NUnit bin folder path and then use: Environment.GetEnvironmentVariable("NUNIT");

        // Without this attribute "DragDrop registration did not succeed" exception occurs while starting without debugging. For more info check: http://stackoverflow.com/questions/135803/dragdrop-registration-did-not-succeed
        [STAThread]
        public static void Main()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = nunitFolder;
            setup.ConfigurationFile = Path.Combine(nunitFolder, "NUnit.exe.config");

            AppDomain nunitDomain = AppDomain.CreateDomain("NUnit", null, setup);
            nunitDomain.ExecuteAssembly(Path.Combine(nunitFolder, "NUnit.exe"), new string[] { Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\CalcWpfNunitTests\bin\Debug\CalcWpfNunitTests.dll") });
        }
    }
}
