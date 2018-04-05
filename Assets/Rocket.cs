﻿using System.Collections;
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
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explodeParticles;
    [SerializeField] ParticleSystem successParticles;

    enum State { Alive, Dying, Transcending }
    [SerializeField] State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
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
                StartSuccess();
                break;
            default:
                StartDeath();
                break;
        }
    }

    private void StartDeath()
    {
        // print("You're dead as shit!");
        state = State.Dying;
        audio.Stop();
        audio.PlayOneShot(explode);
        explodeParticles.Play();
        Invoke("ResetScene", 1f);
    }

    private void StartSuccess()
    {
        // print("Hit finish");
        state = State.Transcending;
        audio.Stop();
        audio.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", 1f);    // This coroutine waits 1 second before executing method
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void ResetScene()
    {
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
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
