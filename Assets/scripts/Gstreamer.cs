using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class Gstreamer : MonoBehaviour
{
    [DllImport("GStreamerReader.dll")] private static extern bool GR_Init(int port1, int port2);
    [DllImport("GStreamerReader.dll")] private static extern IntPtr GR_GetFrame(int flag, Boolean useFisheye,ref int size);
    [DllImport("GStreamerReader.dll")] private static extern void GR_Release();

    public int port1, port2,width,height;
    public byte[] frame1, frame2;
    public ReaderWriterLock rwl1, rwl2;
    public Boolean useFisheye;
    private Thread receiveThread1, receiveThread2;
    public int size1,size2;
 
    // Start is called before the first frame update
    void Start()
    {
        GR_Init(port1, port2);
        receiveThread1 = new Thread(new ThreadStart(ReceiveData1));
        receiveThread1.IsBackground = true;
        receiveThread1.Start();
        receiveThread2 = new Thread(new ThreadStart(ReceiveData2));
        receiveThread2.IsBackground = true;
        receiveThread2.Start();
        //GR_Init(port1, port2);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        int width = 0, height = 0;
        IntPtr data1 = GR_GetFrame(ref width, ref height, 1, useFisheye);
        frame1 = (new Mat(height, width, MatType.CV_8UC3, data1)).ToBytes();
        IntPtr data2 = GR_GetFrame(ref width, ref height, 2,useFisheye);
        frame2 = (new Mat(height, width, MatType.CV_8UC3, data2)).ToBytes();
        */
        //IntPtr data2 = GR_GetFrame(2, useFisheye);
        //frame2 = (new Mat(data2)).ToBytes();
        //IntPtr data1 = GR_GetFrame(1, useFisheye);
        //frame1 = (new Mat(data1)).ToBytes();
    }

    private void OnDestroy()
    {
        if (receiveThread1 != null)
        {
            receiveThread1.Abort();
            receiveThread1.Join();
        }
        if (receiveThread2 != null)
        {
            receiveThread2.Abort();
            receiveThread2.Join();
        }
        GR_Release();
    }

    /*
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
            Debug.Log("switching");
            while (true)
            {
                
                IntPtr data2= GR_GetFrame(2, useFisheye, ref size2);
                frame2 = new byte[size2];
                System.Runtime.InteropServices.Marshal.Copy(data2, frame2, 0, size2);

                //IntPtr data1=GR_GetFrame(1, useFisheye, ref size1);
                //frame1 = new byte[size1];
                //System.Runtime.InteropServices.Marshal.Copy(data1, frame1, 0, size1);

            }
        }
        catch
        {
            return;
        }
    }
    */
    private void ReceiveData1()
    {
        try
        {
            while (true)
            {
                IntPtr data1=GR_GetFrame(1, useFisheye, ref size1);
                frame1 = new byte[size1];
                System.Runtime.InteropServices.Marshal.Copy(data1, frame1, 0, size1);

            }
        }
        catch
        {
            return;
        }
    }

    private void ReceiveData2()
    {
        try
        {
            while (true)
            {
                IntPtr data2 = GR_GetFrame(2, useFisheye, ref size2);
                frame2 = new byte[size2];
                System.Runtime.InteropServices.Marshal.Copy(data2, frame2, 0, size2);

            }
        }
        catch
        {
            return;
        }
    }
}
