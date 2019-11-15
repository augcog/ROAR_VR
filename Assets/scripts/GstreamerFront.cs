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
    VideoCapture reader;
    [DllImport("GStreamerReader.dll")] private static extern void GR_Init(int port1, int port2);
    [DllImport("GStreamerReader.dll")] private static extern IntPtr GR_GetFrame(ref int width, ref int height, int flag);
    [DllImport("GStreamerReader.dll")] private static extern void GR_Release();
    void Start()
    {
        texture = new Texture2D(1280, 720);
        if (flag == 1)
        {
            GR_Init(5000, 5001);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int width = 0, height = 0;
        IntPtr data = GR_GetFrame(ref width, ref height, flag);
        fra = new Mat(height, width, MatType.CV_8UC3, data);
        texture.LoadImage(fra.ToBytes());
        material.mainTexture = texture;
        material.color = Color.white;
    }
    private void OnDestroy()
    {
        GR_Release();
    }
}
