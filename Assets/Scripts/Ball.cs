using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private int maxFuzzy;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color fuzzyColor;
    [SerializeField] private LevelsManager _levelsManager;
    [SerializeField] private Slider LevelBar;
    private bool isFuzzying;
    [SerializeField]private Slider fuzzySlider;
    [SerializeField]private TextMeshProUGUI scoreText;
    private float currentEnergy;
    public const float speed = 10f;
    private GameObject Stacks;
    private Material mat;
    [SerializeField] float energyPenaty = 10f;
    private int score;
    private ParticleSystem[] fireVFX;
    [SerializeField]private Canvas GameoverCanvas;
    [SerializeField]private Canvas StartGameCanvas;
    [SerializeField]private Canvas GameplayCanvas;
    [SerializeField]private Canvas EndLevelCanvas;
    private bool gameOverTrigger;
    private float _delay = 0;
    private Bouncing bouncing;
    private GameObject level;
    private enum GameState{START,INGAME,GAMEOVER,COMPLETE}

    private GameState _gameState;
    
    private void Awake()
    {
        
    }
    
    void Start()
    {
       //InitGame();
       NewInitGame();
    }

    // Update is called once per frame
    void Update()
    {
        // de bug 
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
        TouchHandler();
        FuzzyHandle();
        
        if (IsCompleteLevel())
        {
            ChangeStateTo(GameState.COMPLETE); 
        }else if (IsGameOver())
        {
            ChangeStateTo(GameState.GAMEOVER);
        }
    }

    private void NewInitGame()
    {
        bouncing = FindObjectOfType<Bouncing>();
        isFuzzying = false;
        currentEnergy = 0;
        fuzzySlider.minValue = 0;
        fuzzySlider.maxValue = maxFuzzy;
        mat = GetComponentInChildren<Renderer>().material;
        fireVFX = GetComponentsInChildren<ParticleSystem>();
        score = 0;
        AddScore(0);
        GameoverCanvas.enabled = false;
        GameplayCanvas.enabled = false;
        StartGameCanvas.enabled = true;
        gameOverTrigger = false;
        _gameState = GameState.START;
        level = Instantiate(_levelsManager.GetLevelGameObjectPrefab(0), Vector3.zero, Quaternion.identity);
        Stacks = level.GetComponent<LevelMap>().Stacks;
        int yStart = Stacks.GetComponent<Stacks>().GetListCount() - 1;
        transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
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
    {   // Debug.Log(_gameState.ToString());
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
                bouncing.SetBoundTrigger(false);
                
                break;
            case GameState.COMPLETE :
                EndLevelCanvas.enabled = false;
                break;
            case GameState.GAMEOVER :
                GameoverCanvas.enabled = false;
                gameOverTrigger = false;
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
               bouncing.SetBoundTrigger(true);
               LevelText.text = "Level: " + ( _levelsManager.GetCurrentLevelIndex() + 1 );
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
        transform.Translate(Vector3.down * speed * Time.smoothDeltaTime);
    }
    private void FloorPosition()
    {
        var position = transform.position;
        transform.position = new Vector3(position.x, Mathf.FloorToInt(position.y),position.z);
    }
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
        ResetFuzzy();
        
        ChangeStateTo(GameState.START);
        Destroy(level);
       // level = new GameObject();
        level = Instantiate(_levelsManager.LoadNextLevel(),Vector3.zero, Quaternion.identity);
        Stacks = level.GetComponent<LevelMap>().Stacks;
        int yStart = Stacks.GetComponent<Stacks>().GetListCount() - 1;
        transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
     //   Debug.Log(Stacks.gameObject.name);
        gameOverTrigger = false;
        ChangeStateTo(GameState.START);
        
    }

    private void ResetFuzzy()
    {
        isFuzzying = false;
        currentEnergy = 0;
        fuzzySlider.minValue = 0;
        fuzzySlider.maxValue = maxFuzzy;
    }
    private void ReloadLevel()
    {
        Destroy(level);
        //level = new GameObject();
        ResetFuzzy();
        ChangeStateTo(GameState.START);
        level = new GameObject();
        level = Instantiate(_levelsManager.ReloadLevel(),Vector3.zero, Quaternion.identity);
        Stacks = level.GetComponent<LevelMap>().Stacks;
        int yStart = Stacks.GetComponent<Stacks>().GetListCount() - 1;
        transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
        //   Debug.Log(Stacks.gameObject.name);
        gameOverTrigger = false;
        ChangeStateTo(GameState.START);
        
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
