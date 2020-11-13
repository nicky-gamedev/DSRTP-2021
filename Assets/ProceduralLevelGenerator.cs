using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script assumes that their position and rotation are
//In the height and direction in X of the level to be generated
public class ProceduralLevelGenerator : MonoBehaviour
{
    #region Variables
    [SerializeField] [Tooltip("Number of iterations of the generator (more = longer level)")] int numberOfIterations;
    [SerializeField] [Tooltip("Chance of instantiate puzzles")] [Range(0, 100)] int puzzleChance;
    [SerializeField] [Tooltip("Offset between objects")] Vector3 offset;

    [Header("Content")]
    [SerializeField] GameObject floor;
    [SerializeField] GameObject platform;
    [SerializeField] List<GameObject> levelPrefabs;
    [SerializeField] List<GameObject> puzzlePrefabs;
    #endregion

    void Start()
    {
        Debug.Log("Starting Level generation");

        int floorSeq = 0;
        int puzzleSeq = 0;

        for (int i = 0; i < numberOfIterations; i++)
        {
            Debug.Log("Instantiating...");
            GameObject _floor = Instantiate(floor, transform);
            var collider = floor.transform.GetChild(0).gameObject.GetComponent<Collider>().bounds.max;
            _floor.transform.position = new Vector3(transform.position.x + collider.x * i, transform.position.y, transform.position.z);


            int probability = Random.Range(0, 100);

            if (probability < puzzleChance && puzzleSeq < 2)
            {
                GameObject puzzle = Instantiate(puzzlePrefabs[Random.Range(0, puzzlePrefabs.Count)], _floor.transform);
                puzzleSeq++;
                floorSeq = 0;
            }
            else if(floorSeq < 2)
            {
                GameObject level = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Count)], _floor.transform);
                puzzleSeq = 0;
                floorSeq++;
            }
            else
            {
                Debug.LogWarning("Both variables are greater than 2, something went wrong?");
            }
        }
        Debug.Log("Finished Level generation, with a max sequence of " + puzzleSeq + "puzzles and " + floorSeq + " levels");
    }
}
