using UnityEngine;

public class DistanceBasedUIActivation : MonoBehaviour
{
    public Transform distanceTarget;
    public GameObject uiTarget;
    public float distanceToActivate = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    bool readyToTurnOff = false;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(distanceTarget.transform.position, transform.position) <= distanceToActivate) {
            //uiTarget.SetActive(true);
            readyToTurnOff = true;
        }
        else {
            if (readyToTurnOff) {
                //uiTarget.SetActive(false);
            }
        }
    }
}
