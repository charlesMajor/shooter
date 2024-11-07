using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int livesLeft = 5;

    [SerializeField] private AudioClip playerDead;
    private AudioSource audio;

    private GameManager gameManager;
    private int missileLeft = 0;
    private int bonusTimeLeft = 0;
    private bool isTimerStarted = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        audio = GetComponent<AudioSource>();
    }

    public int GetLivesLeft()
    {
        return livesLeft;
    }

    public void AddLife(int lifeToAdd)
    {
        livesLeft += lifeToAdd;
        gameManager.UpdateUi();
    }

    public void RemoveLife(int lifeToRemove)
    {
        livesLeft -= lifeToRemove;
        gameManager.UpdateUi();
        if (livesLeft <= 0)
        {
            // player death à faire?
            audio.clip = playerDead;
            audio.Play();
        }
        else
        {
            audio.Play();
        }
    }

    public int GetMissileLeft()
    {
        return missileLeft;
    }

    public bool CanShootMissile()
    {
        return missileLeft > 0;
    }

    public void AddMissile(int missibleToAdd)
    {
        missileLeft += missibleToAdd;
        gameManager.UpdateUi();
    }

    public void ShootMissile()
    {
        missileLeft--;
        gameManager.UpdateUi();
    }

    public int GetBonusTimeLeft()
    {
        return bonusTimeLeft;
    }

    public bool IsBonusActive()
    {
        return bonusTimeLeft > 0;
    }

    public void AddBonusTime(int timeToAdd)
    {
        bonusTimeLeft += timeToAdd;
        if (!isTimerStarted)
        {
            StartCoroutine(BonusTimeDecrease());
            isTimerStarted = true;
        }
        gameManager.UpdateUi();
    }

    private IEnumerator BonusTimeDecrease()
    {
        while (bonusTimeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            bonusTimeLeft--;
            gameManager.UpdateUi();
        }
        isTimerStarted = false;
    }
}