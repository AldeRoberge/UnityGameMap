using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GenerateButton("Hey!", null, () => { Debug.Log("Pressed!"); });
        g.transform.parent = gameObject.GetComponentInChildren<HorizontalLayoutGroup>().transform;
    }

    public GameObject GenerateButton(string text, Sprite sprite, UnityAction onClick)
    {
        GameObject b = new GameObject("Button '" + text + "'");
        b.AddComponent<CanvasRenderer>();
        b.AddComponent<Image>().sprite = sprite;
        b.AddComponent<Button>().onClick.AddListener(onClick);

        GameObject t = new GameObject("Text");

        // Make it a part of the canvas
        t.AddComponent<CanvasRenderer>();

        // Add the text, center it and set its value.
        TextMeshProUGUI textMeshProUgui = t.AddComponent<TextMeshProUGUI>();
        textMeshProUgui.text = text;
        textMeshProUgui.color = Color.black;
        textMeshProUgui.alignment = TextAlignmentOptions.Center;
        t.transform.parent = b.transform;

        // Set aligned on center
        RectTransform rect = t.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);

        return b;
    }
}