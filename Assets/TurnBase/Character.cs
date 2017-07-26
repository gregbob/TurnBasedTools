using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    public int CurrentHealth;
    public int MaxHealth;
    public string Name;

    public Character(int maxHealth, string name)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Name = name;
    }

    public bool isAlive()
    {
        return CurrentHealth > 0;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log("Hit for " + damage + ". Current health is " + CurrentHealth + " / " + MaxHealth);
    }

    public void OnTakeDamage()
    {

    }

    public void OnDeath()
    {

    }
}
