using NUnit.Framework.Constraints;
using UnityEngine;

public class GenericStrikeScript : MonoBehaviour
{
    public int spawningAlliance = 0;
    private bool hasPainted = false;
    public int damage = 3;


    private void HandleDoingDamage(Alliance target) {
        Health h = target.gameObject.GetComponent<Health>();
        h.health -= damage;

        GameObject.Destroy(gameObject);
    }

}
