using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int damage = 3;

    void Start()
    {
        StartCoroutine(Despawn());   
    }
    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if(player != null)
        {
            player.Hurt(damage);
        }
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
