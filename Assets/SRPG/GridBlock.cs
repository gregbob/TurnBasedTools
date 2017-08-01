using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridBlock : MonoBehaviour {

    public bool IsVisitable;
    public int row;
    public int col;
    public Grid grid;

    public void OnMouseDown()
    {
        grid.OnBlockClick(this);
    }

    public override string ToString()
    {
        return string.Format("Col: {0}, Row: {1}", col, row);
    }

    public override bool Equals(object other)
    {
        GridBlock g = (GridBlock)other;
        return g.row == row && g.col == col;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
