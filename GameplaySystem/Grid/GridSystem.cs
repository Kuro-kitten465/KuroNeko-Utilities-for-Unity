using UnityEngine;

public class GridSystem
{
    private int _width = 0;
    private int _height = 0;
    private float _cellSize = 1f;
    private Vector3 _originPosition = Vector3.zero;
    private object[,] _gridArray;
    private bool _showDebug = true;
    public bool ShowDebug
    {
        get => _showDebug;
        set 
        {
            _showDebug = value;
            if (_showDebug)
            {
                DrawGrid();
            }
        }
    }

    public GridSystem(int width, int height, float cellSize = 1f, Vector3 originPosition = default)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        _gridArray = new object[_width, _height];
    }

    public void InitializeGrid(object gridObject, bool isUnityObject)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (isUnityObject)
                {
                    var obj = Object.Instantiate((GameObject)gridObject, GetWorldPosition(x, y), Quaternion.identity);
                    obj.name = $"{obj.name} ({x}, {y})";
                    _gridArray[x, y] = obj;
                }
                else
                {
                    _gridArray[x, y] = gridObject;
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void SetValue(int x, int y, object value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }

    public object GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _gridArray[x, y];
        }
        else
        {
            return null;
        }
    }

    public void SetValueByWorldPosition(Vector3 worldPosition, object value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public object GetValueByWorldPosition(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    private void DrawGrid()
    {
        if (!ShowDebug) return;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
    }
}
