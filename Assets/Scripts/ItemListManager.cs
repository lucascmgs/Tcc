using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class ItemListManager : MonoBehaviour
{
    

    #region Public variables
    
    public float DefaultCooldown = 1.0f;
    
    [NonSerialized] public float currentCooldown = 0; public Queue<GameObject> itemList = new Queue<GameObject>();

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
    private GameObject currentItem;

    private GameObject holdItem;
    
    private Vector3 mousePositionInWorld = Vector3.zero;
    
    private Vector2 lastAdequateMousePosition = Vector2.zero;
    
    private Pointer currentMouse;
    
    private SpriteRenderer arrowRenderer;

    private bool canChangeHoldItem = true;
    #endregion
    
    void Start()
    {
        currentMouse = Mouse.current;
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
            itemList.Enqueue(GenerateNewItem());
        }
        ItemListChanged.Invoke();
    }

    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }
        
    }

    
    public void ManagePointerPosition(InputAction.CallbackContext context)
    {
        var positionValue = context.ReadValue<Vector2>();

        Debug.Log(positionValue);
        
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(positionValue);
        mousePositionInWorld.z = 0;

        if (currentItem != null)
        {
            var distanceFromObject = MouseDistanceFromCurrentItem();
            if (distanceFromObject.magnitude > minimumAdequateDistance)
            {
                distanceFromObject.Normalize();
                var itemPos = currentItem.transform.position;
                arrowRenderer.enabled = true;
                directionArrow.transform.position = itemPos - new Vector3(distanceFromObject.x, distanceFromObject.y, 0) * 2;
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
    
    public void ManageCurrentItem(InputAction.CallbackContext context)
    {

        var finishedClick = !context.performed;

        if (!finishedClick && currentCooldown == 0f && currentItem == null)
        {
            
            var newItem = UseItem();

            currentItem = PhotonNetwork.Instantiate(newItem.name, mousePositionInWorld, Quaternion.identity);
            
        } 
        if ( currentItem != null && currentCooldown == 0f)
        {
            if (finishedClick)
            {
                Vector2 newVelocity = - MouseDistanceFromCurrentItem();
                if (newVelocity == Vector2.zero)
                {
                    newVelocity = Vector2.left;
                }
                else
                {
                    currentItem.transform.right = - newVelocity;    
                }
                
                
                var rb = currentItem.GetComponent<Rigidbody2D>();

                newVelocity.Normalize();
                newVelocity = newVelocity * itemDropSpeed;
                rb.velocity = newVelocity;
                currentItem.transform.right = - newVelocity;
                arrowRenderer.enabled = false;
                canChangeHoldItem = true;
                currentItem = null;
                currentCooldown = DefaultCooldown;
            }
            
        }
        
    }

    private Vector2 MouseDistanceFromCurrentItem()
    {
        if (currentItem != null)
        {
            return mousePositionInWorld - currentItem.transform.position;
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
        var item = itemList.Dequeue();
        
        itemList.Enqueue(GenerateNewItem());
        ItemListChanged.Invoke();

        return item;
    }

    private void ChangeHoldItem()
    {
        canChangeHoldItem = false;
    }
}