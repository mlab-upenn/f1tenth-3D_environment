using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject RosConnect;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        RosConnect = GameObject.Find("RosConnector");
    }
    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
        rb =  GetComponent<Rigidbody> ();
        rb.useGravity = false;
        rb.detectCollisions = false;
        Vector3 oldFramePos = RosConnect.GetComponent<RosSharp.RosBridgeClient.PoseStampedSubscriber>().position;
        rb.transform.position = new Vector3(oldFramePos[0], oldFramePos[1]+ 0.5f,oldFramePos[2]);
        Quaternion oldFrameRot = RosConnect.GetComponent<RosSharp.RosBridgeClient.PoseStampedSubscriber>().rotation;
        rb.transform.rotation = oldFrameRot;

    }

}
