using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audio;
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explode;
    [SerializeField] AudioClip startChime;


    enum State { Alive, Dying, Transcending }
    [SerializeField] State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        // todo somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

	}

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch(collision.gameObject.tag)
        { 
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                print("Hit finish");
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);    // This coroutine waits 1 second before executing method
                break;
            default:
                print("You're dead as shit!");
                state = State.Dying;
                Invoke("ResetScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void ResetScene()
    {
        audio.PlayOneShot(startChime);
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        //thrust
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audio.Pause();
        }
    }

    private void ApplyThrust()
    {
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(mainEngine);
        }
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
    }
}
