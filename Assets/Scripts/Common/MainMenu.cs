using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private LevelButton[] _buttons;

	private void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		int level = PlayerPrefs.GetInt("level", 0);

		for (int i = 0; i < level || i < _buttons.Length; i++)
			if(i <= level)
				_buttons[i].SetEnabled();
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
