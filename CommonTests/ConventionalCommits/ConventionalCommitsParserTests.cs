using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.Common.ConventionCommits;


namespace NoeticTools.CommonTests.ConventionalCommits
{
    internal class ConventionalCommitsParserTests
    {
        private ConventionalCommitsParser _target;

        [SetUp]
        public void SetUp()
        {
            _target = new ConventionalCommitsParser();
        }

        [TestCase("")]
        [TestCase("\n\n\n")]
        [TestCase("This is a commit without conventional commit info")]
        [TestCase("Two lines\n")]
        [TestCase("Three lines\n\n")]
        public void MessageWithoutConventionalCommitInfoTest(string commitMessage)
        {
            var result = _target.Parse(commitMessage);

            Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.None));
        }

        [TestCase("feat:")]
        [TestCase("feat: ")]
        [TestCase("feat:  ")]
        [TestCase("fix:  ")]
        [TestCase("fix!:  ")]
        [TestCase("fix(a scope):  ")]
        public void MalformedSingleLineConventionalCommitInfoTest(string commitMessage)
        {
            var result = _target.Parse(commitMessage);

            Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.None));
        }

        [TestCase("feat: Added a real nice feature", CommitChangeTypeId.Feature, 
                     "Added a real nice feature")]
        [TestCase("fix: Fixed nasty bug", CommitChangeTypeId.Fix, 
                     "Fixed nasty bug")]
        [TestCase("build: Build work", CommitChangeTypeId.Build, 
                     "Build work")]
        [TestCase("chore: Did something", CommitChangeTypeId.Chore, 
                     "Did something")]
        [TestCase("ci: Did something", CommitChangeTypeId.ContinuousIntegration, 
                     "Did something")]
        [TestCase("docs: Did something", CommitChangeTypeId.Documentation, 
                     "Did something")]
        [TestCase("style: Did something", CommitChangeTypeId.Style, 
                     "Did something")]
        [TestCase("refactor: Did something", CommitChangeTypeId.Refactoring, 
                     "Did something")]
        [TestCase("perf: Did something", CommitChangeTypeId.Performance, 
                     "Did something")]
        [TestCase("test: Did something", CommitChangeTypeId.Testing, 
                     "Did something")]
        public void SingleLineConventionalCommitInfoTest(string commitMessage, 
                                                         CommitChangeTypeId expectedChangeTypeId,
                                                         string expectedChangeDescription)
        {
            var result = _target.Parse(commitMessage);

            Assert.That(result.ChangeType, Is.EqualTo(expectedChangeTypeId));
            Assert.That(result.HasBreakingChange, Is.False);
            Assert.That(result.ChangeDescription, Is.EqualTo(expectedChangeDescription));
            Assert.That(result.Body, Is.Empty);
            Assert.That(result.Footer, Is.Empty);
        }
    }
}
