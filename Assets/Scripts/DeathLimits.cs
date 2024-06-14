using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLimits : MonoBehaviour
{

    [SerializeField] private GameObject bottomLimit;
    [SerializeField] private GameObject leftLimit;
    private BoxCollider2D boxCollider2D;

    private Collider2D[] hits;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fallToDeath"))
        {
            gameObject.GetComponent<PlayerStats>().Health = 0;
        }
    }

    private void HitLimits()
    {

        hits = Physics2D.OverlapCircleAll(transform.position, 2);

        foreach (Collider2D c in hits)
        {
            if (c.CompareTag("fallToDeath") )
            {
                gameObject.GetComponent<PlayerStats>().Health = 0;
            }
        }
        
    }

}

