using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    GameObject submarine;
    float lastPosition;

    void Start()
    {
        submarine = FindObjectOfType<SubMovement>().gameObject;
        lastPosition = submarine.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        checkSubMovement();
    }

    void checkSubMovement()
    {
        float difPos = submarine.transform.position.y - lastPosition;
        lastPosition = submarine.transform.position.y;

        if(difPos != 0)
        {
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + (difPos * Time.deltaTime * 2));
        }
    }


}
