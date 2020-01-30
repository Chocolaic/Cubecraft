using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AirBlock : Block
{
    public override int BlockID { get { return 0; } }

    public override bool Transparent { get { return true; } }
    public override void SetMeshData(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        
    }
}
