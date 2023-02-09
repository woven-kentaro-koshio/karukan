using RpgAdventure;
using UnityEngine;

[System.Serializable]
public class PlayerScanner 
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;


    public PlayerController Detect(Transform detector)
    {
        if (PlayerController.Instance == null)
        {
            return null;
        }

        Vector3 toplayer = PlayerController.Instance.transform.position - detector.position;
        toplayer.y = 0;

        if (toplayer.magnitude <= detectionRadius)
        {
            if (Vector3.Dot(toplayer.normalized, detector.forward) >
               Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                return PlayerController.Instance;
            }
        }

        return null;
    }
}
