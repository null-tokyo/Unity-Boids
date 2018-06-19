using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour {
	
	//Boidの数
	public int NumBoids = 100;

	//Boidの数
	public GameObject boid;

	public GameObject centerBoid;

	public GameObject[] boids;

	public float groupingDistance = 50f;
	public float SeparationDistance = 10f;

	public float AreaRadius = 50f;

	void CreateBoidsObject() {
		for (int i = 0; i < NumBoids; i++) {
			boids[i] = Instantiate(
				boid,
				new Vector3(
					Random.Range(-50f, 50f),
					Random.Range(-50f, 50f),
					Random.Range(-50f, 50f)
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
		var groupBoids = new List<GameObject>();
		groupBoids.Add(boids[index]);

		//中心を求める
		for (int i = 0; i < NumBoids; i++) {
			if(i != index) {
				float dist = Vector3.Distance(boids[i].transform.position, boids[index].transform.position);
				if(dist < groupingDistance) {
					groupBoids.Add(boids[i]);
					center += boids[i].transform.position;
				}
			}
		}
		center /= NumBoids - 1;

		//中心から一番遠いBoidを求める
		GameObject leaderBoids = boids[index];
		float far = 0;
		if(groupBoids.Count <= 1) {
			groupBoids[0].GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f,1f);
		}else{
			for (int i = 0; i < groupBoids.Count; i++) {
				groupBoids[i].GetComponent<Renderer>().material.color = new Color(1f - groupBoids.Count * 0.1f, 1f - groupBoids.Count * 0.05f, 1f,1f);
				float dist = Vector3.Distance(center, groupBoids[i].transform.position);
				if(far < dist) {
					leaderBoids = groupBoids[i];
					far = dist;
				}
			}
		}

		boids[index].GetComponent<Rigidbody>().velocity += (leaderBoids.transform.position - boids[index].transform.position).normalized * 2;
	}

	//分離
	void Separation(int index) {
		for (int i = 0; i < NumBoids; i++) {
			if (i != index) {
				float dist = Vector3.Distance(
					boids[i].transform.position,
					boids[index].transform.position
				);
				if(dist < SeparationDistance) {
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
		boids = new GameObject[NumBoids];
		CreateBoidsObject();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < NumBoids; i++) {
			Cohesion(i);
			Separation(i);
			Alignment(i);
			float dist = Vector3.Distance(Vector3.zero, boids[i].transform.position);
			if(dist > AreaRadius) {
				boids[i].GetComponent<Rigidbody>().velocity += (-boids[i].transform.position);
			}
		}
	}
}
