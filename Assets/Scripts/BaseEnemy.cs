using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public float huntRange = 5.0f;
    private Alliance alliance;
    private NavMeshAgent agent;
    public GameObject strikePrefab;
    public Animator animator;
    public AudioClip attackSound;
    private AudioSource audioSource;
    public int baseDamage = 5;
    public int attackSpeed = 900;

    public float strikingDistance = 3.0f;

    void Start()
    {
        targetLocation = nextHuntLocation;
        agent = GetComponent<NavMeshAgent>();
        alliance = GetComponent<Alliance>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlayAttackClip() {
        if (audioSource) {
            if (attackSound) {
                audioSource.PlayOneShot(attackSound);
            }
        }
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
                // make sure it's within our max range as well
                if (Vector3.Distance(transform.position, closestMatch.transform.position) < 10.0f)
                {
                    return closestMatch.transform.position;
                }
            }

            return Vector3.zero;
        }
    }

    Vector3 nextHuntLocation
    {
        get
        {
            return new Vector3(
                transform.position.x + UnityEngine.Random.Range(-huntRange, huntRange),
                transform.position.y,
                transform.position.z + UnityEngine.Random.Range(-huntRange, huntRange)
            );
        }
    }

    public void HandleDeath()
    {
        GameObject.DestroyImmediate(gameObject);
    }

    void HandleStrike(Vector3 direction)
    {
        StartCoroutine(AttackCoroutine());
        PlayAttackClip();

        GameObject g = GameObject.Instantiate(strikePrefab);
        GenericStrikeScript s = g.GetComponent<GenericStrikeScript>();
        s.damage = baseDamage;
        s.spawningAlliance = alliance.alliance;
        g.transform.position = transform.position + direction;
        g.transform.forward = transform.forward;

        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, 2.6f, direction);

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
                        if (Vector3.Distance(h.gameObject.transform.position, transform.position) <= 1.0f) {
                            h.health -= baseDamage;
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

    Vector3 lastKnownPositionOfTarget = Vector3.zero;
    public int state = 0;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                // hunting
                UpdateHunting();
                break;
            case 1:
                // attacking
                UpdateAttack();
                break;
            case 2:
                // striking
                UpdateStriking();
                break;
        }

        if (animator) {
            animator.SetFloat("velocity", Vector3.Distance(Vector3.zero, agent.velocity));
        }
    }

    void UpdateAttack()
    {
        if (Vector3.Distance(transform.position, lastKnownPositionOfTarget) >= strikingDistance)
        {
            agent.SetDestination(lastKnownPositionOfTarget);

            // update it
            Vector3 newTarget = closestEnemy;

            if (newTarget == Vector3.zero)
            {
                state = 0;
            }
            else
            {
                lastKnownPositionOfTarget = newTarget;
            }
        }
        else
        {
            // we're in striking range
            state = 2;
        }
    }
    private DateTime strikeDelay = DateTime.Now;
    void UpdateStriking()
    {
        if (Vector3.Distance(transform.position, closestEnemy) >= strikingDistance)
        {
            // go back to hunting
            state = 0;
        }
        else
        {
            if ((DateTime.Now - strikeDelay).TotalMilliseconds >= attackSpeed)
            {
                strikeDelay = DateTime.Now;

                Vector3 direction = Vector3.Normalize(transform.position - lastKnownPositionOfTarget) * -2.0f;
                HandleStrike(direction);
            }
        }
    }

    private bool readyForNewLocation = false;
    private DateTime delayForNewLocation = DateTime.Now;
    private Vector3 targetLocation = Vector3.zero;

    void UpdateHunting()
    {
        if (readyForNewLocation)
        {
            if ((DateTime.Now - delayForNewLocation).TotalMilliseconds >= 700)
            {
                readyForNewLocation = false;
                targetLocation = nextHuntLocation;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetLocation) <= 0.3f)
            {
                delayForNewLocation = DateTime.Now;
                readyForNewLocation = true;
            }
        }

        agent.SetDestination(targetLocation);

        // check for an enemy we can attack yet
        Vector3 targetCheck = closestEnemy;

        if (targetCheck != Vector3.zero)
        {
            lastKnownPositionOfTarget = targetCheck;
            state = 1;
        }
    }
}
