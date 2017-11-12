using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsControl : MonoBehaviour
{
    private static StatsControl instance;

    public float bestTime { get; private set; }

    public int stunnedAmount { get; private set; }

    public int playCount { get; private set; } 

    public StatsControl ()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        loadStats();
    }


    public void saveStats()
    {
        PlayerPrefs.SetFloat("bestTime", bestTime);
        PlayerPrefs.SetInt("playCount", playCount);
        PlayerPrefs.SetInt("stunnedTimes", stunnedAmount);
        PlayerPrefs.Save();
    }

    
    //Load the current player stats which have been recorded in their total playing time. If non existant then create new ones.
    public void loadStats()
    {
        if (!PlayerPrefs.HasKey("playCount"))
        {
            createStats();
        }

        playCount = PlayerPrefs.GetInt("playCount");
        bestTime = PlayerPrefs.GetFloat("bestTime");
        stunnedAmount = PlayerPrefs.GetInt("stunnedTimes");
    }

    public void addPlayCount()
    {
        ++playCount;
        saveStats();
    }

    public void addStunCount()
    {
        stunnedAmount++;
        saveStats();
    }

    public void setBestTime(float newBest)
    {
        bestTime = newBest;
        saveStats();
    }

    //Create stats for the player if they've never played before.
    private void createStats()
    {
        PlayerPrefs.SetFloat("bestTime", 0.0f);
        PlayerPrefs.SetInt("stunnedTimes", 0);
        PlayerPrefs.SetInt("playCount", 0);
    }

    public static StatsControl getInstance()
    {
        return instance;
    }
}
