using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class UITImeFrame : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;

    private RemainingTimeBehaviour _timeBehaviour;

    private float initialTime;

    private Image _image;

    void Start()
    {
        _timeBehaviour = FindObjectOfType<RemainingTimeBehaviour>();
        _image = GetComponent<Image>();
        initialTime = GameOptions.gameTime;
    }


    void Update()
    {
        float remainingTimeRatio = _timeBehaviour.remainingTime / initialTime;

        int imageIndex = 0;

        if (remainingTimeRatio >= 0.75)
        {
            imageIndex = 0;
        }
        else if (remainingTimeRatio >= 0.5 && remainingTimeRatio < 0.75)
        {
            imageIndex = 1;
        }
        else if (remainingTimeRatio >= 0.25 && remainingTimeRatio < 0.5)
        {
            imageIndex = 2;
        }
        else if (remainingTimeRatio > 0 && remainingTimeRatio < 0.25)
        {
            imageIndex = 3;
        }
        else
        {
            imageIndex = 4;
        }

        _image.sprite = sprites[imageIndex];
    }
}