using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Gamestate: {GameLoop.instance.GameState} \n" + 
                    $"Timer: {GameLoop.instance.Timer} \n" +
                    $"Lives: {GameLoop.instance.Lives} \n" +
                    $"Score: {GameLoop.instance.Score} \n" +
                    $"Level: {GameLoop.instance.Level}";
    }
}
