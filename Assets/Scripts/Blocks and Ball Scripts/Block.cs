using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    private int count;
    public Text countText;

    private void Awake()
    {
        countText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    public void SetStartingCount(int count)
    {
        this.count = count;
        countText.text = count.ToString();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        switch (target.collider.name)
        {
            case "Ball":
                if (count > 0)
                {
                    count--;
                    Camera.main.GetComponent<CameraTransitions>().Shake();
                    countText.text = count.ToString();
                    if (count == 0)
                    {
                        Destroy(gameObject);
                        Camera.main.GetComponent<CameraTransitions>().MediumShake();
                    }
                }
                break;
            default:
                break;
        }
    }
}
