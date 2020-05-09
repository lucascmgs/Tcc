using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UINextItemsList : MonoBehaviour
{
    [SerializeField] private ItemListManager itemManager;

    [SerializeField] private Image holdItemImage;
    
    [SerializeField] private Slider cooldownSlider;
    
    private List<GameObject> itemList = new List<GameObject>();
    
    private Image[] itemImages;
    
    void Start()
    {
        itemList = itemManager.itemList;
        itemImages = GetComponentsInChildren<Image>();
        UpdateItemImages();
    }

    private void Update()
    {
        var currentCooldown = itemManager.currentCooldown;
        if (currentCooldown > 0)
        {
            cooldownSlider.gameObject.SetActive(true);
            cooldownSlider.value = currentCooldown / itemManager.DefaultCooldown;
        }
        else
        {
            cooldownSlider.gameObject.SetActive(false);
        }
    }


   

    public void UpdateItemImages()
    {
        if (itemManager.holdItem != null)
        {
            holdItemImage.sprite = itemManager.holdItem.GetComponent<SpriteRenderer>().sprite;
        }
        
        var imageArrayLength = itemImages.Length;
        if (itemList.Count > 0)
        {
            var index = 0;
            var itemArray = itemList.ToArray();
            foreach (var arr in itemImages)
            {
                var spr = itemArray[index].GetComponent<SpriteRenderer>().sprite;
                arr.sprite = spr;
                index++;
            }
        }
    }

    
}