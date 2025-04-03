using Inventory;
using System.Collections;
using UnityEngine;

namespace Scenes.Descent {
    public class ExitDoor : MonoBehaviour, IInteractable
    {
        public void Interact() {
            LevelManager.Instance.NextLevel();
        }
    }

}
