using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSpawner : MonoBehaviour
{
    //List of all the tetrominoes the spawner can spawn
    public GameObject[] Tetrominoes;

    //Function that spawn a new tetromino
    public void NewTetromino()
    {
        //Choose a tetromino randomly from the list and instantiate it to the position of the spawner
        Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }
}
