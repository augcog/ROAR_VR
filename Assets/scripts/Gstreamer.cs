using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using OpenCvSharp;

public class Gstreamer : MonoBehaviour
{
    [DllImport("GStreamerReader.dll")] private static extern void GR_Init(int port1, int port2);
    [DllImport("GStreamerReader.dll")] private static extern IntPtr GR_GetFrame(ref int width, ref int height, int flag);
    [DllImport("GStreamerReader.dll")] private static extern void GR_Release();

    public int port1, port2;
    public Mat frame1, frame2;

    // Start is called before the first frame update
    void Start()
    {
        GR_Init(port1, port2);
    }

    // Update is called once per frame
    void Update()
    {
        int width = 0, height = 0;
        IntPtr data = GR_GetFrame(ref width, ref height, 1);
        frame1 = new Mat(height, width, MatType.CV_8UC3, data);
        data = GR_GetFrame(ref width, ref height, 2);
        frame2 = new Mat(height, width, MatType.CV_8UC3, data);
    }

    private void OnDestroy()
    {
        GR_Release();
    }
}
