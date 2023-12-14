using System.Collections;
using System.Collections.Generic;
using EndlessRun;
using Unity.VisualScripting;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    private int tileStartCount = 10;
    [SerializeField]
    private int minimumStraightFile = 3;
    [SerializeField]
    private int maximumStraightFiles = 15;
    [SerializeField]
    private GameObject startingTile;
    [SerializeField]
    private List<GameObject> turnTiles;
    [SerializeField]
    private List<GameObject> obstacles;


    private Vector3 currentTileLocation = Vector3.zero;
    private Vector3 currentTIleDirection = Vector3.forward;
    private GameObject prevTile;

    private List<GameObject> currentTiles;
    private List<GameObject> currentObstacles;

    private void Start()
    {
        currentTiles = new List<GameObject>();
        currentObstacles = new List<GameObject>();

        Random.InitState(System.DateTime.Now.Millisecond);

        for (int i = 0; i < tileStartCount; i++)
        {
            SpawnTile(startingTile.GetComponent<Tile>());
        }

        SpawnTile(SelectRandomObjectFromList(turnTiles).GetComponent<Tile>());
    }

    private void SpawnTile(Tile tile, bool spawnObstacles = false)
    {
        Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTIleDirection, Vector3.up);

        prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
        currentTiles.Add(prevTile);

        if (spawnObstacles) SpawnObstacle();

        if (tile.type == TileType.STRAIGHT)
        {
            currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTIleDirection);
        }
            
    }

    private void DeletePreviousTiles()
    {
        while(currentTiles.Count != 1)
        {
            GameObject tile = currentTiles[0];
            currentTiles.RemoveAt(0);
            Destroy(tile);
        }

        while (currentObstacles.Count != 0)
        {
            GameObject obstacle = currentObstacles[0];
            currentObstacles.RemoveAt(0);
            Destroy(obstacle);
        }
    }

    public void AddNewDirection(Vector3 direction)
    {
        currentTIleDirection = direction;
        DeletePreviousTiles();

        Vector3 tilePlacementScale;
        if (prevTile.GetComponent<Tile>().type == TileType.SIDEWAYS)
        {
            tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size / 2 + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), 
                currentTIleDirection);
        }
        else
        {
            tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size - (Vector3.one * 2) + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2),
                currentTIleDirection);
        }

        currentTileLocation += tilePlacementScale;

        int currentPathLength = Random.Range(minimumStraightFile, maximumStraightFiles);
        for (int i = 0; i < currentPathLength; i++)
        {
            SpawnTile(startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
        }

        SpawnTile(SelectRandomObjectFromList(turnTiles).GetComponent<Tile>(), false);
    }

    private void SpawnObstacle()
    {
        if (Random.value > 0.2f) return;

        GameObject obstaclePrefab = SelectRandomObjectFromList(obstacles);
        Quaternion newObjectRotation = obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(currentTIleDirection, Vector3.up);

        GameObject obstacle = Instantiate(obstaclePrefab, currentTileLocation, newObjectRotation);
        currentObstacles.Add(obstacle);
    }

    private GameObject SelectRandomObjectFromList(List<GameObject> list)
    {
        if (list.Count == 0)
        {
            return null;
        }
        else
        {
            return list[Random.Range(0, list.Count)];
        }
    }

}
