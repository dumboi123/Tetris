
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Blocks;
    private int _randomNumber = 0;

    void Start()
    {
        GenerateRandomNumb();
        SpawnBlock();
    }
    
    public void GenerateRandomNumb() =>_randomNumber = Random.Range(0, Blocks.Length);
    public void SpawnBlock()
    {
        //int randomNumber = Random.Range(0, Blocks.Length);
        FindObjectOfType<GameManager>().UpdateStatistics(_randomNumber);
        Instantiate(Blocks[_randomNumber], transform.position, Quaternion.identity);
        GenerateRandomNumb();
        FindObjectOfType<GameManager>().UpdateNext(_randomNumber);
    }

}
