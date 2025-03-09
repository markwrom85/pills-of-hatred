using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15.0f;
    public int damage = 1;
    [SerializeField] private GameObject particlePrefab;
    private GameObject particle;
    void Start()
    {
        particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
        particle.transform.position = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);
        }
        Destroy(particle);
        Destroy(this.gameObject);
    }
}
