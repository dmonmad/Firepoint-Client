using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    public static void SetLayerRecursively(string layerName, GameObject _object)
    {
        for(int i = 0; i < _object.transform.childCount; i++)
        {
            _object.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
