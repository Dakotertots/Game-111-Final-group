using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

public class UIManager : MonoBehaviour
{
    private Button restartButton,exitButton,escapeButton;
    private VisualElement goalPrompt,screenFade;
    private Label goalMessage;

    public static Action FadeOutComplete = delegate { };
    public static Action FadeInComplete = delegate { };
    private bool bFadeInComplete, bFadeOutComplete,bResetSequence;
    private PlayerInput pInput;

    private void Awake()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        pInput.Player.Escape.performed += ShowMenu;
    }
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        restartButton = root.Q<Button>("Restart-Button");
        exitButton = root.Q<Button>("Exit-Button");
        escapeButton = root.Q<Button>("Escape-Button");
        goalPrompt = root.Q<VisualElement>("GoalPrompt");
        goalMessage = root.Q<Label>("GoalSign");
        screenFade = root.Q<VisualElement>("screenFade");

        restartButton.SetEnabled(true);
        exitButton.SetEnabled(true);
        escapeButton.SetEnabled(true);
        restartButton.clicked += Restart;
        exitButton.clicked += Exit;
        escapeButton.clicked += Return;
        WinGA.Goal += Goal;
    }
    private void OnDisable()
    {
        WinGA.Goal += Goal;
        restartButton.clicked -= Restart;
        exitButton.clicked -= Exit;
        escapeButton.clicked -= Return;
        pInput.Disable();
        pInput.Player.Escape.performed -= ShowMenu;
    }
    private void Restart()
    {
        StartCoroutine(nameof(ResetSequence));        
    }
    private void Exit()
    {
        Application.Quit();
    }
    private void Return()
    {
        screenFade.style.display = DisplayStyle.None;
        CuboidMaster.EnablePlayerMovement();
    }
    private void ShowMenu(InputAction.CallbackContext c)
    {
        if (screenFade.style.display != DisplayStyle.Flex)
        {
            screenFade.style.opacity = 1;
            screenFade.style.backgroundColor = Color.clear;
            screenFade.style.display = DisplayStyle.Flex;
            goalPrompt.style.display = DisplayStyle.Flex;
            escapeButton.style.display = DisplayStyle.Flex;
            CuboidMaster.DisablePlayerMovement();
        }
        else
        {
            screenFade.style.display = DisplayStyle.None;
            CuboidMaster.EnablePlayerMovement();
        }
    }
    private void Goal()
    {
        screenFade.style.opacity = 1;
        screenFade.style.backgroundColor = Color.clear;
        screenFade.style.display = DisplayStyle.Flex;
        goalPrompt.style.display = DisplayStyle.Flex;
        goalMessage.style.display = DisplayStyle.Flex;
    }
    IEnumerator FadeIn()
    {
        bFadeInComplete = false;
        screenFade.style.opacity = 1;
        screenFade.style.backgroundColor = Color.black;
        screenFade.style.display = DisplayStyle.Flex;

        float fadeRate = 0;
        while (fadeRate < 1)
        {
            fadeRate += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
            screenFade.style.opacity = Mathf.Lerp(1,0,fadeRate);
        }
        screenFade.style.display = DisplayStyle.None;
        FadeInComplete();
        bFadeInComplete = true;
    }
    IEnumerator FadeOut()
    {
        bFadeOutComplete = false;
        screenFade.style.opacity = 0;
        screenFade.style.backgroundColor = Color.black;
        goalPrompt.style.display = DisplayStyle.None;
        goalMessage.style.display = DisplayStyle.None;
        screenFade.style.display = DisplayStyle.Flex;
        float fadeRate = 0;

        while (fadeRate < 1)
        {
            fadeRate += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
            screenFade.style.opacity = Mathf.Lerp(0,1, fadeRate);
        }
        FadeOutComplete();
        bFadeOutComplete = true;
    }
    IEnumerator ResetSequence()
    {        
        StartCoroutine(nameof(FadeOut));        
        while (!bFadeOutComplete)
            yield return new WaitForEndOfFrame();  
        CuboidMaster.ResetGame();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(nameof(FadeIn));
        while (!bFadeInComplete)
            yield return new WaitForEndOfFrame();
    }
}
