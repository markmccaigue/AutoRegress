using AutoRegress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace AutoRegressTests
{
    [TestClass]
    public class TesterTests
    {
        private string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "AutoRegress";

        [TestInitialize()]
        public void Startup()
        {
            System.IO.Directory.CreateDirectory(_path);
            var directory = new System.IO.DirectoryInfo(_path);
            directory.EnumerateFiles().ToList().ForEach(f => f.Delete());
        }

        [TestCleanup()]
        public void Cleanup()
        {
            System.IO.Directory.Delete(_path, recursive: true);
        }

        private class ClassUnderTest
        {
            public bool ShouldFail { get; set; }

            public int GetNumber()
            {
                return ShouldFail ? 2 : 1;
            }

            public static int GetNumberStatic()
            {
                return 1;
            }

            public int Echo(int number)
            {
                return number;
            }

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestThrowsExceptionForUnknownClass()
        {
            Tester.CheckStateForClass(new ClassUnderTest());
        }

        [TestMethod]
        public void TestCompareWorksForCorrectCase()
        {
            var objectUnderTest = new ClassUnderTest();
            Tester.StoreStateForClass(objectUnderTest);
            var result = Tester.CheckStateForClass(objectUnderTest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCompareWorksForIncorrectCase()
        {
            var objectUnderTest = new ClassUnderTest();
            Tester.StoreStateForClass(objectUnderTest);
            objectUnderTest.ShouldFail = true;
            var result = Tester.CheckStateForClass(objectUnderTest);
            Assert.IsFalse(result);
        }
    }
}
