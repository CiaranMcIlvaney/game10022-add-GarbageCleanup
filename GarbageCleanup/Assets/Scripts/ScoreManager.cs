using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Single reference so other scripts can access ScoreManger easily
    public static ScoreManager Instance;

    // Current player score
    public int Score { get; private set; } = 0;

    // Dictionary that tracks how many correct deposits the player has made for each garbage type
    public Dictionary<Garbage, int> Correct = new();

    // Dictionary that tracks how many inccorect deposits the player has made for each garbage type
    public Dictionary<Garbage, int> Wrong = new();

    [Header("Points (Correct Deposit)")]

    // Points given when the player places the correct garbage in the correct bin
    [SerializeField] private int wasteCorrect = 5;
    [SerializeField] private int plasticCorrect = 10;
    [SerializeField] private int paperCorrect = 10;
    [SerializeField] private int electronicCorrect = 25;

    // Points removed when the player places garbage in the wrong bin
    [Header("Penalty (Wrong Deposit)")]
    [SerializeField] private int wasteWrong = 2;
    [SerializeField] private int plasticWrong = 5;
    [SerializeField] private int paperWrong = 5;
    [SerializeField] private int electronicWrong = 15;

    void Awake()
    {
        // Store reference so other scripts can access ScoreManager
        Instance = this;

        // Initialize the dictionaires so each garbage type starts at 0
        foreach (Garbage g in System.Enum.GetValues(typeof(Garbage)))
        {
            Correct[g] = 0;
            Wrong[g] = 0;
        }
    }

    public void AddCorrect(Garbage type)
    {
        // Increase correct count for this garbage type
        Correct[type]++;

        // Add score based on the garbage type
        Score += GetCorrectPoints(type);

        // Print updated score + stats in console
        DebugTotals();
    }

    public void AddWrong(Garbage type)
    {
        // Increase wrong count for this garbage type
        Wrong[type]++;

        // Subtract score based on the garbage type
        Score -= GetWrongPenalty(type);

        // Prevent score from going below zero
        if (Score < 0)
        {
            Score = 0;
        }

        // Print stats to console 
        DebugTotals();
    }

    private int GetCorrectPoints(Garbage type)
    {
        // Switch statment checks which garbage type recieved and returns the right score value
        switch (type)
        {
            case Garbage.Waste: return wasteCorrect;
            case Garbage.Plastic: return plasticCorrect;
            case Garbage.Paper: return paperCorrect;
            case Garbage.Electronic: return electronicCorrect;
            default: return 0;
        }
    }

    private int GetWrongPenalty(Garbage type)
    {
        // Same logic as correct poitns but returns penalty values instead
        switch (type)
        {
            case Garbage.Waste: return wasteWrong;
            case Garbage.Plastic: return plasticWrong;
            case Garbage.Paper: return paperWrong;
            case Garbage.Electronic: return electronicWrong;
            default: return 0;
        }
    }

    private void DebugTotals()
    {
        // Print total score
        Debug.Log($"[SCORE] {Score}");

        // Print how many correct deposits have been made
        Debug.Log($"[Correct] Waste:{Correct[Garbage.Waste]} Plastic:{Correct[Garbage.Plastic]} Paper:{Correct[Garbage.Paper]} Electronic:{Correct[Garbage.Electronic]}");
        
        // Print how many wrong deposits have been made 
        Debug.Log($"[Wrong]   Waste:{Wrong[Garbage.Waste]} Plastic:{Wrong[Garbage.Plastic]} Paper:{Wrong[Garbage.Paper]} Electronic:{Wrong[Garbage.Electronic]}");
    }
}
