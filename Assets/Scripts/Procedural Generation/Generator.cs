using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap masterTilemap; // Your main Tilemap
    public GameObject[] sectionPrefabs; // Your Tilemap prefabs (tunnel, room, etc.)
    public Vector3Int startingPosition; // Where to start placing sections

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
            Transform[] connectors = FindConnectors(instantiatedPrefab);

            // If connectors are found, choose one and set the position for the next section
            if (connectors.Length > 0)
            {
                // Choose a connector to connect with the previous section's exit
                Transform selectedConnector = ChooseConnector(connectors, previousPrefab);

                // Convert the connector's position to local grid space
                Vector3Int connectorPositionInt = Vector3Int.RoundToInt(selectedConnector.localPosition); // Use localPosition instead of position

                // Add the connector's offset to the current position
                position += connectorPositionInt * 2;

                // Debug the new position for verification
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


    // Method to find all connectors (child objects with tag "Connector")
    Transform[] FindConnectors(GameObject prefab)
    {
        // Find all child objects that serve as connectors
        Transform[] connectors = prefab.GetComponentsInChildren<Transform>();

        // Filter connectors based on a tag or name (you can tag them with "Connector")
        connectors = System.Array.FindAll(connectors, t => t.CompareTag("Connector"));

        return connectors;
    }

    // Method to choose the right connector (you can modify the logic based on your needs)
    Transform ChooseConnector(Transform[] connectors, GameObject previousPrefab)
    {
        // For simplicity, choose a random connector (you can implement logic here to choose based on direction)
        Transform selectedConnector = connectors[Random.Range(0, connectors.Length)];

        // You can add additional logic here to decide which connector to pick based on previous prefab's exit

        return selectedConnector;
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
