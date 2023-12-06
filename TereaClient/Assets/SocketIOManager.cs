using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SocketIOClient;
using JetBrains.Annotations;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SocketIOManager : MonoBehaviour
{
    public SocketIOUnity socket;
    public Button SendBTN;
    public InputField SendField;
    public Text chattingWindow;
    [SerializeField]private UserInfo userInfo;
    public Queue<string> chattingQueue = new Queue<string>();
    public Queue<string> playerLoadWaiting = new Queue<string>();
    public Queue<string> playerLeaveWaiting = new Queue<string>();
    public float timer = 0;
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
        if (playerLeaveWaiting.Count >0)
        {
            Destroy(GameObject.Find(playerLeaveWaiting.Dequeue()));
        }
        timer += Time.deltaTime;
        if (timer > 2)
        {
            PlayerPosPacket(transform.position);
            timer = 0;
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
            Debug.Log(ClientName.ToString() + "이름");
            playerLoadWaiting.Enqueue(ClientName.ToString());
            

        });
        socket.On("setUserInfo", (userServerInfo) =>
        {
            string tempJsonSTR = userServerInfo.ToString();
            tempJsonSTR = ServerReflectedJson(tempJsonSTR);
            UserInfo tempInfo = JsonConvert.DeserializeObject<UserInfo>(tempJsonSTR);
            Debug.Log("이름줄게");
            Debug.Log(tempJsonSTR + "이름");
            userInfo = tempInfo;
        });
        socket.On("PlayerPosPacket", (plrPos) =>
        {
            string tempSTR = plrPos.ToString();
            tempSTR = ServerReflectedJson(tempSTR);
            Debug.Log(tempSTR);
            UserPos convertedPos = JsonConvert.DeserializeObject<UserPos>(tempSTR);
            Debug.Log(convertedPos);
        });
    socket.On("logOutUserInfo", (userName) =>
        {
            chattingQueue.Enqueue(userName+"님이 로그아웃했습니다.");
            playerLeaveWaiting.Enqueue(userName.ToString());
        });

    }
    private string ServerReflectedJson(string tempJsonSTR)
    {
        tempJsonSTR = tempJsonSTR.Remove(tempJsonSTR.Length - 1, 1);
        tempJsonSTR = tempJsonSTR.Remove(0, 1);
        return tempJsonSTR;
    }
    public void PlayerPosPacket(Vector3 vec3)
    {
        UserPos userPos = new UserPos
        {
            xPos = vec3.x,
            yPos = vec3.y,
            zPos = vec3.z,
            userName = userInfo.userServerID
        };
        socket.Emit("PlayerPosPacket", JsonConvert.SerializeObject(userPos));
    }
    private void OnApplicationQuit()
    {
        socket.Emit("RemoveUserInList", JsonConvert.SerializeObject(userInfo));
        socket.Disconnect();
        Debug.Log("연결 종료");
    }
}
[System.Serializable]
public class UserInfo
{
    public int userListIndex;
    public string userServerID;
}
[System.Serializable]
public class UserPos
{
    public float xPos;
    public float yPos;
    public float zPos;
    public string userName;
}