using Procedural_Generation;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TestGenerator : MonoBehaviour
{
    public Tilemap masterTilemap; // The main Tilemap
    public GameObject LRPrefab; // Left-to-Right prefab
    public GameObject RTPrefab; // Right-to-Top prefab
    public Vector3Int startingPosition; // Initial position to start generation

    private List<Connector> openConnectors = new List<Connector>(); // List of currently open connectors

    void Start()
    {
        GenerateTestLevel();
    }

    void GenerateTestLevel()
    {
        Vector3Int position = startingPosition;

        // Spawn LR section first
        GameObject firstPrefab = Instantiate(LRPrefab, position, Quaternion.identity);
        Tilemap firstTilemap = firstPrefab.GetComponent<Tilemap>();
        CopyTilesToMasterTilemap(firstTilemap, position);

        Connector[] firstConnectors = FindConnectors(firstPrefab);
        openConnectors.AddRange(firstConnectors);

        if (openConnectors.Count == 0)
        {
            Debug.LogError("No connectors found in LR prefab.");
            return;
        }

        // Choose the right-side connector from LR
        Connector chosenConnector = openConnectors.Find(c => c.connectorType == ConnectorType.L);
        if (chosenConnector == null)
        {
            Debug.LogError("No right connector found in LR prefab.");
            return;
        }

        Vector3 chosenWorldPosition = chosenConnector.transform.position; // World position

        // Spawn RT section next, connecting to the right connector of LR
        GameObject secondPrefab = Instantiate(RTPrefab, chosenWorldPosition, Quaternion.identity);
        Connector[] secondConnectors = FindConnectors(secondPrefab);

        // Find the matching connector in RT prefab
        Connector matchingConnector = System.Array.Find(secondConnectors, c => c.connectorType == ConnectorType.R);
        if (matchingConnector != null)
        {
            Vector3 matchingWorldPosition = matchingConnector.transform.position; // Get its world position

            // Calculate required offset
            Vector3 offset = chosenWorldPosition - matchingWorldPosition;

            // Move second prefab so its connector aligns exactly
            secondPrefab.transform.position += 2* offset;
        }

        Tilemap secondTilemap = secondPrefab.GetComponent<Tilemap>();
        CopyTilesToMasterTilemap(secondTilemap, Vector3Int.RoundToInt(secondPrefab.transform.position));

        Debug.Log("Test level generated: LR -> RT");
    }


    // Find all connectors in a prefab
    Connector[] FindConnectors(GameObject prefab)
    {
        return prefab.GetComponentsInChildren<Connector>();
    }

    // Copy tiles from the instantiated prefab to the master tilemap
    void CopyTilesToMasterTilemap(Tilemap tilemap, Vector3Int positionOffset)
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                masterTilemap.SetTile(positionOffset + pos, tile);
            }
        }
    }

}
