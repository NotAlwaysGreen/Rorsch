using UnityEngine;

public class Key : MonoBehaviour
{
    public Door targetDoor;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            UseKey();
        }
    }

    void UseKey()
    {
        if (targetDoor != null)
        {
            targetDoor.Unlock();
            Debug.Log("Door unlocked with key!");
        }
        else
        {
            Debug.LogWarning("No door assigned to key!");
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}