using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Health : MonoBehaviour
{
    public int maxHealth = 2;
    int _health;
    public TextMeshPro ui;

    public int health { 
        get {
            return _health;
        }
        set {
            if (value <= 0)
                Destroy(gameObject);
            else if (value != _health) {
                if (value > maxHealth)
                    _health = maxHealth;
                else
                    _health = value;
                if (ui)
                    ui.text = string.Concat(Enumerable.Repeat("I", value));;
            }
        }
    }

    private void OnEnable() {
        health = maxHealth;
    }
}
