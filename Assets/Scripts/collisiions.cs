using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class collisiions : MonoBehaviour
{
    [SerializeField] float loaddelay = 2f;
    [SerializeField] AudioClip obstaclesound;
    [SerializeField] AudioClip finishsound;
    [SerializeField] ParticleSystem successparticles;
    [SerializeField] ParticleSystem crashparticles;
    AudioSource rs;
    bool iscontrollable = true;
    bool iscrashable = true;
    void Start()
    {
        rs = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (iscrashable)
        {
            String s = collision.gameObject.tag;
        switch (s)
        {
            case "friendly":
                Debug.Log("We are on a friendly Platform");
                break;
            case "Finish":
                Debug.Log("We are on a landing Platform");
                startfinishsequence();
                break;
            case "fuel":
                Debug.Log("We are on a fueling Platform");
                break;
            default:
                StartCrashSequence();
                break;
        }
        }
    }
    private void Update() {
        respondtodebugkeys();
    }
    private void startfinishsequence()
    {
        if (iscontrollable)
        {
            successparticles.Play();
            rs.Stop();
            Invoke("NextLevel", loaddelay);
            GetComponent<mover>().enabled = false;
            rs.PlayOneShot(finishsound);
            iscontrollable = false;
        }
    }

    private void StartCrashSequence()
    {
        if (iscontrollable)
        {
            crashparticles.Play();
            rs.Stop();
            rs.PlayOneShot(obstaclesound);
            GetComponent<mover>().enabled = false;
            Invoke("Reloadlevel", loaddelay);
            iscontrollable = false;
        }

    }

    void Reloadlevel()
    {
        int currentscene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentscene);
    }
    void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene;

        if (currentScene >= SceneManager.sceneCountInBuildSettings - 1)
        {
            nextScene = 0; // loop back to first scene
        }
        else
        {
            nextScene = currentScene + 1;
        }

        SceneManager.LoadScene(nextScene);
    }
    void respondtodebugkeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            iscrashable = !iscrashable;
        }
    }
    }

