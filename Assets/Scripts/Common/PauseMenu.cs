using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private MonoBehaviour[] turnOffComponents;
    private bool _on = false;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            _on = !_on;

            SetPause(_on);
        }
    }

    private void SetPause(bool flag)
    {
        Cursor.visible = flag;
        ui.SetActive(flag);

        foreach (MonoBehaviour monoBeh in turnOffComponents)
            monoBeh.enabled = !flag;

        if (flag)
		{
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
		else
		{
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
	}

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
