using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWheelGetData : MonoBehaviour
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
        //reader.rwl.AcquireReaderLock(5);
        transform.rotation = Quaternion.Euler(new Vector3(12.56f, 0, -117f * reader.steering));
        //reader.rwl.ReleaseReaderLock();
    }
}
