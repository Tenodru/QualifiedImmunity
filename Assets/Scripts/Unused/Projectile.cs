using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float deathTimer = 0;
    [HideInInspector] public string user;
    [HideInInspector] public float damage;
    IEnumerator timedDestroy;


    public void DestroyProjectile()
    {
        //gameObject.SetActive(false);
    }

    IEnumerator TimedDestroy(float timer)
    {
        yield return new WaitForSeconds(timer);
        DestroyProjectile();
    }

    public void ChangeDestroyTime(float newTime)
    {
        StopCoroutine(timedDestroy);
        timedDestroy = TimedDestroy(newTime);
        StartCoroutine(timedDestroy);
    }

    //private void FixedUpdate()
    //{
    //    transform.Translate(transform.right * speed * Time.fixedDeltaTime, Space.World);
    //}

    public void SetDeathTimer(float _timeToDeath, float _speed, float _damage, string _user)
    {
        damage = _damage;
        deathTimer = _timeToDeath;
        speed = _speed;
        user = _user;

        timedDestroy = TimedDestroy(_timeToDeath);

        if (_timeToDeath != 0) // if this projectile automatically dies after a period of time
        {
            StartCoroutine(timedDestroy);
        }
    }
}

