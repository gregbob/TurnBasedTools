using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorUtils {

    public static bool SceneViewMouseRaycast(Object context, Event e, out RaycastHit hit)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(context.GetHashCode(), FocusType.Passive));

        if (e.type == EventType.mouseDown && e.button == 0)
        {
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //e.Use();
                return true;
            }
        }
        hit = default(RaycastHit);
        return false;
    }
}
