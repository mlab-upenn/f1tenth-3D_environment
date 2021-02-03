using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;

public class Subscriber : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    SubscriberSocket client = new SubscriberSocket();
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        client.Connect("tcp://localhost:12345");
        client.Subscribe("");
        rb = player.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        string message = null;
        bool gotMessage = false;
        while (true)
        {
            gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
            if (gotMessage) break;
        }
    
        string[] args = message.Split(' ');
        rb.useGravity = false;
        rb.detectCollisions = false;
        rb.transform.position = new Vector3(float.Parse(args[0]), 0.5f, float.Parse(args[0]));
        rb.transform.rotation = Quaternion.Euler(0f, float.Parse(args[2]), 0f);
 

    }

    void OnDestory()
    {
        client.Close();
        NetMQConfig.Cleanup();
    }
}
