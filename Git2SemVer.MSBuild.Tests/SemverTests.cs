using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Tests
{
    [TestFixture]
    internal class SemverTests
    {
        [Test]
        public void ComparePrecedenceToReturnsPositiveIfHigherPrecedenceTest()
        {
            var versionA = SemVersion.ParsedFrom(1, 2, 3);
            var versionB = SemVersion.ParsedFrom(1, 2, 2);

            var result = versionA.ComparePrecedenceTo(versionB);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void AlphaHasLowerPrecedenceToBetaTest()
        {
            var versionA = SemVersion.ParsedFrom(1, 2, 2, "Alpha");
            var versionB = SemVersion.ParsedFrom(1, 2, 2, "Beta");

            var result = versionA.ComparePrecedenceTo(versionB);

            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void AlphaHasLowerPrecedenceToInitialDevTest()
        {
            var versionA = SemVersion.ParsedFrom(1, 2, 2, "Alpha");
            var versionB = SemVersion.ParsedFrom(1, 2, 2, "InitialDev");

            var result = versionA.ComparePrecedenceTo(versionB);

            Assert.That(result, Is.EqualTo(-1));
        }

        [TestCase("InitialDev", "Alpha", 1)]
        [TestCase("InitialDev", "Beta", 1)]
        [TestCase("Beta", "Alpha", 1)]
        [TestCase("Alpha", "Beta", -1)]
        [TestCase("Beta", "Betaa", -1)]
        [TestCase("Betb", "Betaa", 1)]
        [TestCase("InitialDevAlpha", "Alpha", 1)]
        [TestCase("InitialDev", "InitialDevAlpha", -1)]
        [TestCase("11.Alpha", "1.Alpha", 1)]
        [TestCase("11.Alpha", "2.Alpha", 1)]
        [TestCase("Alpha.11", "Alpha.2", 1)]
        [TestCase("Alpha", "AlphaInitialDev", -1)]
        [TestCase("InitialDev", "AlphaInitialDev", 1)]
        [TestCase("InitialDev", "BetaInitialDev", 1)]
        public void PrereleaseLabelsPrecedenceTest(string labelLhs, string labelRhs, int expected)
        {
            var versionLhs = SemVersion.ParsedFrom(1, 2, 2, labelLhs);
            var versionRhs = SemVersion.ParsedFrom(1, 2, 2, labelRhs);

            var result = versionLhs.ComparePrecedenceTo(versionRhs);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
