using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenSubBehaviour : MonoBehaviour
{
    private List<GameObject> children;

    [SerializeField] private float tweenFactor = 0.001f;
    [SerializeField] private float descentFactor = 0.1f;
    void Start()
    {
        children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        int id = 0;
        foreach (var child in children)
        {
            child.transform.position += new Vector3(tweenFactor * Mathf.Pow(-1, id) * Mathf.Cos(Time.time + id), -descentFactor, 0);
            id++;
        }
    }
}
