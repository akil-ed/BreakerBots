using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;
public class Paddle : MonoBehaviour {

	public Animator _Animator;
	public float paddleSpeed = 1f;
	public Sprite LeftSprite, RightSprite, IdleSprite;
	public Transform PaddleTop;
	private Vector3 playerPos = new Vector3 (0, -4f, 0);
	float yPos;
	public static bool isShield;
	public GameObject Shield,CharacterSelect,PowerUpSelect,ShootParticle,Controls;
	public GameObject[] Arms;
	bool isCrab;
	public int CharTag,PowerUpIndex=-1;
	public GameObject[] Balls;

	void Start(){
		yPos = transform.position.y;
		isShield = true;
		Invoke ("HideShield", 1.5f);
		InvokeRepeating ("UpdateDirection", 0, 0.2f);
//		PaddleTop.localScale = new Vector3 (0.5f, 0.5f, 1);
		//Invoke ("ChangeLayer", 3);
		_Animator = GetComponent <Animator>();
		//EnableShoot ();
		//Time.timeScale = 0.7f;
	}

	void OnEnable(){
		GM.OnBlackOut += BlackOutLayer;
		EasyTouch.On_DoubleTap += On_DoubleTap;	
	}

	void OnDisable(){
		UnsubscribeEvent ();
	}

	void OnDestroy(){
		UnsubscribeEvent();
	}

	void UnsubscribeEvent(){
		EasyTouch.On_DoubleTap -= On_DoubleTap;	
		GM.OnBlackOut -= BlackOutLayer;
	}

	float xPos;
	int direction;
	void Update () 
	{
		// = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
		if(isTouch && Time.timeScale>0)
			xPos = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;

		playerPos = new Vector3 (Mathf.Clamp (xPos, -3.25f, 3.25f), yPos, 0f);
		transform.position = playerPos;

		if (MouseHelper.mouseDelta.x < -0.1)
			direction = -1;
		else if (MouseHelper.mouseDelta.x > 0.1)
			direction = 1;
		else
			direction = 0;

	}
	public void SetShield(){
		isShield = true;
		Shield.SetActive (true);
		Invoke ("HideShield", 5f);
	}

	void HideShield(){
		isShield = false;
		Shield.SetActive (false);
	}
	public Sprite touchdown,touchup;
	public SpriteRenderer TouchControls;
	bool isTouch;
	void OnMouseDown(){
		TouchControls.sprite = touchdown;
		isTouch = true;
	}

	void OnMouseUp(){
		TouchControls.sprite = touchup;
		isTouch = false;
	}
	bool flipflop;
	void UpdateDirection(){
		if (CharTag!=0)
			return;

		flipflop = !flipflop;
	
		if (flipflop)
			GetComponent <SpriteRenderer> ().sprite = IdleSprite;
		else {
			if (direction<0)
				GetComponent <SpriteRenderer> ().sprite = LeftSprite;
			else if (direction>0)
				GetComponent <SpriteRenderer> ().sprite = RightSprite;
			else
				GetComponent <SpriteRenderer> ().sprite = IdleSprite;

		}
	}

	void BlackOutLayer(){
		//gameObject.layer = 9;
		Transform[] Objects = GetComponentsInChildren <Transform> ();
		foreach (Transform Go in Objects)
			Go.gameObject.layer = 9;
		
		Invoke ("NormalLayer", 7);
	}

	void NormalLayer(){
		Transform[] Objects = GetComponentsInChildren <Transform> ();
		foreach (Transform Go in Objects)
			Go.gameObject.layer = 0;
	}

	private void On_DoubleTap( Gesture gesture){
		Time.timeScale = 0;
		CharacterSelect.SetActive (true);
		CharacterSelect.transform.parent = null;
		Controls.SetActive (false);
		PowerUpSelect.GetComponent <PowerSelect> ().SetSprite (PowerUpIndex);
	}

	public void CheckTag(){
		Controls.SetActive (true);

		if (CharTag == 0)
			for (int i = 0; i < 2; i++)
				Arms [i].SetActive (true);
		else
			for (int i = 0; i < 2; i++)
				Arms [i].SetActive (false);


	}

	public void EnableShoot(){
		switch (CharTag) {
		case 0:
			StartCoroutine (Shoot ());
			break;
		case 1:
			StarFish ();
			break;
		case 2:
			StartCoroutine (Turtle ());
			break;
		}

	}

	IEnumerator Shoot(){
		_Animator.Play ("OpenCrabArms");
		yield return new WaitForSeconds (1);
		for (int i = 0; i < 6; i++) {
			Instantiate (ShootParticle, Arms [0].transform.position, Quaternion.Euler (-90,0,0));
			Instantiate (ShootParticle, Arms [1].transform.position, Quaternion.Euler (-90,0,0));
			yield return new WaitForSeconds (0.3f);
		}
		_Animator.Play ("CloseCrabArms");
	}

	void StarFish(){
		Balls [0].SetActive (true);
		Balls [1].SetActive (true);
	}

	IEnumerator Turtle(){
		Time.timeScale = 0.75f;
		yield return new WaitForSeconds (5);
		Time.timeScale = 1;
	}

	public void PowerUp(){
		Controls.SetActive (true);
	

		switch (PowerUpIndex) {
		case 0:
			SetShield ();
			break;
		case 1:
			EnableShoot ();
			break;
		}

		PowerUpIndex = -1;
	}
}