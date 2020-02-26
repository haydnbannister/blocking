using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Shape : MonoBehaviour
{
    private GameGrid _gameGrid;

    public Block[] blocks;

    public bool inPlay = true;


    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = GameObject.FindWithTag("GameGrid").GetComponent<GameGrid>();
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

        // slowly move down over time
        transform.Translate(Vector3.down * Time.deltaTime);
    }

    private bool IsMovementAllowed(Vector3 movement)
    {

        // check that this movement won't cause a block to leave the game grid
        if (blocks.Select(block => block.transform.position + movement)
            .Any(newPosition => 
                newPosition.z > 6
                && newPosition.z < 0
                && newPosition.x < 0
                && newPosition.x > 6))
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
    }
}