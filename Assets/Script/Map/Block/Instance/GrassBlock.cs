using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : Block
{
    public override int BlockID { get { return 1; } }
    public override bool Transparent { get { return false; } }
}
