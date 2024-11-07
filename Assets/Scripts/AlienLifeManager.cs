using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlienLifeManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private int health = 2;

    [SerializeField] private AudioClip death;
    [SerializeField] private GameObject soundManager;

    void Start()
    {
        player = GameObject.FindWithTag("SpaceMarine");
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.SetActive(false);
            loseHealth(1);
        }

        if (other.gameObject.tag == "Player")
        {
            loseAllHealth();
            other.GetComponent<PlayerController>().RemoveLife(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            loseAllHealth();
        }
    }

    private void loseHealth(int amountToRemove)
    {
        health -= amountToRemove;
        if (health <= 0)
        {
            die();
        }
    }

    private void loseAllHealth()
    {
        health = 0;
        die();
    }

    private void die()
    {
        //soundManager.GetComponent<SoundManager>().playClipAtPosition(death, transform.position);
        this.gameObject.SetActive(false);
    }
}
