using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour {
	
	//Boidの数
	public int NumBoids = 100;

	public GameObject boid;

	public GameObject centerBoid;

	public GameObject bossBoid;

	public GameObject[] boids;

	public float Distance = 10f;

	public float Radius = 10f;

	void CreateBoidsObject() {
		for (int i = 0; i < NumBoids; i++) {
			boids[i] = Instantiate(
				boid,
				new Vector3(
					Random.Range(-100f, 100f),
					Random.Range(-100f, 100f),
					Random.Range(-100f, 100f)
				),
				Quaternion.identity
			);
			boids[i].GetComponent<Rigidbody>().velocity = (
				new Vector3(
					Random.Range(-1f, 1f),
					Random.Range(-1f, 1f),
					Random.Range(-1f, 1f)
				)
			);
		}
	}

	//結合
	void Cohesion (int index) {
		Vector3 center = Vector3.zero;
		for (int i = 0; i < NumBoids; i++) {
			if(i != index) {
				float dist = Vector3.Distance(boids[i].transform.position, boids[index].transform.position);
				center += boids[i].transform.position;
			}
		}
		center /= NumBoids - 1;
		boids[index].GetComponent<Rigidbody>().velocity += (center - boids[index].transform.position) * 0.1f;
	}

	//分離
	void Separation(int index) {
		for (int i = 0; i < NumBoids; i++) {
			if (i != index) {
				float dist = Vector3.Distance(
					boids[i].transform.position,
					boids[index].transform.position
				);
				if(dist < Distance) {
					boids[index].GetComponent<Rigidbody>().velocity -= (boids[i].transform.position - boids[index].transform.position) * 0.8f;
				}
			}
		}
	}

	//整列
	void Alignment (int index) {
		Vector3 averageVelocity = Vector3.zero;
		for (int i = 0; i < NumBoids; i++) {
			if(i != index) {
				float dist = Vector3.Distance(boids[i].transform.position, boids[index].transform.position);
				averageVelocity += boids[i].GetComponent<Rigidbody>().velocity;
			}
		}
		averageVelocity /= NumBoids - 2;
		Rigidbody rigibody = boids[index].GetComponent<Rigidbody>();
		rigibody.velocity += (averageVelocity - rigibody.velocity) * 0.01f;
	}

	


	// Use this for initialization
	void Start () {
		centerBoid.transform.position = Vector3.zero;
		bossBoid.transform.position = Vector3.zero;
		boids = new GameObject[NumBoids];
		CreateBoidsObject();
	}
	
	// Update is called once per frame
	void Update () {
		bossBoid.transform.position = new Vector3(
			Mathf.Cos(Time.time * 10f) * Radius,
			Mathf.Sin(Time.time * 10f) * Radius,
			Mathf.Cos(Time.time * 10f) * Radius
		);
		for (int i = 0; i < NumBoids; i++) {
			Cohesion(i);
			Separation(i);
			Alignment(i);
			float dist = Vector3.Distance(Vector3.zero, boids[i].transform.position);
			if(dist > 500) {
				boids[i].GetComponent<Rigidbody>().velocity = (-boids[i].transform.position) * 0.1f;
			}
		}
	}
}
