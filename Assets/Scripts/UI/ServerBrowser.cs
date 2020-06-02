using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBrowser : MonoBehaviour
{
    public List<GameObject> ServerObjectList = new List<GameObject>();

    public GameObject ServerObjectTemplate;
    public Transform ServerObjectContainer;

    /// <summary>Clears the created servers, stop the active searchs and starts a new one.</summary>
    public void GetServers()
    {
        ClearServers();
        StopAllCoroutines();
        StartCoroutine(RestClient.GetInstance().Get(Constants.MASTERSERVER_URL, UpdateServerList));
    }

    /// <summary>Proccesses the given ServerModel list.</summary>
    /// <param name="_serverList">Adds each server to the server browser object.</param>
    public void UpdateServerList(ServerList _serverList)
    {
        foreach(ServerModel _server in _serverList.servers)
        {
            GameObject _serverObject = Instantiate(ServerObjectTemplate, ServerObjectContainer);
            _serverObject.GetComponent<ServerObjectModel>().Initialize(_server.servername, _server.ip, "?");
            ServerObjectList.Add(_serverObject);
        }
    }

    /// <summary>Destroy the server's objects and clears the list.</summary>
    public void ClearServers()
    {
        for (int i = 0; i < ServerObjectList.Count; i++)
        {
            Destroy(ServerObjectList[i]);
        }
        ServerObjectList.RemoveAll(i => i == null);
    }

    private void OnEnable()
    {
        GetServers();
    }

    /// <summary>Disables the Server Browser's object.</summary>
    public void CloseServerBrowser()
    {
        this.gameObject.SetActive(false);
    }
}
