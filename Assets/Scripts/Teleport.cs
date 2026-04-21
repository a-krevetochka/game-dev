using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	[SerializeField] private Transform _teleportPoint;

	private void OnTriggerEnter(Collider other)
	{
		other.transform.position = _teleportPoint.position;
	}
}