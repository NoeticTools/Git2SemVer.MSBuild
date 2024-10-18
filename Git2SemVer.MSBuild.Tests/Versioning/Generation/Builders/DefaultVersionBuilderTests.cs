using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using NoeticTools.Testing.Common;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.Builders
{
    [TestFixture]
    internal class DefaultVersionBuilderTests
    {
        private DefaultVersionBuilder _target;
        private Mock<IHistoryPaths> _paths;
        private NUnitTaskLogger _logger;
        private Mock<IVersionHistoryPath> _bestPath;
        private Mock<IBuildHost> _host;
        private Mock<IVersionGeneratorInputs> _inputs;
        private Mock<ICommit> _headCommit;
        private Mock<IGitOutputs> _gitOutputs;
        private Mock<IVersionOutputs> _outputs;
        private SemVersion _version;

        [SetUp]
        public void SetUp()
        {
            _logger = new NUnitTaskLogger(false)
            {
                Level = LoggingLevel.Debug
            };
            _bestPath = new Mock<IVersionHistoryPath>();
            _paths = new Mock<IHistoryPaths>();
            _paths.Setup(x => x.BestPath).Returns(_bestPath.Object);

            _target = new DefaultVersionBuilder(_paths.Object, _logger);

            _host = new Mock<IBuildHost>();
            _inputs = new Mock<IVersionGeneratorInputs>();
            _headCommit = new Mock<ICommit>();
            _gitOutputs = new Mock<IGitOutputs>();
            _outputs = new Mock<IVersionOutputs>();

            _host.Setup(x => x.BuildId).Returns(["77"]);
            _inputs.Setup(x => x.WorkingDirectory).Returns("WorkingDirectory");
            _headCommit.Setup(x => x.CommitId).Returns(new CommitId("001"));
            _gitOutputs.Setup(x => x.HeadCommit).Returns(_headCommit.Object);
            _outputs.Setup(x => x.Git).Returns(_gitOutputs.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.Dispose();
        }

        [TestCase("0.5.1", "main")]
        [TestCase("0.5.1", "release")]
        [TestCase("0.5.1", "release/anything")]
        [TestCase("0.5.1", "Release/anything")]
        public void ReleaseBranchInitialDevBuild(string version, string branchName)
        {
            SetupInputs(version, branchName);

            _target.Build(_host.Object, _inputs.Object, _outputs.Object);

            _outputs.VerifySet(x => x.BuildSystemVersion = _version.WithPrerelease("InitialDev", "77"), Times.Once);
        }

        [Test]
        public void DevBranchInitialDevBuild()
        {
            SetupInputs("0.5.1", "JohnsOwnBranch");

            _target.Build(_host.Object, _inputs.Object, _outputs.Object);

            _outputs.VerifySet(x => x.BuildSystemVersion = _version.WithPrerelease("Alpha-InitialDev", "77"), Times.Once);
        }

        [Test]
        public void FeatureBranchInitialDevBuild()
        {
            SetupInputs("0.5.1", "feature/anything");

            _target.Build(_host.Object, _inputs.Object, _outputs.Object);

            _outputs.VerifySet(x => x.BuildSystemVersion = _version.WithPrerelease("Beta-InitialDev", "77"), Times.Once);
        }

        private void SetupInputs(string version, string branchName)
        {
            _version = SemVersion.Parse(version, SemVersionStyles.Strict);
            _bestPath.Setup(x => x.Version).Returns(_version);
            _gitOutputs.Setup(x => x.BranchName).Returns(branchName);
        }
    }
}
