using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audio;
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 10f;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            default:
                print("You're dead as shit!");
                break;
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;    // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        //rotate left
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        //rotate right
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = true;    // resume physics control of rotation

    }

    private void Thrust()
    {
        //thrust
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }
        else
        {
            audio.Pause();
        }
    }
}
