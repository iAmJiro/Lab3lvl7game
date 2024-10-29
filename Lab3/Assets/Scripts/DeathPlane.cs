using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public string deathMessage = "You have died!";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the death plane trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log(deathMessage);
            Destroy(other.gameObject);
        }
    }
}
