using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectibleController : MonoBehaviour
{
    [SerializeField] private int missileToAdd = 5;
    [SerializeField] private float deactivateDelay = 15f;

    [SerializeField] private AudioClip powerUpPickup;
    private AudioSource audio;

    private float rotationSpeed = 90f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterDelay());
        audio.Play();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.AddMissile(missileToAdd);
            gameObject.SetActive(false);
            audio.PlayOneShot(powerUpPickup);
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(deactivateDelay);
        gameObject.SetActive(false);
    }
}