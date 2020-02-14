using Chubecraft.Utilities;
using Cubecraft.Data.World;
using Cubecraft.Net.Protocol.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IPlayerInteraction
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private NetWorkManage netWorkManage;
    private GameObject player;
    private FirstPersonInteraction playerInteraction;
    public Queue<ChunkColumn> chunkQueue = new Queue<ChunkColumn>();
    private World world;
    // Start is called before the first frame update
    void Start()
    {
        this.world = gameObject.GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chunkQueue.Count > 0)
            world.CreateColumn(chunkQueue.Dequeue());
    }
    public void SetPlayerPosition(Vector3 pos)
    {
        if (this.player == null)
        {
            this.player = Instantiate(playerPrefab);
            this.player.name = Global.sessionToken.selectedProfile.name;
            (playerInteraction = this.player.GetComponent<FirstPersonInteraction>()).interact = this;
        }
        playerInteraction.Move(pos);
    }
    public void UpdatePosition(bool onGround, float x, float y, float z)
    {
        netWorkManage.SendPacket(new ClientPlayerPositionPacket(onGround, x, y, z));
    }
    public void UnloadDimension()
    {
        foreach(var column in world.chunks.Values)
        {
            Destroy(column);
        }
        world.chunks.Clear();
    }
    public void BreakSelectBlock(Chunk chunk, Vector3 pos)
    {
        int blockX = Utils.Round(pos.x) - chunk.column.posX * 16,
            blockY = Utils.Round(pos.y) - chunk.position * 16,
            blockZ = Utils.Round(pos.z) - chunk.column.posZ * 16;
        Debug.Log("选中："+ blockX + " " + blockY + " " + blockZ);
        //判断是否临界方块，否则更新临近区块
        if(chunk.GetBlock(blockX, blockY, blockZ).BlockID != 0)
        {
            chunk.SetBlock(blockX, blockY, blockZ, BlockData.AIR);
            if (blockX > 14 && chunk.RightChunk != null)
                chunk.RightChunk.UpdateByBlockChange();
            else if (blockX < 1 && chunk.LeftChunk != null)
                chunk.LeftChunk.UpdateByBlockChange();
            if (blockZ > 14 && chunk.BackChunk != null)
                chunk.BackChunk.UpdateByBlockChange();
            else if (blockZ < 1 && chunk.FrontChunk != null)
                chunk.FrontChunk.UpdateByBlockChange();
            if (blockY > 14 && chunk.UpChunk != null)
                chunk.UpChunk.UpdateByBlockChange();
            else if (blockY < 1 && chunk.DownChunk != null)
                chunk.DownChunk.UpdateByBlockChange();
            chunk.UpdateByBlockChange();
        }
    }
    public World GetWorld()
    {
        return this.world;
    }
}
