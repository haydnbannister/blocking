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
    private ShapeSpawner _shapeSpawner;


    void Start()
    {
        _gameGrid = GameObject.FindWithTag("GameGrid").GetComponent<GameGrid>();
        _shapeSpawner = GameObject.FindWithTag("ShapeSpawner").GetComponent<ShapeSpawner>();


        foreach (Transform child in transform)
        {
            var block = child.GetComponent<Block>();
            if (block != null) blocks.Add(child.GetComponent<Block>());
        }
    }

    void Update()
    {
        if (!inPlay) return;

        // if game has ended since this was created
        if (_shapeSpawner.gameOver) 
        {
            foreach (var block in blocks)
            {
                if (block != null)
                {
                    block.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    block.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    block.gameObject.GetComponent<Rigidbody>().useGravity = true;
                }
            }
            inPlay = false;
            return;
        }

        if (Input.GetKeyDown("up") && IsMovementAllowed(new Vector3(0f, 0f, 1f)))
        {
            transform.Translate(Vector3.forward * 1);
            _gameGrid.PlaySound("Click");
        }

        if (Input.GetKeyDown("down") && IsMovementAllowed(new Vector3(0f, 0f, -1f)))
        {
            transform.Translate(Vector3.back * 1);
            _gameGrid.PlaySound("Click");
        }

        if (Input.GetKeyDown("left") && IsMovementAllowed(new Vector3(-1f, 0f, 0f)))
        {
            transform.Translate(Vector3.left * 1);
            _gameGrid.PlaySound("Click");
        }

        if (Input.GetKeyDown("right") && IsMovementAllowed(new Vector3(1f, 0f, 0f)))
        {
            transform.Translate(Vector3.right * 1);
            _gameGrid.PlaySound("Click");
        }


        if (Input.GetKeyDown("w"))
        {
            Rotate(new Vector3(1, 0, 0), 90f);
        }

        if (Input.GetKeyDown("a"))
        {
            Rotate(new Vector3(0, 1, 0), -90f);
        }

        if (Input.GetKeyDown("d"))
        {
            Rotate(new Vector3(0, 1, 0), 90f);
        }

        if (Input.GetKeyDown("s"))
        {
            Rotate(new Vector3(1, 0, 0), -90f);
        }

        // speed downwards
        if (Input.GetKey("space"))
        {
            transform.Translate(Vector3.down * (Time.deltaTime * 5f), Space.World);
        }

        // slowly move down over time
        transform.Translate(Vector3.down * (Time.deltaTime * 1f), Space.World);
    }

    private void Rotate(Vector3 axis, float angle)
    {
        if (GetComponent<ExplosionPowerup>() || gameObject.name == "Shape2x2x2(Clone)") {
            return;
        }

        // rotate the blocks as per the input
        foreach (var block in blocks)
        {
            block.transform.RotateAround(transform.position, axis, angle);
        }

        if (IsPositionAllowed()) {
            _gameGrid.PlaySound("Click");
            return;
        };
        
        _gameGrid.PlaySound("Invalid");
        // if the new position is not allowed, revert the rotation
        foreach (var block in blocks)
        {
            block.transform.RotateAround(transform.position, axis, angle * -1);
        }
        
    }

    private bool IsPositionAllowed()
    {
        // check that this movement won't cause a block to leave the game grid
        if (blocks.Select(block => block.transform.position)
            .Any(newPosition =>
                (int) Math.Round(newPosition.z, 0) > 6
                || (int) Math.Round(newPosition.z, 0) < 0
                || (int) Math.Round(newPosition.x, 0) < 0
                || (int) Math.Round(newPosition.x, 0) > 6
                || (int) Math.Round(newPosition.y, 0) < 0))
        {
            return false;
        }

        return blocks.All(block => !_gameGrid.IsSpaceOccupied(block.transform.position));
    }

    private bool IsMovementAllowed(Vector3 movement)
    {
        // check that this movement won't cause a block to leave the game grid
        if (blocks.Select(block => block.transform.position + movement)
            .Any(newPosition =>
                (int) Math.Round(newPosition.z, 0) > 6
                || (int) Math.Round(newPosition.z, 0) < 0
                || (int) Math.Round(newPosition.x, 0) < 0
                || (int) Math.Round(newPosition.x, 0) > 6))
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
        _gameGrid.PlaySound("Land");
        
        // set to nearest whole coordinate. Prevents object going part way through another in between frames and before collison detection
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3((int) Math.Round(pos.x, 0), (int) Math.Round(pos.y, 0), (int) Math.Round(pos.z, 0));

        _shapeSpawner.SpawnShape();
    }


    private void DebugBlocks()
    {
        foreach (var block in blocks)
        {
            print(block.name + " Thinks it's at " + block.transform.position);
        }
    }
}