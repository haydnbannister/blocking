using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// The three dimensional grid that cubes are stored in
public class GameGrid : MonoBehaviour
{
    public GameObject blasterEffectExplosionPrefab;
    private ShapeSpawner _shapeSpawner;
    public GameObject endGameUI;
    public GameObject newGameUI;
    public TextMeshProUGUI scoreText;
    public int score = 0;

    private const int GameHeight = 12;
    
    // add 10 as safety margin, assumption that no block is more than 10 high
    public Block[,,] blocks = new Block[7, GameHeight + 10, 7];
    

    void Start()
    {
        _shapeSpawner = GameObject.FindWithTag("ShapeSpawner").GetComponent<ShapeSpawner>();
    }

    // add a shape to the grid when it lands
    public void AddBlocks(List<Block> newBlocks)
    {
        foreach (var block in newBlocks)
        {
            var pos = block.transform.position;

            var x = (int) Math.Round(pos.x, 0);
            var z = (int) Math.Round(pos.z, 0);
            var y = (int) Math.Round(pos.y, 0);
            blocks[x, y, z] = block;

            if (block.GetComponentInParent<Blaster>() != null)
            {
                block.GetComponentInParent<Blaster>().Blast();
                BlasterEffect(x, y, z);
                return;
            }
            if (block.GetComponentInParent<Hammer>() != null)
            {
                HammerEffect(x, z);
                Destroy(block.gameObject);
                return;
            }
        }

        CheckForFilledLayers();

        // check if the game is over
        foreach (var block in blocks)
        {
            if (block != null)
            {
                if ((int) Math.Round(block.transform.position.y, 0) > GameHeight -1)
                {
                    HandleGameOver();
                }
            }
        }
    }

    private void BlasterEffect(int xb, int yb, int zb)
    {
        for (var y = 0; y <= GameHeight - 1; y++)
        {
            for (var x = 0; x <= 6; x++)
            {
                for (var z = 0; z <= 6; z++)
                {
                    if (blocks[x, y, z] != null)
                    {
                        var matchingAxis = 0;
                        if (xb == x) matchingAxis++;
                        if (yb == y) matchingAxis++;
                        if (zb == z) matchingAxis++;

                        if (matchingAxis > 1)
                        {
                            Instantiate(blasterEffectExplosionPrefab, new Vector3(x, y, z), Quaternion.identity);
                            Destroy(blocks[x, y, z].gameObject);
                            blocks[x, y, z] = null;
                        }
                    }
                }
            }
        }    
    }

    private void HammerEffect(int xb, int zb)
    {
        int changes = 0;
        for (var y = 1; y <= GameHeight - 1; y++)
        {
            if (blocks[xb, y, zb] != null)
            {
                if (blocks[xb, y - 1, zb] == null)
                {
                    blocks[xb, y -1, zb] = blocks[xb, y, zb];
                    blocks[xb, y, zb].transform.position = new Vector3(xb, y -1, zb);
                    blocks[xb, y, zb] = null;
                    changes++;
                }
            }
        } 
        // recurse to cascade through bigger gaps
        if (changes > 0) HammerEffect(xb, zb);
    }

    public bool IsSpaceOccupied(Vector3 space)
    {
        var x = (int) space.x;
        var z = (int) space.z;

        var yUp = (int) Math.Ceiling(space.y);
        var yDown = (int) Math.Floor(space.y);

        if (yUp > GameHeight - 1 || yDown > GameHeight - 1)
        {
            return false;
        }
        return blocks[x, yUp, z] != null || blocks[x, yDown, z] != null;
    }


    public void CheckForFilledLayers()
    {
        for (var y = 0; y <= GameHeight - 1; y++)
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
                y--;
                score++;
                scoreText.text = score.ToString();
            }
        }
    }

    private void ClearLayer(int layer)
    {
        for (var y = layer; y < GameHeight - 1; y++)
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
                        blocks[x, y, z].transform.Translate(Vector3.down * 1, Space.World
                        );
                    }

                    blocks[x, y, z] = blocks[x, y + 1, z];
                }
            }
        }
        print("Layer " + layer + "Has been filled in");
    }

    public void HandleGameOver() 
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
        _shapeSpawner.EndGame();

        endGameUI.SetActive(false);
        newGameUI.SetActive(true);
    }

    public void RestartScene() 
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}