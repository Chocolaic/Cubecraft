using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IBlock
{
    public int BlockID { get; set; }
    public Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }


}
