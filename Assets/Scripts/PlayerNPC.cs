using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerNPC : MonoBehaviour
{
    private Alliance alliance;
    private NavMeshAgent agent;
    public GameObject strikePrefab;
    public Animator animator;
    private AudioSource audioSource;
    public AudioClip attackSound;


    private Health h;

    void Start()
    {
        h = GetComponent<Health>();
        alliance = GetComponent<Alliance>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlayAttackClip() {
        if (audioSource) {
            if (attackSound) {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = closestEnemy;

        agent.SetDestination(target);

        if (Vector3.Distance(transform.position, target) <= 1.0f)
        {
            UpdateStriking(target);
        }

        if (animator)
        {
            animator.SetFloat("velocity", Vector3.Distance(Vector3.zero, agent.velocity));
        }
    }

    private DateTime strikeDelay = DateTime.Now;
    void UpdateStriking(Vector3 target)
    {
        if ((DateTime.Now - strikeDelay).TotalMilliseconds >= 900)
        {
            strikeDelay = DateTime.Now;

            Vector3 direction = Vector3.Normalize(transform.position - target) * -0.5f;

            HandleStrike(direction);
        }
    }

    void HandleStrike(Vector3 direction)
    {
        StartCoroutine(AttackCoroutine());
        PlayAttackClip();

        int calculatedDamage = 5 + Global._attackLevel + Global._strengthLevel;

        GameObject g = GameObject.Instantiate(strikePrefab);
        GenericStrikeScript s = g.GetComponent<GenericStrikeScript>();
        s.damage = calculatedDamage;
        s.spawningAlliance = alliance.alliance;
        g.transform.position = transform.position + direction;
        g.transform.forward = transform.forward;

        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, 1.0f, direction);

        for (int i = 0; i < allHits.Length; i++)
        {
            Alliance targetAlliance = allHits[i].collider.gameObject.GetComponent<Alliance>();

            if (targetAlliance)
            {
                if (targetAlliance.alliance != alliance.alliance)
                {
                    Health h = allHits[i].collider.gameObject.GetComponent<Health>();
                    if (h)
                    {
                        if (Vector3.Distance(transform.position, h.gameObject.transform.position) <= 1.0f) {
                            h.health -= calculatedDamage;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("attacking", false);
    }

    Vector3 closestEnemy
    {
        get
        {
            Alliance[] allAllianceFlags = GameObject.FindObjectsByType<Alliance>(FindObjectsSortMode.None);

            Alliance closestMatch = null;

            for (int i = 0; i < allAllianceFlags.Length; i++)
            {
                if (alliance.alliance != allAllianceFlags[i].alliance)
                {
                    if (closestMatch == null)
                    {
                        closestMatch = allAllianceFlags[i];
                    }
                    else
                    {
                        float existingDistance = Vector3.Distance(transform.position, closestMatch.transform.position);
                        float newDistance = Vector3.Distance(transform.position, allAllianceFlags[i].transform.position);

                        if (newDistance < existingDistance)
                        {
                            closestMatch = allAllianceFlags[i];
                        }
                    }
                }
            }

            if (closestMatch != null)
            {
                return closestMatch.transform.position;
            }

            return Vector3.zero;
        }
    }

    public void HandleDeath()
    {
        GameObject.DestroyImmediate(gameObject);
    }
}
