using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBackgroundBehaviour : MonoBehaviour
{
    GameObject submarine;
    float lastPosition;

    void Start()
    {
        submarine = FindObjectOfType<SubMovement>().gameObject;
        lastPosition = submarine.transform.position.y;
    }

    void Update()
    {
        checkSubMovement();
    }

    void checkSubMovement()
    {
        float difPos = submarine.transform.position.y - lastPosition;
        lastPosition = submarine.transform.position.y;

        if (difPos != 0)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (difPos * Time.deltaTime * 2), 10);
        }
    }
}
