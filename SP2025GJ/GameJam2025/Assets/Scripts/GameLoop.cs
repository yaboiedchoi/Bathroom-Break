using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GameState
{
    LevelPreview,
    Game,
    Success,
    Fail,
    GameOver
}

public class GameLoop : MonoBehaviour
{
    // singleton
    public static GameLoop instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // fields

    [SerializeField]
    private uint lives = 3;

    [SerializeField]
    private uint level = 0;

    [SerializeField]
    private bool gameSuccess = false;

    [SerializeField]
    private int score = 0;

    [SerializeField]
    private GameObject player;

    private PlayerMovement _pm;
    private PlayerLook _pl;
    private PlayerPickup _pp;
    private PunchObject _po;
    private PlayerInteract _pi;

    // GameObjects

    [SerializeField]
    private GameObject chairDeskPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject bookshelfPrefab;

    [SerializeField]
    private GameObject[] listOfActiveObj;

    [SerializeField]
    private GameObject menuUI;

    [SerializeField]
    private SpawnAgents agentManager;

    // audio
    [SerializeField]
    private AudioClip theme;
    [SerializeField]
    private AudioClip yes;
    [SerializeField]
    private AudioClip no;
    [SerializeField]
    private AudioSource _as;

    // pause menu

    private bool paused;

    private bool isESCPressed;
    private bool isESCPressedPrevious;

    // TIMERS

    // holds the counting timer
    [SerializeField]
    private float timer = 0;

    // temporary for testing, change later
    // max time allowed for gameplay
    [SerializeField]
    private float gameTime = 10;

    // temporary for testing
    // used for level preview before starting the level
    // can be replaced by a bool like a Ready! button or something like that
    [SerializeField]
    private float previewTime = 5;

    // temporary for testing
    // used for success / failure screen time
    // can be replaced with a button boolean if needed
    [SerializeField]
    private float sfTime = 5;

    // holds the current gamestate
    [SerializeField]
    private GameState gameState;

    // properties

    // gamestate property, to be able to read current game state through singleton
    // TODO: SET PROPERTY TEMPORARY, SO REPLACE WITH A METHOD
    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

    /// <summary>
    /// Property for lives count
    /// </summary>
    public uint Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    /// <summary>
    /// Property for score
    /// </summary>
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    /// <summary>
    /// Property for timer
    /// </summary>
    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

    public uint Level
    {
        get { return level; }
        set { level = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.LevelPreview;
        timer = previewTime;
        level = 1;
        listOfActiveObj = GameObject.FindGameObjectsWithTag("Reset");
        _pl = player.GetComponent<PlayerLook>();
        _pm = player.GetComponent<PlayerMovement>();
        _pp = player.GetComponent<PlayerPickup>();
        _po = player.GetComponent<PunchObject>();
        _pi = player.GetComponent<PlayerInteract>();
        isESCPressed = false;
        isESCPressedPrevious = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Cursor.visible);
        // is tab key down?
        isESCPressed = Input.GetKey(KeyCode.Tab);

        // toggle pausing
        // only will pause on first click of key
        if (isESCPressed && !isESCPressedPrevious)
        {
            paused = !paused;
        }

        // if the game is not paused
        if (!paused)
        {
            // check player movement, player should be able to move
            // when unpaused,enable player movement and disable mouse
            if (!_pl.enabled && !_pm.enabled)
            {
                _pl.enabled = true;
                _pm.enabled = true;
                _pp.enabled = true;
                _po.enabled = true;
                _pi.enabled = true;
            }
            // if cursor is visible
            // turn cursor back invisible
            if (Cursor.visible == true)
            {
                // disable cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            // if Pause menu UI is open
            if (menuUI.activeSelf)
            {
                menuUI.SetActive(false);
            }
        }

        // FSM
        switch (gameState)
        {
            case GameState.LevelPreview:
                // TODO: add level preview visuals

                // disable all player interaction
                if (_pl.enabled && _pm.enabled)
                {
                    _pl.enabled = false;
                    _pm.enabled = false;
                    _pp.enabled = false;
                    _po.enabled = false;
                    _pi.enabled = false;
                }
                if (Cursor.visible == false)
                {
                    // re enable cursor
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                /*
                // if timer runs out
                if (timer <= 0)
                {
                    // set timer to gameTime
                    // and set gamestate to game
                    // reset game success too
                    timer = gameTime;
                    gameState = GameState.Game;
                    gameSuccess = false;

                    // reenable movement, if disabled
                    if (!_pl.enabled && !_pm.enabled)
                    {
                        _pl.enabled = true;
                        _pm.enabled = true;
                        _pp.enabled = true;
                        _po.enabled = true;
                        _pi.enabled = true;
                    }
                    // if cursor is visible
                    if (Cursor.visible == true)
                    {
                        // disable cursor
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
                */
                break;
            case GameState.Game:
                //check if game has been won
                gameSuccess = CheckForWin();

                // TODO: add game visuals
                // play theme on game state
                if (_as.isPlaying)
                    _as.Stop();
                if (_as.clip != theme)
                    _as.clip = theme;
                if (!_as.isPlaying)
                    _as.Play();

                // when timer is <= 0
                if (timer <= 0)
                {
                    // if game was successful
                    if (gameSuccess)
                        gameState = GameState.Success;
                    // if game was failed
                    else
                        gameState = GameState.Fail;
                    // set timer to success/fail time
                    timer = sfTime;

                    if (_as.isPlaying)
                        _as.Stop();
                }



                break;
            case GameState.Success:
                // TODO: add level success visuals

                // disable all player interaction
                if (_pl.enabled && _pm.enabled)
                {
                    _pl.enabled = false;
                    _pm.enabled = false;
                    _pp.enabled = false;
                    _po.enabled = false;
                    _pi.enabled = false;
                }
                if (Cursor.visible == false)
                {
                    // re enable cursor
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                /*
                // when timer is <= 0
                if (timer <= 0)
                {
                    // add points for success
                    // add 1 to level
                    score += 100;
                    level++;
                    // set timer to level preview
                    // set gamestate to level preview
                    timer = previewTime;
                    gameState = GameState.LevelPreview;
                    ResetGame();
                }
                */
                break;
            case GameState.Fail:
                // TODO: add level failure visuals

                // disable all player interaction
                if (_pl.enabled && _pm.enabled)
                {
                    _pl.enabled = false;
                    _pm.enabled = false;
                    _pp.enabled = false;
                    _po.enabled = false;
                    _pi.enabled = false;
                }
                if (Cursor.visible == false)
                {
                    // re enable cursor
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                /*
                // when timer is <= 0
                if (timer <= 0)
                {
                    // set timer to level preview
                    // set gamestate to game IF you have more than 0 lives, 
                    //if not, set gamestate to game over
                    timer = previewTime;
                    // subtract life
                    lives--;
                    // when you're out of lives
                    if (lives <= 0)
                    {
                        // set game over time
                        // and set game state to game over
                        timer = sfTime;
                        gameState = GameState.GameOver;
                    }
                    // when you still have lives
                    else {
                        // set level preview time
                        // set game state
                        // reset placement of objects
                        timer = sfTime;
                        gameState = GameState.LevelPreview;
                        ResetGame();
                    }
                }
                */
                break;
            case GameState.GameOver:
                // TODO: add game over visuals 

                // disable all player interaction
                if (_pl.enabled && _pm.enabled)
                {
                    _pl.enabled = false;
                    _pm.enabled = false;
                    _pp.enabled = false;
                    _po.enabled = false;
                    _pi.enabled = false;
                }
                if (Cursor.visible == false)
                {
                    // re enable cursor
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                // when timer is <= 0
                /*
                if (timer <= 0)
                {
                    // reset all things
                    ResetAll();
                    ResetGame();
                    // GOES BACK TO LEVEL PREVIEW, TEMPORARY FOR NOW
                    timer = sfTime;
                    gameState = GameState.LevelPreview;
                }
                */
                break;
            default:
                Debug.LogError("gameState variable is corrupted or not set!");
                break;
        }


        // if timer is more than 0, count down
        // stop timers if paused
        if (timer > 0 && !paused)
        {
            timer -= Time.deltaTime;
        }
        if (paused)
        {
            // when paused, disable player movement and enable mouse
            if (_pl.enabled && _pm.enabled)
            {
                _pl.enabled = false;
                _pm.enabled = false;
                _pp.enabled = false;
                _po.enabled = false;
                _pi.enabled = false;
            }
            if (Cursor.visible == false)
            {
                // re enable cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (!menuUI.activeSelf)
            {
                menuUI.SetActive(true);
            }
        }

        isESCPressedPrevious = isESCPressed;
    }

    // private methods

    /// <summary>
    /// reset all variables between games
    /// </summary>
    private void ResetAll()
    {
        timer = 0;
        score = 0;
        lives = 3;
        level = 0;
    }

    private void SkipGameState()
    {
        timer = 0;
    }

    // public methods

    /// <summary>
    /// Call this method when the player succeeds at a game
    /// </summary>
    public void GameSuccess()
    {
        gameSuccess = true;
    }

    /// <summary>
    /// Adds points to the total score count
    /// </summary>
    /// <param name="points">points to add to the total</param>
    public void AddScore(int points)
    {
        score += points;
    }
    public void ResetGame()
    {
        if (listOfActiveObj.Length != 3)
        {
            listOfActiveObj = GameObject.FindGameObjectsWithTag("Reset");
        }

        for (int i = 0; i < listOfActiveObj.Length; i++)
        {
            Destroy(listOfActiveObj[i]);
        }
        GameObject newPlayer = Instantiate(playerPrefab);
        listOfActiveObj[0] = newPlayer;
        player = newPlayer;
        _pl = player.GetComponent<PlayerLook>();
        _pm = player.GetComponent<PlayerMovement>();
        _pp = player.GetComponent<PlayerPickup>();
        _po = player.GetComponent<PunchObject>();
        _pi = player.GetComponent<PlayerInteract>();
        listOfActiveObj[1] = Instantiate(bookshelfPrefab);
        listOfActiveObj[2] = Instantiate(chairDeskPrefab);
    }

    /// <summary>
    /// Change the Game State
    /// </summary>
    public void ChangeGameState()
    {
        // from which state?
        switch (gameState)
        {
            case GameState.LevelPreview:
                // level preview only goes to game
                // set timer to gameTime
                // and set gamestate to game
                // reset game success too
                timer = gameTime;
                gameState = GameState.Game;
                gameSuccess = false;

                // reenable movement, if disabled
                if (!_pl.enabled && !_pm.enabled)
                {
                    _pl.enabled = true;
                    _pm.enabled = true;
                    _pp.enabled = true;
                    _po.enabled = true;
                    _pi.enabled = true;
                }
                // if cursor is visible
                if (Cursor.visible == true)
                {
                    // disable cursor
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                break;
            case GameState.Game:
                // time based change, already coded into FSM
                Debug.LogWarning("Cannot change out of Game, time based only!");
                break;
            case GameState.Success:
                // only goes to level preview
                // add points for success
                // add 1 to level
                score += 100;
                level++;
                // set timer to level preview
                // set gamestate to level preview
                timer = previewTime;
                gameState = GameState.LevelPreview;
                ResetGame();

                // yes stinger
                if (_as.isPlaying)
                    _as.Stop();
                if (_as.clip != yes)
                    _as.clip = yes;
                if (!_as.isPlaying)
                    _as.Play();

                break;
            case GameState.Fail:
                // only goes to game over / preview
                // set timer to level preview
                // set gamestate to game IF you have more than 0 lives, 
                //if not, set gamestate to game over
                timer = previewTime;
                // subtract life
                lives--;
                // when you're out of lives
                if (lives <= 0)
                {
                    // set game over time
                    // and set game state to game over
                    timer = sfTime;
                    gameState = GameState.GameOver;
                }
                // when you still have lives
                else
                {
                    // set level preview time
                    // set game state
                    // reset placement of objects
                    timer = sfTime;
                    gameState = GameState.LevelPreview;
                    ResetGame();
                }

                // no stinger
                if (_as.isPlaying)
                    _as.Stop();
                if (_as.clip != no)
                    _as.clip = no;
                if (!_as.isPlaying)
                    _as.Play();

                break;
            case GameState.GameOver:
                // only goes to level preview after fully resetted
                // reset all things
                ResetAll();
                ResetGame();
                // GOES BACK TO LEVEL PREVIEW, TEMPORARY FOR NOW
                timer = sfTime;
                gameState = GameState.LevelPreview;


                // no stinger
                if (_as.isPlaying)
                    _as.Stop();
                if (_as.clip != no)
                    _as.clip = no;
                if (!_as.isPlaying)
                    _as.Play();

                break;
        }
    }

    private bool CheckForWin()
    {
        //get list of agents
        List<GameObject> agents = agentManager.GetAgents();
        //loop and check if all idle
        foreach (GameObject agent in agents)
        {
            if (agent.GetComponent<AgentBehavior>().GetState() != AgentBehavior.AgentStates.Idle)
            {
                return false;
            }
        }

        return true;
    }
}
