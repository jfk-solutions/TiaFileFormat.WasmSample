using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using TiaFileFormat;
using TiaFileFormat.Database;
using TiaFileFormat.Database.StorageTypes;
using TiaFileFormat.Wrappers;
using TiaFileFormatWasm.Classes.Response;

public partial class TiaWasmExports
{
    private TiaDatabaseFile database;

    internal static TiaDatabaseFile OpenProject(byte[] data)
    {
        //var data = Convert.FromBase64String(base64);
        using var ms = new MemoryStream(data);
        var tfp = TiaFileProvider.CreateFromSingleStream(ms);
        var database = TiaDatabaseFile.Load(tfp);
        return database;
    }

    static IEnumerable<Folder> BuildFoldersTia(StorageBusinessObject sb)
    {
        var fld = new Folder() { Name = sb.ProcessedName, Id = sb.Header?.StoreObjectId?.InstId ?? 0 };
        if (sb.ProjectTreeChildren.Any())
        {
            fld.Children = new List<Folder>();
            foreach (var child in sb.ProjectTreeChildren)
            {
                if (UnsupportedFolderTypes.ListSubTypes.Contains(child.CoreAttributes?.Subtype))
                    continue;
                foreach (var flattenedNode in BuildFoldersTia(child))
                {
                    fld.Children.Add(flattenedNode);
                }
            }
        }
        yield return fld;
    }

    [JSExport]
    internal static string GetFolders(byte[] bytes)
    {
        var database = OpenProject(bytes);

        var fr = new FolderResult() { Folders = new List<Folder>() };

        if (database.RootObject.StoreObjectIds.TryGetValue("Project", out var prj))
            fr.Folders.AddRange(BuildFoldersTia((StorageBusinessObject)prj.StorageObject));
        if (database.RootObject.StoreObjectIds.TryGetValue("Library", out var lb))
            fr.Folders.AddRange(BuildFoldersTia((StorageBusinessObject)lb.StorageObject));

        return JsonSerializer.Serialize(fr);
    }
}