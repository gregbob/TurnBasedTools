using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gregbob
{
    public class FaceSelector : Selector
    {

        private List<Selection> _selected;
        private SelectionMode _mode;

        public override Selection[] Selected
        {
            get
            {
                return _selected.ToArray();
            }
        }

        public FaceSelector()
        {
            _selected = new List<Selection>();
            _mode = SelectionMode.Rectangle;
        }

        public override void Deselect()
        {
            _selected.Clear();
        }

        public override void OnSceneView()
        {
            Select();
            DisplaySelection();
        }

        public override void Select()
        {
            Event e = Event.current;
            RaycastHit hit;

            if (EditorUtils.SceneViewMouseRaycast(GetHashCode(), e, out hit))
            {
                GridBlock block = hit.collider.GetComponent<GridBlock>();
                if (block == null)
                    return;
                Selection s = new Selection(block, hit.normal);

                if ((e.type == EventType.MouseDown && e.button == 0))
                {
                    if (_mode == SelectionMode.Rectangle)
                    {
                        Deselect();
                    }

                    if (!_selected.Contains(s))
                    {
                        _selected.Add(s);
                        Debug.Log("adding");
                    }
                    e.Use();
                } else if (e.type == EventType.MouseDrag && e.button == 0)
                {
                    if (_mode == SelectionMode.Rectangle)
                    {
                        if (_selected.Count > 1)
                        {
                            _selected.RemoveAt(1);
                        }
                        _selected.Insert(1, s);
                    } else
                    {
                        if (!_selected.Contains(s))
                        {
                            _selected.Add(s);
                        }
                    }
                    e.Use();
                }
            } else if ((e.type == EventType.MouseDown && e.button == 0)) // Clicked on nothing
            {
                Deselect();
                e.Use();
            }
        }

        public override void DisplaySelection ()
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Color c = Color.green;
            c.a = .25f;

            if (_mode == SelectionMode.Rectangle)
            {
                if (_selected.Count > 0)
                {
                    Handles.DrawSolidRectangleWithOutline(FindQuad(), c, Color.black);
                }
                    
            } else
            {
                foreach (Selection s in _selected)
                {
                    Handles.DrawSolidRectangleWithOutline(s.WorldCoordinatesOfFace(), c, Color.black);
                }
            }
             
            

            

        }

        public override void SetSelectionMode(SelectionMode mode)
        {
            Deselect();
            _mode = mode;
        }

        private Vector3[] FindQuad()
        {
   
            Vector3[] quad = new Vector3[4];
            List<Vector3> verts = new List<Vector3>();
            verts.AddRange(_selected[0].WorldCoordinatesOfFace());

            // Makes sure all faces in quad are on the same plane and axis
            if (_selected.Count > 1 && (_selected[0].face.Normal == _selected[1].face.Normal))
            {
                verts.AddRange(_selected[1].WorldCoordinatesOfFace());
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
            if (Mathf.Abs(_selected[0].face.Normal.x) == 1)
            {
                quad[0] = new Vector3(minX, minY, minZ);
                quad[1] = new Vector3(minX, minY, maxZ);
                quad[2] = new Vector3(minX, maxY, maxZ);
                quad[3] = new Vector3(minX, maxY, minZ);
            }
            // XZ plane
            else if (Mathf.Abs(_selected[0].face.Normal.y) == 1)
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
    }

}
