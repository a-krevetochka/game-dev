using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoLethalTrap : MonoBehaviour
{
	[SerializeField, Range(0,1)] private float _damage;
	[SerializeField] private float _cooldown;

	private PlayerRagdoll _player = null;
	private bool _work = false;

	private void DoDamage()
	{
		if(_work)
			_player.TakeDamage(_damage);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<PlayerRagdoll>(out PlayerRagdoll player))
		{
			_work = true;
			_player = player;
			InvokeRepeating(nameof(DoDamage), 0, _cooldown);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<PlayerRagdoll>(out PlayerRagdoll player))
		{
			_work = false;
			CancelInvoke();
		}
	}

	private void OnDisable()
	{
		CancelInvoke();
	}
}
