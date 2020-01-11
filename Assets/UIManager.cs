using System.Collections;
using System.Collections.Generic;
using Migration.Packets;
using Migration.Packets.DontTransfer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;
using Object = World.Map.Objects.Object;

public class UIManager : Singleton<UIManager>
{
    private Sprite buttonBackground;

    private Sprite rotateButtonSprite;
    private Sprite informationButtonSprite;

    private List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        rotateButtonSprite = Resources.Load<Sprite>("Sprites/UI/Button/Rotate");
        informationButtonSprite = Resources.Load<Sprite>("Sprites/UI/Button/QuestionMark");
        
        
        buttonBackground = Resources.Load<Sprite>("Sprites/UI/Button/Button");

        buttons = new List<GameObject>();
    }

    public void ShowOptionsFor(Object obj)
    {
        ClearButtons();
        AddButton(GenerateRotateObjectButton(obj));
        AddButton(GenerateInformationButton(obj));
    }

    public void AddButton(GameObject button)
    {
        buttons.Add(button);
        button.transform.SetParent(gameObject.GetComponentInChildren<HorizontalLayoutGroup>().transform, false);
    }

    public void ClearButtons()
    {
        foreach (GameObject o in buttons)
        {
            Destroy(o);
        }

        buttons.Clear();
    }

    public GameObject GenerateRotateObjectButton(Object toRotate)
    {
        return GenerateButton("Rotate", rotateButtonSprite, () =>
        {
            toRotate.Rotate();
            GameServerConnectionConcrete.Instance.SendPacket(new RotateObjectPacket()
            {
                objectUID = 2,
                newRotation = toRotate.GetRotation()
            });
        });
    }
    
    public GameObject GenerateInformationButton(Object toRotate)
    {
        return GenerateButton("Information", informationButtonSprite, () =>
        {
            Debug.Log("Object pos : " + toRotate.tileLoc);
            Debug.Log("Object type : " + toRotate.objectType);
        });
    }
    

    public GameObject GenerateButton(string text, Sprite sprite, UnityAction onClick)
    {
        GameObject b = new GameObject("Button '" + text + "'");
        b.AddComponent<CanvasRenderer>();
        b.AddComponent<Image>().sprite = buttonBackground;
        b.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f); // Global size
        Button button = b.AddComponent<Button>();
        button.onClick.AddListener(onClick);

        // Create icon image
        GameObject image = new GameObject("Image");

        Image img = image.AddComponent<Image>();
        img.sprite = sprite;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50); // Icon size
        image.transform.SetParent(b.transform, false);

        // Create text
        GameObject t = new GameObject("Text");
        t.AddComponent<CanvasRenderer>();

        // Add the text, center it and set its value.
        TextMeshProUGUI textMeshProUgui = t.AddComponent<TextMeshProUGUI>();
        textMeshProUgui.text = text;
        textMeshProUgui.fontSize = 15f; // Font size
        textMeshProUgui.color = Color.black;
        textMeshProUgui.alignment = TextAlignmentOptions.Center;
        textMeshProUgui.GetComponent<RectTransform>().SetTop(42); // Text location (bottom)
        t.transform.SetParent(b.transform, false);

        // Aligns text on center
        RectTransform rect = t.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);

        return b;
    }
}