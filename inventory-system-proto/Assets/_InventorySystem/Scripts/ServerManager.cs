using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerManager : MonoBehaviour
{
    private string serverURL = "https://wadahub.manerai.com/api/inventory/status";
    private string authKey = "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSQpq6c7STWfGxzIhxPfDh8MaP";

    [System.Serializable]
    private class InventoryRequestData
    {
        public string identifier;
        public string eventType;

        public InventoryRequestData(string id, string type)
        {
            identifier = id;
            eventType = type;
        }
    }

    public void HandleItemAdded(string itemId)
    {
        SendRequest(itemId, "added");
    }

    public void HandleItemRemoved(string itemId)
    {
        SendRequest(itemId, "removed");
    }

    private void SendRequest(string itemId, string eventType)
    {
        InventoryRequestData requestData = new InventoryRequestData(itemId, eventType);
        string jsonData = JsonUtility.ToJson(requestData);

        StartCoroutine(SendToServer(jsonData));
    }

    private IEnumerator SendToServer(string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", authKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Server response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Server error: " + request.error);
        }
    }
}