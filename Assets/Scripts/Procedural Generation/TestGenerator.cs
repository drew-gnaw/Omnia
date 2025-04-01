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
        Connector[] firstConnectors = FindConnectors(firstPrefab);

        // Add its connectors to the open list
        foreach (Connector conn in firstConnectors)
        {
            openConnectors.Add(conn);
        }

        if (openConnectors.Count == 0)
        {
            Debug.LogError("No connectors found in LR prefab.");
            return;
        }

        // Choose the right-side connector from LR
        Connector chosenConnector = openConnectors.Find(c => c.connectorType == ConnectorType.R);
        if (chosenConnector == null)
        {
            Debug.LogError("No right connector found in LR prefab.");
            return;
        }

        Vector3Int chosenPosition = Vector3Int.RoundToInt(chosenConnector.transform.position);

        // Spawn RT section next, connecting to the right connector of LR
        GameObject secondPrefab = Instantiate(RTPrefab, chosenPosition, Quaternion.identity);
        Connector[] secondConnectors = FindConnectors(secondPrefab);

        // Align second prefab properly
        Connector matchingConnector = System.Array.Find(secondConnectors, c => c.connectorType == ConnectorType.L);
        if (matchingConnector != null)
        {
            Vector3Int newConnectorPosition = Vector3Int.RoundToInt(matchingConnector.transform.position);
            Vector3Int offset = chosenPosition - newConnectorPosition;
            secondPrefab.transform.position += (Vector3)offset;
        }

        Debug.Log("Test level generated: LR -> RT");
    }

    // Find all connectors in a prefab
    Connector[] FindConnectors(GameObject prefab)
    {
        return prefab.GetComponentsInChildren<Connector>();
    }
}
