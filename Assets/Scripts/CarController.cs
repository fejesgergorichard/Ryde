using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[Serializable]
public class Dot_Truck
{
	public WheelCollider leftWheel;
	public GameObject leftWheelMesh;
	public WheelCollider rightWheel;
	public GameObject rightWheelMesh;
	public bool motor;
	public bool steering;
	public bool reverseTurn; 
}

public class CarController : MonoBehaviour {
	private Rigidbody rb;
	private float mass;
	private DateTime flipStartTime;
	private ParticleSystem[] particleSystems;
	private bool tapRight;
	private bool tapLeft;
	private Vector3 initialPosition;
	private Quaternion initialRotation;


	[Range(0,1000)]
	public int FlipTimeInMs;
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public List<Dot_Truck> truck_Infos;

	public Vector3 CenterOfMass;
	public float radius = 5f;
	[Range(0,50)]
	public int Smokiness = 3;
	[Range(0.01f, 0.5f)]
	public float lightSwitchSpeed = 0.1f;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = CenterOfMass;
		mass = rb.mass;
		flipStartTime = DateTime.Now;
		particleSystems = GetComponentsInChildren<ParticleSystem>();
		initialPosition = transform.position;
		initialRotation = transform.rotation;

	}

	public void VisualizeWheel(Dot_Truck wheelPair)
    {
        Quaternion rot;
        Vector3 pos;
        wheelPair.leftWheel.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;

        wheelPair.rightWheel.GetWorldPose(out pos, out rot);

        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = FlipHorizontally(rot);
    }

	

    public void Update()
	{
		tapLeft = tapRight = false;

		if (Input.touches.Length != 0)
		{
			if (Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Stationary) //"touches[0]" means first finger to touch
			{
				if (Input.touches[0].position.x < Screen.width / 2)
                {
					tapLeft = true;
                }
				else
				{
					tapRight = true;
				}
			}
		}

		float motor = 0;
		if (tapLeft)
			motor = -maxMotorTorque;
		else if (tapRight)
			motor = maxMotorTorque;

		float steering = Math.Min(maxSteeringAngle, Math.Max(-maxSteeringAngle, DeviceTiltAngle.Get() / 1.5f));

        float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));

		if (brakeTorque > 0.001) {
			brakeTorque = maxMotorTorque;
			motor = 0;
		} else {
			brakeTorque = 0;
		}

		foreach (Dot_Truck truck_Info in truck_Infos)
		{
			if (truck_Info.steering == true) {
				truck_Info.leftWheel.steerAngle = truck_Info.rightWheel.steerAngle = ((truck_Info.reverseTurn) ? -1 : 1) * steering;
			}

			if (truck_Info.motor == true)
			{
				truck_Info.leftWheel.motorTorque = motor;
				truck_Info.rightWheel.motorTorque = motor;
			}

			truck_Info.leftWheel.brakeTorque = brakeTorque;
			truck_Info.rightWheel.brakeTorque = brakeTorque;

			VisualizeWheel(truck_Info);
		}

		// pedal to the metal
		if (Input.GetAxis("Vertical") > 0)
		{
			SetTaillightColor(Color.white);
			SetMotorSmokeEmission(Input.GetAxis("Vertical") * 30 * Smokiness);
		}
		// braking
		else if (Input.GetAxis("Vertical") < 0 || brakeTorque > 0)
        {
			SetTaillightColor(Color.red);
			SetMotorSmokeEmission(2 * Smokiness);
		}
		// idle state
        else
        {
			SetTaillightColor(Color.white);
			SetMotorSmokeEmission(10 * Smokiness);
		}

		if (Input.GetKey(KeyCode.R))
        {
            Flip();
        }
    }

	public void Restart()
    {
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		rb.velocity = Vector3.zero;
	}

	private void SetTaillightColor(Color color)
    {
		//Material myMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Brakelight.mat", typeof(Material));

		//Color currentAlbedo = myMaterial.GetColor("_Color");
		//Color currentEmission = myMaterial.GetColor("_EmissionColor");

		//if (!color.Equals(currentAlbedo))
		//{
		//	myMaterial.SetColor("_Color", Color.Lerp(currentAlbedo, color, lightSwitchSpeed));
		//	myMaterial.SetColor("_EmissionColor", Color.Lerp(currentEmission, color, lightSwitchSpeed));
		//}
	}


	/// <summary>
	/// Flips the given quaternion parameter horizontally (Z rotation + 180°)
	/// </summary>
	/// <param name="rot">rotation Quaternion to be flipped</param>
	/// <returns>Flipped rotation Quaternion</returns>
	private Quaternion FlipHorizontally(Quaternion rot)
	{
		Vector3 rotEuler = rot.eulerAngles;
		rotEuler = new Vector3(rotEuler.x, rotEuler.y, rotEuler.z + 180);

		return Quaternion.Euler(rotEuler);
	}


	private void Flip()
    {
        // the player is not flipping and the flip time passed
        if (DateTime.Compare(flipStartTime.AddMilliseconds(FlipTimeInMs), DateTime.Now) < 0)
        {
            flipStartTime = DateTime.Now;
            rb.AddForce(new Vector3(0, 5 * mass, 0), ForceMode.Impulse);
            rb.AddRelativeTorque(new Vector3(0, 0, 3 * mass), ForceMode.Impulse);

			// Play the explosion particles
            foreach (var ps in particleSystems)
            {
                if (ps.name == "Light" || ps.name == "Spark" || ps.name == "Smoke")
                {
                    ps.Play();
                }
            }
        }
    }


    private void SetMotorSmokeEmission(float val)
    {
        foreach (var ps in particleSystems)
        {
            if (ps.name == "MotorSmoke")
            {
                var e = ps.emission;
                e.enabled = true;
                e.rateOverTime = val;
            }
        }
    }

    /// Gizmo creation for visualizing the center of mass
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position + transform.rotation * rb.centerOfMass, radius);
    //}
}