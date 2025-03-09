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
    private GameObject particle;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        /*transform.Translate(0, 0, speed * Time.deltaTime);
        particle.transform.position = transform.position;*/
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);
        }
        if(other.tag != "punch"){
        Destroy(particle);
        Destroy(this.gameObject);
        }
    }
}
