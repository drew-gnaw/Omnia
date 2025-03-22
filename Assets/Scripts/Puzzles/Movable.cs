using System.Collections.Generic;
using System.Linq;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Puzzle {
    public class Movabale : MonoBehaviour, IReceiver {
        [@TypeFilter(typeof(ReceiverBehaviour))]
        [SerializeField] private SerializableType behaviour;
        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [SerializeField] private Transform destPosition;
        [SerializeField] private Transform startPosOverride;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private ConveyerEndPiece endpointPiece;
        [SerializeField] private GameObject linePiece;
        [SerializeField] private PuzzleAssets symbols;
        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);
        private int positionIndex = 0;
        private List<ISignal> signalList = new();
        private Rigidbody2D rb;
        private List<Vector3> checkPointLocation = new();
        private List<GameObject> conveyerLineObjects = new();
        private Vector3 targetPosition;
        private bool shouldMove;

        private PlayerFeetBehaviour playerFeet;
        private bool isOnPlatform;


        void Awake() {
            signalList = signals?.Unbox() ?? new();
        }
        public float GetPlatformSpeed() {
            return moveSpeed;
        }

        public Vector2 GetDestination() {
            return targetPosition;
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.GetComponent<PlayerFeetBehaviour>() != null) {
                playerFeet = collision.GetComponent<PlayerFeetBehaviour>();
                isOnPlatform = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.GetComponent<PlayerFeetBehaviour>() != null) {
                playerFeet = null;
                isOnPlatform = false;
            }
        }

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            checkPointLocation.Add(startPosOverride != null ? startPosOverride.position :  gameObject.transform.position);
            checkPointLocation.Add(destPosition.position);
            targetPosition = checkPointLocation.First();
            Redraw();
        }

        private void Redraw() {
            ClearConveyer();
            DrawConveyer();
            SetConveyerColor();
        }
        private void ClearConveyer() {
            foreach (GameObject conveyer in conveyerLineObjects) {
                Destroy(conveyer);
            }

            conveyerLineObjects.Clear();
        }
        private void DrawConveyer() {
            if (endpointPiece == null || linePiece == null) {
                Debug.LogWarning($"Missing conveyer assets: endpoint: {endpointPiece}, linePiece: {linePiece}");
                return;
            }

            if (checkPointLocation.Count <= 1) {
                Debug.LogWarning($"Should have more than 1 checkpoint : {checkPointLocation.Count}");
                return;
            }

            if (signalList.Count == 0) {
                Debug.LogWarning($"Should have more than zero signals: {signalList.Count}");
                return;
            }

            void DrawEndPiece(ConveyerEndPiece piece) {
                conveyerLineObjects.Add(piece.gameObject);
                piece.SetSymbol(signalList.First().SignalColor.GetSymbol(symbols));
            }

            Vector2 start = checkPointLocation.First();
            Vector2 end = checkPointLocation.Last();
            Vector2 direction = (end - start).normalized;
            float distance = Vector2.Distance(start, end);

            ConveyerEndPiece startPiece = Instantiate(endpointPiece, start, Quaternion.identity, transform.parent);
            ConveyerEndPiece endPiece = Instantiate(endpointPiece, end, Quaternion.identity, transform.parent);
            DrawEndPiece(startPiece);
            DrawEndPiece(endPiece);

            // Start and End Pieces should face each other
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            startPiece.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            endPiece.transform.rotation = Quaternion.Euler(0, 0, angle);

            float lineLength = linePiece.GetComponent<SpriteRenderer>().bounds.size.x;
            int numPieces = Mathf.FloorToInt(distance / lineLength);

            for (int i = 1; i <= numPieces; i++) {
                Vector2 position = start + direction * (i * lineLength);
                GameObject midPiece = Instantiate(linePiece, position, Quaternion.identity, transform.parent);
                conveyerLineObjects.Add(midPiece);
                midPiece.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void SetConveyerColor() {
            if (signalList.Count == 0) {
                Debug.LogWarning($"Should have more than zero signals: {signalList.Count}");
                return;
            }

            conveyerLineObjects.ForEach(obj => obj.GetComponent<SpriteRenderer>().color = signalList.First().SignalColor.Color);
        }

        private void OnEnable() {
            foreach (ISignal signal in signalList) {
                signal.SignalEvent += SignalReceived;
            }
        }

        private void OnDisable() {
            foreach (ISignal signal in signalList) {
                signal.SignalEvent -= SignalReceived;
            }
        }

        private void SignalReceived(ISignal signal) {
            shouldMove = ReceiverBehaviour.Accept(signalList);
        }

        private void getNextPosition() {
            positionIndex = (positionIndex + 1) % checkPointLocation.Count;
            targetPosition = checkPointLocation[positionIndex];
        }
        private void FixedUpdate() {
            if (shouldMove) {
                rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
                if (isOnPlatform && playerFeet != null) {
                    playerFeet.SetPlayerTransform(Vector2.MoveTowards(playerFeet.GetPlayerTransform(), targetPosition, Time.fixedDeltaTime * moveSpeed));
                }

                if (Vector2.Distance(rb.position, targetPosition) < 0.01f) {
                    getNextPosition();
                }

            }
        }

    }
}
