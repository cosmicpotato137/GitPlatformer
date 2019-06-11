using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    public int index;

    Animator animator;

    public UnityEvent OnSelectedEvent;
    public UnityEvent OnToggleEvent;

    void Start()
    {
        if (OnSelectedEvent == null)
            OnSelectedEvent = new UnityEvent();
        if (OnToggleEvent == null)
            OnToggleEvent = new UnityEvent();

        animator = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Update()
    {

        if (index == menuButtonController.buttonIndex)
        {
            animator.SetBool("Hover", true);
        }
        else
        {
            animator.SetBool("Hover", false);
        }
    }

    public void Select()
    {
        OnSelectedEvent.Invoke();
    }

    public void Toggle()
    {
        OnToggleEvent.Invoke();
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadWait(name));
    }

    private IEnumerator LoadWait(string name)
    {
        yield return new WaitForSeconds(.2f);

        Debug.Log("Loading Scene:" + name);
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        StartCoroutine(WaitQuit());
    }

    private IEnumerator WaitQuit()
    {
        yield return new WaitForSeconds(.2f);
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
