using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int currentScore = 0;


    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public CharacterSound sound;

    private int lastScoreHighlight = 0;
    private void Start()
    {
        // reset
        currentScore = 0;
        lastScoreHighlight = 0;
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;

        if (currentScore - lastScoreHighlight > scoreHighlightRange)
        {
            sound.PlayScoreHighlight();
            lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring()
    {
        // set high score
        if (currentScore > ScoreData.highScore)
        {
            ScoreData.highScore = currentScore;
        }
    }
}