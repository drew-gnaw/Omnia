using Procedural_Generation;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour {
    public Tilemap masterTilemap; // The main Tilemap
    public GameObject[] sectionPrefabs; // Prefabs containing tilemap sections (tunnels, rooms, etc.)
    public Vector3Int startingPosition; // Initial position to start generation

    private List<Connector> openConnectors = new List<Connector>(); // List of currently open connectors
    private HashSet<Vector2Int> occupiedSpaces = new HashSet<Vector2Int>();

    [SerializeField] internal int pieceWidth = 24;
    [SerializeField] internal int pieceHeight = 12;

    Vector2Int WorldToPieceGrid(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x / (pieceWidth * 2)), Mathf.RoundToInt(position.y / (pieceHeight * 2)));
    }


    void Start() {
        GenerateLevel();
    }

    void GenerateLevel() {
        Vector3 position = startingPosition;

        // Spawn the first section
        GameObject firstPrefab = Instantiate(sectionPrefabs[Random.Range(0, sectionPrefabs.Length)], position, Quaternion.identity);
        CopyTilesToMasterTilemap(firstPrefab.GetComponent<Tilemap>(), Vector3Int.zero);
        occupiedSpaces.Add(WorldToPieceGrid(Vector2.zero));


        Connector[] firstConnectors = FindConnectors(firstPrefab);

        // Add its connectors to the open list
        foreach (Connector conn in firstConnectors) {
            openConnectors.Add(conn);
        }

        for (int i = 1; i < 20; i++) // Example: generate 10 sections
        {
            Debug.Log(occupiedSpaces.Count);
            foreach (Vector2Int space in occupiedSpaces) {
                Debug.Log(space);
            }

            if (openConnectors.Count == 0) {
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
            GameObject prefabToUse = GetCompatiblePrefab(chosenConnector.connectorType, chosenWorldPosition);

            if (prefabToUse == null) {
                Debug.LogWarning("No valid prefab found for connector type: " + chosenConnector.connectorType);
                openConnectors.Remove(chosenConnector);
                continue;
            }

// Instantiate the new prefab
            GameObject instantiatedPrefab = Instantiate(prefabToUse, chosenWorldPosition, Quaternion.identity);

// Find the matching connector in the instantiated prefab
            Connector matchingConnector = null;
            foreach (Connector conn in FindConnectors(instantiatedPrefab)) {
                if (conn.connectorType == Connector.GetCompatibleConnectorType(chosenConnector.connectorType)) {
                    matchingConnector = conn;
                    break;
                }
            }

            if (matchingConnector == null) {
                Debug.LogError("Failed to find matching connector in instantiated prefab");
                Destroy(instantiatedPrefab);
                openConnectors.Remove(chosenConnector);
                continue;
            }

// Calculate the offset to align the connectors
            Vector3 offset = chosenWorldPosition - matchingConnector.transform.position;
            instantiatedPrefab.transform.position += 3 * offset;

            Tilemap secondTilemap = instantiatedPrefab.GetComponent<Tilemap>();

            Debug.Log("Tilemap for " + instantiatedPrefab.name + " has size " + secondTilemap.size);

            CopyTilesToMasterTilemap(secondTilemap, Vector3Int.RoundToInt(instantiatedPrefab.transform.position));

            occupiedSpaces.Add(WorldToPieceGrid(instantiatedPrefab.transform.position));

            openConnectors.Remove(chosenConnector);

// Only add the other connector from the prefab (since each prefab has exactly 2)
            Connector[] newConnectors = FindConnectors(instantiatedPrefab);
            foreach (Connector conn in newConnectors) {
                if (conn != matchingConnector) {
                    openConnectors.Add(conn);
                    break; // Only add one connector (the non-matching one)
                }
            }
        }
    }

    // Find all connectors in a prefab
    Connector[] FindConnectors(GameObject prefab) {
        return prefab.GetComponentsInChildren<Connector>();
    }

    // Find a valid prefab that has a connector matching the required type
    GameObject GetCompatiblePrefab(ConnectorType requiredType, Vector3 position)
    {
        GameObject[] shuffledPrefabs = (GameObject[])sectionPrefabs.Clone();
        ShufflePrefabs(shuffledPrefabs);

        foreach (GameObject prefab in shuffledPrefabs)
        {
            Connector[] connectors = FindConnectors(prefab);
            ShuffleConnectors(connectors);

            Connector matchingConnector = null;
            Connector exitConnector = null;

            foreach (Connector conn in connectors)
            {
                if (conn.connectorType == Connector.GetCompatibleConnectorType(requiredType))
                    matchingConnector = conn;
                else
                    exitConnector = conn;
            }

            if (matchingConnector == null || exitConnector == null)
                continue;

            // Calculate exit piece space
            Vector3 exitWorldPosition = position + (exitConnector.transform.position - matchingConnector.transform.position) * 3;

            if (IsPieceSpaceAvailable(exitWorldPosition))
            {
                return prefab;
            }
        }
        return null;
    }


    void ShufflePrefabs(GameObject[] prefabs) {
        for (int i = prefabs.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = prefabs[i];
            prefabs[i] = prefabs[randomIndex];
            prefabs[randomIndex] = temp;
        }
    }


    // Copy tiles to the master tilemap
    void CopyTilesToMasterTilemap(Tilemap tilemap, Vector3Int positionOffset) {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null) {
                masterTilemap.SetTile(positionOffset + pos, tile);
            }
        }

        Destroy(tilemap.gameObject);
    }

    void ShuffleConnectors(Connector[] connectors) {
        for (int i = connectors.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (connectors[i], connectors[randomIndex]) = (connectors[randomIndex], connectors[i]);
        }
    }

    bool IsPieceSpaceAvailable(Vector3 worldPosition)
    {
        Vector2Int pieceGridPos = WorldToPieceGrid(worldPosition);
        return !occupiedSpaces.Contains(pieceGridPos);
    }
}
