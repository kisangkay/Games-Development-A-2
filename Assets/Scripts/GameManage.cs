using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int carriedkeys = 0;
    public int totalKeys = 4;
    private float timeLimit = 300f; //5 mins
    private float timer;
    private GameObject carriedkey;
    public GameObject player;
    public OpenExit exitgate;
    public bool startedcountdown;

    void Start()
    {
        timer = timeLimit;  // Reset the timer
        startedcountdown=false;
        Debug.Log("Timer is: " + timer + " seconds");
    }

    void Update()
    {
        if (startedcountdown == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer =0;//was going to negative
                gameover();
            }
        }
    }
    public void startcountdown(){
        //start timer
        startedcountdown = true;  // Start the countdown
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Redkey") || other.CompareTag("Bluekey") || other.CompareTag("Greenkey")
        || other.CompareTag("Yellowkey"))
        {
            Pickupkeys(other.gameObject);
        }
    }


    void gameover()
    {
        Debug.Log("Game Over! You ran out of time!");//show ui and button for main menu/restarting
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
    }
    
    void Pickupkeys(GameObject obj)
    {
        carriedkeys++;
        Destroy(obj);
        Debug.Log("Picked up a key, total keys = "+ carriedkeys);

        if (carriedkeys ==4)//Open the gate
    {
        exitgate.ToggleExit();
    }
    }
}
