using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : ScriptableObject {

    public string unitName;

    public int maxHealth;
    public int currentHealth;

    public int baseAttackStrength;
    public int currentAttackStrength;

    public int baseDefenseStrength;
    public int currentDefenseStrength;

    public bool TakeDamage(int dmg) {

        int damage = dmg/currentDefenseStrength;
        if(damage <= 0) {
            damage = 0;
        }
        currentHealth -= damage;

        if(currentHealth <= 0) {
            currentHealth = 0;
            return true;
        }
        else {
            return false;
        }

    }

    public void Heal(int amount) {

        currentHealth += amount;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }

    }
    
}