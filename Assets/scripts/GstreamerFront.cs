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
    public int flag;
    Mat fra;
    private Gstreamer gst;

    void Start()
    {
        texture = new Texture2D(1280, 720);
        gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
    }

    // Update is called once per frame
    void Update()
    {
        // gst.rwl2.AcquireReaderLock(0);
        // fra = gst.frame2;
        texture.LoadImage(gst.frame2.ToBytes());
        // gst.rwl2.ReleaseReaderLock();
        material.mainTexture = texture;
        material.color = Color.white;
    }
}
