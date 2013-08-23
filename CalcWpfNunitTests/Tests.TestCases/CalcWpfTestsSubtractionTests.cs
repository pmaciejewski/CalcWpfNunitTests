using NUnit.Framework;

namespace CalcWpfNunitTests.Tests.TestCases
{
    [TestFixture]
    [Category("Subtraction")]
    public class CalcWpfTestsSubtractionTests : CalcWpfTestsAbstract
    {

        /// <summary>
        /// 3 - 1 = 2
        /// </summary>
        [Test]
        public void TestSubPositivePositive()
        {
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("2", GetResultValue());
        }

        /// <summary>
        /// 1 - -2 = 3
        /// </summary>
        [Test]
        public void TestSubPositiveNegative()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("3", GetResultValue());
        }

        /// <summary>
        /// -1 - 2 = -3
        /// </summary>
        [Test]
        public void TestSubNegativePositive()
        {
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("-3", GetResultValue());
        }

        /// <summary>
        /// -1 - -3 = 2
        /// </summary>
        [Test]
        public void TestSubNegativeNegative()
        {
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("2", GetResultValue());
        }
    }
}
