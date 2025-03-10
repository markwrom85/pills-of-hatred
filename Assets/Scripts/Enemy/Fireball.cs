using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15.0f;
    public int damage = 1;
    [SerializeField] private GameObject particlePrefab;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyFireball());
    }

    private IEnumerator DestroyFireball(){
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        StopCoroutine(DestroyFireball());
        StartCoroutine(DestroyFireball());
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);
        }
        if(other.tag != "punch"){
        Destroy(this.gameObject);
        }
    }
}
