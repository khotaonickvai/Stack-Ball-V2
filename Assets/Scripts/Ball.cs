using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField] private int maxFuzzy;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color fuzzyColor;
    private bool isFuzzying;
    private Slider fuzzySlider;
    private TextMeshProUGUI scoreText;
    private float currentEnergy;
    public const float speed = 10f;
    private GameObject Stacks;
    private Material mat;
    [SerializeField] float energyPenaty = 10f;
    private int score;
    private ParticleSystem[] fireVFX;
    private Canvas GameoverCanvas;
    private Canvas StartGameCanvas;
    private Canvas GameplayCanvas;
    private Canvas EndLevelCanvas;
    private bool gameOverTrigger;
    private float _delay = 0;
    private enum GameState{START,INGAME,GAMEOVER,COMPLETE}

    private GameState _gameState;
    private void Awake()
    {
        Debug.Log("Class Ball Awalke");
    }
    
    void Start()
    {
       InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        TouchHandler();
        FuzzyHandle();
        
        
    }
    private void InitGame()
    {       

        Stacks = GameObject.FindGameObjectWithTag("Stacks");
        int yStart = Stacks.GetComponent<Stacks>().GetListCount() - 1;
        transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
        fuzzySlider = GameObject.FindGameObjectWithTag("FuzzySlider").GetComponent<Slider>();
        fuzzySlider = GameObject.FindGameObjectWithTag("FuzzySlider").GetComponent<Slider>();
        isFuzzying = false;
        currentEnergy = 0;
        fuzzySlider.minValue = 0;
        fuzzySlider.maxValue = maxFuzzy;
        mat = GetComponentInChildren<Renderer>().material;
        fireVFX = GetComponentsInChildren<ParticleSystem>();
        score = 0;
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameoverCanvas = GameObject.FindGameObjectWithTag("GameoverCanvas").GetComponent<Canvas>();
        GameplayCanvas = GameObject.FindWithTag("GameplayCanvas").GetComponent<Canvas>();
        StartGameCanvas = GameObject.FindWithTag("StartGameCanvas").GetComponent<Canvas>();
        EndLevelCanvas = GameObject.FindWithTag("EndLevelCanvas").GetComponent<Canvas>();
        AddScore(0);
        GameoverCanvas.enabled = false;
        GameplayCanvas.enabled = false;
        StartGameCanvas.enabled = true;
        gameOverTrigger = false;
        //  isIngame = false;
    }
    
    private void FuzzyHandle()
    {
        fuzzySlider.value = Mathf.Lerp(fuzzySlider.value, currentEnergy, 0.2f);
        if (fuzzySlider.value >= maxFuzzy)
        {
            isFuzzying = true;
        }
        if (isFuzzying && currentEnergy <= Mathf.Epsilon)
        {
            isFuzzying = false;
        }

        if (isFuzzying)
        {
            PlayFireFX();
           
            if (mat.color != fuzzyColor)
            {
                mat.color = fuzzyColor;
            }
            currentEnergy -= energyPenaty * Time.deltaTime;
        }
        else
        {
           StopFireFX();
          //  fireVFX.Stop(true);
            if (mat.color != normalColor)
            {
                mat.color = normalColor;
            }
        }
    
    }
    public void AddEnergy()
    {
        currentEnergy++;
    }
    public bool IsFuzzying()
    {
        return isFuzzying;
    }

    private void PlayFireFX()
    {
        foreach (var fire in fireVFX)
        {
            if (!fire.isPlaying)
            {
                fire.Play(true);
            }
        }
    }

    private void StopFireFX()
    {
        foreach (var fire in fireVFX)
        {
            if (fire.isPlaying)
            {
                fire.Stop(true);
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "SCORE: " + score;
    }
    private bool IsTouch()
    {
        if (_delay > Mathf.Epsilon) return false;
        if (Input.touchCount > 0)
        {
            return true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            return true;
        }else if (Input.GetKey(KeyCode.Mouse0))
        {
            return true;
        }
        else return false;
    }

    private void ChangeStateTo(GameState state)
    {
        // exit old state :
        switch (_gameState)
        {
            case GameState.START :
                StartGameCanvas.enabled = false;
                break;
            case GameState.INGAME :
                GameplayCanvas.enabled = false;
                break;
            case GameState.COMPLETE :
                break;
            case GameState.GAMEOVER :
                break;
        }
        // begin new state
        _gameState = state;
        switch (state)
        {
           case GameState.START :
               StartGameCanvas.enabled = true;
               break;
           case GameState.INGAME :
               GameplayCanvas.enabled = true;
               break;
           case GameState.COMPLETE :
               EndLevelCanvas.enabled = true;
               break;
           case GameState.GAMEOVER :
               GameoverCanvas.enabled = true;
               break;
        }
    }

    private void TouchHandler()
    {
        if (IsTouch())
        {
            switch (_gameState)
            {
                case GameState.START :
                    ChangeStateTo(GameState.INGAME);
                    BlockTouch(0.1f);
                    break;
                case GameState.INGAME :
                    Move();
                    break;
                case GameState.COMPLETE :
                    NextLevel();
                    break;
                case GameState.GAMEOVER :
                    ReloadLevel();
                    break;
            }
        }
        else
        {
           FloorPosition();
           SubtractEnergy();
        }
      
    }

    private void SubtractEnergy()
    {
        if (currentEnergy >= Mathf.Epsilon)
        {
            currentEnergy -= energyPenaty * Time.deltaTime;
        }
    }

    private void Move()
    {
        if (IsCompleteLevel())
        {
           ChangeStateTo(GameState.COMPLETE); 
        }else if (IsGameOver())
        {
            ChangeStateTo(GameState.GAMEOVER);
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.smoothDeltaTime);
        }
        
    }

    private void FloorPosition()
    {
        var position = transform.position;
        transform.position = new Vector3(position.x, Mathf.FloorToInt(position.y),position.z);
    }
    
    /*private void move()
    {
        
       
        if (IsTouch())
        {
            if (!isIngame)
            {
                if (GameoverCanvas.enabled)
                {

                    var scensse =  SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Additive);
                    scensse.allowSceneActivation = false;
                 
                }
                else
                {
                    StartCoroutine(PlayGame());
                    return;
                }
                
            }
            
            transform.Translate(Vector3.down * speed * Time.smoothDeltaTime);
            
        }
        else
        {
            transform.position = new Vector3(transform.position.x, Mathf.FloorToInt(transform.position.y),
                transform.position.z);
            if (currentEnergy >= Mathf.Epsilon)
            {
                currentEnergy -= energyPenaty * Time.deltaTime;
            }
           
        }
    }*/
    private bool IsCompleteLevel()
    {
        if (transform.position.y < 0)
        {
            BlockTouch(1f);
            return true;
        }

        return false;
    }
    private bool IsGameOver()
    {
        if (gameOverTrigger)
        {
            BlockTouch(1f);
            return true;
        }
        return false;
    }
    private void NextLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        var scencecount = SceneManager.sceneCount;
        if (index == scencecount - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(index + 1);
        }
        
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BlockTouch(float timeToBlock)
    {
        _delay = timeToBlock;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delay);
        _delay = 0;
    }
    public void SetGameOverTrigger()
    {
        gameOverTrigger = true;
    }
    
    
}
