using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private Transform _harp;
	[SerializeField] private LineRenderer _line;
	[SerializeField] private Rigidbody _playerRb;
	[SerializeField] private SpringJoint _joint;
	[SerializeField] private KeyCode _getClosestKey;
	[SerializeField] private float _getClosestSpeed;
	private Camera _cam;
	private bool _onWork = false;
	private Transform _harpAnchor;

	public SpringJoint Joint
	{
		set { _joint = value; }
	}

	private void Start()
	{
		_cam = Camera.main;
		_harpAnchor = new GameObject("HarpAnchor").transform;
		_harpAnchor.SetParent(_cam.transform);
		_harpAnchor.position = _harp.position;
		_harpAnchor.rotation = _harp.rotation;
		_line.enabled = false;
	}

	void Update()
    {
        if (Input.GetMouseButton(0))
            TryShoot();

        if (Input.GetMouseButtonUp(0))
			FreeHarp();

		if (Input.GetKey(_getClosestKey))
			GetClosest();


		if (_onWork)
		{
			_line.SetPosition(0, _harpAnchor.position);
			_line.SetPosition(1, _harp.position);
		}
	}

	private void GetClosest()
	{
		float distance = _joint.maxDistance -= _getClosestSpeed * Time.deltaTime;

		if(distance > 1)
			_joint.maxDistance = distance;
	}

	public void FreeHarp()
	{
		if (!_onWork)
			return;

		_onWork = true;
		_harp.SetParent(null);

		StopAllCoroutines();
		StartCoroutine(FlyBack());
	}


	private void TryShoot()
	{
		if (_onWork)
			return;

		Ray ray = _cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			_onWork = true;
			_line.enabled = true;
			_harp.SetParent(null);

			StopAllCoroutines();
			StartCoroutine(FlyForward(hit.point));
		}

		DJ.Play("wjooh");
	}

	private IEnumerator FlyForward(Vector3 point)
	{
		Vector3 startPosition = _harp.position;
		for (float t = 0; t < 1; t+= Time.deltaTime * 4)
		{
			_harp.position = Vector3.Lerp(startPosition, point, t);
			yield return null;
		}
		_player.EnableWalk = false;

		_joint.connectedAnchor = point;
		_joint.spring = 500f;
		_joint.damper = 0f;

		float distance = (point - _playerRb.position).magnitude;
		_joint.maxDistance = distance;
	}

	private IEnumerator FlyBack()
	{
		if(_joint != null)
			_joint.spring = 0;
		_player.EnableWalk = true;
		_player.ReturnHarpoon();

		Vector3 startPosition = _harp.position;
		Quaternion startRotation = _harp.rotation;
		for (float t = 0; t < 1; t+= Time.deltaTime * 4)
		{
			_harp.position = Vector3.Lerp(startPosition, _harpAnchor.position, t);
			_harp.rotation = Quaternion.Lerp(startRotation, _harpAnchor.rotation, t);
			yield return null;
		}

		_harp.position = _harpAnchor.position;
		_harp.rotation = _harpAnchor.rotation;

		_line.enabled = false;
		_harp.SetParent(_cam.transform);
		_onWork = false;
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
