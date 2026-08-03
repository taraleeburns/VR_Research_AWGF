using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeAndLoadScene : MonoBehaviour
{
    [Header("References")]
    public FadeScreen fadeScreen;

    [Header("Scene To Load")]
    public string sceneName;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    public void StartFadeAndLoad()
    {
        StartCoroutine(FadeThenLoad());
    }

    private IEnumerator FadeThenLoad()
    {
        fadeScreen.SetFadeDuration(fadeDuration);
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(sceneName);
    }
}