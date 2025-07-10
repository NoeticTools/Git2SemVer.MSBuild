using System.Text.RegularExpressions;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public sealed class ConventionalCommitsParser(ConventionalCommitsSettings convCommitsSettings) : IConventionalCommitsParser
{
    private readonly Regex _bodyRegex = new("""
                                            \A
                                            (
                                              (?<body>\S ((\n|\r\n)|.)*? )
                                              (\Z | (?: (\n|\r\n) ) )
                                            )?
                                            (

                                              (?<footer>
                                                (
                                                  (?: (\n|\r\n)+ )
                                                  (?<token> (BREAKING\sCHANGE) | ( \w[\w-]+ ) )
                                                  ( \( (?<scope>\w[\w-]+) \) )?
                                                  ( \:\s|\s\# )
                                                  (?<value>
                                                    .*
                                                    (?:
                                                      (\n|\r\n).*
                                                    )*?
                                                  )
                                                )+
                                              )?
                                              \Z
                                            )
                                            """,
                                            RegexOptions.IgnorePatternWhitespace |
                                            RegexOptions.Multiline);

    private readonly Regex _summaryRegex = new("""
                                               \A
                                                 (?<ChangeType>\w[\w\-]*)
                                                   (\((?<scope>[\w\-\.]+)\))?(?<breakFlag>!)?: \s+(?<desc>\S.*?)
                                               \Z
                                               """,
                                               RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

    public CommitMessageMetadata Parse(string commitSummary, string commitMessageBody)
    {
        var summaryMatch = _summaryRegex.Match(commitSummary);
        if (!summaryMatch.Success)
        {
            return new CommitMessageMetadata(convCommitsSettings);
        }

        var changeType = summaryMatch.GetGroupValue("ChangeType");
        var breakingChangeFlagged = summaryMatch.GetGroupValue("breakFlag").Length > 0;
        var changeDescription = summaryMatch.GetGroupValue("desc");

        var bodyMatch = _bodyRegex.Match(commitMessageBody);
        var bodyMatches = _bodyRegex.Matches(commitMessageBody);
        foreach (Match match in bodyMatches)
        {
            if (!match.Success)
            {
                continue;
            }

            var group = match.Groups["footer"];
            if (!group.Success)
            {
            }
        }

        var body = bodyMatch.GetGroupValue("body");

        var keyValuePairs = GetFooterKeyValuePairs(bodyMatch);

        return new CommitMessageMetadata(changeType,
                                         changeDescription,
                                         body,
                                         breakingChangeFlagged,
                                         keyValuePairs, 
                                         convCommitsSettings);
    }

    private static Dictionary<string, List<string>> GetFooterKeyValuePairs(Match match)
    {
        var keyValuePairs = new Dictionary<string, List<string>>();

        var tokensGroup = match.Groups["token"];
        var valuesGroup = match.Groups["value"];
        if (!tokensGroup.Success || !valuesGroup.Success)
        {
            return keyValuePairs;
        }

        var keywords = tokensGroup.Captures;
        var values = valuesGroup.Captures;

        for (var captureIndex = 0; captureIndex < keywords.Count; captureIndex++)
        {
            var keyword = keywords[captureIndex].Value;
            if (string.IsNullOrEmpty(keyword))
            {
                continue;
            }
            var value = values[captureIndex].Value.TrimEnd();
            if (!keyValuePairs.ContainsKey(keyword))
            {
                keyValuePairs.Add(keyword, []);
            }
            keyValuePairs[keyword].Add(value);
        }

        return keyValuePairs;
    }
}