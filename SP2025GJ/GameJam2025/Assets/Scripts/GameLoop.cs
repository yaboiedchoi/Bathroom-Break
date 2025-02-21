using System.Collections;
using System.Collections.Generic;
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
    private float previewTime = 10;

    // temporary for testing
    // used for success / failure screen time
    // can be replaced with a button boolean if needed
    [SerializeField]
    private float sfTime = 10;

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

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.LevelPreview;
        timer = 10;
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState) 
        {
            case GameState.LevelPreview: 
                // TODO: add level preview visuals

                // if timer runs out
                if (timer <= 0)
                {
                    // set timer to gameTime
                    // and set gamestate to game
                    // reset game success too
                    timer = gameTime;
                    gameState = GameState.Game;
                    gameSuccess = false;
                }
                break;
            case GameState.Game: 
                // TODO: add game visuals

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
                }
                break;
            case GameState.Success: 
                // TODO: add level success visuals

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
                }
                break;
            case GameState.Fail: 
                // TODO: add level failure visuals

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
                        timer = sfTime;
                        gameState = GameState.LevelPreview;
                    }
                }
                break;
            case GameState.GameOver: 
                // TODO: add game over visuals 
                
                // when timer is <= 0
                if (timer <= 0)
                {
                    // reset all things
                    ResetAll();
                    // GOES BACK TO LEVEL PREVIEW, TEMPORARY FOR NOW
                    timer = sfTime;
                    gameState = GameState.LevelPreview;
                }
                break;
            default: 
                Debug.LogError("gameState variable is corrupted or not set!");
                break;
        }

        // if timer is more than 0, count down
        if (timer > 0) 
        {
            timer -= Time.deltaTime;
        }
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
}
