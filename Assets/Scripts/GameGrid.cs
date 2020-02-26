using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The three dimensional grid that cubes are stored in
public class GameGrid : MonoBehaviour
{
	public Block[,,] blocks = new Block[7, 7, 15];
	
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
		    blocks[x, z, y] = block;
	    }
    }
    
}
