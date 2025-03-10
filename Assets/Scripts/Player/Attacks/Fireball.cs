using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15.0f;
    public int damage = 2;
    private Rigidbody rb;

    public bool wasParried = false;
    [SerializeField] GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyFireball());
    }

    private IEnumerator DestroyFireball()
    {
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        StopCoroutine(DestroyFireball());
        StartCoroutine(DestroyFireball());
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        /*Punch punch = other.
        if (player != null)
        {
            player.Hurt(damage);
        }*/

        if(other.tag == "Player")
        {
            other.GetComponentInParent<PlayerCharacter>().Hurt(damage);
        }
        else if(other.tag == "punch")
        {
            other.GetComponent<PlayerCharacter>().isInvincible = true;
            other.GetComponent<PlayerCharacter>().InvincibleCooldown();
        }
        else if (other.tag != "punch" && wasParried)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
