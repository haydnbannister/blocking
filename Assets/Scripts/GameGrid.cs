using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using OSSC;

// The three dimensional grid that cubes are stored in
public class GameGrid : MonoBehaviour
{
    public SoundController soundController;
    public GameObject blasterEffectExplosionPrefab;
    private ShapeSpawner _shapeSpawner;
    public GameObject endGameUI;
    public TextMeshProUGUI scoreText;
    public Image pauseButtonImage;
    public Sprite[] pauseImages; //[playing,paused]
    public int score = 0;
    public Shape currentShape;

    public MobileController mobileController;

    public CameraRig cameraRig;

    public int numBlocksOnGrid = 0;


    private const int GameHeight = 13;

    // add 10 as safety margin, assumption that no block is more than 10 high
    public Block[,,] blocks = new Block[7, GameHeight + 10, 7];


    void Start()
    {
        Time.timeScale = 1;
        _shapeSpawner = GameObject.FindWithTag("ShapeSpawner").GetComponent<ShapeSpawner>();
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            TogglePause();
        }
    }

    public void HandleMobileControl(string direction)
    {
        if (currentShape != null)
        {
            if (direction == "q" || direction == "e")
            {
                cameraRig.HandleRotation(direction);
            }
            else
            {
                currentShape.handleMovement(direction);
            }
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            pauseButtonImage.sprite = pauseImages[0];
            Time.timeScale = 0;
        }
        else
        {
            pauseButtonImage.sprite = pauseImages[1];
            Time.timeScale = 1;
        }
    }

    public void PlaySound(string soundName)
    {
        PlaySoundSettings settings = new PlaySoundSettings();
        settings.Init();
        settings.name = soundName;
        soundController.Play(settings);
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
            numBlocksOnGrid++;

            ExplosionPowerup expPowerUp = block.GetComponentInParent<ExplosionPowerup>();
            if (expPowerUp != null)
            {
                string type = expPowerUp.type;
                switch (type)
                {
                    case "blaster":
                        VerticalBlasterEffect(x, y, z);
                        break;
                    case "3x3x3":
                        BombEffect(x, y, z, 1);
                        break;
                    case "5x5x5":
                        BombEffect(x, y, z, 2);
                        break;
                    default:
                        Console.WriteLine("Unhandled explosion powerup type: " + type);
                        break;
                }
                PlaySound("Boom");
                expPowerUp.Blast();
                return;
            }
        }

        CheckForFilledLayers();

        // check if the game is over
        foreach (var block in blocks)
        {
            if (block != null)
            {
                if ((int)Math.Round(block.transform.position.y, 0) > GameHeight - 1)
                {
                    HandleGameOver();
                }
            }
        }
    }

    private void VerticalBlasterEffect(int xb, int yb, int zb)
    {
        for (var y = 0; y <= GameHeight - 1; y++)
        {
            if (blocks[xb, y, zb] != null)
            {
                Destroy(blocks[xb, y, zb].gameObject);
                blocks[xb, y, zb] = null;
                numBlocksOnGrid--;
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
                    blocks[xb, y - 1, zb] = blocks[xb, y, zb];
                    blocks[xb, y, zb].transform.position = new Vector3(xb, y - 1, zb);
                    blocks[xb, y, zb] = null;
                    changes++;
                }
            }
        }
        // recurse to cascade through bigger gaps
        if (changes > 0) HammerEffect(xb, zb);
    }

    private void BombEffect(int xb, int yb, int zb, int size)
    {
        for (var y = yb - size; y <= yb + size; y++)
        {
            for (var x = xb - size; x <= xb + size; x++)
            {
                for (var z = zb - size; z <= zb + size; z++)
                {
                    if (x >= 0 && y >= 0 && z >= 0 && x <= 6 && y < GameHeight && z <= 6)
                    {
                        if (blocks[x, y, z] != null)
                        {
                            Destroy(blocks[x, y, z].gameObject);
                            blocks[x, y, z] = null;
                            numBlocksOnGrid--;
                        }
                    }
                }
            }
        }
    }

    public bool IsSpaceOccupied(Vector3 space)
    {
        var x = (int)space.x;
        var z = (int)space.z;

        var yUp = (int)Math.Ceiling(space.y);
        var yDown = (int)Math.Floor(space.y);

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
                numBlocksOnGrid = numBlocksOnGrid - 49;
                score++;
                scoreText.text = "SCORE:" + score.ToString();
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
        if (!_shapeSpawner.gameOver)
        {
            _shapeSpawner.EndGame();
            PlaySound("GameOver");
        }
        if (_shapeSpawner.powerupToggle != null)
        {
            if (!mobileController.isMobile() && !mobileController.powerupToggle.UiControls)
            {
                _shapeSpawner.powerupToggle.gameObject.SetActive(true);
            }
        }
        endGameUI.SetActive(false);
    }

    public void RestartScene()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}