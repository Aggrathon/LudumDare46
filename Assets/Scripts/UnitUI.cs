using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public static UnitUI active { get; protected set; }

    public Text nameText;
    public Text descText;
    public List<Button> buttons;
    
    private void Awake() {
        active = this;
        while(buttons.Count < 6) {
            var but = Instantiate(buttons[0], buttons[0].transform.parent);
            buttons.Add(but);
        }
        gameObject.SetActive(false);
    }

    public void Show(Unit unit) {
        nameText.text = unit.name;
        descText.text = string.Format("   Actions: {} / {}\n   Health: {} / {}\n   ", 1, 2, unit.health.health, unit.health.maxHealth) + "[WEAPON]";
        foreach(Button but in buttons)
            but.gameObject.SetActive(false);
        int index = 0;
        buttons[index].gameObject.SetActive(true);
        buttons[index].GetComponentInChildren<TextMeshPro>().text = "Move";
        buttons[index].onClick.RemoveAllListeners();
        buttons[index].onClick.AddListener(()=>{});
        index++;
        buttons[index].gameObject.SetActive(true);
        buttons[index].GetComponentInChildren<TextMeshPro>().text = "Skip";
        buttons[index].onClick.RemoveAllListeners();
        buttons[index].onClick.AddListener(()=>{});

        gameObject.SetActive(true);
    }
}
