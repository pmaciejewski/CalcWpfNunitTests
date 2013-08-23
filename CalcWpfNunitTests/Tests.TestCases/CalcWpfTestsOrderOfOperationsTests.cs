using System;
using NUnit.Framework;

namespace CalcWpfNunitTests.Tests.TestCases
{
    [TestFixture]
    [Category("OrderOfOperations")]
    class CalcWpfTestsOrderOfOperationsTests : CalcWpfTestsAbstract
    {

        /// <summary>
        /// 1 + 2 * 3 - 4 = 3
        /// This will fail.
        /// </summary>
        [Test]
        [Category("ToFail")]
        public void TestOrderAddMulSubToFail()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetMulButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNumberButtonName("4"));
            ClickButtonWithId(GetResButtonName);
            // Should expect 3
            AsserEquals("4", GetResultValue());
        }

        /// <summary>
        /// 1 + 2 / 3 - 4 = -2.3(3)
        /// </summary>
        [Test]
        [Category("Important")]
        public void TestOrderAddDivSub()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetDivButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNumberButtonName("4"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals((1.0 + 2.0 / 3.0 - 4.0).ToString(), GetResultValue());
        }

        /// <summary>
        /// 1 + 2 * 3 / 4 - 5 = -2.5
        /// </summary>
        [Test]
        public void TestOrderAddMulDivSub()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetMulButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetDivButtonName);
            ClickButtonWithId(GetNumberButtonName("4"));
            ClickButtonWithId(GetSubButtonName);
            ClickButtonWithId(GetNumberButtonName("5"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals((1.0 + 2.0 * 3.0 / 4.0 - 5.0).ToString(), GetResultValue());
        }

        /// <summary>
        /// 1 + 2 * sqrt(3) = 4,4641016151...
        /// </summary>
        [Test]
        [Category("Important")]
        public void TestOrderAddMulSqrt()
        {
            ClickButtonWithId(GetNumberButtonName("1"));
            ClickButtonWithId(GetAddButtonName);
            ClickButtonWithId(GetNumberButtonName("2"));
            ClickButtonWithId(GetMulButtonName);
            ClickButtonWithId(GetRoot2ButtonName);
            ClickButtonWithId(GetNumberButtonName("3"));
            ClickButtonWithId(GetResButtonName);
            AsserEquals(Math.Round(1.0 + 2.0 * Math.Sqrt(3.0), 13).ToString(), Math.Round(Double.Parse(GetResultValue()), 13).ToString());
            // need to round as 4,46410161513775 != 4,46410161513776
        }
    }
}
