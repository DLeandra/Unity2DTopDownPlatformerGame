using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;

    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();

        // Initialize the health bar size and color
        UpdateBar(Health.totalHealth);
    }

    // Reduces the player's health and updates the bar
    public void Damage(float damage)
    {
        Health.totalHealth -= damage;
        if (Health.totalHealth < 0f)
        {
            Health.totalHealth = 0f; // Prevent health from going below zero
        }

        // Update the health bar
        UpdateBar(Health.totalHealth);
    }

    // Updates the health bar size and color
    private void UpdateBar(float health)
    {
        SetSize(health);

        // Change the color based on health thresholds
        if (health < 0.3f)
        {
            barImage.color = Color.red; // Critical health
        }
        else if (health < 0.6f)
        {
            barImage.color = Color.yellow; // Medium health
        }
        else
        {
            barImage.color = Color.green; // Good health
        }
    }

    // Sets the size of the health bar
    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f, 1f); // Update the scale
    }

    // Returns the current health value
    public float GetHealth()
    {
        return Health.totalHealth;
    }
}
