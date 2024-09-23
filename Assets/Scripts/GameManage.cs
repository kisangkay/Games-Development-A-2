using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int keysCollected = 0;
    public int totalKeys = 4;
    public float timeLimit = 300f; // 5 minutes
    private float timer;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (keysCollected >= totalKeys)
        {
            // Load the leaderboard scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
        }

        if (timer <= 0)
        {
            // Handle time out
        }
    }

    public void CollectKey()
    {
        keysCollected++;
    }
}
