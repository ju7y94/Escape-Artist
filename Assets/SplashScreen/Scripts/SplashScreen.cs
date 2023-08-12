using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{
    [Header ("Skip Splash:")]
    [SerializeField] private string string_SkipButton; //What is the button to skip the SplashScreen    
    [SerializeField] float float_WaitBeforePrompt;
    [SerializeField] float float_UIPrompt = 10; //How long to show the prompt to skip SplashScreen
    [SerializeField] GameObject gameobject_SkipPrompt; //UI element that prompt user to skip SplashScreen
    [Header ("SplashScreen:")]
    [SerializeField] float float_SplashDuration; //Duration of the SplashScreen
    [SerializeField] private string string_NextScene; //Scene to load after SplashScreen finishes
    [SerializeField] float float_ExtraDuration; //How much extra time between SplashScreen and loading next scene
    void Start()
    {
        StartCoroutine(Splash());
        StartCoroutine(SkipBtn());
        PromptLogic();
    }
    private void PromptLogic()
    {
        if (float_UIPrompt > 0)
        {    
            gameobject_SkipPrompt.SetActive(false);
        }
        
        if (float_WaitBeforePrompt > 0)
        {
            gameobject_SkipPrompt.SetActive(false);
            StartCoroutine(TimedPrompt());
        }
        else
        {
            gameobject_SkipPrompt.SetActive(true);
        }
    }
    IEnumerator TimedPrompt()
    {
        yield return new WaitForSecondsRealtime(float_WaitBeforePrompt);
        gameobject_SkipPrompt.SetActive(true);
        StartCoroutine(SkipBtn());
    }
    private void Update()
    {
        if (Input.GetKeyDown(string_SkipButton) && float_UIPrompt > 0)
        {
            SceneManager.LoadScene(string_NextScene);
        }
    }
    IEnumerator SkipBtn()
    {
        yield return new WaitForSecondsRealtime(float_UIPrompt);
        gameobject_SkipPrompt.SetActive(false);
    }

    IEnumerator Splash()
    {
        yield return new WaitForSecondsRealtime(float_SplashDuration + float_ExtraDuration);
        SceneManager.LoadScene(string_NextScene);
    }
}
