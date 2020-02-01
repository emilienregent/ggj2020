using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    Hashtable jobsImages = new Hashtable();
    private float actionStartTime = 0;
    private bool isInAction = false;
    private float actionDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Sprite fishSpriteImage = Resources.Load<Sprite>("fishPositionImage");

        jobsImages.Add(Jobs.Fish1, fishSpriteImage);
        jobsImages.Add(Jobs.Fish2, fishSpriteImage);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInAction == true)
        {
            float actionTime = (Time.time - actionStartTime);
            float progress = actionTime / actionDuration;
            Debug.Log("gauge fill " + progress);
            if (progress > 1)
            {
                triggerActionSuccess();
                isInAction = false;
            }
        }
    }

    public void actionStart()
    {
        actionStartTime = Time.time;

        if (currentJob != Jobs.None)
        {
            isInAction = true;
        }

    }

    public void actionStop()
    {
        isInAction = false;
        actionStartTime = 0;
    }

    public void triggerActionSuccess()
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

                //Increase the number of Colliders in the array
                i++;
            }
            isInAction = true;
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
            isInAction = true;
        }
    }

    public void checkJob()
    {
        Collider2D myBoxCollider = GetComponent<Collider2D>();

        // liste des colliders sur lesquels je suis
        List<Collider2D> hitColliders = new List<Collider2D>();
        myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

        // Pour chaque collider sur lequel je suis...
        int i = 0;
        while (i < hitColliders.Count)
        {
            currentJob = hitColliders[i].gameObject.GetComponent<PositionModel>().job;

            i++;
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = (Sprite)jobsImages[currentJob];
            break;
        }

        if (hitColliders.Count == 0)
        {
            currentJob = Jobs.None;
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = null;
        }

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
