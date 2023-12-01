using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SocketIOClient;
using JetBrains.Annotations;
using UnityEngine.UI;

public class SocketIOManager : MonoBehaviour
{
    public SocketIOUnity socket;
    public Button SendBTN;
    public InputField SendField;
    public Text chattingWindow;
    [SerializeField] string nickName;
    public Queue<string> chattingQueue = new Queue<string>();
    public Queue<string> playerLoadWaiting = new Queue<string>();

    private void Start()
    {
        SetSocket("http://localhost:3000");
        SendBTN.onClick.AddListener(() =>
        {
            SendPressed();
        });

    }
    private void Update()
    {
        if (chattingQueue.Count >0)
        {
            chattingWindow.text += "\n" + chattingQueue.Dequeue();
        }
        if (playerLoadWaiting.Count >0)
        {
            GameObject.CreatePrimitive(PrimitiveType.Cube).name = playerLoadWaiting.Dequeue();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            socket.Emit("connectUser", "ㅁㅇㄴㄹ");
        }


    }
    private void SendPressed()
    {
        socket.Emit("chat message", SendField.text);
    }
    private void SetSocket(string uri)
    {
        SocketIOUnity tempIO = new SocketIOUnity(uri);
        tempIO.OnConnected += (sender,e)=>
        {
            Debug.Log(tempIO.ServerUri+"에 연결됨");
            
        };

        socket = tempIO;
        socket.Connect();
        socket.On("chat message", (msg) =>
        {
            Debug.Log(msg.ToString());
            chattingQueue.Enqueue(msg.ToString());
        });
        socket.On("connectUser", (ClientName) =>
        {
            Debug.Log("이름줄게");
            Debug.Log(ClientName + "이름");
            playerLoadWaiting.Enqueue(ClientName.ToString());
            nickName = ClientName.ToString();

        });

    }
    
    private void OnApplicationQuit()
    {
        socket.Disconnect();
        Debug.Log("연결 종료");
    }
}