using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class ItemListManager : MonoBehaviour
{
    [SerializeField] private GameObject directionArrow;
    private SpriteRenderer arrowRenderer;
    
    [SerializeField] private float cooldown = 1.0f;
    [SerializeField] private float minimumAdequateDistance = 1.4f;

    private float currentCooldown = 0;

    [SerializeField] private float itemDropSpeed = 5.0f;
    
    public Queue<GameObject> itemList = new Queue<GameObject>();

    public int itemListMaxSize = 2;

    public List<GameObject> PossibleItems;

    public UnityEvent ItemListChanged;

    private GameObject CurrentItem;

    
    private Vector3 mousePositionInWorld = Vector3.zero;
    private Vector2 lastAdequateMousePosition = Vector2.zero;
    
    
    private Pointer currentMouse;

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

        if (CurrentItem != null)
        {
            var distanceFromObject = MouseDistanceFromCurrentItem();
            if (distanceFromObject.magnitude > minimumAdequateDistance)
            {
                distanceFromObject.Normalize();
                var itemPos = CurrentItem.transform.position;
                arrowRenderer.enabled = true;
                directionArrow.transform.position = itemPos - new Vector3(distanceFromObject.x, distanceFromObject.y, 0) * 2;
                directionArrow.transform.right = distanceFromObject;
                CurrentItem.transform.right = distanceFromObject;
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
        var name = context.action.name;
        
        Debug.Log("Name: " + name + " Finished: " + finishedClick);
        
        if (!finishedClick && currentCooldown == 0f && CurrentItem == null)
        {
            currentCooldown = cooldown;
            var newItem = UseItem();

            CurrentItem = PhotonNetwork.Instantiate(newItem.name, mousePositionInWorld, Quaternion.identity);
            
        } 
        if ( CurrentItem != null)
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
                    CurrentItem.transform.right = - newVelocity;    
                }
                
                
                var rb = CurrentItem.GetComponent<Rigidbody2D>();

                newVelocity.Normalize();
                newVelocity = newVelocity * itemDropSpeed;
                rb.velocity = newVelocity;
                CurrentItem.transform.right = - newVelocity;
                arrowRenderer.enabled = false;
                CurrentItem = null;
            }
            
        }
        
    }

    private Vector2 MouseDistanceFromCurrentItem()
    {
        if (CurrentItem != null)
        {
            return mousePositionInWorld - CurrentItem.transform.position;
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
}