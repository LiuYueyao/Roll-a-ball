using UnityEngine;
using System.Collections;
using UnityEngine.UI; // For text
using System;
using System.Text.RegularExpressions;  

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	//public Text speed;
	public Text countText;
	public Text winText;
	public Text instruText;
	public Text timerText;
	public Button Restart;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
	private string s;
	//private InputField speedInput;
	private float startTime;
	private float endTime;
	//0: not started 1: playing 2: ended
	private int startFlag = 0;  


	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();
		count = 0;
		//countText.text = "Count: " + count.ToString;  // must after setting count
		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";
		instruText.text = "Use arrow keys to move the ball and pick up all the cubes.";
		s = "5";
		//timerText.text = "Timer: 0.00";

		//Button btn = this.GetComponent<Button>();
		Restart.onClick.AddListener(Onclick);

	}

	private void Onclick()
	{
		Application.LoadLevel ("rollaball");
	}

	public void getInput(string speed)
	{
		float fs = 0;
		if (float.TryParse (speed, out fs) && fs < 50) {
			s = speed;
			Console.WriteLine ("Speed = {0}", speed);
		}
		else 
			//warning
			print("Please input numbers less than 50");
	}

	void updateTime(int flag)
	{
		if (flag == 1)
			timerText.text = "Time: "+(Time.time - startTime).ToString ("f2");
		else
			timerText.text = "Time: "+ endTime.ToString ("f2");
	}
	
	// Each physics step..
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (Input.GetKeyDown(KeyCode.UpArrow) 
			|| Input.GetKeyDown(KeyCode.DownArrow)
			|| Input.GetKeyDown(KeyCode.LeftArrow) 
			|| Input.GetKeyDown(KeyCode.RightArrow)
			|| Input.GetKeyDown(KeyCode.A)
			|| Input.GetKeyDown(KeyCode.S)
			|| Input.GetKeyDown(KeyCode.D)
			|| Input.GetKeyDown(KeyCode.W))
		{
			if (startFlag == 0) {
				instruText.text = "";  // clear instruction
				startFlag = 1;
				startTime = Time.time;
			}
		}

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		rb.AddForce (movement * float.Parse(s));

		updateTime (startFlag);
		 
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		//Destroy(other.gameObject);

		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);
			count ++;
			// Run the 'SetCountText()' function (see below) instesd of "countText.text = "Count: " + count.ToString;"
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 12) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
			endTime = Time.time - startTime;
			startFlag = 2;
		}
	}
}