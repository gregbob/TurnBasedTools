using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor {


    private Grid _grid;

    void OnEnable()
    {
        _grid = target as Grid;
        //Debug.Log(_grid)
    }

    void OnSceneGUI ()
    {

        RaycastHit hit;
        if (EditorUtils.SceneViewMouseRaycast(this, Event.current, out hit))
        {
            Debug.Log(hit.normal);
        }

        if (_grid != null && _grid.Rows > 0)
        {
            if (Handles.Button(_grid[0, 0].GetComponent<MeshFilter>().sharedMesh.vertices[0], Quaternion.identity, .1f, .1f, Handles.CubeCap))
            {
                _grid.AddRow();
            }

        }


    }



}
