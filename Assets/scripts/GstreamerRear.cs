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
    public int flag;
    Mat fra;
    [DllImport("GStreamerReader.dll")] private static extern void GR_Init(int port1, int port2);
    [DllImport("GStreamerReader.dll")] private static extern IntPtr GR_GetFrame(ref int width, ref int height, int flag);
    [DllImport("GStreamerReader.dll")] private static extern void GR_Release();
    void Start()
    {
        texture = new Texture2D(1280, 720);
        textureleft = new Texture2D(720, 720);
        textureright = new Texture2D(720, 720);

        if (flag == 1)
        {
            GR_Init(5000, 5001);
            Debug.Log("GStreamer Ready");
        }
    }

    // Update is called once per frame
    void Update()
    {
        int width = 0, height = 0;
        IntPtr data = GR_GetFrame(ref width, ref height, flag);
        fra = new Mat(height, width, MatType.CV_8UC3, data);
        texture.LoadImage(fra.ToBytes());
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
    private void OnDestroy()
    {
        GR_Release();
    }
}
