using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public float power = 2;
    private int dots = 15;

    private Vector2 startPos;

    private bool shoot, aiming;

    private GameObject Dots;
    private List<GameObject> projectilePath;

    private Rigidbody2D ballBody;

    public GameObject ballPrefab;
    public GameObject ballsContainer;

    void Start()
    {
        Dots = GameObject.Find("Dots");
        projectilePath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        HideDots();
    }


    void Update()
    {
        ballBody = ballPrefab.GetComponent<Rigidbody2D>();
        Aim();
        Rotate();
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
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(ballsContainer.transform);
            ballBody = ball.GetComponent<Rigidbody2D>();
            ballBody.AddForce(ShootForce(Input.mousePosition));
        }
        
    }
}
