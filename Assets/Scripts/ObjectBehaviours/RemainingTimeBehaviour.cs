using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RemainingTimeBehaviour : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    
    [NonSerialized] public float remainingTime;
    void Start()
    {
        remainingTime = GameOptions.gameTime;
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime >= 0)
        {
            timeText.text = "" + ((int) Math.Truncate(remainingTime) + 1);    
        }
        else
        {
            timeText.text = "0";
        }
        
        
    }

    void EndGame(string result)
    {
        ServerManager serverManager = FindObjectOfType<ServerManager>();
        if (serverManager != null)
        {
            string message = "EndGame;" + result + ";" + remainingTime;
            serverManager.Send(message);
        }

    }
}
