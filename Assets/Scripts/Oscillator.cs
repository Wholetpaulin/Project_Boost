using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // This is an attribute that tells the script to act a certain way
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;


    Vector3 startingPos; // Needed for absolute movement storage

    float movementFactor; // 0 for not moved 1 for moved fully

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if(period <= Mathf.Epsilon) { return; } // Protects against period is zero
        float cycles = Time.time / period;  // grows continually from 0

        const float tau = 2 * Mathf.PI;     // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
