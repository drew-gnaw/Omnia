using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;

    public void UpdateBar(float current, float max) {
        slider.value = current / max;
    }

    // Temporary for Courage buff
    public void SetColor(Color c) {
        slider.fillRect.GetComponent<Image>().color = c;
    }
}

