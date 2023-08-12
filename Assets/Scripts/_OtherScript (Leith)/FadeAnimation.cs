// Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using System.Collections;

public class FadeAnimation : MonoBehaviour
{
    public float fadeSpeed = 0.5f;

    private Renderer renderer;
    private Color currentColor, targetColor;

    void Start ()
    {
        renderer = GetComponent<Renderer>();
        currentColor = renderer.material.color;
        targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.0f);
    }

    public IEnumerator FadeOut()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * fadeSpeed;
            renderer.material.color = Color.Lerp(currentColor, targetColor, t);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}