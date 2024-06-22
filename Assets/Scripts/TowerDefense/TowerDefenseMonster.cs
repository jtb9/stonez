using UnityEngine;
using UnityEngine.AI;

public class TowerDefenseMonster : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    
    Vector3 getGoal() {
        GameObject g = GameObject.Find("Goal");
        return g.transform.position;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(getGoal());
    }

    void Update()
    {
        animator.SetFloat("velocity", Vector3.Distance(agent.velocity, Vector3.zero));
    }
}
