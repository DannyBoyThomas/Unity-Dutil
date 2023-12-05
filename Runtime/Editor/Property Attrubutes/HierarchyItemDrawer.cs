using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEditor;
[UnityEditor.InitializeOnLoad]
public class HierarchyItemDrawer
{

    static HierarchyItemDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierachyWindowItemOnGUI;
    }


    public static void HandleHierachyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj == null)
            return;
        //Does object have attribute of HierachyColorAttribute?
        var type = obj.GetType();

        //Does any component on this object contain HierarchyColorAttribute?
        var components = (obj as GameObject).GetComponents<Component>();
        foreach (var component in components)
        {
            if (component == null) { return; }
            var componentType = component.GetType();
            if (component is CustomHierarchyItem)
            {
                DrawCustomHierarchyItem(component as CustomHierarchyItem, selectionRect);
                //return;
            }

            var attributes = componentType.GetCustomAttributes(typeof(HierarchyItem), true);
            if (attributes.Length == 0)
                continue;
            //Get the color from the attribute
            var colorAttribute = attributes[0] as HierarchyItem;
            var color = colorAttribute.Color;
            //Draw the background
            var position = new Rect(selectionRect.x + selectionRect.width, selectionRect.y, 10, selectionRect.height - 1);
            EditorGUI.DrawRect(position, color);
            //EditorGUI.LabelField(selectionRect, obj.name, new GUIStyle()
            //{
            //	normal = new GUIStyleState() { textColor = colorAttribute.TextColor }
            //});

        }

        //var attributes = type.GetCustomAttributes(typeof(HierarchyColorAttribute), true);
        //Debug.Log(attributes.Length);
        //if (attributes.Length == 0)
        //	return;
        ////Get the color from the attribute
        //var colorAttribute = attributes[0] as HierarchyColorAttribute;
        //var color = colorAttribute.Color;
        ////Draw the background
        //	var position = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
        //EditorGUI.DrawRect(selectionRect, color);


    }

    static void DrawCustomHierarchyItem(CustomHierarchyItem item, Rect selectionRect)
    {
        var position = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
        EditorGUI.DrawRect(position, item.backgroundColor);
        //text
        TextAnchor alignment = item.isDivider ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        GUIStyle labelGUIStyle = new GUIStyle
        {
            normal = new GUIStyleState { textColor = item.textColor },
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            alignment = alignment,
            font = item.font
        };
        float xPos = selectionRect.position.x + 18f;
        float yPos = selectionRect.position.y;
        float xSize = selectionRect.size.x - 18f;
        float ySize = selectionRect.size.y;
        Rect textRect = new Rect(xPos, yPos, xSize, ySize);
        EditorGUI.LabelField(textRect, item.name.ToUpper(), labelGUIStyle);


    }
}