using System.Xml.Linq;
using NoeticTools.Git2SemVer.Tool.MSBuild.Projects.GroupElements;


namespace NoeticTools.Git2SemVer.Tool.MSBuild.Projects;

internal class ProjectDocument : IProjectDocument
{
    private readonly FileInfo _file;
    private readonly XDocument _xml;

    public ProjectDocument(XDocument xml, FileInfo file)
    {
        _xml = xml;
        _file = file;
        Properties = new PropertyGroup(xml);
    }

    public PropertyGroup Properties { get; }

    public void Save()
    {
        _xml.Save(_file.FullName);
    }
}