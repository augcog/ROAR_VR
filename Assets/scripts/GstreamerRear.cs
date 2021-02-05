using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using System.Threading;
using System.Threading.Tasks;

public class GstreamerRear : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    //private Texture2D textureleft;
    //private Texture2D textureright;
    public Material materialleft;
    //public Material materialright;
    private Gstreamer gst;
    public int width,height;
    //private IntPtr f1;
    //private byte[] frame1;
    void Start()
    {
        //texture = new Texture2D(width , height);
        //width = gst.width;
        //height = gst.height;
        texture = new Texture2D(width, height, TextureFormat.RGB24, false, false);
        //textureleft = new Texture2D(height, height);
        //textureright = new Texture2D(height, height);
        ////gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
        //f1 = gst.f1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //frame1 = new byte[gst.size1];
        //System.Runtime.InteropServices.Marshal.Copy(f1, frame1, 0, gst.size1);
        ////texture.LoadRawTextureData(gst.frame1);
        texture.Apply();
        materialleft.mainTexture = texture;
        //materialleft.color = Color.white;
    }
}
