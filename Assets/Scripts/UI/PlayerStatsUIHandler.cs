using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles the Player Stats UI.
/// </summary>
public class PlayerStatsUIHandler : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI timeBudgetCount;

    PlayerStatsHandler playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStatsHandler>();
        if (playerStats != null)
        {
            timeBudgetCount.text = ((int)playerStats.GetCurrentTimeBudget()).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
