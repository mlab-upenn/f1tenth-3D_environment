using System.Collections.Concurrent;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;

public class NetMqListener
{   
    public string portNum;
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        var timeout = new System.TimeSpan(0, 0, 0);
        using (var subSocket = new SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect("tcp://localhost" + portNum);
            subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string frameString;
                if (!subSocket.TryReceiveFrameString(timeout, out frameString)) continue;
                Debug.Log(frameString);
                _messageQueue.Enqueue(frameString);
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup(false);
    }

    public void Update()
    {
        while (!_messageQueue.IsEmpty)
        {
            string message;
            if (_messageQueue.TryDequeue(out message))
            {
                _messageDelegate(message);
            }
            else
            {
                break;
            }
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void OnApplicationQuit()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}


public class Subscriber : MonoBehaviour
{
    public const float PI = 3.1415926535897931f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private string portNum;
    Rigidbody rb;
    private NetMqListener _netMqListener;
    
    private void HandleMessage(string message)
    {
        Console.WriteLine(message);
        var splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 3) return;
        var z = float.Parse(splittedStrings[0]);
        var x = float.Parse(splittedStrings[1]);
        var theta = float.Parse(splittedStrings[2]);
        rb.transform.position = new Vector3(x, 0.5f, z);
        rb.transform.rotation = Quaternion.Euler(0f, (theta)/PI*180f, 0f);

    }
    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.detectCollisions = false;
        _netMqListener = new NetMqListener(HandleMessage);
        _netMqListener.portNum = portNum;
        _netMqListener.Start();
    }

    private void Update()
    {
        _netMqListener.Update();
    }

    private void OnDestroy()
    {
        _netMqListener.OnApplicationQuit();
    }
}
