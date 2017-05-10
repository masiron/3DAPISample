using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour 
{
	[SerializeField] GameObject bulletPrefabs;
	[SerializeField] GameObject muzzle;
	[SerializeField] float shotSpeed;
	private Vector3 muzzlePos;
	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	private Vector3 torque;
	private float motor;
	private float steering;
	private Vector3 wheelPos;
	private Quaternion rotate;

	public Rigidbody cannonBase;

	public void ApplyWheelPosition (WheelCollider collider)
	{
		Transform wheel = collider.transform.GetChild (0);


		collider.GetWorldPose (out wheelPos, out rotate);
		wheel.transform.position = wheelPos;
		wheel.transform.rotation = rotate * Quaternion.Euler(0f, 0f, 90f);
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.T))
		{
			motor = maxMotorTorque * 1.0f;
		}
		else if (Input.GetKey (KeyCode.G)) 
		{
			motor = maxMotorTorque * -1.0f;
		}
		else
		{
			motor = 0.0f;
		}

		if (Input.GetKey (KeyCode.F))
		{
			steering = maxSteeringAngle * -1.0f;
		} 
		else if (Input.GetKey (KeyCode.H)) 
		{
			steering = maxSteeringAngle * 1.0f;
		}
		else
		{
			steering = 0.0f;
		}

		if (Input.GetButtonDown ("Jump")) 
		{
			muzzlePos = muzzle.transform.position;
			GameObject Bullet = (GameObject)Instantiate (bulletPrefabs, muzzlePos, Quaternion.identity);
			Rigidbody BulletRigid = Bullet.GetComponent<Rigidbody> ();
			BulletRigid.AddForce (muzzlePos * shotSpeed);
		}
			
		float cannonX = Input.GetAxis ("Vertical");
		float cannonY = Input.GetAxis ("Horizontal");

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyWheelPosition (axleInfo.leftWheel);
			ApplyWheelPosition (axleInfo.rightWheel);
		}

		torque = new Vector3 (cannonX * 2.0f, cannonY * 2.0f, 0);
	}

	void FixedUpdate() 
	{
		cannonBase.AddTorque (torque);
	}

}
[System.Serializable]
public class AxleInfo 
{
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}