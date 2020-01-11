using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;
using Object = World.Map.Objects.Object;

public class UIManager : Singleton<UIManager>
{
    private Sprite rotateButtonSprite;

    private List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        rotateButtonSprite = Resources.Load<Sprite>("/Sprites/UI/Rotate");
        
        buttons = new List<GameObject>();
    }

    public void ShowOptionsFor(Object obj)
    {
        ClearButtons();
        AddButton(GenerateRotateObjectButton(obj));
    }

    public void AddButton(GameObject button)
    {
        buttons.Add(button);
        button.transform.SetParent(gameObject.GetComponentInChildren<HorizontalLayoutGroup>().transform, false);
    }

    private void ClearButtons()
    {
        foreach (GameObject o in buttons)
        {
            Destroy(o);
        }

        buttons.Clear();
    }

    public GameObject GenerateRotateObjectButton(Object toRotate)
    {
        return GenerateButton("Rotate", rotateButtonSprite, toRotate.Rotate);
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
        t.transform.SetParent(b.transform, false);

        // Set aligned on center
        RectTransform rect = t.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);

        return b;
    }
}