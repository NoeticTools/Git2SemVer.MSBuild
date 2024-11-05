using System.Xml.Linq;
using Injectio.Attributes;


namespace NoeticTools.Git2SemVer.Tool.MSBuild.Projects;

[RegisterSingleton]
internal sealed class ProjectDocumentReader : IProjectDocumentReader
{
    public ProjectDocument Read(FileInfo file)
    {
        var xml = XDocument.Load(file.FullName);
        return new ProjectDocument(xml, file);
    }
}