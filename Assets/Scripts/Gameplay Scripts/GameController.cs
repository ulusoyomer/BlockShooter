using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private ShootCountText shootCountText;
    public Text ballsCountText;

    public List<GameObject> levels;

    public GameObject[] block;
    private GameObject level1;
    private GameObject level2;


    private Vector2 level1Pos;
    private Vector2 level2Pos;

    public int shotCount;
    public int ballsCount;
    public int score;

    private GameObject ballsContainer;

    public GameObject gameOver;

    private bool firsShot = true;

    private void Awake()
    {
        shootCountText = GameObject.Find("ShotCountText").GetComponent<ShootCountText>();
        ballsCountText = GameObject.Find("BallCountText").GetComponent<Text>();
        ballsContainer = GameObject.Find("BallsContainer");
    }

    void Start()
    {
        ballsCount = PlayerPrefs.GetInt("BallsCount",5);
        ballsCountText.text = ballsCount.ToString();
        Physics2D.gravity = new Vector2(0, -17);
        SpawnLevel();
        GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", true);
    }


    void Update()
    {
        if (ballsContainer.transform.childCount == 0 && shotCount == 4)
        {
            gameOver.SetActive(true);
            GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn",false);
        }
        if (shotCount > 2)
        {
            firsShot = false;
        }
        else
            firsShot = true;
        CheckBlocks();
    }

    void SpawnNewLevel(int numberLevel1, int numberLevel2, int min, int max)
    {
        if (shotCount > 1)
        {
            Camera.main.GetComponent<CameraTransitions>().RotateCameraToFront();
        }
        shotCount = 1;

        level1Pos = new Vector2(5.5f, 1);
        level2Pos = new Vector2(5.5f, -3.4f);

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

            if (ballsCount >= PlayerPrefs.GetInt("BallsCount",5))
            {
                PlayerPrefs.SetInt("BallsCount",ballsCount);
            }

            if (firsShot)
            {
                score += 5;
            }
            else
                score += 2;
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
                shootCountText.TopText = "Final Shot";
                shootCountText.BottomText = "3/3";
                shootCountText.Flash();
                break;
            default:
                break;
        }
    }

}
