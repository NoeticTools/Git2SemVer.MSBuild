//using Moq;
//using NoeticTools.Common;
//using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
//using NoeticTools.Git2SemVer.Tool.BuildPropsFile;
//using NoeticTools.Git2SemVer.Tool.SetupCommand;
//using NoeticTools.Testing.Common;
//using NoeticTools.Testing.Common.Interops.DotNet;

//namespace NoeticTools.Git2SemVer.Tool.Tests
//{
//    public class SetupCommandRunnerTests
//    {
//        private const string SolutionPropsFilename = "Directory.Build.props";
//        private NUnitTaskLogger _logger;
//        private Mock<IDotNetTool> _dotnetTool;
//        private Mock<IEmbeddedResources> _embeddedResources;
//        private Mock<IFiles> _files;
//        private SetupCommandRunner _target;
//        private Mock<IBuildPropsDocumentReader> _propsDocumentIO;
//        private Mock<IBuildPropsDocument> _propsDocument;

//        [SetUp]
//        public void Setup()
//        {
//            _logger = new NUnitTaskLogger(false);
//            _dotnetTool = new Mock<IDotNetTool>();
//            _files = new Mock<IFiles>();
//            _propsDocumentIO = new Mock<IBuildPropsDocumentReader>();
//            _embeddedResources = new Mock<IEmbeddedResources>();
//            _dotnetTool.Setup(x => x.GetSolutionProjects(It.IsAny<string>())).Returns(new[] { "project1" });
//            _propsDocument = new Mock<IBuildPropsDocument>();
//            _propsDocumentIO.Setup(x => x.Read(It.IsAny<FileInfo>())).Returns(_propsDocument.Object);
//        }

//        [Test]
//        public void WhenSolutionPathDoesNoExist_ErrorIsLogged()
//        {
//            _files.Setup(x => x.Exists(It.Is<string>(s => s.EndsWith($@"\{SolutionPropsFilename}")))).Returns(false);
//            _target = new SetupCommandRunner(new DirectoryInfo("xxxx"), 
//                                 _dotnetTool.Object, _embeddedResources.Object,
//                                 _propsDocumentIO.Object, _logger);
//            Assert.That(_logger.HasLoggedErrors, Is.False);

//            _target.Run();

//            Assert.That(_logger.HasLoggedErrors, Is.True);
//            _embeddedResources.Verify(x => x.WriteResourceFile(SolutionPropsFilename, It.IsAny<string>()), Times.Never);
//        }

//        [Test]
//        public void WhenSolutionPropsFileDoesNotExist_ItIsCreated()
//        {
//            _files.Setup(x => x.Exists(It.Is<string>(s => s.EndsWith($@"\{SolutionPropsFilename}")))).Returns(false);
//            _target = new SetupCommandRunner(new DirectoryInfo("../../../.."), 
//                                 _dotnetTool.Object, _embeddedResources.Object,
//                                 _propsDocumentIO.Object,
//                                 _logger);

//            _target.Run();

//            Assert.That(_logger.HasLoggedErrors, Is.False);
//            _embeddedResources.Verify(x => x.WriteResourceFile(SolutionPropsFilename, It.IsAny<string>()), Times.Once);
//        }

//        //[Test]
//        //public void WhenSolutionPropsFileExists_ItIsNotOverwritten()
//        //{
//        //    _files.Setup(x => x.Exists(It.Is<string>(s => s.EndsWith($@"\{SolutionPropsFilename}")))).Returns(true);
//        //    _target = new Runner(new DirectoryInfo("../../../.."), 
//        //                         _dotnetTool.Object, _embeddedResources.Object, 
//        //                         _propsDocumentIO.Object, 
//        //                         _logger);

//        //    _target.Run();

//        //    Assert.That(_logger.HasLoggedErrors, Is.False);
//        //    _embeddedResources.Verify(x => x.WriteResourceFile(SolutionPropsFilename, It.IsAny<string>()), Times.Never);
//        //}
//    }
//}

