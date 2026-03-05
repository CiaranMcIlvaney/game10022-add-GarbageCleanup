using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinController : MonoBehaviour
{
    [Header("Waste Type")]

    // This determines what type of garbage this bin accepts
    public Garbage acceptedType = Garbage.Waste;

    public static Dictionary<Garbage, int> CorrectDeposits = new Dictionary<Garbage, int>()
    {
        { Garbage.Waste, 0},
        { Garbage.Recyclable, 0 },
        { Garbage.Textile, 0 },
        { Garbage.Electronic, 0 }
    };
    // Called when the player interacts with the bin 
    public void TryDeposit(InventoryController inventory)
    {
        // Ask the inventory what type of garbage is currently selected 
        var currentType = inventory.GetCurrentGarbageType();

        // If currentType is null then that means the inventory is emtpy or there is no item selected
        if (currentType == null)
        {
            Debug.Log("[Bin] No item in hand to deposit.");
            return;
        }

        // If the item type matches what this bin accepts:
        if (currentType.Value == acceptedType)
        {
            // Remove the item from the inventory
            GameObject removed = inventory.RemoveCurrent();

            // Destroy the game object from the scene completely
            Destroy(removed);

            // Increment correct deposit counter for that type
            CorrectDeposits[acceptedType]++;

            // Print success message
            Debug.Log($"SUCCESS! Deposited {currentType.Value} into {acceptedType} bin.");

            // Debug totals for UI later on
            Debug.Log($"[Total - Correct Deposits]" + $"Waste:{CorrectDeposits[Garbage.Waste]}" + $"Plastic:{CorrectDeposits[Garbage.Recyclable]}" + $"Paper:{CorrectDeposits[Garbage.Textile]}" + $"Electronic:{CorrectDeposits[Garbage.Electronic]}");
        }
        else
        {
            // If types do not match print fail message 
            Debug.Log($"FAIL! You tried to deposit {currentType.Value} into {acceptedType} bin.");
        }
    }
}
