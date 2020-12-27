using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private ShootCountText shootCountText;

    public List<GameObject> levels;

    public GameObject[] block;
    private GameObject level1;
    private GameObject level2;


    private Vector2 level1Pos;
    private Vector2 level2Pos;

    public int shotCount;

    private void Awake()
    {
        shootCountText = GameObject.Find("ShotCountText").GetComponent<ShootCountText>();
    }

    void Start()
    {
        PlayerPrefs.DeleteKey("Level");
        Physics2D.gravity = new Vector2(0, -17);
        SpawnLevel();
    }


    void Update()
    {
        CheckBlocks();
    }

    void SpawnNewLevel(int numberLevel1, int numberLevel2, int min, int max)
    {
        if (shotCount > 1)
        {
            Camera.main.GetComponent<CameraTransitions>().RotateCameraToFront();
        }
        shotCount = 1;

        level1Pos = new Vector2(3.5f, 1);
        level2Pos = new Vector2(3.5f, -3.4f);

        level1 = levels[numberLevel1];
        level2 = levels[numberLevel2];

        Instantiate(level1, level1Pos, Quaternion.identity);
        Instantiate(level2, level2Pos, Quaternion.identity);

        SetBlocksCount(min, max);
    }

    void SetBlocksCount(int min, int max)
    {
        block = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < block.Length; i++)
        {
            int count = Random.Range(min, max);
            block[i].GetComponent<Block>().SetStartingCount(count);
        }
    }

    void SpawnLevel()
    {
        switch (PlayerPrefs.GetInt("Level", 0))
        {
            case 0:
                SpawnNewLevel(0, 17, 3, 5);
                break;
            case 1:
                SpawnNewLevel(1, 18, 3, 5);
                break;
            case 2:
                SpawnNewLevel(2, 19, 3, 6);
                break;
            case 3:
                SpawnNewLevel(5, 20, 4, 7);
                break;
            case 4:
                SpawnNewLevel(12, 28, 5, 8);
                break;
            case 5:
                SpawnNewLevel(14, 29, 7, 10);
                break;
            case 6:
                SpawnNewLevel(15, 30, 6, 12);
                break;
            case 7:
                SpawnNewLevel(16, 31, 9, 15);
                break;
            default:
                break;
        }

    }

    void CheckBlocks()
    {
        block = GameObject.FindGameObjectsWithTag("Block");
        if (block.Length < 1)
        {
            RemoveBalls();
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
            SpawnLevel();
        }
    }

    void RemoveBalls()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (var item in balls)
        {
            Destroy(item);
        }
    }

    public void ChechShotCount()
    {
        switch (shotCount)
        {
            case 1:
                shootCountText.TopText = "Shot";
                shootCountText.BottomText = "1/3";
                shootCountText.Flash();
                break;
            case 2:
                shootCountText.TopText = "Shot";
                shootCountText.BottomText = "2/3";
                shootCountText.Flash();
                break;
            case 3:
                shootCountText.TopText = "Shot";
                shootCountText.BottomText = "3/3";
                shootCountText.Flash();
                break;
            default:
                break;
        }
    }

}
