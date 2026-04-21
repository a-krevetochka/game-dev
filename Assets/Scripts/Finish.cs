using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
	[SerializeField] private string _nextSceneName;

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent<Player>(out Player player))
		{
			if(_nextSceneName != "")
				SceneManager.LoadScene(_nextSceneName);
			else
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
