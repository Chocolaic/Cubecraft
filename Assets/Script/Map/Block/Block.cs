using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    public BlockType type;
    public float hardness;
    public float blocklight;
    public int maxheap;

    public Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }

    public virtual void OnBlockDestroyed()
    {

    }
    public virtual void OnBlockInteracted()
    {

    }
}
