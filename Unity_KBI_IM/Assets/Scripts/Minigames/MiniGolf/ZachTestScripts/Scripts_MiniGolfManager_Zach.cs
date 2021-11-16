using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NO LONGER IN USE
// USING OLD INPUT SYSTEM NOW

[DefaultExecutionOrder(-50)]
public class Scripts_MiniGolfManager_Zach : Scripts_BaseManager_Wyatt
{
    protected override void Awake()
    {
        base.Awake();

        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Game Timers
    public GameObject globalTimer;
    // Game Over UI  
    public GameObject gameOverPanel;
    // UI for the questions 
    public GameObject questionPanel; 
    // Answer choices for the question 
    public GameObject answers; 

    void Start() {
        StartCoroutine(ShowAnswers());
    }
    void Update() {

        int endGlobalTime = 0;
        int.TryParse(globalTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endGlobalTime);

        if (endGlobalTime <= 25) {
            globalTimer.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        }
        if (endGlobalTime <= 10) {
            GameOver(); 
        } 
    }

    // <summary> 
    // Sets global timer invisible and displays the game over panel
    // </summary>
    void GameOver() {
        globalTimer.SetActive(false);
        questionPanel.SetActive(false); 
        answers.SetActive(false); 
        gameOverPanel.SetActive(true);
    }    

    // <summary> 
    // Wait a few seconds before showing the answer choices for the given question
    // </summary>
    IEnumerator ShowAnswers() {
        yield return new WaitForSeconds(2f); 

        answers.SetActive(true); 
    }
}
