using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NormalizerTest
{
    [TestClass]
    public class NormalizerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var input = @"ســـلاـــمٌ علــــــیُُُُُيِّّّّّيکُم";
            var result = TextNormalizer.Normalizer.Normalize(input);
            Console.WriteLine(result);
            Debug.Assert(result == @"سلام علیییکم");
        }
    }
}
