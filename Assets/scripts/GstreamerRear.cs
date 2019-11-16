using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using OpenCvSharp;
public class GstreamerRear : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    private Texture2D textureleft;
    private Texture2D textureright;
    public Material materialleft;
    public Material materialright;
    private Gstreamer gst;

    void Start()
    {
        texture = new Texture2D(1280, 720);
        textureleft = new Texture2D(720, 720);
        textureright = new Texture2D(720, 720);
        gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
    }

    // Update is called once per frame
    void Update()
    {
        texture.LoadImage(gst.frame1);
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
    }
}
