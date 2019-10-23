using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class WebStream : MonoBehaviour
{

    [HideInInspector]
    public Byte[] JpegData;
    [HideInInspector]
    public string resolution = "1280x720";

    private Texture2D texture;
    private Stream stream;
    private HttpWebResponse resp;
    //public MeshRenderer frame;
    //private Material material;
    public Image image;
    public void Start()
    {
        //material = image.material;
        GetVideo();

    }

   
    public void StopStream()
    {
        stream.Close();
        resp.Close();
    }

    public void GetVideo()
    {
        texture = new Texture2D(1280, 720);

        string url = "http://192.168.50.131:8887/video_b";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        resp = (HttpWebResponse)req.GetResponse();
        //Debug.Log(resp.ContentLength);
        //Debug.Log(resp.ContentType);
        // get response stream
        stream = resp.GetResponseStream();
        image.material.color = Color.white;
        StartCoroutine(GetFrame());
    }

    public IEnumerator GetFrame()
    {
        Byte[] JpegData = new byte[921600];
        while (true)
        {
            int bytesToRead = FindLength(stream);
            //Debug.Log("Left To Read" + bytesToRead);
            if (bytesToRead == -1)
            {
                yield break;
            }

            int leftToRead = bytesToRead;

            while (leftToRead > 0)
            {
                //Debug.Log("Left To Read" + leftToRead);
                leftToRead -= stream.Read(JpegData, bytesToRead - leftToRead, leftToRead);
                yield return null;
            }

            MemoryStream ms = new MemoryStream(JpegData, 0, bytesToRead, false, true);

            texture.LoadImage(ms.GetBuffer());
            image.material.mainTexture = texture;
            image.material.color = Color.white;
            stream.ReadByte(); // CR after bytes
            stream.ReadByte(); // LF after bytes
        }
    }

    int FindLength(Stream stream)
    {
        int b;
        string line = "";
        int result = -1;
        bool atEOL = false;
        while ((b = stream.ReadByte()) != -1)
        {
            //Debug.Log("b" + b);
            if (b == 10) continue; // ignore LF char
            if (b == 13)
            { // CR
                if (atEOL)
                {  
                    stream.ReadByte(); // eat last LF

                    return result;
                }
                if (line.StartsWith("Content-length:"))
                {
                    result = Convert.ToInt32(line.Substring("Content-length:".Length).Trim());
                   
                }
                else
                {
                    line = "";
                }
                atEOL = true;
            }
            else
            {
                atEOL = false;
                line += (char)b;
                //Debug.Log("line" + line);
            }
        }
        return -1;
    }
}