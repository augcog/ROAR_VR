using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class WebStreamRear : MonoBehaviour
{

    [HideInInspector]
    public Byte[] JpegData;
    [HideInInspector]
    public string resolution = "1280x720";

    private Texture2D texture;
    private Texture2D textureleft;
    private Texture2D textureright;
    private Stream stream;
    private HttpWebResponse resp;
    //public MeshRenderer frameleft;
    //public MeshRenderer frameright;

    public Material materialleft;
    public Material materialright;
    public void Start()
    {
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
        textureleft = new Texture2D(720, 720);
        textureright = new Texture2D(720, 720);


        string url = "http://192.168.50.131:8887/video_a";
        //string url = "http://10.0.0.217:8887/video_a";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        resp = (HttpWebResponse)req.GetResponse();
        //Debug.Log(resp.ContentLength);
        //Debug.Log(resp.ContentType);
        // get response stream
        stream = resp.GetResponseStream();
        materialleft.color = Color.white;
        materialright.color = Color.white;
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
            byte[] Image = ms.GetBuffer();
          
            texture.LoadImage(Image);
            Color[] c = texture.GetPixels(0, 0, 720, 720);
            textureleft.SetPixels(c);
            textureleft.Apply();
            materialleft.mainTexture = textureleft;
            materialleft.color = Color.white;

            c = texture.GetPixels(560, 0, 720, 720);
            textureright.SetPixels(c);
            textureright.Apply();
            materialright.mainTexture = textureright;
            materialright.color = Color.white;

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