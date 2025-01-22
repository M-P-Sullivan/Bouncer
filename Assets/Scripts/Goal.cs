using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Sprite closedGoal;

    private bool isOpen = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            spriteRenderer.sprite = closedGoal;
            GoalCounter.instance.incrementGoals();
            isOpen = false;
        }
        
    }
}
