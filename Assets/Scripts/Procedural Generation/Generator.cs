using Procedural_Generation;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap masterTilemap; // The main Tilemap
    public GameObject[] sectionPrefabs; // Prefabs containing tilemap sections (tunnels, rooms, etc.)
    public Vector3Int startingPosition; // Initial position to start generation

    private List<Connector> openConnectors = new List<Connector>(); // List of currently open connectors
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>(); // Track used positions

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector3 position = startingPosition;

        // Spawn the first section
        GameObject firstPrefab = Instantiate(sectionPrefabs[Random.Range(0, sectionPrefabs.Length)], position, Quaternion.identity);
        Connector[] firstConnectors = FindConnectors(firstPrefab);

        // Add its connectors to the open list
        foreach (Connector conn in firstConnectors)
        {
            openConnectors.Add(conn);
        }

        for (int i = 1; i < 10; i++) // Example: generate 10 sections
        {
            if (openConnectors.Count == 0)
            {
                Debug.LogWarning("No open connectors left. Stopping generation.");
                break;
            }

            Debug.Log("These are the connectors open on iteration " + i);
            foreach (Connector conn in openConnectors) {
                Debug.Log(conn.transform.position);
            }

            // Choose a random open connector to build from
            Connector chosenConnector = openConnectors[Random.Range(0, openConnectors.Count)];
            Vector3 chosenWorldPosition = chosenConnector.transform.position;

// Find a valid prefab that has a compatible connector type
            GameObject prefabToUse = GetCompatiblePrefab(chosenConnector.connectorType);

            if (prefabToUse == null)
            {
                Debug.LogWarning("No valid prefab found for connector type: " + chosenConnector.connectorType);
                openConnectors.Remove(chosenConnector);
                continue;
            }

// Instantiate the new prefab
            GameObject instantiatedPrefab = Instantiate(prefabToUse, Vector3.zero, Quaternion.identity);

// Find the matching connector in the instantiated prefab
            Connector matchingConnector = null;
            foreach (Connector conn in FindConnectors(instantiatedPrefab))
            {
                if (conn.connectorType == Connector.GetCompatibleConnectorType(chosenConnector.connectorType))
                {
                    matchingConnector = conn;
                    break;
                }
            }

            if (matchingConnector == null)
            {
                Debug.LogError("Failed to find matching connector in instantiated prefab");
                Destroy(instantiatedPrefab);
                openConnectors.Remove(chosenConnector);
                continue;
            }

// Calculate the offset to align the connectors
            Vector3 offset = chosenWorldPosition - matchingConnector.transform.position;
            instantiatedPrefab.transform.position = offset;

            Tilemap secondTilemap = instantiatedPrefab.GetComponent<Tilemap>();
            CopyTilesToMasterTilemap(secondTilemap, Vector3Int.RoundToInt(instantiatedPrefab.transform.position));

// Remove used connector
            openConnectors.Remove(chosenConnector);

// Only add the other connector from the prefab (since each prefab has exactly 2)
            Connector[] newConnectors = FindConnectors(instantiatedPrefab);
            foreach (Connector conn in newConnectors)
            {
                if (conn != matchingConnector)
                {
                    openConnectors.Add(conn);
                    break; // Only add one connector (the non-matching one)
                }
            }
        }
    }

    // Find all connectors in a prefab
    Connector[] FindConnectors(GameObject prefab)
    {
        return prefab.GetComponentsInChildren<Connector>();
    }

    // Find a valid prefab that has a connector matching the required type
    GameObject GetCompatiblePrefab(ConnectorType requiredType)
    {
        foreach (GameObject prefab in sectionPrefabs)
        {
            Connector[] connectors = FindConnectors(prefab);
            foreach (Connector conn in connectors)
            {
                if (conn.connectorType == Connector.GetCompatibleConnectorType(requiredType))
                {
                    return prefab;
                }
            }
        }
        return null;
    }

    // Copy tiles to the master tilemap
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
