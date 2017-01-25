using System;
using System.Linq;
using NUnit.Framework;
using SuccincT.Options;

namespace Framework45.TestProject
{
    [TestFixture]
    public class SuccinctViaNugetTests
    {
        [Test]
        public void ProjectUsingFramework45_CanRunAgainstSuccinctFromNuget()
        {
            var option = Option<int>.Some(1);
            Assert.IsTrue(option.HasValue);
        }

        [Test]
        public void ProjectUsingFramework45_GetsTheCorrectVersionOfSuccinctFromNuget()
        {
            Option<int>.Some(1);
            var assemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              where assembly.FullName ==
                              "SuccincT, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null"
                              select assembly).ToList();
            Assert.AreEqual(1, assemblies.Count);
        }
    }
}
