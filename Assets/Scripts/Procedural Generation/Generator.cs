using Procedural_Generation;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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
        Vector3Int position = startingPosition;

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

            // Choose a random open connector to build from
            Connector chosenConnector = openConnectors[Random.Range(0, openConnectors.Count)];
            Vector3Int chosenPosition = Vector3Int.RoundToInt(chosenConnector.transform.position);

            // Find a valid prefab that has a compatible connector
            GameObject selectedPrefab = GetValidPrefab(chosenConnector.connectorType, out Connector matchingConnector);

            if (selectedPrefab == null)
            {
                Debug.LogWarning("No valid prefab found for connector type: " + chosenConnector.connectorType);
                openConnectors.Remove(chosenConnector);
                continue;
            }

            // Instantiate the new prefab and align it properly
            GameObject instantiatedPrefab = Instantiate(selectedPrefab, chosenPosition, Quaternion.identity);
            Vector3Int newConnectorPosition = Vector3Int.RoundToInt(matchingConnector.transform.position);
            Vector3Int offset = chosenPosition - newConnectorPosition;
            instantiatedPrefab.transform.position += (Vector3)offset;

            // Copy the tiles to the master tilemap
            Tilemap tilemap = instantiatedPrefab.GetComponent<Tilemap>();
            CopyTilesToMasterTilemap(tilemap, offset);

            // Remove the used connector and add new ones
            openConnectors.Remove(chosenConnector);
            Connector[] newConnectors = FindConnectors(instantiatedPrefab);
            foreach (Connector conn in newConnectors)
            {
                if (conn != matchingConnector) // Don't add the used connector
                {
                    openConnectors.Add(conn);
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
    GameObject GetValidPrefab(ConnectorType requiredType, out Connector matchingConnector)
    {
        foreach (GameObject prefab in sectionPrefabs)
        {
            Connector[] connectors = FindConnectors(prefab);
            foreach (Connector conn in connectors)
            {
                if (conn.connectorType == Connector.GetCompatibleConnectorType(requiredType))
                {
                    matchingConnector = conn;
                    return prefab;
                }
            }
        }

        matchingConnector = null;
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
