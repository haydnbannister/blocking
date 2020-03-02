using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The three dimensional grid that cubes are stored in
public class GameGrid : MonoBehaviour
{
    public Block[,,] blocks = new Block[7, 15, 7];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // add a shape to the grid when it lands
    public void AddBlocks(Block[] newBlocks)
    {
        foreach (var block in newBlocks)
        {
            var pos = block.transform.position;
            var x = (int) pos.x;
            var z = (int) pos.z;
            var y = (int) pos.y;
            
            if (blocks[x, y, z] != null) // seems to be some blocks that have undergone rotation have coordinate issues
            {
                print("Adding " + block.name + "to " + x + y + z + " but this space already contains " + blocks[x, y, z].name)
            }
            
            blocks[x, y, z] = block;
        }
    }

    public bool IsSpaceOccupied(Vector3 space)
    {
        var x = (int) space.x;
        var z = (int) space.z;

        // lower y by two, one due to 0-indexed array and one due to being part way through the next block down
        var yUp = (int) Math.Ceiling(space.y - 1f);
        var yDown = (int) Math.Floor(space.y - 1f);

        return blocks[x, yUp, z] != null || blocks[x, yDown, z] != null;
    }
}
