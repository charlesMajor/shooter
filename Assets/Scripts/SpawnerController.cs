using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.SetActive(false);
            loseHealth();
        }
    }

    private void loseHealth()
    {
        healthPoints -= 1;
        if (healthPoints <= 0)
        {
            destroySpawner();
        }
    }

    private void destroySpawner()
    {
        this.gameObject.SetActive(false);
        gameManager.updateSpawnerList();
    }

}
