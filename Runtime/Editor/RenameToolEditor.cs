using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEditor;
using System.Linq;
using Codice.CM.Client.Gui;
namespace Dutil
{
    //number
    //remove (clone)
    public class RenameToolEditor : EditorWindow
    {
        List<GameObject> hiddenObjects = new List<GameObject>();
        public enum Mode { Prepend, Append, Replace }
        public enum SearchMode { Name, Tag, Layer, Selected }
        public enum CaseMode { Original, Lower, Upper, Title }
        public enum IndexMode { None, Relative, Absolute }
        Mode mode = Mode.Replace;
        SearchMode searchMode = SearchMode.Name;
        CaseMode caseMode = CaseMode.Original;
        IndexMode indexMode = IndexMode.None;
        string searchTerm = "";
        string newNameInput = "";
        bool searchRegex = false;
        bool searchShowInHierarchy = false;
        bool matchCase = false;
        bool liveSearch = true;
        string replaceTerm = "";
        bool replaceMatchCase = false;
        bool replaceRegex = false;
        List<FoundObject> foundObjects = new List<FoundObject>();
        float listNameWidth = 100;
        bool showQuickSettings = false;
        string searchAnd = "";
        List<string> allNewNames = new List<string>();
        Dictionary<string, int> timesNameUsed = new Dictionary<string, int>();
        [MenuItem("Dutil/Editors/Rename Tool %&n")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            RenameToolEditor window = (RenameToolEditor)EditorWindow.GetWindow(typeof(RenameToolEditor), false, "Rename Tool");
            //set size
            window.minSize = new Vector2(400, 400);
            window.Show();


        }
        void OnSelectionChange()
        {
            if (liveSearch && searchMode == SearchMode.Selected)
            {
                Search();
                Repaint();
            }
        }
        void OnEnable()
        {
            Search();
            Repaint();
        }


        void OnGUI()
        {
            string beforeSearch = searchTerm;
            allNewNames = AllNewNames();
            searchAnd = Event.current.shift ? "Search & " : "";
            EditorGUILayout.BeginVertical("Box");
            //header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search Options", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            //GUI.backgroundColor = Colours.Blue.Shade(2);
            liveSearch = EditorGUILayout.Toggle("Auto Search", liveSearch);
            searchMode = (SearchMode)EditorGUILayout.EnumPopup("Search Mode", searchMode);
            if (searchMode != SearchMode.Selected)
            {
                //button search
                if (!liveSearch && GUILayout.Button("Search", new GUILayoutOption[] { GUILayout.Height(24) }))
                {
                    Search();
                }
                EditorGUILayout.BeginHorizontal();
                searchTerm = GUILayout.TextField(searchTerm, EditorStyles.textField, GUILayout.Height(20));
                AddPlaceholder(searchTerm, "Search Term");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                searchRegex = EditorGUILayout.Toggle("Regex", searchRegex);
                matchCase = EditorGUILayout.Toggle("Match Case", matchCase);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                searchShowInHierarchy = EditorGUILayout.Toggle("Show In Hierarchy", searchShowInHierarchy);
                EditorGUILayout.EndHorizontal();

            }
            else
            {
                EditorGUILayout.LabelField("No options in 'Selected Mode'");
            }
            //search options
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();




            //Replace Options
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rename Options", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            mode = (Mode)EditorGUILayout.EnumPopup("Mode", mode);
            caseMode = (CaseMode)EditorGUILayout.EnumPopup("Case Mode", caseMode);
            indexMode = (IndexMode)EditorGUILayout.EnumPopup("Index Mode", indexMode);
            if (mode == Mode.Replace)
            {
                EditorGUILayout.BeginHorizontal();
                replaceMatchCase = EditorGUILayout.Toggle("Match Case", replaceMatchCase);
                replaceRegex = EditorGUILayout.Toggle("Regex", replaceRegex);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                replaceTerm = GUILayout.TextField(replaceTerm, EditorStyles.textField, GUILayout.Height(20));
                AddPlaceholder(replaceTerm, "Before");
                if (GUILayout.Button("Copy Search Term", new GUILayoutOption[] { GUILayout.Height(20) }))
                {
                    replaceTerm = searchTerm;
                }
                EditorGUILayout.EndHorizontal();
            }
            //give name
            GUI.SetNextControlName("NewNameInput");
            newNameInput = GUILayout.TextField(newNameInput, EditorStyles.textField, GUILayout.Height(20));
            AddPlaceholder(newNameInput, "After");
            EditorGUILayout.Space();
            //set color

            GUI.backgroundColor = Colours.Blue.Shade(2);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Rename", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                List<FoundObject> list = Found;
                foreach (FoundObject fo in list)
                {
                    Undo.RecordObject(fo.go, "Rename");
                    fo.go.name = GetRename(fo);
                }
            }
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Group", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                List<FoundObject> list = Found;
                //ignore those with parent
                list = list.Where(x => x.go.transform.parent == null).ToList();
                GameObject group = new GameObject("Group");
                Undo.RegisterCreatedObjectUndo(group, "Group");
                foreach (FoundObject fo in list)
                {
                    Undo.SetTransformParent(fo.go.transform, group.transform, "Group");
                }
                Selection.objects = new GameObject[] { group };
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            //reset color







            //actions
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            showQuickSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showQuickSettings, "Quick Settings");
            if (showQuickSettings)
            {

                QuickSettings();

            }
            EditorGUILayout.EndVertical();






            //list
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            int foundObjectsThatAreHidden = foundObjects.Count(x => hiddenObjects.Contains(x.go));
            EditorGUILayout.LabelField("Found Objects: " + foundObjects.Count + ". Hiding: " + foundObjectsThatAreHidden);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (hiddenObjects.Count > 0 && GUILayout.Button("Show All", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                hiddenObjects.Clear();
            }
            if (foundObjectsThatAreHidden > 0 && GUILayout.Button("Show Selected", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                hiddenObjects.RemoveAll(x => foundObjects.Where(y => hiddenObjects.Contains(y.go)).Select(y => y.go).Contains(x));
            }
            EditorGUILayout.EndHorizontal();


            if (foundObjects.Count > 0)
            {
                List<FoundObject> list = Found;
                foreach (FoundObject fo in list)
                {

                    EditorGUILayout.BeginHorizontal();
                    bool nowHidden = EditorGUILayout.Toggle(true, GUILayout.Width(20));
                    if (!nowHidden)
                    {
                        hiddenObjects.Add(fo.go);
                    }
                    //split evenly horizontally

                    bool showFoundTerm = searchMode == SearchMode.Tag || searchMode == SearchMode.Layer;
                    string foundTerm = showFoundTerm ? $" ({GetObjectSearchField(fo.go)})" : "";
                    EditorGUILayout.LabelField(fo.go.name + foundTerm, EditorStyles.boldLabel, GUILayout.Width(listNameWidth), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(">", GUILayout.Width(20));
                    EditorGUILayout.LabelField(GetRename(fo), EditorStyles.boldLabel, GUILayout.Width(listNameWidth), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("No Objects Found");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();







            //on input changed
            if (GUI.changed)
            {
                if (liveSearch)
                {
                    if (searchTerm.Length > 0 || searchMode == SearchMode.Selected)
                        Search();
                    else if (searchTerm.Length == 0 && searchMode != SearchMode.Selected)
                        foundObjects.Clear();
                }
            }
        }
        void QuickSettings()
        {
            bool isShiftDown = Event.current.shift;
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                mode = Mode.Replace;
                searchMode = SearchMode.Name;
                caseMode = CaseMode.Original;
                searchTerm = "";
                newNameInput = "";
                searchRegex = false;
                matchCase = false;
                indexMode = IndexMode.None;
                liveSearch = true;
                replaceTerm = "";
                replaceMatchCase = false;
                searchShowInHierarchy = false;
                replaceRegex = false;
                foundObjects.Clear();
                hiddenObjects.Clear();

                //select replace field

            }
            if (GUILayout.Button($"{searchAnd}Remove (Clone)", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                if (isShiftDown)
                {
                    searchMode = SearchMode.Name;
                    searchRegex = true;
                    searchTerm = " \\(Clone\\)";
                    Search();
                }
                indexMode = IndexMode.None;
                replaceRegex = true;
                replaceTerm = " \\(Clone\\)";
                newNameInput = "";
            }
            if (GUILayout.Button($"{searchAnd}Remove (Number)", new GUILayoutOption[] { GUILayout.Height(24) }))
            {

                if (isShiftDown)
                {
                    searchMode = SearchMode.Name;
                    searchRegex = true;
                    searchTerm = " \\(\\d+\\)";
                    Search();
                }
                indexMode = IndexMode.None;
                replaceRegex = true;
                replaceTerm = " \\(\\d+\\)";
                newNameInput = "";

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Replace All", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                indexMode = IndexMode.None;
                mode = Mode.Replace;
                replaceRegex = true;
                replaceTerm = @"^(.*)$";
                newNameInput = "";
                EditorGUI.FocusTextInControl("NewNameInput");
            }
            EditorGUILayout.EndHorizontal();
        }
        void AddPlaceholder(string value, string hint)
        {
            if (string.IsNullOrEmpty(value))
            {
                Rect pos = new Rect(GUILayoutUtility.GetLastRect());
                GUIStyle style = new GUIStyle
                {
                    alignment = TextAnchor.UpperLeft,
                    padding = new RectOffset(3, 0, 2, 0),
                    fontStyle = FontStyle.Italic,
                    normal =
            {
                textColor = Color.grey
            }
                };
                EditorGUI.LabelField(pos, hint, style);
            }
        }
        void Search()
        {
            foundObjects.Clear();

            if (searchMode == SearchMode.Selected)
            {
                foundObjects = Selection.gameObjects.Select(x => new FoundObject(x)).ToList();

                return;
            }
            if (searchTerm.Length == 0)
            {
                return;
            }
            //using search box
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                string searchField = GetObjectSearchField(obj);
                if (searchRegex && IsValidRegex(searchTerm))
                {
                    if (matchCase)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(searchField, searchTerm))
                        {
                            foundObjects.Add(new FoundObject(obj));
                        }
                    }
                    else
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(searchField, searchTerm, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            foundObjects.Add(new FoundObject(obj));
                        }

                    }
                }
                else
                {
                    if (matchCase)
                    {
                        if (searchField.Contains(searchTerm))
                        {
                            foundObjects.Add(new FoundObject(obj));
                        }
                    }
                    else
                    {
                        if (searchField.ToLower().Contains(searchTerm.ToLower()))
                        {
                            foundObjects.Add(new FoundObject(obj));
                        }
                    }

                }
            }
            if (searchShowInHierarchy)
            {

                Selection.objects = Found.Select(x => x.go).ToArray();

            }
        }
        string GetObjectSearchField(GameObject obj)
        {
            return searchMode switch
            {
                SearchMode.Name => obj.name,
                SearchMode.Tag => obj.tag,
                SearchMode.Layer => LayerMask.LayerToName(obj.layer),
                _ => obj.name,
            };
        }
        List<FoundObject> Found
        {
            get
            {
                //sort by hierarchy
                List<FoundObject> objs = foundObjects.Where(x => !hiddenObjects.Contains(x.go)).ToList();

                //sort by name
                if (objs.Count > 1)
                {
                    objs.Sort((x, y) => x.go.name.CompareTo(y.go.name));
                    objs.Sort((x, y) => x.go.transform.GetSiblingIndex().CompareTo(y.go.transform.GetSiblingIndex()));
                }
                return objs;
            }
        }

        List<string> AllNewNames()
        {
            List<string> names = new List<string>();

            List<FoundObject> list = Found;
            foreach (FoundObject fo in list)
            {
                names.Add(GetRename(fo, false));
            }
            timesNameUsed = names.GroupBy(x => x).ToDictionary(x => x.Key, x => 0);
            return names;
        }
        string GetRename(FoundObject fo, bool allowIndexing = true)
        {


            string newName = GetNewName(fo);
            newName = caseMode switch
            {
                CaseMode.Lower => newName.ToLower(),
                CaseMode.Upper => newName.ToUpper(),
                CaseMode.Title => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName),
                _ => newName,
            };
            if (allowIndexing)
            {
                if (indexMode == IndexMode.Relative)
                {
                    //only count where the new names are identical before this one
                    int index = 0;
                    if (timesNameUsed.ContainsKey(newName))
                    {
                        index = timesNameUsed[newName]++;
                    }

                    index++;
                    newName += $" ({index})";
                }
                else if (indexMode == IndexMode.Absolute)
                {
                    int index = Found.IndexOf(fo);
                    index++;
                    newName += $" ({index})";
                }
            }
            return newName;
        }
        string GetNewName(FoundObject fo)
        {
            //alow replacing input with $1 if searchmode is NAME
            //new name must consider regex $1

            if (mode == Mode.Prepend)
            {
                // if (searchRegex && searchMode == SearchMode.Name && IsValidRegex(searchTerm))
                // {

                //     return System.Text.RegularExpressions.Regex.Replace(fo.go.name, searchTerm, newNameInput + "$1");
                // }
                // else
                // {
                return newNameInput + fo.go.name;

                //}
            }
            else if (mode == Mode.Append)
            {
                // if (searchRegex && searchMode == SearchMode.Name && IsValidRegex(searchTerm))
                // {
                //     return System.Text.RegularExpressions.Regex.Replace(fo.go.name, searchTerm, @"$1" + newNameInput);
                // }
                // else
                // {
                return fo.go.name + newNameInput;
                //}
            }
            else if (mode == Mode.Replace && replaceTerm.Length > 0)
            {
                //check if regex and match case
                if (replaceRegex && replaceMatchCase && IsValidRegex(replaceTerm))
                {
                    return System.Text.RegularExpressions.Regex.Replace(fo.go.name, replaceTerm, newNameInput);
                }
                else if (replaceRegex && !replaceMatchCase && IsValidRegex(replaceTerm))
                {
                    return System.Text.RegularExpressions.Regex.Replace(fo.go.name, replaceTerm, newNameInput, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
                else if (!replaceRegex && replaceMatchCase)
                {
                    return fo.go.name.Replace(replaceTerm, newNameInput);
                }
                else // !replaceRegex && !replaceMatchCase
                {
                    string escapeSpecialChars = System.Text.RegularExpressions.Regex.Escape(replaceTerm);
                    if (IsValidRegex(escapeSpecialChars))
                        return System.Text.RegularExpressions.Regex.Replace(fo.go.name, escapeSpecialChars, newNameInput, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }


            }

            return fo.go.name;


        }
        bool IsValidRegex(string pattern)
        {
            try
            {
                System.Text.RegularExpressions.Regex.Match("", pattern);
            }
            catch (System.ArgumentException)
            {
                return false;
            }

            return true;
        }
        //update all the time




    }
    class FoundObject
    {
        public GameObject go;
        public FoundObject(GameObject go)
        {
            this.go = go;
        }

    }
}