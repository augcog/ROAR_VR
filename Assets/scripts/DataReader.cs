using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using UnityEngine;

public class DataReader : MonoBehaviour
{

    private const int PORT = 4567;
    private const int COMMAND_THROTTLE = 0;
    private const int COMMAND_STEERING = 1;
    public float throttle = 0, steering = 0;
    public ReaderWriterLock rwl;
    private Thread receiveThread;

    public static DataReader instance;
    // Start is called before the first frame update
    void Start()
    {
        rwl = new ReaderWriterLock();
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
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
    }

    private void ReceiveData()
    {
        UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT));
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
        byte[] buffer = new byte[4096];
        byte[] tmpBuffer = new byte[4096];
        byte[] valBuffer = new byte[8];
        int bufLen = 0;

        while (true)
        {
            byte[] data = client.Receive(ref remote);
            Buffer.BlockCopy(data, 0, buffer, bufLen, data.Length);
            bufLen += data.Length;
            while (bufLen >= 8)
            {
                Buffer.BlockCopy(buffer, 0, valBuffer, 0, 8);
                Array.Reverse(valBuffer);
                UInt32 cmd = BitConverter.ToUInt32(valBuffer, 4);
                int val = BitConverter.ToInt32(valBuffer, 0);
                bufLen -= 8;
                Buffer.BlockCopy(buffer, 8, tmpBuffer, 0, bufLen);
                buffer = tmpBuffer;

                if (cmd == COMMAND_THROTTLE)
                {
                    rwl.AcquireWriterLock(5);
                    throttle = val / 32767f;
                    rwl.ReleaseWriterLock();
                }
                else
                {
                    rwl.AcquireWriterLock(5);
                    steering = val / 32767f;
                    rwl.ReleaseWriterLock();
                }
            }
        }
    }
}