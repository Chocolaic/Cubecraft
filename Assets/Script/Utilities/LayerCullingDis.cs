using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCullingDis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        distances[LayerMask.NameToLayer("Chunk")] = 32;
        camera.layerCullDistances = distances;
        camera.layerCullSpherical = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
