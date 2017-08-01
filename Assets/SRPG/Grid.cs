using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Grid : MonoBehaviour {


    

    [SerializeField]
    private GridBlock[] _grid;
    private int _rows; // Number of columns
    private int _cols; // Number of rows

    [SerializeField]
    private GridBlock defaultBlock;

    [SerializeField]
    private GameObject _blockContainer;

    public int Rows
    {
        get { return _rows; }
    }

    public int Columns
    {
        get { return _cols; }
    }

    public GridBlock this[int col, int row]
    {
        get { return _grid[row * _cols + col]; }
        set { _grid[row * _cols + col] = value; }
    }

    void Start ()
    {
        GenerateGrid(1, 3);
    }

    public void Resize(int numCols, int numRows)
    {
        if (_grid == null)
        {           
            _grid = new GridBlock[numRows * numCols];
        }
        GridBlock[] newGrid = new GridBlock[numRows * numCols];
        int minX = Math.Min(_cols, numCols);
        int minY = Math.Min(_rows, numRows);

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

        if (_grid != null && _blockContainer != null)
        {
            DestroyImmediate(_blockContainer);
        }
    }

    public void GenerateGrid(int numCols, int numRows)
    {
        DestroyGrid();
        _blockContainer = new GameObject("Blocks");
        _blockContainer.transform.SetParent(transform);

        _grid = new GridBlock[numCols * numRows];
        _rows = numRows;
        _cols = numCols;
        for (int col = 0; col < _cols; col++)
        {
            for (int row = 0; row < _rows; row++)
            {
                GenerateBlock(col, row).transform.SetParent(_blockContainer.transform);
            }
        }
    }

    public GridBlock GenerateBlock(int col, int row)
    {
        GridBlock block = Instantiate(defaultBlock);
        block.transform.position = new Vector3(col, 0, row);
        block.row = row;
        block.col = col;
        block.grid = this;
        this[col, row] = block;
        return block;
    }

    /*
    Add rows
    - Copy elements of the last row to the new row
        - a[(length - 1) * width + j] = a[(length + 1) * width + j]
    */
    public void AddRow()
    {
        //Resize(_rows + 1, _cols);
        //Debug.Log(_rows + " " + _cols);
        //for (int col = 0; col < _cols; col++)
        //{
        //    GenerateBlock(_rows - 1, col);
        //}
    }

    public void AddColumn()
    {

    }

    public void OnBlockClick(GridBlock block)
    {
        Debug.Log("Clicked " + block.row + ", " + block.col);
    }

}
