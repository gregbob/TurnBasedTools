using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor {


    private Grid _grid;
    private Vector3[] _selectedQuad;


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
            Mesh m = hit.collider.GetComponent<GridBlock>().GetComponent<MeshFilter>().mesh;
            Vector3[] verts = m.vertices;
            Vector3[] normals = m.normals;
            Vector2[] uvs = m.uv;
            _selectedQuad = FindQuadOfBoxCollider(hit.transform, hit.normal, verts, normals);

            //uvs[0] = uvs[3];
            //uvs[3] = uvs[2];
            //uvs[2] = uvs[1];
            //uvs[1] = temp;
            //m.uv = uvs;


        }

        if (_selectedQuad != null)
        {
            Color c = Color.green;
            c.a = .25f;
            Handles.DrawSolidRectangleWithOutline(_selectedQuad, c, Color.black);
            //Debug.Log(_selectedQuad)

        }

        if (_grid != null && _grid.Rows > 0)
        {
            //for (int row = 0; row < _grid.Rows; row++)
            //{
            //    for (int col = 0; col < _grid.Columns; col++)
            //    {
            //        CreateBlockVertCap(_grid[row, col]);
            //    }
            //}

            //DebugNormals(_grid[0, 0]);  

        }




    }

    private bool CreateBlockVertCap(GridBlock block)
    {

        Mesh m = block.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = m.vertices;
        Vector3[] normals = m.normals;

        
        return true;

        //return Handles.Button(block.transform.TransformPoint(verts[0]), Quaternion.identity, .1f, .1f, Handles.CubeHandleCap) ||
        //    Handles.Button(block.transform.TransformPoint(verts[1]), Quaternion.identity, .1f, .1f, Handles.CubeHandleCap) ||
        //    Handles.Button(block.transform.TransformPoint(verts[2]), Quaternion.identity, .1f, .1f, Handles.CubeHandleCap) ||
        //    Handles.Button(block.transform.TransformPoint(verts[3]), Quaternion.identity, .1f, .1f, Handles.CubeHandleCap);
    }

    private void DebugNormals(GridBlock block)
    {
        Mesh m = block.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = m.vertices;
        Vector3[] normals = m.normals;
        Vector2[] uvs = m.uv;
        Debug.Log(uvs.Length);

        for (int i = 0; i < verts.Length; i++)
        {
            Color temp = Handles.color;
            Handles.color = Color.blue;
            Handles.DrawLine(block.transform.TransformPoint(verts[i]), block.transform.TransformPoint(verts[i] + normals[i]));
            Handles.color = temp;
        }
    }

    private Vector3[] FindQuadOfBoxCollider(Transform t, Vector3 normal, Vector3[] verts, Vector3[] normals)
    {
        Vector3[] quadVerts = new Vector3[4];
        int count = 0;
        for(int i = 0; i < normals.Length; i++)
        {
            if (normals[i] == normal)
            {
                quadVerts[count] = t.TransformPoint(verts[i]);
                count++;
            }
        }

        return quadVerts;
    }



}
