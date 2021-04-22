using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
public class Publisher : MonoBehaviour
{
    // Start is called before the first frame update
    
    public string portNum;
    public Camera ImageCamera;
    public string FrameId = "Camera";
    public int resolutionWidth = 640;
    public int resolutionHeight = 480;
    [Range(0, 100)]
    public int qualityLevel = 50;
    PublisherSocket server = new PublisherSocket(); 

    private Texture2D texture2D;
    private Rect rect;

    void Start()
    {
        InitializeGameObject();
        server.Bind("tcp://*:" + portNum);
        Camera.onPostRender += UpdateImage;
    }

    private void UpdateImage(Camera _camera)
    {
        if (texture2D != null && _camera == this.ImageCamera)
            onPostRender();
    }

    private void InitializeGameObject()
    {
        texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
        // ImageCamera.targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
    }

    private void onPostRender()
    { 
        texture2D.ReadPixels(rect, 0, 0);
        byte[] data = texture2D.EncodeToJPG(qualityLevel);
        server.SendMoreFrame("A") // Topic
              .SendFrame(data); // Message
    }
}
