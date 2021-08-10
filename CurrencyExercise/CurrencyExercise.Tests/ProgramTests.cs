using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyExercise.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void GetCurrencyNames_HasUSD()
        {
            var result = Program.GetCurrencyNames();
            Assert.AreEqual("United States Dollar", result["USD"]);
        }
    }
}
