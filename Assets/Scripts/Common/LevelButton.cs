using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
	[SerializeField] private Button _button;
	[SerializeField] private Image _image;
	[SerializeField] private Sprite _icon;
	[SerializeField] private TextMeshProUGUI _text;

	public void SetEnabled()
	{
		_button.interactable = true;
		_image.color = Color.white;
		_image.sprite = _icon;
		_text.gameObject.SetActive(false);
	}
}
