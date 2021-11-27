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

    // UI for the answer choices of each hole 
    public GameObject holeAnswers; 
    // The ball 
    public GameObject ball; 
    // List of holes 
    public GameObject[] holes; 
    // The index of the hole that the ball went in 
    private int holeIndex; 
    
    // UI for the correct answer hole score 
    public GameObject correctAnswerText; 
    // UI for the incorrect answer hole score 
    public GameObject incorrectAnswerText;
    // Bool for whether the animation has played already 
    private bool animPlayed = false; 

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

        // Once the ball has entered a hole, check which hole it went into 
        if (!ball.activeSelf) {
            for (int i = 0; i < holes.Length; i++) {
                Scripts_Hole_MiniGolf_BrianLin holeScript = holes[i].GetComponent<Scripts_Hole_MiniGolf_BrianLin>();
                if (holeScript.holeIn) {
                    holeIndex = i; 
                    break; 
                }
            }
            
            if (!animPlayed) {
                StartCoroutine(WaitAnimation());
                animPlayed = true;  
            }          
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
    // and the answers that each hole represents 
    // </summary>
    IEnumerator ShowAnswers() {
        yield return new WaitForSeconds(2f); 
        holeAnswers.SetActive(true); 
        answers.SetActive(true); 

    }

    IEnumerator WaitAnimation() {
        correctAnswerText.SetActive(true);
        yield return new WaitForSeconds(2f); 
        correctAnswerText.SetActive(false);
    }
}
