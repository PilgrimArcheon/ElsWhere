using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private Transform muzzlePrefab;
    [SerializeField] private Transform hitEffect;
    public float rotateAmount;
    public AudioSource sfx;
    public AudioClip shootSound, hitSound;

    private Rigidbody bulletRigidbody;

    private void Awake() 
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        float speed = 100f;
        bulletRigidbody.velocity = transform.forward * speed;

        if (muzzlePrefab != null) {
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX.gameObject, psChild.main.duration);
			}
		}
    }

    private void FixedUpdate() 
    {
        transform.Rotate(0, 0, rotateAmount, Space.Self);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyAI>().TakeDamage(25.0f);
        }
        GameObject hitFx = Instantiate(hitEffect, transform.position, Quaternion.identity).gameObject;
		Destroy(hitFx, 2f);
        sfx.PlayOneShot(hitSound);
        Destroy(gameObject);
    }
}