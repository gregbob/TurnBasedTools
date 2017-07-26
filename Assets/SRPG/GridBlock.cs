using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridBlock : MonoBehaviour {

    public bool IsVisitable;
    public int row;
    public int col;
    public Collider coll;
    public Grid grid;

    public void OnMouseDown()
    {
        grid.OnBlockClick(this);
    }
}
