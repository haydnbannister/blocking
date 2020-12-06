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
    private CameraRig cameraRig;

    void Start()
    {
        _gameGrid = GameObject.FindWithTag("GameGrid").GetComponent<GameGrid>();
        _shapeSpawner = GameObject.FindWithTag("ShapeSpawner").GetComponent<ShapeSpawner>();
        cameraRig = GameObject.Find("CameraRig").GetComponent<CameraRig>();

        foreach (Transform child in transform)
        {
            var block = child.GetComponent<Block>();
            if (block != null) blocks.Add(child.GetComponent<Block>());
        }
    }

    void Update()
    {
        handleMovement("");
    }

    public void handleMovement(string direction)
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

        if (Time.timeScale == 0) return;


        if ((Input.GetKeyDown("up") || direction == "up") && IsMovementAllowed(cameraRig.GetDirection("up")))
        {
            transform.Translate(cameraRig.GetDirection("up"));
            _gameGrid.PlaySound("Click");
        }

        if ((Input.GetKeyDown("down") || direction == "down") && IsMovementAllowed(cameraRig.GetDirection("down")))
        {
            transform.Translate(cameraRig.GetDirection("down"));
            _gameGrid.PlaySound("Click");
        }

        if ((Input.GetKeyDown("left") || direction == "left") && IsMovementAllowed(cameraRig.GetDirection("left")))
        {
            transform.Translate(cameraRig.GetDirection("left"));
            _gameGrid.PlaySound("Click");
        }

        if ((Input.GetKeyDown("right") || direction == "right") && IsMovementAllowed(cameraRig.GetDirection("right")))
        {
            transform.Translate(cameraRig.GetDirection("right"));
            _gameGrid.PlaySound("Click");
        }


        if ((Input.GetKeyDown("w") || direction == "w"))
        {
            Rotate(cameraRig.GetRotation("w"));
        }
        if ((Input.GetKeyDown("s") || direction == "s"))
        {
            Rotate(cameraRig.GetRotation("s"));
        }
        if ((Input.GetKeyDown("a") || direction == "a"))
        {
            Rotate(cameraRig.GetRotation("a"));
        }
        if ((Input.GetKeyDown("d") || direction == "d"))
        {
            Rotate(cameraRig.GetRotation("d"));
        }


        // speed downwards
        if (Input.GetKey("space") || direction == "space")
        {
            transform.Translate(Vector3.down * (Time.deltaTime * 5f), Space.World);
        }

        // slowly move down over time
        transform.Translate(Vector3.down * (Time.deltaTime * 1f), Space.World);
    }

    private void Rotate(Vector3 axis)
    {
        if (GetComponent<ExplosionPowerup>() || gameObject.name == "Shape2x2x2(Clone)")
        {
            return;
        }

        float angle = 90f;

        // rotate the blocks as per the input
        foreach (var block in blocks)
        {
            block.transform.RotateAround(transform.position, axis, angle);
        }

        if (IsPositionAllowed())
        {
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
                (int)Math.Round(newPosition.z, 0) > 6
                || (int)Math.Round(newPosition.z, 0) < 0
                || (int)Math.Round(newPosition.x, 0) < 0
                || (int)Math.Round(newPosition.x, 0) > 6
                || (int)Math.Round(newPosition.y, 0) < 0))
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
        _gameGrid.PlaySound("Land");

        // set to nearest whole coordinate. Prevents object going part way through another in between frames and before collison detection
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3((int)Math.Round(pos.x, 0), (int)Math.Round(pos.y, 0), (int)Math.Round(pos.z, 0));

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