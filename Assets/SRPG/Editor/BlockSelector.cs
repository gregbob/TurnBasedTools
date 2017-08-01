using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gregbob
{
    public class BlockSelector
    {
        public struct BlockSelection
        {
            public GridBlock Block;
            public Vector3 Normal;

            public BlockSelection(GridBlock block, Vector3 normal)
            {
                this.Block = block;
                this.Normal = normal;
            }
        }

        public class FaceSelection
        {
            public GridBlock Block;
            public Vector3 Normal;
            public Vector3[] Vertices;

            public FaceSelection(GridBlock block, Vector3 normal)
            {
                Block = block;
                Normal = normal;
                FindVerticies();
            }

            private void FindVerticies()
            {
                Mesh m = Block.GetComponent<MeshFilter>().sharedMesh;
                Vector3[] verts = m.vertices;
                Vector3[] normals = m.normals;
                Vector2[] uvs = m.uv;
                Vector3[] quadVerts = new Vector3[4];
                int count = 0;
                for (int i = 0; i < normals.Length; i++)
                {
                    if (normals[i] == Normal)
                    {
                        quadVerts[count] = Block.transform.TransformPoint(verts[i]);
                        count++;
                    }
                }

                Vertices = quadVerts;
            }

        }


        public enum BlockSelectionMode { RECTFACE, RECTBLOCK, FACE }

        private FaceSelection[] _squareSel = new FaceSelection[2];
        private Vector3[] _selectedQuad;
        private List<BlockSelection> _selectedBlocks;
        private BlockSelectionMode _selectionMode;

        public BlockSelectionMode SelectionMode { get { return _selectionMode; } set { _selectionMode = value; } }
        //public GridBlock[] SelectedBlocks
        //{
        //    get
        //    {
        //        if (_selectionMode == BlockSelectionMode.RECTBLOCK || _selectionMode == BlockSelectionMode.RECTFACE)
        //        {
        //            throw new System.Exception("Not implemented");
        //        } else
        //        {
        //            return _selectedBlocks.ToArray();
        //        }
        //    }
        //}

        public BlockSelector()
        {
            _selectionMode = BlockSelectionMode.RECTFACE;
            //_selectedBlocks = new List<GridBlock>();

        }

        public void OnSceneView()
        {
            if (_selectionMode == BlockSelectionMode.RECTFACE)
            {
                HandleFaceSelect();
                DisplaySelectedFaces();
            }

        }



        private void HandleFaceSelect()
        {
            Event e = Event.current;
            RaycastHit hit;

            if (EditorUtils.SceneViewMouseRaycast(GetHashCode(), e, out hit))
            {

                if ((e.type == EventType.MouseDown && e.button == 0))
                {
                    GridBlock block = hit.collider.GetComponent<GridBlock>();
                    if (block == null)
                        return;

                    Deselect();
                    _squareSel[0] = new FaceSelection(block, hit.normal);


                    _selectedQuad = FindQuad();
                    e.Use();
                }
                else if (e.type == EventType.MouseDrag && e.button == 0)
                {
                    GridBlock block = hit.collider.GetComponent<GridBlock>();
                    if (block == null)
                        return;

                    _squareSel[1] = new FaceSelection(block, hit.normal);


                    _selectedQuad = FindQuad();
                    e.Use();
                }


            }
            else if (e.type == EventType.MouseDown && e.button == 0) // Click on nothing
            {
                Deselect();
                e.Use();
            }
        }

        private void DisplaySelectedFaces()
        {
            if (_selectedQuad != null && _selectedQuad.Length >= 4)
            {
                Color c = Color.green;
                c.a = .25f;
                Handles.DrawSolidRectangleWithOutline(_selectedQuad, c, Color.black);
            }
        }

        private void Deselect()
        {
            for (int i = 0; i < _squareSel.Length; i++)
            {
                _squareSel[i] = null;

            }
            _selectedQuad = null;
        }

        private Vector3[] FindQuad()
        {
            Vector3[] quad = new Vector3[4];
            List<Vector3> verts = new List<Vector3>();
            verts.AddRange(_squareSel[0].Vertices);

            // Makes sure all faces in quad are on the same plane and axis
            if (_squareSel[1] != null && (_squareSel[0].Normal == _squareSel[1].Normal))
            {
                verts.AddRange(_squareSel[1].Vertices);
            }
            float minX = float.MaxValue, maxX = float.MinValue, minZ = float.MaxValue, maxZ = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
            for (int i = 0; i < verts.Count; i++)
            {
                minX = Mathf.Min(minX, verts[i].x);
                minY = Mathf.Min(minY, verts[i].y);
                minZ = Mathf.Min(minZ, verts[i].z);
                maxX = Mathf.Max(maxX, verts[i].x);
                maxY = Mathf.Max(maxY, verts[i].y);
                maxZ = Mathf.Max(maxZ, verts[i].z);
            }

            // Determine plane
            // YZ plane
            if (Mathf.Abs(_squareSel[0].Normal.x) == 1)
            {
                quad[0] = new Vector3(minX, minY, minZ);
                quad[1] = new Vector3(minX, minY, maxZ);
                quad[2] = new Vector3(minX, maxY, maxZ);
                quad[3] = new Vector3(minX, maxY, minZ);
            }
            // XZ plane
            else if (Mathf.Abs(_squareSel[0].Normal.y) == 1)
            {
                quad[0] = new Vector3(minX, minY, minZ);
                quad[1] = new Vector3(minX, minY, maxZ);
                quad[2] = new Vector3(maxX, minY, maxZ);
                quad[3] = new Vector3(maxX, minY, minZ);
            }
            // XY plane
            else
            {
                quad[0] = new Vector3(minX, minY, minZ);
                quad[1] = new Vector3(minX, maxY, minZ);
                quad[2] = new Vector3(maxX, maxY, minZ);
                quad[3] = new Vector3(maxX, minY, minZ);
            }

            return quad;

        }

        //private void HandleBlockSelect()
        //{
        //    Event e = Event.current;
        //    RaycastHit hit;

        //    if (EditorUtils.SceneViewMouseRaycast(GetHashCode(), e, out hit))
        //    {
        //        if ((e.type == EventType.MouseDown && e.button == 0))
        //        {
        //            GridBlock block = hit.collider.GetComponent<GridBlock>();
        //            if (block == null)
        //                return;

        //            _squareSel[0].Block = block;
        //            e.Use();
        //        }
        //    }
        //}

        //private void DisplaySelectedBlocks()
        //{
        //    if (_squareSel[0].Block != null)
        //    {
        //        Color temp = Handles.color;
        //        Color c = Color.green;
        //        c.a = .5f;
        //        Handles.color = c;
        //        //Handles.DrawWireCube(_squareSel[0].Block.transform.position, _squareSel[0].Block.transform.localScale);
        //        Handles.CubeHandleCap(29, _squareSel[0].Block.transform.position, Quaternion.identity, 1, EventType.Repaint);
        //        Handles.color = temp;
        //    }

        //}

        //private Vector3[] FindQuadOfBoxCollider(BlockSelection selection)
        //{
        //    Mesh m = selection.Block.GetComponent<MeshFilter>().sharedMesh;
        //    Vector3[] verts = m.vertices;
        //    Vector3[] normals = m.normals;
        //    Vector2[] uvs = m.uv;
        //    Vector3[] quadVerts = new Vector3[4];
        //    int count = 0;
        //    for (int i = 0; i < normals.Length; i++)
        //    {
        //        if (normals[i] == selection.Normal)
        //        {
        //            quadVerts[count] = selection.Block.transform.TransformPoint(verts[i]);
        //            count++;
        //        }
        //    }

        //    return quadVerts;
        //}

    }

}

