using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShoebill : MonoBehaviour
{
    public void OnTriggerEnter(Collider coll)
    {
        coll.transform.localPosition = new Vector3(Random.Range(-2.0f, 12.5f), 1.85f, 9.18f);
    }

}
