using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingBehaviour : MonoBehaviour {
    private readonly float cycleScalingBase = 2f; // Higher the number, the faster one phase is 
    private readonly float bobbingAmount = 0.1f; //Amplitude
    private float cycleScaling; // Higher the number, the faster one phase is 
    private float timer = 0;
    private float verticalOffset = 0;
    [SerializeField] private bool shouldHaveRandomOffset = true;
    public bool ShouldBob { get; set; } = true;
    private void Start() {
           cycleScaling = cycleScalingBase + (shouldHaveRandomOffset ? Random.Range(-0.5f, 0.5f) : 0);
    }
    private void Update() {
        if (ShouldBob) Bob();
    }
    void Bob() {
        float previousOffset = verticalOffset;
        float waveslice = Mathf.Sin(cycleScaling * timer);
        timer += Time.deltaTime;
        if (timer > Mathf.PI * 2) {
            timer = timer - (Mathf.PI * 2);
        }

        verticalOffset = waveslice * bobbingAmount;
        float translateChange = verticalOffset - previousOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y + translateChange, transform.position.z);
    }

}
