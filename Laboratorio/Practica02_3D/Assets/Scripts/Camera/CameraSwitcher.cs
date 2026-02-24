using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

public GameObject fpsCam;
public GameObject tpsCam;
bool usingFps = false;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            usingFps = !usingFps;
            fpsCam.SetActive(usingFps);
            tpsCam.SetActive(!usingFps);
        }
    }
}
