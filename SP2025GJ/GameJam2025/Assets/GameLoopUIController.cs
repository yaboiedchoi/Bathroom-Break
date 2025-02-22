using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopUIController : MonoBehaviour
{
    // ALWAYS IN THIS ORDER
    // 0 - preview
    // 1 - success
    // 2 - fail
    // 3 - gameover

    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        switch (GameLoop.instance.GameState)
        {
            // can only come from success, fail, or level preview
            // enable preview ui only
            case GameState.LevelPreview:
                if (!gameObjects[0].activeSelf)
                    gameObjects[0].SetActive(true);
                if (gameObjects[1].activeSelf)
                    gameObjects[1].SetActive(false);
                if (gameObjects[2].activeSelf)
                    gameObjects[2].SetActive(false);
                if (gameObjects[3].activeSelf)
                    gameObjects[3].SetActive(false);
                break;
            // can only come from game state
            // show success UI only
            case GameState.Success:
                if (!gameObjects[1].activeSelf)
                    gameObjects[1].SetActive(true);
                break;
            // can only come from game state
            // show fail ui only
            case GameState.Fail:
                if (!gameObjects[2].activeSelf) 
                    gameObjects[2].SetActive(true);
                break;
            // can only come from fail state
            // disable fail ui and enable game over ui
            case GameState.GameOver:
                if (!gameObjects[3].activeSelf)
                    gameObjects[3].SetActive(true);
                if (gameObjects[2].activeSelf)
                    gameObjects[2].SetActive(false);
                break;
            default:
                // disable all UIs if in gamestate not listed above
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    // if its active, set to inactive
                    if (gameObjects[i].activeSelf)
                        gameObjects[i].SetActive(false);
                }
                break;
        }

    }
}
