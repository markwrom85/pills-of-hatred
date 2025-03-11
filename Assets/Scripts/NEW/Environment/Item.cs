using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public event Action<GameObject> OnItemDestroyed;
    private PlayerStats playerStats;
    [Header("Point Counts")]
    [SerializeField] private float points = 1f;
    [SerializeField] private float hateAmount = 5f;

    void OnTriggerEnter(Collider other)
    {
        playerStats = other.GetComponent<PlayerStats>();
        HandleDestroy(other.gameObject);    
        Destroy(gameObject);
    }

    void HandleDestroy(GameObject player)
    {
        playerStats.AddScore(points);
        playerStats.AddHate(hateAmount);

        OnItemDestroyed(gameObject);
    }
}