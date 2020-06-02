using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    /// <summary>Set the layer on the object and its children.</summary>
    /// <param name="layerName">The layer's name to be set</param>
    /// <param name="_object">The object's name which layer should be changed.</param>
    public static void SetLayerRecursively(string layerName, GameObject _object)
    {
        for(int i = 0; i < _object.transform.childCount; i++)
        {
            _object.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
