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

    void OnSingleClick()
    {
        Debug.Log("Single Clicked");
    }

    void OnDoubleClick()
    {

        Console.GetInstance().Log(this.gameObject.GetComponent<ServerObjectModel>().ip.GetParsedText());
        UIManager.instance.ConnectToServer(this.gameObject.GetComponent<ServerObjectModel>().ip.GetParsedText());
        Console.GetInstance().Log("Trying to connect");
    }

    void OnMultiClick()
    {
        Debug.Log("MultiClick Clicked");
    }
}
