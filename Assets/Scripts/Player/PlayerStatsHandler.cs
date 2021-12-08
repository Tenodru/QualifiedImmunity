using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles and provides references for global Player stats.
/// </summary>
public class PlayerStatsHandler : MonoBehaviour
{
    // Singleton instantiation.
    public static PlayerStatsHandler current;


    [Header("Starting Stats")]
    public float playerStartTimeBudget = 10.0f;


    // Current Stats
    float playerCurrentTimeBudget = 10.0f;

    // Other References
    PlayerStatsUIHandler playerStatsUI;

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PlayerStatsUIHandler>() != null)
        {
            playerStatsUI = FindObjectOfType<PlayerStatsUIHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes the player's current Time budget by the given amount.
    /// Positive values = Increase, Negative values = Decrease.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeTimeBudget(float amount)
    {
        playerCurrentTimeBudget += amount;

        if (playerStatsUI != null)
        {
            playerStatsUI.timeBudgetCount.text = playerCurrentTimeBudget.ToString();
        }
    }

    /// <summary>
    /// Gets the player's current Time budget.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentTimeBudget()
    {
        return playerCurrentTimeBudget;
    }
}
