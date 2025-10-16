namespace GtCores.Components;

public class ImportMap
{
    public static IReadOnlyDictionary<string, object> ImportMapDefinition = new Dictionary<string, object>
    {
        ["imports"] = new Dictionary<string, string>
        {
            ["@popperjs/core"] = "./lib/@popperjs/core/dist/umd/popper.min.js",
            ["bootstrap"] = "./lib/bootstrap/dist/js/bootstrap.esm.min.js"
        }
    };
}