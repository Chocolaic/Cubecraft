using Cubecraft.Data.World;
using Cubecraft.Net.Protocol.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IPlayerAction
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private NetWorkManage netWorkManage;
    private GameObject player;
    public Queue<ChunkColumn> chunkQueue = new Queue<ChunkColumn>();
    World world;
    private bool playerSpawned;
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
            this.player.GetComponent<FirstPersonController>().action = this;
        }
        this.player.transform.position = pos;
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
    public World GetWorld()
    {
        return this.world;
    }
}
