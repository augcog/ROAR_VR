using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using OpenCvSharp;

public class Gstreamer : MonoBehaviour
{
    [DllImport("GStreamerReader.dll")] private static extern bool GR_Init(int port1, int port2);
    [DllImport("GStreamerReader.dll")] private static extern IntPtr GR_GetFrame(ref int width, ref int height, int flag);
    [DllImport("GStreamerReader.dll")] private static extern void GR_Release();

    public int port1, port2;
    public byte[] frame1, frame2;
    public ReaderWriterLock rwl1, rwl2;
    private Thread receiveThread;

    // Start is called before the first frame update
    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        receiveThread.Abort();
        receiveThread.Join();
        GR_Release();
    }

    private void ReceiveData()
    {
        try
        {
            while (true)
            {
                if (GR_Init(port1, port2))
                {
                    break;
                }
            }
            while (true)
            {
                int width = 0, height = 0;
                IntPtr data1 = GR_GetFrame(ref width, ref height, 1);
                frame1 = (new Mat(height, width, MatType.CV_8UC3, data1)).ToBytes();
                IntPtr data2 = GR_GetFrame(ref width, ref height, 2);
                frame2 = (new Mat(height, width, MatType.CV_8UC3, data2)).ToBytes();
            }
        }
        catch
        {
            return;
        }
    }
}
