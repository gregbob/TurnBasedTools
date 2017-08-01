using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gregbob
{

    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {

        
        
        private Grid _grid;
        private Vector3 test;

        private string _width;
        private string _height;

        private Selector _selector;


        void OnEnable()
        {
            _grid = target as Grid;
            SetGridBlockSelectionStyle(EditorSelectedRenderState.Hidden);
            Tools.hidden = true;
            _selector = new FaceSelector();
        }

        void OnDisable()
        {
            SetGridBlockSelectionStyle(EditorSelectedRenderState.Highlight);
            Tools.hidden = false;
        }

        void OnSceneGUI()
        {
            Event e = Event.current;


            _selector.OnSceneView();


            Handles.BeginGUI();

            GUILayout.Window(3, new Rect(0, 16, 100, Screen.height - 40), (id) =>
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("Rectangle select"))
                {
                    _selector.SetSelectionMode(Selector.SelectionMode.Rectangle);
                }
                if (GUILayout.Button("Additive select"))
                {
                    _selector.SetSelectionMode(Selector.SelectionMode.Additive);

                }

                if (GUILayout.Button("Create grid"))
                {
                    _grid.GenerateGrid(int.Parse(_width), int.Parse(_height));
                    SetGridBlockSelectionStyle(EditorSelectedRenderState.Hidden);

                }
                _width = GUILayout.TextField(_width);
                _height = GUILayout.TextField(_height);
                GUILayout.EndVertical();
            }, "Toolbar");
           

            Handles.EndGUI();




        }

     

        private void SetGridBlockSelectionStyle(EditorSelectedRenderState renderState)
        {
            if (_grid == null)
            {
                return;
            }

            for (int i = 0; i < _grid.Columns; i++)
            {
                for (int j = 0; j < _grid.Rows; j++)
                {
                    EditorUtility.SetSelectedRenderState(_grid[i, j].GetComponent<Renderer>(), renderState);
                }
            }
        }

        //private Rotate
        //uvs[0] = uvs[3];
        //uvs[3] = uvs[2];
        //uvs[2] = uvs[1];
        //uvs[1] = temp;
        //m.uv = uvs;



    }

}
