using System.Text.RegularExpressions;
using Injectio.Attributes;

namespace NoeticTools.Common.Tools
{
    [RegisterSingleton]
    public sealed class ContentEditor : IContentEditor
    {
        public string RemoveLinesWith(string signature, string content)
        {
            var regex = new Regex($@"{Environment.NewLine}[ \t]*?{Regex.Escape(signature)}[ \t]*?(?={Environment.NewLine})", RegexOptions.Multiline);
            return regex.Replace(content, "", 100000);
        }
    }
}
