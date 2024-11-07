using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arena"))
        {
            gameObject.SetActive(false);
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Alien"))
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Explode();
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider other in colliders)
        {
            if (other.CompareTag("Alien"))
                other.gameObject.SetActive(false);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}