using _Scripts._Input;
using _Scripts._Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour, IButton
{
    public event Action Blue, Red, Yellow, Green;
    private Color originalColor, targetColor;
    private readonly Dictionary<string, (Action listener, Action silencer, Action action)> objectActions = new();
    private SpriteRenderer spriteRenderer;
    private readonly float transitionDuration = 0.15f;

    public void ButtonPressed()
    {
        StartCoroutine(AnimateColor());
        objectActions[gameObject.name].action();
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        targetColor = originalColor + originalColor;

        objectActions.Add("Blue", (
            () => InputHandler.UpArrowBtnAction += ButtonPressed,
            () => InputHandler.UpArrowBtnAction -= ButtonPressed, 
            () => Blue?.Invoke()
        ));
        objectActions.Add("Red", (
            () => InputHandler.RightArrowBtnAction += ButtonPressed,
            () => InputHandler.RightArrowBtnAction -= ButtonPressed,
            () => Red?.Invoke()
        ));
        objectActions.Add("Yellow", (
            () => InputHandler.DownArrowBtnAction += ButtonPressed,
            () => InputHandler.DownArrowBtnAction -= ButtonPressed,
            () => Yellow?.Invoke()
        ));
        objectActions.Add("Green", (
            () => InputHandler.LeftArrowBtnAction += ButtonPressed,
            () => InputHandler.LeftArrowBtnAction -= ButtonPressed,
            () => Green?.Invoke()
        ));
    }

    private void OnEnable()
    {
        StartCoroutine(AnimateColor());
        objectActions[gameObject.name].listener();
    }

    private IEnumerator AnimateColor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    private void OnDisable()
    {
        objectActions[gameObject.name].silencer();
    }
}
