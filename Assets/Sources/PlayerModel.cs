using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Jobs
{
    Fish1, Fish2, Direction, None
}

public class PlayerModel : MonoBehaviour
{

    [SerializeField]
    private Jobs currentJob = Jobs.None;
    public ContactFilter2D fishContactFilter;
    public ContactFilter2D actionContactFilter;
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
                boxCollider.OverlapCollider(fishContactFilter, hitColliders);

                // Pour chaque collider dans la fishing zone ...
                int i = 0;
                while (i < hitColliders.Count)
                {
                    //Output all of the collider names
                    Debug.Log("Hit : " + hitColliders[i].name + i);

                    // dégage la planche au loinnnn 
                    hitColliders[i].transform.position = new Vector2(-50.0f, -50.0f);
                    fish();
                    //Increase the number of Colliders in the array
                    i++;
                }
            }

            if (currentJob == Jobs.Fish2)
            {
                GameObject fishingZone1 = GameObject.Find("FishZone2");
                Collider2D boxCollider = fishingZone1.GetComponent<Collider2D>();

                // liste des colliders dans la fishing zone
                List<Collider2D> hitColliders = new List<Collider2D>();
                boxCollider.OverlapCollider(fishContactFilter, hitColliders);

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

        // bouton de prise de position
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            if (currentJob == Jobs.None)
            {
                Collider2D myBoxCollider = GetComponent<Collider2D>();

                // liste des colliders sur lesquels je suis
                List<Collider2D> hitColliders = new List<Collider2D>();
                myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

                // Pour chaque collider sur lequel je suis...
                int i = 0;
                while (i < hitColliders.Count)
                {
                    //Output all of the collider names
                    Debug.Log("Hit : " + hitColliders[i].name + i);
                    Debug.Log(hitColliders[i].gameObject);
                    // dégage la planche au loinnnn 
                    //hitColliders[i].transform.position = new Vector2(-50.0f, -50.0f);
                    currentJob = hitColliders[i].gameObject.GetComponent<PositionModel>().job;
                    //Increase the number of Colliders in the array
                    i++;
                }
            } else {
                currentJob = Jobs.None;
            }
           
        }

    }

    public void fish()
    {

    }

    public void setCurrentJob(Jobs job)
    {
        currentJob = job;
    }

    public Jobs getCurrentJob()
    {
        return currentJob;
    }

}
