using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData: ScriptableObject
{
    public  static int Width = 10;
    public  static int Height = 20;
    public static Transform[,] Grid = new Transform[Width, Height];

    public float FallTime = 0.5f;
    public List<Level> Levels = new List<Level>();
}
[System.Serializable]
public struct Level
{
    public Sprite[] Sprites ;
}