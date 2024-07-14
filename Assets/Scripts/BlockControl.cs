
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationPoint = Vector3.zero;
    [SerializeField] private GameData _gameData;
    private float _previousTime = 0;
    private int _currentLevel = 0;
    private bool _gameOver = false;
    void Start() => SetColor();
    void Update()
    {
        MoveLeftRight();
        RotateUp();
        MoveDown();
    }

    private void SetColor()
    {
        _currentLevel = FindObjectOfType<GameManager>()._level / 10;
        int color = Random.Range(0, _gameData.Levels[_currentLevel].Sprites.Length);
        foreach (Transform children in transform)
            children.GetComponent<SpriteRenderer>().sprite = _gameData.Levels[_currentLevel].Sprites[color];
    }
    private void MoveLeftRight()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!CheckMovable()) transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!CheckMovable()) transform.position -= new Vector3(1, 0, 0);
        }

    }
    private void MoveDown()
    {
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow) ? _gameData.FallTime / 10 : _gameData.FallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!CheckMovable())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckRows();
                this.enabled = false;
                if(!_gameOver)
                    FindObjectOfType<Spawner>().SpawnBlock();
            }
            _previousTime = Time.time;
        }
    }
    private bool CheckMovable()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedX < 0 || roundedY < 0 || roundedY >= GameData.Height || roundedX >= GameData.Width)
                return false;
            if (GameData.Grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }
    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            // Debug.Log("Grid: " + GameData.Grid.Length);
            // Debug.Log("X: " + roundedX + ", Y: " + roundedY);
            if (roundedY >= GameData.Height)
            {
                _gameOver = true;
                this.enabled = false;
                FindObjectOfType<GameManager>().ShowGameOver();
            }
            else
                GameData.Grid[roundedX, roundedY] = children;
        }
    }
    private void CheckRows()
    {
        int lineCount = 0;
        for (int rowNumber = GameData.Height - 1; rowNumber >= 0; rowNumber--)
        {
            if (HasFullRow(rowNumber))
            {
                RemoveRow(rowNumber);
                DeclineRow(rowNumber);
                FindObjectOfType<GameManager>().UpdateScore(10);
                lineCount++;
            }
        }

        if (lineCount > 0) FindObjectOfType<GameManager>().UpdateLines(lineCount);
    }

    private void DeclineRow(int row)
    {
        for (int currentRow = row; currentRow < GameData.Height; currentRow++)
        {
            for (int column = 0; column < GameData.Width; column++)
            {
                if (GameData.Grid[column, currentRow] != null)
                {
                    GameData.Grid[column, currentRow - 1] = GameData.Grid[column, currentRow];
                    GameData.Grid[column, currentRow] = null;
                    GameData.Grid[column, currentRow - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void RemoveRow(int row)
    {
        for (int column = 0; column < GameData.Width; column++)
        {
            Destroy(GameData.Grid[column, row].gameObject);
            GameData.Grid[column, row] = null;
        }
    }

    private bool HasFullRow(int row)
    {
        for (int column = 0; column < GameData.Width; column++)
        {
            if (GameData.Grid[column, row] == null) return false;
        }
        return true;
    }

    private void RotateUp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
            RotateChildren(90);
            if (!CheckMovable())
            {
                transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
                RotateChildren(-90);
            }
        }
    }
    private void RotateChildren(sbyte rotateAmount)
    {
        foreach (Transform children in transform)
            children.transform.RotateAround(children.transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -rotateAmount);
    }
}
