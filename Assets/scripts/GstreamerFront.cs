using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class GstreamerFront : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    public Material material;
    FileStream fs;
    void Start()
    {
        texture = new Texture2D(1280, 720);
    }

    // Update is called once per frame
    void Update()
    {

        fs = new FileStream("b.jpg", FileMode.Open, FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[fs.Length];
        fs.Read(bytes, 0, (int)fs.Length);
        texture.LoadImage(bytes);
        material.mainTexture = texture;
        material.color = Color.white;
    }
}
