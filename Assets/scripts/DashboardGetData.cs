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
        reader.rwl.AcquireReaderLock(5);
        transform.rotation = Quaternion.Euler(new Vector3(12.56f, 0, -117f * reader.steering));
        reader.rwl.ReleaseReaderLock();
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go.name == "ArrowUp")
            {
                float s = (reader.throttle > 0 ? reader.throttle : 0);
                Color color = go.GetComponent<Image>().color;
                color.a = s;
                go.GetComponent<Image>().color = color;
            }
            if (go.name == "ArrowDown")
            {
                float s = (reader.throttle < 0 ? -reader.throttle : 0);
                Color color = go.GetComponent<Image>().color;
                color.a = s;
                go.GetComponent<Image>().color = color;
            }
        }
    }
}
