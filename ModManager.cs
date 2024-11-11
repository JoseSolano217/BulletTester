using Godot;
using GodotPlugins.Game;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

public class ModManager
{
    // TODO: Make a structure to be read by the mod system
    public string importPath = "";
    string modPath = "res://";

    public List<Pattern> moddedPatterns = new List<Pattern>();
    public void Import()
    {
        moddedPatterns.Clear();

        using var dir = DirAccess.Open(importPath);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (dir.CurrentIsDir())
                {
                    GD.Print($"Found directory: {fileName}");
                }
                else 
                {
                    GD.Print($"Found file: {fileName}");
                    if (Path.GetExtension(fileName) == ".pck")
                    {
                        string modName = Path.GetFileNameWithoutExtension(fileName);
                        GD.Print($".pck file detected, trying to load {fileName} in {importPath}.");
                        GD.Print($"Complete file: {importPath}/{fileName}");
                        Assembly.LoadFile(importPath + "/" + modName + ".dll");
                        var success = ProjectSettings.LoadResourcePack(importPath + "/" + fileName);

                        if (success) 
                        {
                            GD.Print("Success loading, adding modifications.");

                            CheckPatternFolder(modName, modPath);
                            var modScene = ResourceLoader.Load(modPath + modName + ".tscn");
                            if (modScene is PackedScene scene)
                            {
                                var mod = scene.Instantiate();
                                //var mod = instanceNode.GetScript();
                                GD.Print("Script is " + mod.ToString());
                                GD.Print("Type: " + mod.GetType().Name);

                                /*for (int i = 0; i < TypeDescriptor.GetProperties(mod).Count; i++)
                                {
                                    AttributeCollection attribute = TypeDescriptor.GetProperties(mod)[i].Attributes;
                                    string attributeName = TypeDescriptor.GetProperties(mod)[i].Name;
                                    if (attribute[typeof(BindableAttribute)].Equals(BindableAttribute.Yes))
                                    {
                                        GD.Print($"Attribute {attributeName} is bindable true");
                                    } else
                                    {
                                        GD.Print($"Attribute {attributeName} is bindable false");
                                    }
                                }*/

                                var modsList = mod.Call("get_patterns");//mod.GetType().GetMethod("GetPatterns").Invoke(mod, null);
                                Godot.Collections.Array modArray = modsList.AsGodotArray();
                                
                                //GD.Print($"Loaded mods as a {modsList.GetType().Name}");
                            }
                            /*var instance = (Node)modScene;
                            var mod = modScene.Instantiate();
                            GD.Print("Script is " + mod.ToString());
                            GD.Print("Type: " + mod.GetType().Name);
                            var modsList = mod.GetType().GetMethod("GetPatterns").Invoke(mod, null);
                            GD.Print($"Loaded mods as a {modsList.GetType().Name}");*/
                        } else
                        {
                            GD.Print($"Unknown error loading, file {fileName} was not loaded.");
                        }
                    }
                }
                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.Print("An error occurred when trying to access the path.");
        }
    }

    private void CheckPatternFolder(string modName, string path)
    {
        GD.Print("Opening " + path);
        using var dir = DirAccess.Open(path);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            GD.Print("Using " + dir.GetCurrentDir());
            while (fileName != "")
            {
                if (!dir.CurrentIsDir())
                {
                    GD.Print("Found file: " + fileName);
                    GD.Print("Full path: " + path + "/" + fileName);
                    if (fileName == modName + ".tscn")
                    {
                        GD.Print("There is one " + fileName);
                        //PackedScene pattern = ResourceLoader.Load<PackedScene>(modPath + fileName);
                        //var mod = pattern.Instantiate();
                        //var patternList = mod.Call("GetPatterns");//mod.GetType().GetMethod("GetPatterns").Invoke(mod, null);
                        //GD.Print($"Pattern method returned: {patternList.GetType().Name}");
                    }
                } else
                {
                    GD.Print("Found folder: " + fileName);
                    CheckPatternFolder(modName, path + "/" + fileName);
                }
                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.Print("Directory not found.");
        }
    }
}
