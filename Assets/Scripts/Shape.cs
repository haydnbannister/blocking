using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Shape : MonoBehaviour
{
    private GameGrid _gameGrid;

    public List<Block> blocks;

    public bool inPlay = true;


    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = GameObject.FindWithTag("GameGrid").GetComponent<GameGrid>();
        
        foreach (Transform child in transform)
        {
            blocks.Add(child.GetComponent<Block>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPlay) return;

        if (Input.GetKeyDown("up") && IsMovementAllowed(new Vector3(0f, 0f, 1f)))
        {
            transform.Translate(Vector3.forward * 1);
        }

        if (Input.GetKeyDown("down") && IsMovementAllowed(new Vector3(0f, 0f, -1f)))
        {
            transform.Translate(Vector3.back * 1);
        }

        if (Input.GetKeyDown("left") && IsMovementAllowed(new Vector3(-1f, 0f, 0f)))
        {
            transform.Translate(Vector3.left * 1);
        }

        if (Input.GetKeyDown("right") && IsMovementAllowed(new Vector3(1f, 0f, 0f)))
        {
            transform.Translate(Vector3.right * 1);
        }
        
        
        
        
        if (Input.GetKeyDown("w"))
        {
            foreach (var block in blocks)
            {
                block.transform.RotateAround(transform.position, new Vector3(1, 0, 0), 90f);
            }
        }

        if (Input.GetKeyDown("a"))
        {
            foreach (var block in blocks)
            {
                block.transform.RotateAround(transform.position, new Vector3(0, 1, 0), -90f);
            }        }

        if (Input.GetKeyDown("d"))
        {
            foreach (var block in blocks)
            {
                block.transform.RotateAround(transform.position, new Vector3(0, 1, 0), 90f);
            }        }

        if (Input.GetKeyDown("s"))
        {
            foreach (var block in blocks)
            {
                block.transform.RotateAround(transform.position, new Vector3(1, 0, 0), -90f);
            }        }
        
        if (Input.GetKeyDown("space"))
        {
            DebugBlocks();
        }

        // slowly move down over time
        transform.Translate(Vector3.down * Time.deltaTime, Space.World);
    }

    private bool IsMovementAllowed(Vector3 movement)
    {

        foreach (var block in blocks)
        {
            var newPosition = block.transform.position + movement;
            if (
                (int) newPosition.z > 6
                || (int) newPosition.z < 0
                || (int) newPosition.x < 0
                || (int) newPosition.x > 6
            )
            {
                return false;
            }
        }
        
        
         // check that this movement won't cause a block to leave the game grid
         if (blocks.Select(block => block.transform.position + movement)
             .Any(newPosition => 
                 (int)Math.Round(newPosition.z, 0) > 6
                 || (int)Math.Round(newPosition.z, 0) < 0
                 || (int)Math.Round(newPosition.x, 0) < 0
                 || (int)Math.Round(newPosition.x, 0) > 6))
         {
             return false;
         }

        // check that this movement won't cause a block hit another horizontally
        return blocks.All(block => !_gameGrid.IsSpaceOccupied(block.transform.position + movement));
    }


    public void Land()
    {
        if (!inPlay)
            return;

        inPlay = false;

        _gameGrid.AddBlocks(blocks);

        GameObject.FindWithTag("ShapeSpawner").GetComponent<ShapeSpawner>().SpawnShape();
    }


    private void DebugBlocks()
    {
        foreach (var block in blocks)
        {
            print(block.name + " Thinks it's at " + block.transform.position);
        }
    }
}