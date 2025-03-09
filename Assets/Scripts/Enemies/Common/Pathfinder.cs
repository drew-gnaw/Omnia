using System;
using System.Collections.Generic;
using System.Linq;
using Omnia.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Utils;
using Cell2D = UnityEngine.Vector2Int;
using Cell3D = UnityEngine.Vector3Int;

namespace Enemies.Common {
    public class Pathfinder : PersistentSingleton<Pathfinder> {
        [SerializeField] internal LayerMask solid;

        private readonly HashSet<Cell2D> obstacles = new();
        private Tilemap[] tiles;
        private BoundsInt bound;

        public void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void OnAwake() {
            tiles = FindObjectsOfType<Tilemap>().Where(it => CollisionUtils.isLayerInMask(it.gameObject.layer, solid)).ToArray();
            bound = tiles.Select(CompressAndGet).Aggregate(MaxBound);
            obstacles.Clear();

            var positions = bound.allPositionsWithin;
            foreach (var position in positions) {
                if (tiles.Any(it => it.HasTile(position))) obstacles.Add((Cell2D)position);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            OnAwake();
        }

        public List<Vector2> FindPath(Vector2 a, Vector2 b) {
            if (tiles.Length < 1) return new List<Vector2>();
            var basis = tiles[0];

            var a0 = (Cell2D)basis.WorldToCell(a);
            var b0 = (Cell2D)basis.WorldToCell(b);
            return AStar(a0, b0).ConvertAll(it => (Vector2)basis.GetCellCenterWorld((Cell3D)it));
        }

        private List<Vector2Int> AStar(Vector2Int start, Vector2Int target) {
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
            PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
            Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float> { [start] = Heuristic(start, target) };

            openSet.Enqueue(start, fScore[start]);

            while (openSet.Count > 0) {
                Vector2Int current = openSet.Dequeue();
                if (current == target)
                    return ReconstructPath(cameFrom, current);

                closedSet.Add(current);
                foreach (Vector2Int neighbor in GetNeighbors(current)) {
                    if (closedSet.Contains(neighbor) || IsObstacle(neighbor))
                        continue;

                    float tentativeGScore = gScore[current] + Vector2Int.Distance(current, neighbor);
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor]) {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, target);
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }

            return new List<Vector2Int>(); // No path found
        }

        private float Heuristic(Vector2Int a, Vector2Int b) {
            return Vector2Int.Distance(a, b);
        }

        private List<Vector2Int> GetNeighbors(Vector2Int node) {
            return new List<Vector2Int> {
                new Vector2Int(node.x + 1, node.y),
                new Vector2Int(node.x - 1, node.y),
                new Vector2Int(node.x, node.y + 1),
                new Vector2Int(node.x, node.y - 1)
            };
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
            List<Vector2Int> path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        private static BoundsInt CompressAndGet(Tilemap it) {
            it.CompressBounds();
            return it.cellBounds;
        }

        private static BoundsInt MaxBound(BoundsInt a, BoundsInt b) {
            var min = new Cell3D(Math.Min(a.xMin, b.xMin), Math.Min(a.yMin, b.yMin));
            var max = new Cell3D(Math.Max(a.xMax, b.xMax), Math.Max(a.yMax, b.yMax));
            return new BoundsInt(min, max - min);
        }
    }
}
