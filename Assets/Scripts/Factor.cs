using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factor : MonoBehaviour
{
    [SerializeField] private bool destroyable;
    [SerializeField] private float angle;
    [SerializeField] private int scoreGain;
    private Vector3 scaleTouch;
    private Vector3 defaultScale;
    private bool touched;
    private GameObject ball;
    private const float relativeForceEx = 15f;
    private Rigidbody rb;
    private const float timeToDestroy = 1.5f;
    private Ball ballClass;
    private const int RennderRange = 8;
    private Renderer _renderer;
    private bool firestTouch;
    public bool canExplore;
    private void Awake()
    {
        defaultScale = transform.parent.localScale;
        scaleTouch = new Vector3(1.5f, 0.8f, 1.5f);
        if (destroyable)
        {
            canExplore = true;
        }
        else
        {
            canExplore = false;
        }
        rb = GetComponent<Rigidbody>();
        DisableRigid();
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballClass = ball.GetComponent<Ball>();
        _renderer = GetComponentInChildren<Renderer>();
        
    }
    void Start()
    {
        _renderer.enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (GetRennderCondition())
        {
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }

       
        CheckOnColider();
            
        
    }

    private void CheckOnColider() {
        if (touched) return;
        if(CheckTouch())
        {
            
            if (destroyable||ballClass.IsFuzzying())
            {
                touched = true;
                Explode();
                ballClass.AddScore(scoreGain);
                return;
            }else
            {
                touched = true;
                Gameover();
                ballClass.SetGameOverTrigger();
            }
            ballClass.AddEnergy();
            
        }
        if (CheckUnder())
        {
            StartCoroutine(ScaleTouchAnim());
        }

        if (CheckCanExplore())
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
    }
    private void EnableRigidbody()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
    private void DisableRigid()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private bool CheckTouch()
    {
        float eulerY = transform.eulerAngles.y;
        float anpha = eulerY - angle / 2;
        float beta = eulerY + angle / 2;
        return (CheckCanExplore())
               &&
               (((anpha < 0)||(eulerY >= 360 - angle/2))&& beta > 0);
    }

    private bool CheckUnder()
    {
        return ball.transform.position.y < transform.position.y ;
    }

    private bool CheckCanExplore()
    {
        return ball.transform.position.y < transform.position.y - 0.5;
    }
    private bool GetRennderCondition()
    {
        var position = transform.position.y;
        var ballPosition = ball.transform.position.y;
        if (Mathf.Abs(position - ballPosition) < RennderRange )
        {
            return true;
        }

        return false;
    }

    public bool IsDestroyable()
    {
        return destroyable;
    }

    private IEnumerator ScaleTouchAnim()
    {
        bool endAnim = false;
        bool maxSized = false;
        var parentTranform = transform.parent;
        while (!endAnim)
        {
            if (Mathf.Abs(scaleTouch.magnitude - transform.parent.localScale.magnitude) <= Mathf.Epsilon)
            {
                maxSized = true;
            }
            if (!maxSized)
            {
                transform.parent.localScale = Vector3.Lerp(defaultScale,scaleTouch,0.01f);
            }
            else
            {
                if (Mathf.Abs(defaultScale.magnitude - transform.parent.localScale.magnitude) <= Mathf.Epsilon)
                {
                    endAnim = true;
                }
                transform.parent.localScale = Vector3.Lerp(scaleTouch,defaultScale,0.01f );
            }
           
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
