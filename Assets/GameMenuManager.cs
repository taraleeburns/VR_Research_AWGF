using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menu; 
    public InputActionProperty showButton;
    public Transform head; 
    public CanvasGroup canvasGroup;

public float startFadeTime = 1f;
public float toggleFadeTime = 0.2f;
public float hideDistance = 10f;

bool isVisible = false;

    // Start is called before the first frame update
 IEnumerator Start()
{
    menu.SetActive(true);

    canvasGroup.alpha = 0;

    yield return new WaitForSeconds(1f);

    PositionMenu();

    yield return StartCoroutine(FadeMenu(1, startFadeTime));

    isVisible = true;
} void PositionMenu()
{
    if (menu == null)
    {
        Debug.LogError("Menu is NOT assigned in GameMenuManager!");
        return;
    }

    if (head == null)
    {
        Debug.LogError("Head is NOT assigned in GameMenuManager!");
        return;
    }

    menu.transform.position = head.position + head.forward * 3f;
    menu.transform.rotation = Quaternion.LookRotation(head.forward);
}
    // Update is called once per frame
    void Update()
{
    if (isVisible)
{
    float distance = Vector3.Distance(head.position, menu.transform.position);

    if (distance > hideDistance)
    {
        StartCoroutine(FadeMenu(0, toggleFadeTime));
        isVisible = false;
    }
}if (showButton.action.WasPressedThisFrame())
    {
        if (isVisible)
        {
            StartCoroutine(FadeMenu(0, toggleFadeTime));
            isVisible = false;
        }
        else
        {
            PositionMenu();
            menu.SetActive(true);

            StartCoroutine(FadeMenu(1, toggleFadeTime));
            isVisible = true;
        }
    }
}

IEnumerator FadeMenu(float targetAlpha, float duration)
{
    float startAlpha = canvasGroup.alpha;
    float time = 0;

    while (time < duration)
    {
        time += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
        yield return null;
    }

    canvasGroup.alpha = targetAlpha;

    if (targetAlpha == 0)
    {
        menu.SetActive(false);
    }
}
}