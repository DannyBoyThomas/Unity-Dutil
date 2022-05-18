using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class AutoInsert : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        if (!Tools.AutoInsertDutil) { return; }
        string assetPath = Regex.Replace(path, @".meta$", string.Empty);
        if (!assetPath.EndsWith(".cs")) return;

        var code = File.ReadAllLines(assetPath).ToList();
        if (code.Any(line => line.Contains("using Dutil;"))) return;//already added by IDE

        //insert Dutil
        int idx = code.FindIndex(line => line
            .Contains("public class " + Path.GetFileNameWithoutExtension(assetPath)));
        code.Insert(idx - 1, "using Dutil;");

        //correct indentation

        var finalCode = string.Join("\n", code.ToArray());
        File.WriteAllText(assetPath, finalCode);
        AssetDatabase.Refresh();

       
    }
}
