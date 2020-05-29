using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, ClientVariables.DecalDestroyTime);
    }
}
