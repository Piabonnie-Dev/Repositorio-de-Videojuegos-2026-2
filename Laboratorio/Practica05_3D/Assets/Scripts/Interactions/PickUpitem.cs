using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpitem : MonoBehaviour
{


    void OnTriggerEnter(Collider other) 
    {
if (other.CompareTag("Player"))
 {
Debug.Log("Objeto recogido");
Destroy(gameObject);

}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
