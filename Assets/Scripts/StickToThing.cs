using System;
using UnityEngine;

public class StickToThing : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] Vector3 offset = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnValidate()
    {
        if (target)
            transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
