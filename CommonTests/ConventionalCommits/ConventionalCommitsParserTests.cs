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
        public void MalformedSingleLineConventionalCommitInfoTest(string commitMessage)
        {
            var result = _target.Parse(commitMessage);

            Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.None));
        }

        [TestCase("feat: Added a real nice feature", CommitChangeTypeId.Feature)]
        [TestCase("fix: Fixed nasty bug", CommitChangeTypeId.Fix)]
        [TestCase("build: Build work", CommitChangeTypeId.Build)]
        [TestCase("chore: Did something", CommitChangeTypeId.Chore)]
        [TestCase("ci: Did something", CommitChangeTypeId.ContinuousIntegration)]
        [TestCase("docs: Did something", CommitChangeTypeId.Documentation)]
        [TestCase("style: Did something", CommitChangeTypeId.Style)]
        [TestCase("refactor: Did something", CommitChangeTypeId.Refactoring)]
        [TestCase("perf: Did something", CommitChangeTypeId.Performance)]
        [TestCase("test: Did something", CommitChangeTypeId.Testing)]
        /*
         * build:, chore:, ci:, docs:, style:, refactor:, perf:, test:
         */
        public void SingleLineConventionalCommitInfoTest(string commitMessage, CommitChangeTypeId expectedChangeTypeId)
        {
            var result = _target.Parse(commitMessage);

            Assert.That(result.ChangeType, Is.EqualTo(expectedChangeTypeId));
        }
    }
}
