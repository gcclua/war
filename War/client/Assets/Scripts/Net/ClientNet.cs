﻿using Net.Tcp;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientNet : MonoBehaviour {
    
    private static ClientNet instance;
    public static ClientNet Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ClientNet");
                obj.tag = "NET";
                instance = obj.AddComponent<ClientNet>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private Net.Tcp.TcpClient m_Client;
    private CActor m_Actor;
    void Awake () {
        m_Actor = new CActor();
        m_Client = new Net.Tcp.TcpClient(m_Actor);
        m_Client.Start();
    }
    
    void OnDestroy()
    {
        m_Client.Close();
    }
    
    // Update is called once per frame
    void Update () {
        m_Client.Update();
        if (!m_Client.IsConnected)
        {
            GameObject net = GameObject.Find("ClientNet");
            // 加载预制体
            GameObject _netNotice = Resources.Load("NetDisconnected") as GameObject;
            // 对象初始化
            GameObject root = GameObject.Find("Canvas");
            GameObject netNotice = Instantiate(_netNotice, root.transform);
            Destroy(net);
        }
       
    }
    
    public void Send(byte[] message)
    {
        m_Client.Send(message);
        
    }

}

public class CActor : ICTcpActor
{
    public void SetConnName(string connName)
    {

    }
    public void Handle(byte[] message)
    {
        string msgName;
        var msg = ProtoHelper.DecodeWithName(message, out msgName);
        Debug.Log("CActor->" + msgName);
        NetDispacher.Instance.DispachEvent(msgName, msg);
        
    }

    ICTcpConnection tcpConnection;
    public void Initialize(ICTcpConnection conn)
    {
        tcpConnection = conn;
        tcpConnection.Connect("172.20.120.146", 26001);
    }

    public void OnConnected(SocketError err)
    {
        if (err == SocketError.Success)
        {
            Debug.Log("连接");
            var msg = new mmopb.login_ack();
            msg.error = "Hello!";
            tcpConnection.Send(ProtoHelper.EncodeWithName(msg));
        }
    }

    public void OnDisconnected(SocketError err)
    {

    }
  
}
