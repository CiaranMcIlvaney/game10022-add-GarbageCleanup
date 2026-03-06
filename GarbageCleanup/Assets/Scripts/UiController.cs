using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI cleanedText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Correct Deposits")]
    [SerializeField] private TextMeshProUGUI wasteText;
    [SerializeField] private TextMeshProUGUI recyclableText;
    [SerializeField] private TextMeshProUGUI textileText;
    [SerializeField] private TextMeshProUGUI electronicText;

    void Update()
    {
        // Score
        scoreText.text = $"Score: {ScoreManager.Instance.Score}";

        // Cleaned percentage of map
        int percent = TrashProgress.GetPercent();
        cleanedText.text = $"Cleaned: {percent}%";

        // Correct counts of items placed in the correct bins
        wasteText.text = $"Waste: {ScoreManager.Instance.Correct[Garbage.Waste]}";
        recyclableText.text = $"Recyclable: {ScoreManager.Instance.Correct[Garbage.Recyclable]}";
        textileText.text = $"Textile: {ScoreManager.Instance.Correct[Garbage.Textile]}";
        electronicText.text = $"Electronic: {ScoreManager.Instance.Correct[Garbage.Electronic]}";
        feedbackText.text = $"{ScoreManager.Instance.feedback}";
    }
}
