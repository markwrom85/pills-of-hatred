using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int health;

    public int hateCount = 1;
    private int hateMax = 100;
    public int pickupHateIncrease = 1;
    public int killHateIncrease = 5;
    public int parryHateIncrease = 3;

    private int pickupScore = 1;
    private int killScore = 5;

    [SerializeField] private PlayerUI playerUI;

    void Start()
    {
        health = 5;
        hateCount = 1;
    }

    public void Hurt(int damage)
    {
        health -= damage;
        //Debug.Log($"Health: {health}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pickup")
        {
            playerUI.AddPlayerScore(pickupScore * hateCount);
            HateScoreCalc(pickupHateIncrease);
            Destroy(other.gameObject);
        }
    }

    private void HateScoreCalc(int hateIncrease)
    {
        hateCount += hateIncrease;

        //clamp hateCount
        if (hateCount > hateMax)
        {
            hateCount = hateMax;
        }
    }
}
