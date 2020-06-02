using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestClient
{
    private static RestClient _instance;

    private RestClient() { }

    public static RestClient GetInstance()
    {
        if (_instance == null)
        {
            _instance = new RestClient();
        }
        return _instance;
    }

    /// <summary>Sends a request to a url and throws the formatted result back.</summary>
    /// <param name="url">Url of the master server.</param>
    /// <param name="callBack">The callback function.</param>
    /// <returns>Returns a list of ServerModels</returns>
    public IEnumerator Get(string url, System.Action<ServerList> callBack)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Console.GetInstance().Log("Client couldn't connect with master-server: \n[" + www.error + "]");
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    ServerList servers = JsonUtility.FromJson<ServerList>("{\"servers\":" + jsonResult + "}");
                    callBack(servers);
                }
            }
        }
    }
}
