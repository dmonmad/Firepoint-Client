using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCountDetector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount == 1)
            OnSingleClick();
        else if (clickCount == 2)
            OnDoubleClick();
        else if (clickCount > 2)
            OnMultiClick();
    }

    /// <summary>Detects a single click</summary>
    void OnSingleClick()
    {
        //Debug.Log("Single Clicked");
    }

    /// <summary>Detects a double click.</summary>
    void OnDoubleClick()
    {
        UIManager.instance.ConnectToServer(this.gameObject.GetComponent<ServerObjectModel>().ip.GetParsedText());
    }

    /// <summary>Detects a multiple click.</summary>
    void OnMultiClick()
    {
        //Debug.Log("MultiClick Clicked");
    }
}
