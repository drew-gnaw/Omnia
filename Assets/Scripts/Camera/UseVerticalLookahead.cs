using Cinemachine;
using UnityEngine;

    public class UseVerticalLookahead : MonoBehaviour {
        public Rigidbody2D playerRb;              // Assign the player's Rigidbody2D in the Inspector
        public float offsetYWhileFalling = -3f;   // How far to move the camera down
        public float lerpSpeed = 3f;              // Smoothing speed

        private CinemachineVirtualCamera virtualCam;
        private CinemachineFramingTransposer framingTransposer;
        private float defaultYOffset;

        void Start()
        {
            virtualCam = GetComponent<CinemachineVirtualCamera>();
            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                defaultYOffset = framingTransposer.m_TrackedObjectOffset.y;
            }
        }

        void LateUpdate()
        {
            if (framingTransposer == null || playerRb == null) return;

            bool lookingDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            float targetYOffset = lookingDown ? offsetYWhileFalling : defaultYOffset;

            Vector3 offset = framingTransposer.m_TrackedObjectOffset;
            offset.y = Mathf.Lerp(offset.y, targetYOffset, Time.deltaTime * lerpSpeed);
            framingTransposer.m_TrackedObjectOffset = offset;
        }

    }
