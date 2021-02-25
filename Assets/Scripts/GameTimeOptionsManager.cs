using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameTimeOptionsManager : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUgui;

    void Start()
    {
        _textMeshProUgui = GetComponent<TextMeshProUGUI>();
        UpdateTime();
    }

    public void AddTime()
    {
        GameOptions.GameTime += 1;
        UpdateTime();
    }

    public void SubtractTime()
    {
        if (GameOptions.GameTime > 30)
        {
            GameOptions.GameTime -= 1;
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        _textMeshProUgui.text = "Time\n" + GameOptions.GameTime.ToString();
    }
}