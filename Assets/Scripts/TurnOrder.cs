using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnOrder : MonoBehaviour
{
    [SerializeField] protected Transform modelImage;

    void Start()
    {
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show(List<Unit> order) {
        while(modelImage.transform.parent.childCount < order.Count)
            Instantiate(modelImage, modelImage.transform.parent);
        for (int i = 0; i < order.Count; i++)
        {
            var trans = modelImage.parent.GetChild(i);
            trans.GetChild(0).GetComponent<Image>().color = order[i].color;
            trans.GetChild(1).GetComponent<TextMeshProUGUI>().text = order[i].name;
        }
        gameObject.SetActive(true);
    }
}
