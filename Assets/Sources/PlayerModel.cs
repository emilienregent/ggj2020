using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Jobs
{
    Fish1, Fish2, Direction
}

public class PlayerModel : MonoBehaviour
{

    [SerializeField]
    private Jobs currentJob = Jobs.Fish1;
    public ContactFilter2D contactFilter;
    public GameObject boat;
    private bool actionButtonTriggered = false;
    private bool actionButtonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return; // No gamepad connected.
        }

        if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            actionButtonPressed = true;
        }

        if (gamepad.rightTrigger.wasReleasedThisFrame)
        {
            actionButtonPressed = false;
        }

        if (actionButtonPressed == true)
        {
            if (currentJob == Jobs.Fish1)
            {
                GameObject fishingZone1 = GameObject.Find("FishZone1");
                Collider2D boxCollider = fishingZone1.GetComponent<Collider2D>();

                // liste des colliders dans la fishing zone
                List<Collider2D> hitColliders = new List<Collider2D>();
                boxCollider.OverlapCollider(contactFilter, hitColliders);

                // Pour chaque collider dans la fishing zone ...
                int i = 0;
                while (i < hitColliders.Count)
                {
                    //Output all of the collider names
                    Debug.Log("Hit : " + hitColliders[i].name + i);

                    // dégage la planche au loinnnn 
                    hitColliders[i].transform.position = new Vector2(-50.0f, -50.0f);
                    //Increase the number of Colliders in the array
                    i++;
                }
            }
        }

        // bouton d'action
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            
        }

    }

}
