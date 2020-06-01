using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        ShowCursor();
    }

    public void PlayAgain()
    {
        StartCoroutine(LoadLevel(1));
        HideCursor();
    }

    public void ReturnToMain()
    {
        StartCoroutine(LoadLevel(0));
        ShowCursor();
    }

    public void Exit()
    {
        StartCoroutine(ApplicationExit());
    }

    public void EnterHelp()
    {
        StartCoroutine(LoadLevel(3));
        ShowCursor();
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    private IEnumerator ApplicationExit()
    {
        yield return new WaitForSeconds(.25f);
        Application.Quit();
    }
}
