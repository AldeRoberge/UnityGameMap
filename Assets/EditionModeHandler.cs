using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using World.Map;

public enum EditType
{
    Paths,
    Objects
}

public class EditionModeHandler : Singleton<EditionModeHandler>
{
    public Button editionModeToggle;

    public EditType editing = EditType.Objects;

    public Sprite objectMode;
    public Sprite pathMode;

    // Start is called before the first frame update
    void Start()
    {
        if (editionModeToggle == null)
        {
            Debug.Log("Error : EditionModeToggle button is null. Make sure it set it in the EditionModeHandler script.");
            return;
        }

        objectMode = Resources.Load<Sprite>("Sprites/UI/Button/EditModeToggle/Object");
        pathMode = Resources.Load<Sprite>("Sprites/UI/Button/EditModeToggle/Path");

        editionModeToggle.onClick.AddListener(() =>
        {
            SwitchEditingType();
            UpdateEditingType();
        });

        
    }

    public void UpdateEditingType()
    {
        switch (editing)
        {
            case EditType.Objects:
                GameMap.Instance.input.interactionMap.SetGridIsVisible(false);
                GameMap.Instance.input.interactionMap.UnselectTile();

                editionModeToggle.GetComponent<Image>().sprite = objectMode;
                break;
            case EditType.Paths:
                GameMap.Instance.input.interactionMap.SetGridIsVisible(true);
                GameMap.Instance.input.UnselectObjects();

                editionModeToggle.GetComponent<Image>().sprite = pathMode;
                break;
            default:
                Debug.Log("Unknown editing type '" + editing + "'.");
                return;
        }
    }

    private void SwitchEditingType()
    {
        switch (editing)
        {
            case EditType.Paths:
                editing = EditType.Objects;
                break;
            case EditType.Objects:
                editing = EditType.Paths;
                break;
            default:
                Debug.Log("Unknown editing type : '" + editing + "'.");
                return;
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle Grid
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Showing grid...");
            editionModeToggle.onClick.Invoke();
        }
    }
}