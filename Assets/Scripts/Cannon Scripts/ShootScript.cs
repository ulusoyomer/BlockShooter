using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootScript : MonoBehaviour
{
    private GameController gc;

    public float power = 2;

    private Vector2 startPos;

    private bool shoot,aiming;

    private GameObject Dots;
    private List<GameObject> projectilePath;

    private Rigidbody2D ballBody;

    public GameObject ballPrefab;
    public GameObject ballsContainer;

    private void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        Dots = GameObject.Find("Dots");
    }

    void Start()
    {
        projectilePath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        HideDots();
    }


    void Update()
    {
        ballBody = ballPrefab.GetComponent<Rigidbody2D>();
        if (gc.shotCount <=3 && !isMouseOverUI())
        {
            Aim();
            Rotate();
        }
        
    }

    void Aim()
    {
        if (shoot)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (!aiming)
            {
                aiming = true;
                startPos = Input.mousePosition;
                gc.ChechShotCount();
            }
            else
            {
                PathCalculation();
            }
        }
        else if (aiming && !shoot)
        {
            aiming = false;
            HideDots();
            StartCoroutine(Shoot());
            if (gc.shotCount == 1)
            {
                Camera.main.GetComponent<CameraTransitions>().RotateCameraToSide();
            }
        }
    }

    Vector2 ShootForce(Vector3 force)
    {
        return (new Vector2(startPos.x, startPos.y) - new Vector2(force.x,force.y)) * power;
    }

    Vector2 DothPath(Vector2 startP, Vector2 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    void PathCalculation()
    {
        Vector2 vel = ShootForce(Input.mousePosition) * Time.fixedDeltaTime / ballBody.mass;
        for (int i = 0; i < projectilePath.Count; i++)
        {
            projectilePath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 15f;
            Vector3 point = DothPath(transform.position, vel, t);
            point.z = 1;
            projectilePath[i].transform.position = point;
        }
    }

    void ShowDots()
    {
        for (int i = 0; i < projectilePath.Count; i++)
        {
            projectilePath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    bool isMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void HideDots()
    {
        for (int i = 0; i < projectilePath.Count; i++)
        {
            projectilePath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    void Rotate()
    {
        var dir = GameObject.Find("dot (1)").transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < gc.ballsCount; i++)
        {
            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(ballsContainer.transform);
            ballBody = ball.GetComponent<Rigidbody2D>();
            ballBody.AddForce(ShootForce(Input.mousePosition));

            var balls = gc.ballsCount - i;
            gc.ballsCountText.text = (gc.ballsCount - i - 1).ToString();
        }
        yield return new WaitForSeconds(0.5f);
        gc.shotCount++;
        gc.ballsCountText.text = gc.ballsCount.ToString();
    }
}
