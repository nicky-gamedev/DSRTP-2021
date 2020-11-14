using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] GameObject end;

    [Header("Navmesh Parameters")]
    [SerializeField] Vector3 BoundsCenter = Vector3.zero;
    [SerializeField] Vector3 BoundsSize = new Vector3(4000f, 20f, 20f);

    [SerializeField] LayerMask BuildMask;
    [SerializeField] LayerMask NullMask;

    [SerializeField] NavMeshData NavMeshData;
    [SerializeField] NavMeshDataInstance NavMeshDataInstance;
    #endregion

    void Start()
    {
        Debug.Log("Starting Level generation: " + Time.realtimeSinceStartup.ToString());

        int floorSeq = 0;
        int puzzleSeq = 0;

        for (int i = 0; i < numberOfIterations; i++)
        {
            Debug.Log("Instantiating...");
            GameObject _floor = Instantiate(floor, transform);
            var collider = floor.transform.GetChild(0).gameObject.GetComponent<Collider>().bounds.max;
            _floor.transform.position = new Vector3(transform.position.x + collider.x * i * 2, transform.position.y, transform.position.z);


            int probability = Random.Range(0, 100);

            if (probability < puzzleChance && puzzleSeq < 1)
            {
                GameObject puzzle = Instantiate(puzzlePrefabs[Random.Range(0, puzzlePrefabs.Count)], _floor.transform);
                puzzleSeq++;
                floorSeq = 0;
                continue;
            }
            else if(floorSeq < 1)
            {
                GameObject level = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Count)], _floor.transform);
                puzzleSeq = 0;
                floorSeq++;
                continue;
            }
            else
            {
                GameObject level = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Count)], _floor.transform);
                floorSeq = 0;
            }
        }
        Debug.Log("Instantiating final floor...");
        GameObject _end_floor = Instantiate(floor, transform);
        var _collider = floor.transform.GetChild(0).gameObject.GetComponent<Collider>().bounds.max;
        _end_floor.transform.position = new Vector3(transform.position.x + _collider.x * numberOfIterations * 2, transform.position.y, transform.position.z);
        Debug.Log("Finished Level generation : " + Time.realtimeSinceStartup.ToString());

        GameObject end_level = Instantiate(end, _end_floor.transform);

        GameManager gm = GameManager.instance;
        gm.timeRemaining = gm.fullTime;
        gm.counting = true;

        Debug.Log("Building Navmesh: " + Time.realtimeSinceStartup.ToString());
        Build();
        Debug.Log("Build finished " + Time.realtimeSinceStartup.ToString());
        UpdateNavmeshData();
        Debug.Log("Update " + Time.realtimeSinceStartup.ToString());
    }


    void AddNavMeshData()
    {
        if (NavMeshData != null)
        {
            if (NavMeshDataInstance.valid)
            {
                NavMesh.RemoveNavMeshData(NavMeshDataInstance);
            }
            NavMeshDataInstance = NavMesh.AddNavMeshData(NavMeshData);
        }
    }

    void UpdateNavmeshData()
    {
        StartCoroutine(UpdateNavmeshDataAsync());
    }

    IEnumerator UpdateNavmeshDataAsync()
    {
        AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(
            NavMeshData,
            NavMesh.GetSettingsByID(0),
            GetBuildSources(BuildMask),
            new Bounds(BoundsCenter, BoundsSize));
        yield return op;

        AddNavMeshData();
        Debug.Log("Update finished " + Time.realtimeSinceStartup.ToString());
    }

    void Build()
    {
        NavMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(NullMask),
            new Bounds(BoundsCenter, BoundsSize),
            Vector3.zero,
            Quaternion.identity);
        AddNavMeshData();
    }

    List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            new Bounds(BoundsCenter, BoundsSize),
            mask,
            NavMeshCollectGeometry.PhysicsColliders,
            0,
            new List<NavMeshBuildMarkup>(),
            sources);
        Debug.Log("Sources found: " + sources.Count.ToString());
        return sources;
    }
}
