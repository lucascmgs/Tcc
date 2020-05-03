using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingTimeBehaviour : MonoBehaviour
{
    public float initialRemainingTime = 120;
    private TextMeshProUGUI timeText;
    
    [NonSerialized] public float remainingTime;
    void Start()
    {
        remainingTime = initialRemainingTime;
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if ((int) remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        timeText.text = "" + (int) remainingTime;
    }
}
