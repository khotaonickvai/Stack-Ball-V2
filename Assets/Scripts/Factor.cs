using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factor : MonoBehaviour
{
    [SerializeField] private bool destroyable;
    [SerializeField] private float angle;
    [SerializeField] private int scoreGain;
    private bool touched;
    private GameObject ball;
    private const float relativeForceEx = 15f;
    private Rigidbody rb;
    private const float timeToDestroy = 1.5f;
    private Ball ballClass;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        DisableRigid();
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballClass = ball.GetComponent<Ball>();
        
    }
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {

        CheckOnColider();
            
        
    }

    private void CheckOnColider() {
        if (touched) return;
        if(CheckTouch())
        {
            touched = true;
            if (destroyable||ballClass.IsFuzzying())
            {
                Explode();
                ballClass.AddScore(scoreGain);
                return;
            }else
            {
                Gameover();
                ballClass.SetGameOverTrigger();
               // ballClass.SetGameOver();
                
            }
            ballClass.AddEnergy();
            
        }
        if (CheckUnder())
        {
            touched = true;
            Explode();
            if (ballClass.IsFuzzying())
            {
                ballClass.AddScore(scoreGain);
                return;
            }
            
            ballClass.AddEnergy();
            
        }
    }

    private void Gameover()
    {
        Debug.Log("Game OVer");
    }
    
    private void Explode()
    {
        EnableRigidbody();
        rb.velocity = Vector3.up * relativeForceEx;
        rb.AddRelativeForce(new Vector3(100,100,100));
        Destroy(gameObject, timeToDestroy);
        //transform.position = Vector3.up * 5;
    }
    private void EnableRigidbody()
    {
        //rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
    private void DisableRigid()
    {
       // rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private bool CheckTouch()
    {
        // Debug.Log(transform.rotation.eulerAngles.y); 
        float eulerY = transform.eulerAngles.y;
        //if(eulerY >= 360 - angle/2)
        /*if (eulerY > 180)
        {
            eulerY -= 180;
        }*/
        float anpha = eulerY - angle / 2;
        float beta = eulerY + angle / 2;
        // Debug.Log(anpha);
       // Debug.Log("stacks :" + stacks.transform.rotation.eulerAngles.y);
        //return ball.transform.position.y < transform.position.y;
        return (CheckUnder())
                &&
                (((anpha < 0)||(eulerY >= 360 - angle/2))&& beta > 0);
    }

    private bool CheckUnder()
    {
        return ball.transform.position.y < transform.position.y;
    }
}
