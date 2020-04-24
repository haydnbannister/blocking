﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    public void AddBlocks(List<Block> newBlocks)
    {
        foreach (var block in newBlocks)
        {
            var pos = block.transform.position;

            var x = (int)Math.Round(pos.x, 0);
            var z = (int)Math.Round(pos.z, 0);
            var y = (int)Math.Round(pos.y, 0);

            blocks[x, y, z] = block;
        }
        CheckForFilledLayers();
    }

    public bool IsSpaceOccupied(Vector3 space)
    {
        var x = (int) space.x;
        var z = (int) space.z;

        var yUp = (int) Math.Ceiling(space.y);
        var yDown = (int) Math.Floor(space.y);
        
        return blocks[x, yUp, z] != null || blocks[x, yDown, z] != null;
    }


    public void CheckForFilledLayers()
    {
        for (var y = 0; y <= 14; y++)
        {

            var isLayerFull = true;
            for (var x = 0; x <= 6; x++)
            {
                for (var z = 0; z <= 6; z++)
                {
                    if (blocks[x, y, z] == null)
                    {
                        isLayerFull = false;
                    }
                }            
            }
            if (isLayerFull)
            {
                ClearLayer(y);
            }
        }
    }

    private void ClearLayer(int layer)
    {
        for (var y = layer; y <= 13; y++)
        {
            for (var x = 0; x <= 6; x++)
            {
                for (var z = 0; z <= 6; z++)
                {
                    if (y == layer)
                    {
                        Destroy(blocks[x, y, z].gameObject);
                    }
                    if (blocks[x, y, z] != null)
                    {
                        blocks[x,y,z].transform.Translate(Vector3.down * 1, Space.World
                        );
                    }
                    blocks[x, y, z] = blocks[x, y+1, z];
                }            
            }
        }
        print("Layer " + layer + "Has been filled in");
    }
    
}
