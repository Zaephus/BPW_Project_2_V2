using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour {

    public TMP_Text nameText;

    public Slider healthSlider;
    public Text healthText;

    public void SetHUD() {}

    public void SetHealth(int health) {
        healthSlider.value = health;
        healthText.text = $"{health}";
    }
}