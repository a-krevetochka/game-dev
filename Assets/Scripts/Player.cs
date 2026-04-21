using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	[Header("Look")]
	[SerializeField] private Transform _headTransform;
	[SerializeField] private float _horizontalLookSensitivity;
	[SerializeField] private float _verticalLookSensitivity;
	private float _rotationY = 0;
	private float _rotationX = 0;

	[Header("Move")]
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _airSpeedScale;
	[SerializeField] private LayerMask _groundLayer;
	private Vector3 _moveVector = Vector3.zero;
	
	[Header("Jump")]
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private Vector3 _groundSensorOffset;
	[SerializeField] private float _groundSensorRadius;
	[SerializeField] private float _jumpCooldown;
	private float _lastJumpTime = 0;

	private Rigidbody _rb;
	private bool _enableWalk = true;
	[SerializeField] private Vector3 _startRotation; //crutch

	public bool EnableWalk { get => _enableWalk; set { _enableWalk = value; } }
	public bool IsGrounded => Grounded();

	private void Start()
	{
		_rotationY = _startRotation.x;
		_rotationX = _startRotation.y;
		_rb = GetComponent<Rigidbody>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Input.ResetInputAxes();
	}

	public void ReturnHarpoon()
	{
		_moveVector = Vector3.zero;
		_moveVector += transform.forward * Input.GetAxis("Vertical") * _moveSpeed;
		_moveVector += transform.right * Input.GetAxis("Horizontal") * _moveSpeed;

		Vector3 velocity = _rb.velocity;

		if (_moveVector.y < 0 && velocity.y > _moveVector.y)
			velocity.y = _moveVector.y;
		if (_moveVector.y > 0 && velocity.y < _moveVector.y)
			velocity.y = _moveVector.y;

		if (_moveVector.x < 0 && velocity.x > _moveVector.x)
			velocity.x = _moveVector.x;
		if (_moveVector.x > 0 && velocity.x < _moveVector.x)
			velocity.x = _moveVector.x;

		if (_moveVector.z < 0 && velocity.z > _moveVector.z)
			velocity.z = _moveVector.z;
		if (_moveVector.z > 0 && velocity.z < _moveVector.z)
			velocity.z = _moveVector.z;

		_rb.velocity = velocity;
	}

	public void ResetRotation()
	{
		_rotationY = _startRotation.y;
		_rotationX = _startRotation.x;
	}

	void Update()
	{
		_rotationY += Input.GetAxisRaw("Mouse Y") * _verticalLookSensitivity;
		_rotationY = Mathf.Clamp(_rotationY, -88, 88);

		_rotationX += Input.GetAxisRaw("Mouse X") * _horizontalLookSensitivity;
		transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
		_headTransform.localRotation = Quaternion.AngleAxis(_rotationY, Vector3.left);

		if (Input.GetKey(KeyCode.Space))
			TryJump();
	}

	private void TryJump()
	{
		if (Time.time - _lastJumpTime < _jumpCooldown || !IsGrounded)
			return;

		_lastJumpTime = Time.time;

		_moveVector.y = _jumpSpeed;
		_rb.velocity = _moveVector;
	}

	private bool Grounded()
	{
		Vector3 offset = transform.rotation * _groundSensorOffset;
		return Physics.OverlapSphere(transform.position + offset, _groundSensorRadius, _groundLayer).Length > 0;
	}

	private void FixedUpdate()
	{
		if (!IsGrounded)
		{
			_moveVector = Vector3.zero;
			_moveVector += transform.forward * Input.GetAxis("Vertical") * _moveSpeed * _airSpeedScale;
			_moveVector += transform.right * Input.GetAxis("Horizontal") * _moveSpeed * _airSpeedScale;

			_rb.AddForce(_moveVector, ForceMode.Acceleration);
			return;
		}

		_moveVector = Vector3.zero;
		_moveVector += transform.forward * Input.GetAxis("Vertical") * _moveSpeed;
		_moveVector += transform.right * Input.GetAxis("Horizontal") * _moveSpeed;
		_moveVector.y = _rb.velocity.y;

		_rb.velocity = _moveVector;
		_rb.angularVelocity = Vector3.zero;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0,0,0.45f, 0.45f);
		Vector3 offset = transform.rotation * _groundSensorOffset;
		Gizmos.DrawSphere(transform.position + offset, _groundSensorRadius);
	}
}
