using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameras : MonoBehaviour {

    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;
    private int i = 0;

    // Use this for initialization
    void Start () {
        cam1.enabled = true;
        cam2.enabled = false;
        cam3.enabled = false;
        cam4.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        /* if (Input.GetKeyDown(KeyCode.Tab))
         {
             cam1.enabled = !cam2.enabled;
             cam2.enabled = !cam3.enabled;
             cam3.enabled = !cam4.enabled;
             cam4.enabled = !cam1.enabled;
         }*/
        
        switch (i)
        {
            case 1:
                cam1.enabled = false;
                cam2.enabled = true;
                break;
            case 2:
                cam2.enabled = false;
                cam3.enabled = true;
                break;
            case 3:
                cam3.enabled = false;
                cam4.enabled = true;
                break;
            default:
                cam4.enabled = false;
                cam1.enabled = true;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            i++;

        if (i > 3)
            i = 0;
    }
}
