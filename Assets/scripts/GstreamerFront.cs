using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using OpenCvSharp;
public class GstreamerFront : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    public Material material;
    private Gstreamer gst;
    public int width, height;

    void Start()
    {
        //texture = new Texture2D(width, height);
        //width = gst.width;
        //height = gst.height;
        texture = new Texture2D(width, height,TextureFormat.RGB24,false,false);
        gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
    }

    // Update is called once per frame
    void Update()
    {
        //texture.LoadImage(gst.frame2);
        texture.LoadRawTextureData(gst.frame2);
        texture.Apply();
        material.mainTexture = texture;
        material.color = Color.white;
    }
}
