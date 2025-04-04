using UnityEngine;
namespace Procedural_Generation {


    public enum ConnectorType
    {
        L, // Left
        R, // Right
        T, // Top
        B  // Bottom
    }

    public class Connector : MonoBehaviour {
        public static ConnectorType GetCompatibleConnectorType(ConnectorType connectorType) {
            switch (connectorType) {
                case ConnectorType.L:
                    return ConnectorType.R;
                case ConnectorType.R:
                    return ConnectorType.L;
                case ConnectorType.T:
                    return ConnectorType.B;
                case ConnectorType.B:
                    return ConnectorType.T;
                default:
                    //error
                    return ConnectorType.L;
            }

        }

        public ConnectorType connectorType;
    }
}

