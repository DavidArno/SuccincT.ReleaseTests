using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SuccincT.Options;

namespace Win8.TestProject
{
    [TestClass]
    public class SuccinctViaNugetTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var option = Option<int>.Some(1);
            Assert.IsTrue(option.HasValue);
        }

        [TestMethod]
        public async Task ProjectUsingFramework451_GetsTheCorrectVersionOfSuccinctFromNuget()
        {
            Option<int>.Some(1);
            var folder = Package.Current.InstalledLocation;

            var assemblies = (from assembly in await folder.GetFilesAsync()
                              where assembly.Name == "SuccincT.dll"
                              select assembly).ToList();
            Assert.AreEqual(1, assemblies.Count);
        }
    }
}
