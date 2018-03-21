using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}
    
    private void ProcessInput()
    {
        //thrust
        if (Input.GetKey(KeyCode.Space))
        {
            print("space pressed");
        }
        //rotate left
        if (Input.GetKey(KeyCode.D))
        {
            print("D pressed");
        }
        //rotate right
        else if (Input.GetKey(KeyCode.A))
        {
            print("A pressed");
        }
    }
}
