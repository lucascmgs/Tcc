﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ItemListManager : MonoBehaviour
{
    #region Public variables

    public float NextCoolDown = 1.0f;

    [NonSerialized] public float currentCooldown = 0;
    
    [NonSerialized] public GameObject holdItem;

    
    public List<GameObject> itemList = new List<GameObject>();

    public int itemListMaxSize = 2;

    public List<GameObject> PossibleItems;

    public UnityEvent ItemListChanged;

    #endregion

    #region Private serialized variables

    [SerializeField] private GameObject directionArrow;

    [SerializeField] private float minimumAdequateDistance = 1.4f;

    [SerializeField] private float itemDropSpeed = 5.0f;

    #endregion

    #region Private non-serialized variables

    private Button holdButton;

    private GameObject currentItem;


    private Vector3 pointerPositionInWorld = Vector3.zero;

    private Pointer currentPointer;

    private SpriteRenderer arrowRenderer;

    private bool canChangeHoldItem = true;

    private bool isPressing = false;

    #endregion

    void Start()
    {
        holdButton = FindObjectOfType<Button>();
        if (Touchscreen.current != null)
        {
            currentPointer = Touchscreen.current;
        }
        else
        {
            currentPointer = Mouse.current;
        }

        Random.InitState(System.DateTime.UtcNow.Second);
        if (ItemListChanged == null)
        {
            ItemListChanged = new UnityEvent();
        }

        arrowRenderer = directionArrow.GetComponent<SpriteRenderer>();
        arrowRenderer.enabled = false;
        InitializeItemList();
    }

    void InitializeItemList()
    {
        for (int i = 0; i < itemListMaxSize; i++)
        {
            itemList.Insert(0, GenerateNewItem());
        }

        ItemListChanged.Invoke();
    }


    private bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = currentPointer.position.ReadValue();
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void Update()
    {
        if (currentPointer.press.isPressed)
        {
            ManagePointerMove(currentPointer.position.ReadValue());

            if (!isPressing)
            {
                isPressing = true;
                ManageClick(false);
            }
        }
        else
        {
            if (isPressing)
            {
                ManageClick(true);
                isPressing = false;
            }
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }
    }


    public void ManagePointerMove(Vector2 positionValue)
    {
        pointerPositionInWorld = Camera.main.ScreenToWorldPoint(positionValue);
        pointerPositionInWorld.z = 0;

        if (currentItem != null)
        {
            var distanceFromObject = PointerDistanceFromCurrentItem();
            if (distanceFromObject.magnitude > minimumAdequateDistance)
            {
                distanceFromObject.Normalize();
                var itemPos = currentItem.transform.position;
                arrowRenderer.enabled = true;
                directionArrow.transform.position =
                    itemPos - new Vector3(distanceFromObject.x, distanceFromObject.y, 0) * 2;
                directionArrow.transform.right = distanceFromObject;
                currentItem.transform.right = distanceFromObject;
            }
            else
            {
                arrowRenderer.enabled = false;
            }
        }
        else
        {
            arrowRenderer.enabled = false;
        }
    }

    public void ManageClick(bool finishedClick)
    {
        if (!finishedClick && IsPointerOverUIElement())
        {
            return;
        }
        
        if (currentCooldown > 0f)
        {
            return;
        }

        if (!finishedClick && currentItem == null)
        {
            var newItem = UseItem();

            currentItem = Instantiate(newItem, pointerPositionInWorld, Quaternion.identity);
        }

        if (currentItem != null)
        {
            if (finishedClick)
            {
                Vector2 newVelocity = -PointerDistanceFromCurrentItem();
                if (newVelocity == Vector2.zero)
                {
                    newVelocity = Vector2.left;
                }
                else
                {
                    currentItem.transform.right = -newVelocity;
                }


                var rb = currentItem.GetComponent<Rigidbody2D>();

                newVelocity.Normalize();
                newVelocity = newVelocity * itemDropSpeed;
                rb.velocity = newVelocity;
                currentItem.transform.right = -newVelocity;
                arrowRenderer.enabled = false;
                canChangeHoldItem = true;
                currentItem = null;
                currentCooldown = NextCoolDown;
                holdButton.interactable = true;
            }
        }
    }

    private Vector2 PointerDistanceFromCurrentItem()
    {
        if (currentItem != null)
        {
            return pointerPositionInWorld - currentItem.transform.position;
        }

        return Vector2.positiveInfinity;
    }

    GameObject GenerateNewItem()
    {
        int randomIndex = (int) Random.Range(0, PossibleItems.Count);
        return PossibleItems[randomIndex];
    }

    private GameObject UseItem()
    {
        var index = itemList.Count - 1;
        var item = itemList[0];
        itemList.RemoveAt(0);

        itemList.Insert(index, GenerateNewItem());
        ItemListChanged.Invoke();

        canChangeHoldItem = true;

        return item;
    }

    public void ChangeHoldItem()
    {
        if (canChangeHoldItem)
        {
            canChangeHoldItem = false;

            holdButton.interactable = false;

            if (holdItem == null)
            {
                holdItem = UseItem();
            }
            else
            {
                var temp = holdItem;
                holdItem = itemList[0];
                itemList[0] = temp;
            }
        }
        
    }
}