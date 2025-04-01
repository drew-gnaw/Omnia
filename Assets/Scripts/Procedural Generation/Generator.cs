using Procedural_Generation;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap masterTilemap; // Your main Tilemap
    public GameObject[] sectionPrefabs; // Your Tilemap prefabs (tunnel, room, etc.)
    public Vector3Int startingPosition; // Where to start placing sections

    // Set to store the positions of already connected connectors
    private HashSet<Vector3Int> connectedPositions = new HashSet<Vector3Int>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector3Int position = startingPosition;
        GameObject previousPrefab = null;

        // Loop to instantiate and place sections
        for (int i = 0; i < 10; i++) // Example: Generate 10 sections
        {
            // Choose a random prefab to place
            GameObject selectedPrefab = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];

            // Instantiate the Tilemap prefab
            GameObject instantiatedPrefab = Instantiate(selectedPrefab, position, Quaternion.identity);

            // Get the connectors of the current prefab
            Connector[] connectors = FindConnectors(instantiatedPrefab);

            // If connectors are found, choose one and set the position for the next section
            if (connectors.Length > 0)
            {
                // Choose a connector to connect with the previous section's exit
                Connector selectedConnector = ChooseConnector(connectors, previousPrefab);

                // Move the new section's position to match the connector
                Vector3Int connectorPositionInt = Vector3Int.RoundToInt(selectedConnector.transform.localPosition);
                position += 4 * connectorPositionInt;

                // Check if the new position has already been connected
                if (connectedPositions.Contains(position))
                {
                    Debug.LogWarning("Position already connected. Skipping.");
                    continue; // Skip this iteration to avoid generating on an already connected position
                }

                // Add the position of the new connector to the set of connected positions
                connectedPositions.Add(position);
                Debug.Log("We are connecting " + previousPrefab?.name + " to " + selectedPrefab.name);
                Debug.Log("New position after connector: " + position);
            }
            else
            {
                Debug.LogWarning("No connectors found in prefab: " + selectedPrefab.name);
            }

            // Access the Tilemap component of the instantiated prefab
            Tilemap tilemap = instantiatedPrefab.GetComponent<Tilemap>();

            // Copy the tiles from the prefab's Tilemap to the master Tilemap
            CopyTilesToMasterTilemap(tilemap, position);

            // Update previousPrefab to the current prefab for the next iteration
            previousPrefab = selectedPrefab;
        }
    }

    // Method to find all connectors (child objects with the Connector script attached)
    Connector[] FindConnectors(GameObject prefab)
    {
        // Find all child objects that have the Connector script attached
        return prefab.GetComponentsInChildren<Connector>();
    }

    // Method to choose the right connector (logic based on connector types)
    Connector ChooseConnector(Connector[] connectors, GameObject previousPrefab)
    {
        // If this is the first piece, just return the first connector
        if (previousPrefab == null) {
            return connectors[0];
        }

        // For simplicity, let's assume we're picking the first connector that matches the type of the previous section's exit.
        Connector previousConnector = previousPrefab.GetComponentInChildren<Connector>();

        // Example: If the previous connector is "L", pick a "R" connector to match
        foreach (Connector connector in connectors)
        {
            if (CanConnect(connector, previousConnector))
            {
                return connector;
            }
        }

        // If no valid connector found, choose a random one (you can improve this logic)
        return connectors[Random.Range(0, connectors.Length)];
    }

    // Method to check if two connectors can connect based on their types
    bool CanConnect(Connector current, Connector previous)
    {
        if (current.connectorType == ConnectorType.L && previous.connectorType == ConnectorType.R)
            return true;
        if (current.connectorType == ConnectorType.R && previous.connectorType == ConnectorType.L)
            return true;
        if (current.connectorType == ConnectorType.T && previous.connectorType == ConnectorType.B)
            return true;
        if (current.connectorType == ConnectorType.B && previous.connectorType == ConnectorType.T)
            return true;

        return false; // No connection
    }

    // Method to copy the tiles from the prefab Tilemap to the master Tilemap
    void CopyTilesToMasterTilemap(Tilemap tilemap, Vector3Int position)
    {
        // Loop through all tiles in the prefab Tilemap and copy them to the master Tilemap
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                masterTilemap.SetTile(position + pos, tile);
            }
        }
    }
}
