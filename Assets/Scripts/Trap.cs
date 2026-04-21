using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{
	[SerializeField] private float _deathTime;
	[SerializeField] private float _pushForce;

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent<PlayerRagdoll>(out PlayerRagdoll player))
		{
			if (player.Ready)
				if (_pushForce != 0)
					player.TryDeath((player.transform.position - transform.position).normalized * _pushForce, _deathTime);
				else
					player.TryDeath(Vector3.zero, _deathTime);
		}
	}
}
