using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Visuals
{
    
    public class Selecteable : EnableableMonoBehaviour
    {
        public UnityEvent onMouseEnter = new UnityEvent();
        public UnityEvent onMouseExit = new UnityEvent();

        public UnityEvent onSelection = new UnityEvent();
        public UnityEvent onDeselection = new UnityEvent();

        //

        private Renderer renderer;
        private Color startcolor;

        public Color selectionColor = Color.white;
        public Color hoverColor = Color.yellow;

        public bool isSelected;

        public void Start()
        {
            renderer = gameObject.GetComponent<Renderer>();
            startcolor = renderer.material.color;
        }

        void OnMouseEnter()
        {
            
            
            if (!IsEnabled) return;
            if (isSelected) return;
            renderer.material.color = hoverColor;

            onMouseEnter.Invoke();
        }

        void OnMouseExit()
        {
            if (!IsEnabled) return;
            if (isSelected) return;
            renderer.material.color = startcolor;

            onMouseExit.Invoke();
        }

        // Invokes the events and changes the color of the object on selection.
        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;

            if (isSelected)
            {
                renderer.material.color = selectionColor;
                onSelection.Invoke();
            }
            else
            {
                renderer.material.color = startcolor;
                onDeselection.Invoke();
            }
        }
    }
}