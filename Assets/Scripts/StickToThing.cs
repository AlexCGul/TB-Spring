using System;
using System.Collections;
using UnityEngine;

public class StickToThing : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public Vector3 offset = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.25f);
        if (!target)
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (target)
            transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
            transform.position = target.transform.position + offset;
    }
}
