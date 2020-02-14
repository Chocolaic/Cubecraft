using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTransform : MonoBehaviour
{
    bool timeLoop;
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        timeLoop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLoop)
        {
            gameObject.transform.Rotate(-speed * Time.deltaTime, 0, 0);
        }
    }
}
