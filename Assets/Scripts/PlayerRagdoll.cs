using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRagdoll : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private Harpoon _harpoon;
	[SerializeField] private Image _damageIndicator;
	[SerializeField] private UnityEvent _onDie;
	[SerializeField] private UnityEvent _onRise;
	private Rigidbody _rb;
	private RigidbodyConstraints _constrains;
	private bool _ready = true;
	private Collider _collider;
	private PhysicMaterial _rbMaterial;
	private Quaternion _startRotation;
	private SpringJoint _joint;
	private float _hp = 1f;

	public bool Ready => _ready;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_constrains = _rb.constraints;
		_collider = GetComponent<Collider>();
		_joint = GetComponent<SpringJoint>();
		_rbMaterial = _collider.material;
		_startRotation = transform.rotation;
	}

	private void Update()
	{
		if(_hp < 1 && _ready)
		{
			_damageIndicator.color = new Color(1,1,1, 1 - _hp);
			_hp += Time.deltaTime / 4;
			if(_hp > 1)
			{
				_hp = 1;
				_damageIndicator.color = new Color(1, 1, 1, 0);
			}
		}
	}

	public void TakeDamage(float damage)
	{
		if (!_ready)
			return;

		_hp -= damage;
		if(_hp <= 0)
			TryDeath(Vector3.zero, 1);
		else
			DJ.Play("hurt");
	}

	public void TryDeath(Vector3 pushForce, float deathTime)
	{
		if (!_ready)
			return;
		_ready = false;

		StartCoroutine(DeathRoutine(pushForce, deathTime));
	}

	private IEnumerator DeathRoutine(Vector3 pushForce, float deathTime)
	{
		if (pushForce != Vector3.zero)
			Drop(pushForce);
		else
			Drop();
		yield return new WaitForSeconds(deathTime);

		Rise();
	}

	public void Drop()
	{
		_player.enabled = false;
		_rb.constraints = RigidbodyConstraints.None;
		_collider.material = null;
		_rb.AddForceAtPosition(	new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
			transform.position +new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));

		_onDie.Invoke();
	}


	public void Drop(Vector3 pushForce)
	{
		Drop();
		_rb.AddForce(pushForce, ForceMode.VelocityChange);
	}

	public void Rise()
	{
		_hp = 1;
		_damageIndicator.color = new Color(1, 1, 1, 0);
		_rb.isKinematic = true;
		Destroy(_joint);

		StartCoroutine(TeleportToSpawnRoutine());
	}

	public IEnumerator TeleportToSpawnRoutine()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime*2);

		_collider.material = _rbMaterial;
		_rb.isKinematic = false;
		_rb.constraints = _constrains;
		_rb.velocity = Vector3.zero;
		transform.rotation = _startRotation;
		transform.position = LevelManager.SpawnPoint.position;

		_joint = gameObject.AddComponent<SpringJoint>();
		_joint.spring = 0;
		_joint.autoConfigureConnectedAnchor = false;

		if (_harpoon != null)
			_harpoon.Joint = _joint;

		_player.enabled = true;
		_player.ResetRotation();
		_ready = true;

		_onRise.Invoke();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
