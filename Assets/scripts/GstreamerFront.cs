using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class GstreamerFront : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    public Material material;
    private Gstreamer gst;
    public int width, height;
    // private IntPtr f2;
    // private byte[] frame2;
    void Start()
    {
        //texture = new Texture2D(width, height);
        //width = gst.width;
        //height = gst.height;
        texture = new Texture2D(width, height,TextureFormat.RGB24,false,false);
        gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
        //f2 = gst.f2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //texture.LoadImage(gst.frame2);
        //frame2 = new byte[gst.size2];
        //System.Runtime.InteropServices.Marshal.Copy(f2, frame2, 0, gst.size2);
        texture.LoadRawTextureData(gst.frame2);
        texture.Apply();
        material.mainTexture = texture;
    }
}
