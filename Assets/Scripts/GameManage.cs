using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int carriedkeys = 0;
    public int totalKeys = 4;
    public float timeLimit = 300f; // 5 minutes
    private float timer;
    private GameObject carriedkey;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (carriedkeys == totalKeys)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
        }

        if (timer <= 0)//5mins run out,
        {
            //gameover image, button to main menu
            gameover();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Redkey") || other.CompareTag("Bluekey") || other.CompareTag("Greenkey")
        || other.CompareTag("Yellowkey") && carriedkey == null)
        {
            Pickupkeys(other.gameObject);
        }
    }


    void gameover()
    {
        Debug.Log("Game Over! Time's up!");//show ui and button for main menu/restarting
    }

    void Pickupkeys(GameObject obj)
    {
        // Rigidbody rb = obj.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.isKinematic = true; // Disable physics while holding
        // }

        carriedkeys++;
        Destroy(obj);
        Debug.Log("Picked up a key, total keys = "+ carriedkeys);
    }
}
