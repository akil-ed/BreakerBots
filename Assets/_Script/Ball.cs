using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float ballInitialVelocity = 600f;

	public bool AutoStart;
	private Rigidbody2D rb;
	private bool ballInPlay;
	bool isDead;
	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		GM.instance.AddBall (gameObject);
		if (AutoStart)
			StartPlay ();
	}

	void Update ()
	{
		if (Input.GetButtonDown("Fire1") && ballInPlay == false)
		{
			StartPlay ();
		}

		if (transform.position.y < -5&&!isDead) {
		//	GM.instance.LoseLife ();
			GM.instance.RemoveBall (gameObject);
			isDead = true;
			Destroy (gameObject);
		}

		//rb.velocity.
	}

	void StartPlay(){
		transform.parent = null;
		ballInPlay = true;
		rb.isKinematic = false;
		rb.simulated = true;
		rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
	}
}
