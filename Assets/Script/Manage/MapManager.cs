using Cubecraft.Data.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
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
        }
        this.player.transform.position = pos;
    }
}
