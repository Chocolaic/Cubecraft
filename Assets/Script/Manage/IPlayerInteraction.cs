using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface IPlayerInteraction
{
    void UpdatePosition(bool onGround, float x, float y, float z);
    void BreakSelectBlock(Chunk chunk, Vector3 pos);
}
