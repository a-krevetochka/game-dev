using UnityEngine;
using UnityEngine.Events;

public class UniversalTrigger : MonoBehaviour
{
	[SerializeField] private UnityEvent _action;
	[SerializeField] private bool _oneUse;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			_action.Invoke();
			if(_oneUse)
				Destroy(gameObject);
		}
	}
}
