using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardGetData : MonoBehaviour
{
    private DataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        reader = GameObject.Find("Utility").GetComponent<DataReader>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f * reader.throttle));
    }
}
