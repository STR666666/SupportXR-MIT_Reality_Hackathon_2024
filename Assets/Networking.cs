using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class Networking : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public string WebsocketURL;
    WebSocket websocket;
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log(WebsocketURL);
        websocket = new WebSocket(WebsocketURL);
        websocket.OnOpen += () =>
        {
          Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
          Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
          Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
          Debug.Log("OnMessage!");
          Debug.Log(bytes);
          var message = System.Text.Encoding.UTF8.GetString(bytes);
          var vctrs = message.Split(':');
          Player2.transform.position = new Vector3(float.Parse(vctrs[0]),float.Parse(vctrs[1]),float.Parse(vctrs[2]));
          Player2.transform.rotation = Quaternion.Euler(float.Parse(vctrs[3]),float.Parse(vctrs[4]),float.Parse(vctrs[5]));
          //Player2.transform.Find("Left Controller").position = new Vector3(vctrs[0],vctrs[1],vctrs[2])
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        await websocket.Connect();
    }

    async void SendWebSocketMessage()
  {
    if (websocket.State == WebSocketState.Open)
    {
        Debug.Log("HAIIII");
      await websocket.SendText(Player1.transform.position.x + ":" + Player1.transform.position.y + ":" + Player1.transform.position.z);
    }
  }

    // Update is called once per frame
    void Update()
    {
        
    }
}
