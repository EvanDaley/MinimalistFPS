using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour 
{
	public GameObject bulletPrefab;

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	void Update () 
	{
		if (!isLocalPlayer)
			return;
		
		var x = Input.GetAxis ("Horizontal") * 0.1f;
		var z = Input.GetAxis ("Vertical") * 0.1f;

		transform.Translate (x, 0, z);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
	}

	[Command]
	void CmdFire()
	{
		// create the bullet object from the bullet prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			transform.position - transform.forward,
			Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = -transform.forward*4;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		Destroy(bullet, 5.0f);        
	}
}
