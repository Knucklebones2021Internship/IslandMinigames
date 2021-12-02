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
    // List of questions 
    public GameObject[] questions; 
    // Index for the current question 
    private int questionIndex = 0; 
    // Answer choices for each question 
    public GameObject[] answers; 

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
            // Check which hole it went into 
            for (int i = 0; i < holes.Length; i++) {
                Scripts_Hole_MiniGolf_BrianLin holeScript = holes[i].GetComponent<Scripts_Hole_MiniGolf_BrianLin>();
                if (holeScript.holeIn) {
                    holeIndex = i; 
                    break; 
                }
            }
            
            if (!animPlayed) {
                if (holeIndex == 0) {
                    StartCoroutine(WaitCorrectAnimation());
                    animPlayed = true;  
                }
                else {
                    StartCoroutine(WaitIncorrectAnimation());
                    animPlayed = true;                      
                }
            } 

            // After the ball goes in the hole, go to the next question 
            // and respawn the ball 
            StartCoroutine(NextQuestion());
            Scripts_TestBall_MiniGolf_BrianLin ballScript = ball.GetComponent<Scripts_TestBall_MiniGolf_BrianLin>(); 
            ballScript.BallRespawn(); 
            ball.SetActive(true);  
            animPlayed = false;                   
        }
    }

    // <summary> 
    // Sets global timer invisible and displays the game over panel
    // </summary>
    void GameOver() {
        globalTimer.SetActive(false);
        questionPanel.SetActive(false); 
        answers[questionIndex].SetActive(false); 
        gameOverPanel.SetActive(true);
    }    

    // <summary> 
    // Wait a few seconds before showing the answer choices for the given question
    // and the answers that each hole represents 
    // </summary>
    IEnumerator ShowAnswers() {
        yield return new WaitForSeconds(2f); 
        holeAnswers.SetActive(true); 
        answers[questionIndex].SetActive(true); 

    }

    // <summary> 
    // Wait a few seconds for the "correct" text animation to finish 
    // </summary>
    IEnumerator WaitCorrectAnimation() {
        correctAnswerText.SetActive(true);
        yield return new WaitForSeconds(2f); 
        correctAnswerText.SetActive(false);
    }

    // <summary> 
    // Wait a few seconds for the "incorrect" text animation to finish 
    // </summary>
    IEnumerator WaitIncorrectAnimation() {
        incorrectAnswerText.SetActive(true);
        yield return new WaitForSeconds(2f); 
        incorrectAnswerText.SetActive(false);
    }    

    // <summary> 
    // Wait 1 second before the next question and answers appear 
    // </summary>
    IEnumerator NextQuestion() {
        questionPanel.SetActive(false);
        answers[questionIndex].SetActive(false); 
        questions[questionIndex].SetActive(false);       
        yield return new WaitForSeconds(1f); 
        questionIndex++;   
        questionPanel.SetActive(true);
        questions[questionIndex].SetActive(true);   
        answers[questionIndex].SetActive(true); 
    }     
}
