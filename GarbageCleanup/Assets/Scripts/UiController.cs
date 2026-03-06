using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI cleanedText;

    [Header("Correct Deposits")]
    [SerializeField] private TextMeshProUGUI wasteText;
    [SerializeField] private TextMeshProUGUI plasticText;
    [SerializeField] private TextMeshProUGUI paperText;
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
        plasticText.text = $"Plastic: {ScoreManager.Instance.Correct[Garbage.Plastic]}";
        paperText.text = $"Paper: {ScoreManager.Instance.Correct[Garbage.Paper]}";
        electronicText.text = $"Electronic: {ScoreManager.Instance.Correct[Garbage.Electronic]}";
    }
}
