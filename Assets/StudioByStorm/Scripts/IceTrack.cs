using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrack : MonoBehaviour
{
    public List<GameObject> tracks = new List<GameObject>();
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        deactivateTracks();
        anim.SetTrigger("activate");
    }

    void OnDisable()
    {
        deactivateTracks();
    }

    void deactivateTracks()
    {
        for(int i = 0; i < tracks.Count; i++) {
            tracks[i].SetActive(false);
        }
    }
}
