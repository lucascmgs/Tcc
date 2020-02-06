using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class ItemListManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float cooldown = 1.0f;

    private float currentCooldown = 0;

    [SerializeField] private float itemDropSpeed = 5.0f;
    
    public Queue<GameObject> itemList = new Queue<GameObject>();

    public int itemListMaxSize = 2;

    public List<GameObject> PossibleItems;

    public UnityEvent ItemListChanged;

    private GameObject CurrentItem;

    public GameObject HoldItem;
    
    private Vector3 mousePositionInWorld = Vector3.zero;
    private Vector3 previousMousePositionInWorld = Vector3.zero;
    private Pointer currentMouse;

    void Start()
    {
        currentMouse = Mouse.current;
        Random.InitState(System.DateTime.UtcNow.Second);
        if (ItemListChanged == null)
        {
            ItemListChanged = new UnityEvent();
        }
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
        
        if (mousePositionInWorld != previousMousePositionInWorld)
        {
            previousMousePositionInWorld = mousePositionInWorld;    
        }

        Debug.Log(positionValue);
        
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(positionValue);
        mousePositionInWorld.z = 0;

        if (CurrentItem != null)
        {
            CurrentItem.transform.position = mousePositionInWorld;
        }
    }
    public void ManageCurrentItem(InputAction.CallbackContext context)
    {
        
        
        var finishedClick = context.performed;
        var name = context.action.name;
        
        Debug.Log("Name: " + name + " Finished: " + finishedClick);
        
        if (finishedClick && currentCooldown == 0f && CurrentItem == null)
        {
            currentCooldown = cooldown;
            var newItem = UseItem();

            CurrentItem = PhotonNetwork.Instantiate(newItem.name, mousePositionInWorld, Quaternion.identity);
            
        } 
        if ( CurrentItem != null)
        {
            var newVelocity = new Vector2(mousePositionInWorld.x - previousMousePositionInWorld.x, mousePositionInWorld.y - previousMousePositionInWorld.y);
            if (newVelocity == Vector2.zero)
            {
                newVelocity = Vector2.left;
            }
            else
            {
                CurrentItem.transform.right = - newVelocity;    
            }

            if (!finishedClick)
            {
                var rb = CurrentItem.GetComponent<Rigidbody2D>();

                newVelocity.Normalize();
                newVelocity = newVelocity * itemDropSpeed;
                rb.velocity = newVelocity;
                CurrentItem.transform.right = - newVelocity;
                CurrentItem = null;
            }
            
        }
        
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