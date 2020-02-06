using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NextItemsList : MonoBehaviour
{
    public GameObject ItemManager;

    private Queue<GameObject> itemList = new Queue<GameObject>();

    private Image[] itemImages;

    // Start is called before the first frame update
    void Start()
    {
        itemList = ItemManager.GetComponent<ItemListManager>().itemList;
        itemImages = GetComponentsInChildren<Image>();
        UpdateItemImages();
    }
    
    public void UpdateItemImages()
    {
        var imageArrayLength = itemImages.Length;
        Debug.Log(imageArrayLength);
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
            Debug.Log(index);
        }
    }

    
}