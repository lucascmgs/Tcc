using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxBehaviour : MonoBehaviour
{
    private TextMeshProUGUI thisText;
    [SerializeField] private float textSpeed = 1;
    private int initialTextSize;
    
    void Start()
    {
        thisText = GetComponent<TextMeshProUGUI>();
        initialTextSize = thisText.text.Length;
        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText()
    {
        thisText.maxVisibleCharacters = 0;
        while (thisText.maxVisibleCharacters < initialTextSize)
        {
            thisText.maxVisibleCharacters += 1;
            yield return new WaitForSeconds(0.1f/textSpeed);
        }
    }
    
}
