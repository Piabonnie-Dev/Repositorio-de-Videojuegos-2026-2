using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
public float distance = 5f;
public float height = 2f;
public float smoothSpeed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
 Vector3 desiredPos = target.position - target.forward * distance
+ Vector3.up * height;
transform.position = Vector3.Lerp(transform.position,
desiredPos, smoothSpeed * Time.deltaTime);
transform.LookAt(target);
    }
}
