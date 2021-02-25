using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PhoneComboManager : MonoBehaviour
{
    [SerializeField] private float timeToReduceCombo = 5f;


    private ServerManager _serverManager;

    [SerializeField] private int ranksToIncreaseLevel = 2;
    
    private int comboRank = 0;
    private int comboLevel = 0;

    [SerializeField] private int maxComboLevel = 3;

    private Coroutine comboCoroutine;


    public void IncrementRank()
    {
        Debug.Log("Incrementou");
        comboRank++;
        Debug.Log("Combo Rank: " + comboRank);
        ResetReduction();
        UpdateComboLevel();
    }
    
    private void Start()
    {
        _serverManager = FindObjectOfType<ServerManager>();

        if (_serverManager != null)
        {
            comboCoroutine = StartCoroutine(NextReduction());
        }
    }

    IEnumerator NextReduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToReduceCombo);
            if (comboRank > 0)
            {
                comboRank--;
                Debug.Log("Decrementou");
                UpdateComboLevel();
            }
        }
    }

    private void ResetReduction()
    {
        StopCoroutine(comboCoroutine);
        comboCoroutine = StartCoroutine(NextReduction());
    }

    private void UpdateComboLevel()
    {
        int previousComboLevel = comboLevel;
        comboLevel = comboRank / ranksToIncreaseLevel;
        if (comboLevel > maxComboLevel)
        {
            comboLevel = maxComboLevel;
        }
        if (previousComboLevel != comboLevel)
        {
            Debug.Log("New combo level: " + comboLevel);
            InformCombo();
        }
    }

    private void InformCombo()
    {
        if (_serverManager != null)
        {
            Debug.Log("Teco");
            _serverManager.Send("ComboLevel;" + comboLevel.ToString());
        }
    }
    
}
