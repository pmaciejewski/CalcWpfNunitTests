using NUnit.Framework;

namespace CalcWpfNunitTests.Tests.TestCases
{
    [TestFixture]
    [Category("Addition")]
    public class CalcWpfTestsAdditionTests : CalcWpfTestsAbstract
    {
        /// <summary>
        /// 1 + 2 = 3
        /// </summary>
        [Test]
        public void TestAddPositivePositive()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("4", GetResultValue());
        }

        /// <summary>
        /// 3 + -1 = 2
        /// </summary>
        [Test]
        public void TestAddPositiveNegative()
        {
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("2", GetResultValue());
        }

        /// <summary>
        /// -1 + 3 = 2
        /// </summary>
        [Test]
        public void TestAddNegativePositive()
        {
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("2", GetResultValue());
        }

        /// <summary>
        /// -1 + -2 = -3
        /// </summary>
        [Test]
        public void TestAddNegativeNegative()
        {
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNegativeButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals("-3", GetResultValue());
        }
    }
}
