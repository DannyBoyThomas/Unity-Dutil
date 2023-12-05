using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEditor;
using System.Linq;
public class EasyPlacer : EditorWindow
{
    float gap = 0;
    // [MenuItem("Dutil/Easy Placer %&p")]
    // static void Init()
    // {
    //     // Get existing open window or if none, make a new one:
    //     EasyPlacer window = (EasyPlacer)EditorWindow.GetWindow(typeof(EasyPlacer), false, "Easy Placer");
    //     window.Show();
    // }
    void OnGUI()
    {
        Color oldBG = GUI.backgroundColor;

        List<GameObject> selected = Selection.gameObjects.ToList();


        //MOVE
        EditorGUILayout.BeginVertical("Box");
        //label
        EditorGUILayout.LabelField("Move", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Floor", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            DutilTools.SnapToFloor();
        }


        if (GUILayout.Button("Align", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            Align(selected);
        }
        if (GUILayout.Button("Snap", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            gap = 0;
            Snap(selected);
        }

        EditorGUILayout.EndHorizontal();

        float beforeGap = gap;
        gap = EditorGUILayout.FloatField("Gap", gap);
        if (beforeGap != gap)
        {
            Snap(selected, gap);
        }
        //Snap box
        EditorGUILayout.BeginVertical("Box");
        //label
        EditorGUILayout.LabelField("Snap to surface", EditorStyles.label);
        EditorGUILayout.BeginHorizontal();
        //positive axis buttons
        if (GUILayout.Button("X+", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, Vector3.right);
        }
        if (GUILayout.Button("Y+", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, Vector3.up);
        }
        if (GUILayout.Button("Z+", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, Vector3.forward);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        //negative axis buttons
        if (GUILayout.Button("X-", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, -Vector3.right);
        }
        if (GUILayout.Button("Y-", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, -Vector3.up);
        }
        if (GUILayout.Button("Z-", new GUILayoutOption[] { GUILayout.Height(24) }))
        {
            SnapToSurface(selected, -Vector3.forward);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        //end of snap box

        EditorGUILayout.EndVertical();



        //GENERATE
        EditorGUILayout.BeginVertical("Box");
        //label
        EditorGUILayout.LabelField("Generate", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

    }
    void SnapToSurface(List<GameObject> list, Vector3 axis)
    {
        Undo.RecordObjects(list.ToArray(), "Snap to surface");
        foreach (GameObject go in list)
        {
            Renderer rend = go.GetComponent<Renderer>();
            if (rend != null)
            {
                float dist = rend.bounds.extents.magnitude;
                RaycastHit hit;
                Vector3 localAxis = go.transform.InverseTransformDirection(axis);
                if (Physics.Raycast(go.transform.position, localAxis, out hit))
                {
                    go.transform.position = hit.point - localAxis * dist / 2f;
                }
            }
        }
    }
    void Snap(List<GameObject> list, float offset = 0)
    {
        if (list.Count <= 1)
        {
            return;
        }
        Vector3 minBounds;
        Vector3 maxBounds;
        GetBounds(list, out minBounds, out maxBounds);

        Vector3 average = list.Select(x => x.transform.position).ToList().Average();
        //which axis is the longest?
        Vector3 axis = GetAxis(minBounds, maxBounds);
        Undo.RecordObjects(list.ToArray(), "Snap");
        //order by postition
        list = list.OrderBy(x =>
        {
            if (axis == Vector3.right)
            {
                return x.transform.position.x;
            }
            else if (axis == Vector3.up)
            {
                return x.transform.position.y;
            }
            else if (axis == Vector3.forward)
            {
                return x.transform.position.z;
            }
            return 0;

        }).ToList();
        Transform current = list[0].transform;
        Vector3 currentMaxBounds = current.GetComponent<Renderer>().bounds.max;
        for (int i = 1; i < list.Count; i++)
        {
            Transform t = list[i].transform;
            Vector3 thisMinBounds = t.GetComponent<Renderer>().bounds.min;
            if (axis == Vector3.right)
            {
                float dif = thisMinBounds.x - currentMaxBounds.x;
                t.position = new Vector3(t.position.x - dif, t.position.y, t.position.z);
            }
            else if (axis == Vector3.up)
            {
                float dif = thisMinBounds.y - currentMaxBounds.y;
                t.position = new Vector3(t.position.x, t.position.y - dif, t.position.z);
            }
            else if (axis == Vector3.forward)
            {
                float dif = thisMinBounds.z - currentMaxBounds.z;
                t.position = new Vector3(t.position.x, t.position.y, t.position.z - dif);
            }
            t.position += axis * offset;
            current = t;
            currentMaxBounds = current.GetComponent<Renderer>().bounds.max;

        }
    }
    Vector3 GetAxis(Vector3 minBounds, Vector3 maxBounds)
    {
        Vector3 axis = Vector3.zero;
        //which axis is the longest?
        float x = maxBounds.x - minBounds.x;
        float y = maxBounds.y - minBounds.y;
        float z = maxBounds.z - minBounds.z;
        if (x > y && x > z)
        {
            axis = Vector3.right;
        }
        else if (y > x && y > z)
        {
            axis = Vector3.up;
        }
        else if (z > x && z > y)
        {
            axis = Vector3.forward;
        }
        return axis;
    }
    void Align(List<GameObject> list)
    {
        if (list.Count <= 1)
        {
            return;
        }
        Vector3 minBounds;
        Vector3 maxBounds;
        GetBounds(list, out minBounds, out maxBounds);
        Vector3 axis = Vector3.zero;
        Vector3 average = list.Select(x => x.transform.position).ToList().Average();
        //which axis is the longest?
        float x = maxBounds.x - minBounds.x;
        float y = maxBounds.y - minBounds.y;
        float z = maxBounds.z - minBounds.z;
        Undo.RecordObjects(list.ToArray(), "Align");
        for (int i = 0; i < list.Count; i++)
        {
            Transform t = list[i].transform;

            if (x > y && x > z)
            {
                axis = Vector3.right;
                //align other 2 axis z, and y
                t.position = new Vector3(t.position.x, average.y, average.z);

            }
            else if (y > x && y > z)
            {
                axis = Vector3.up;
                //align other 2 axis z, and x
                t.position = new Vector3(average.x, t.position.y, average.z);
            }
            else if (z > x && z > y)
            {
                axis = Vector3.forward;
                //align other 2 axis x, and y
                t.position = new Vector3(average.x, average.y, t.position.z);
            }
        }
    }
    void GetBounds(List<GameObject> list, out Vector3 minBounds, out Vector3 maxBounds)
    {
        minBounds = Vector3.zero;
        maxBounds = Vector3.zero;
        foreach (GameObject go in list)
        {
            if (go.GetComponent<Renderer>() != null)
            {
                if (minBounds == Vector3.zero)
                {
                    minBounds = go.GetComponent<Renderer>().bounds.min;
                    maxBounds = go.GetComponent<Renderer>().bounds.max;
                }
                else
                {
                    minBounds = Vector3.Min(minBounds, go.GetComponent<Renderer>().bounds.min);
                    maxBounds = Vector3.Max(maxBounds, go.GetComponent<Renderer>().bounds.max);
                }
            }
        }
    }
}