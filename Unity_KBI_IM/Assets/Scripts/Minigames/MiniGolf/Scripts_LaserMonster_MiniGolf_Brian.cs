using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_LaserMonster_MiniGolf_Brian : MonoBehaviour
{

    public float xSpread; 
    public float zSpread; 
    public float yOffset; 
    public Transform centerPoint; 

    public float rotSpeed; 
    public bool rotateClockwise; 
    
    // Rotation timer 
    float timer = 0;

    private float Rotation = 0; 
    public float rotationRate; 

    // Laser Projectile that the monster shoots 
    public GameObject laser; 
    // Timer to shoot the laser 
    float shootTimer = 3.0f; 
    bool firing;

    [SerializeField] Animator anim;
    [SerializeField] Transform projectileEmitter;
    GameObject player;

	// <summary> 
	// Move around in a circle and shoot the laser projectile every 3 seconds 
	// </summary>     
	void Update() {
        if (player == null){
            player =  player = FindObjectOfType<Scripts_MiniGolf_BallController_Zach>().gameObject;
            return;
        }
        timer += Time.deltaTime * rotSpeed; 
        Orbit(); 
        
        if (!firing) {
            if (shootTimer <= 0) {
                StartCoroutine(Fire());
            } else {
                shootTimer -= Time.deltaTime; 
            }
		}
    }

    IEnumerator Fire() {
        firing = true;

        // begin charge anim
        anim.SetBool("Charge", true);
        float chargeDuration = 0.833f; // length of the charge animation
        yield return new WaitForSeconds(chargeDuration);
        anim.SetBool("Charge", false);

        // fire projectile
        Vector3 playerDirection = (player.transform.position - projectileEmitter.position).normalized;
        GameObject projectile = Instantiate(laser) as GameObject;
        projectile.transform.position = projectileEmitter.position;
        projectile.transform.rotation = Quaternion.LookRotation(playerDirection);
        projectile.transform.rotation *= Quaternion.Euler(90, 0, 0);
        Rigidbody rb = projectile.GetComponent<Rigidbody>(); 
        rb.velocity = playerDirection * 5;
        Destroy(projectile, 1.5f);

        // begin fire anim
        anim.SetBool("Fire", true);
        float fireDuration = 1.677f; // length of the fire animation
        yield return new WaitForSeconds(fireDuration);

        // return to idle state
        anim.SetBool("Fire", false);

        // reset state
        shootTimer = 3.0f;
        firing = false;
	}

    // <summary> 
    // Orbit around a central point while rotating 
    // </summary>
    void Orbit()
    {
        if (rotateClockwise) {
            float x = -Mathf.Cos(timer) * xSpread; 
            float z = Mathf.Sin(timer) * zSpread; 
            Vector3 pos = new Vector3(x, yOffset, z); 
            transform.position = pos + centerPoint.position; 
        } else {
            float x = Mathf.Cos(timer) * xSpread; 
            float z = Mathf.Sin(timer) * zSpread; 
            Vector3 pos = new Vector3(x, yOffset, z); 
            transform.position = pos + centerPoint.position; 
        }
        if (Rotation == 360) { Rotation = 0; }
        Rotation = Rotation + rotationRate * Time.deltaTime;
        //gameObject.transform.rotation = Quaternion.Euler(0, Rotation + 180, 0);         
        transform.rotation = Quaternion.LookRotation(transform.position - centerPoint.position, Vector3.up);
    }


}
