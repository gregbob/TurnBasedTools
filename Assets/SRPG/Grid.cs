using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    private GridBlock[] _grid;
    private int _rows; // Number of columns
    private int _cols; // Number of rows

    [SerializeField]
    private GridBlock defaultBlock;

    public int Rows
    {
        get { return _rows; }
    }

    public int Columns
    {
        get { return _cols; }
    }

    public GridBlock this[int row, int col]
    {
        get { return _grid[row * _cols + col]; }
        set { _grid[row * _cols + col] = value; }
    }

    void Start ()
    {
        GenerateGrid(1, 3);
    }

    public void Resize(int numRows, int numCols)
    {
        if (_grid == null)
        {           
            _grid = new GridBlock[numRows * numCols];
        }
        GridBlock[] newGrid = new GridBlock[numRows * numCols];
        int minX = Math.Min(_rows, numRows);
        int minY = Math.Min(_cols, numCols);

        for (int i = 0; i < minY; i++)
        {
            Array.Copy(_grid, i * _rows, newGrid, i * numRows, minX);
        }
        _grid = newGrid;
        _rows = numRows;
        _cols = numCols;
    }


    public void DestroyGrid()
    {
        if (_grid != null)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    Destroy(this[row, col].gameObject);
                }
            }
        }
    }

    public void GenerateGrid(int width, int length)
    {
        DestroyGrid();

        _grid = new GridBlock[width * length];
        _rows = width;
        _cols = length;
        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < length; col++)
            {
                GenerateBlock(row, col);
            }
        }
    }

    public void GenerateBlock(int row, int col)
    {
        GridBlock block = Instantiate(defaultBlock);
        block.transform.position = new Vector3(row, 0, col);
        block.row = row;
        block.col = col;
        block.grid = this;
        this[row, col] = block;
    }

    /*
    Add rows
    - Copy elements of the last row to the new row
        - a[(length - 1) * width + j] = a[(length + 1) * width + j]
    */
    public void AddRow()
    {
        Resize(_rows + 1, _cols);
        Debug.Log(_rows + " " + _cols);
        for (int col = 0; col < _cols; col++)
        {
            GenerateBlock(_rows - 1, col);
        }
    }

    public void AddColumn()
    {

    }

    public void OnBlockClick(GridBlock block)
    {
        Debug.Log("Clicked " + block.row + ", " + block.col);
    }

}
