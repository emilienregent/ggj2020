using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum Jobs
{
    Fish1, Fish2, Direction, Repair, BailOut, None
}

public class PlayerModel : MonoBehaviour
{

    [SerializeField]
    private Jobs currentJob = Jobs.None;
    public ContactFilter2D fishContactFilter;
    public ContactFilter2D actionContactFilter;
    private bool actionButtonTriggered = false;
    private bool actionButtonPressed = false;
    Hashtable jobsImages = new Hashtable();
    private float actionStartTime = 0;
    private bool isInAction = false;
    public float actionDuration = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        GameObject boat = GameObject.Find("Ship");
        transform.SetParent(boat.transform);
        Sprite fishSpriteImage = Resources.Load<Sprite>("fishPositionImage");
        Sprite directionSpriteImage = Resources.Load<Sprite>("directionPositionImage");

        jobsImages.Add(Jobs.Fish1, fishSpriteImage);
        jobsImages.Add(Jobs.Fish2, fishSpriteImage);
        jobsImages.Add(Jobs.Direction, directionSpriteImage);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInAction == true)
        {
            float actionTime = (Time.time - actionStartTime);
            float progress = actionTime / actionDuration;
            transform.Find("Gauge/gaugeFill").GetComponent<Transform>().localScale = new Vector3(125.0f * progress, 10.0f, 1.0f);
            Debug.Log("gauge fill " + progress);
            if (progress > 1)
            {
                triggerActionSuccess();
                isInAction = false;
                transform.Find("Gauge/gaugeFill").GetComponent<Transform>().localScale = new Vector3(0.0f, 10.0f, 1.0f);
            }
        }
    }

    // appui sur le bouton a
    public void actionStart()
    {
        actionStartTime = Time.time;

        if (currentJob != Jobs.None)
        {
            isInAction = true;
        } else // j'ai pas de taff, et je suis sur la zone du gouvernail, je deviens captain !
        {
            Collider2D myBoxCollider = GetComponent<Collider2D>();

            // liste des colliders sur lesquels je suis
            List<Collider2D> hitColliders = new List<Collider2D>();
            myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

            // Pour chaque collider sur lequel je suis...
            int i = 0;
            while (i < hitColliders.Count)
            {
                Debug.Log(hitColliders[i].gameObject.GetComponent<PositionModel>().job);
                if (hitColliders[i].gameObject.GetComponent<PositionModel>().job == Jobs.Direction)
                {
                    currentJob = Jobs.Direction;
                    transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = (Sprite)jobsImages[currentJob];
                    transform.parent.GetComponent<BoatController>().setCaptain(gameObject);
                    break;
                }
                i++;
            }
        }

    }

    // relache du bouton a
    public void actionStop()
    {
        isInAction = false;
        actionStartTime = 0;
        if (currentJob == Jobs.Direction)
        {
            currentJob = Jobs.None;
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = null;
            transform.parent.GetComponent<BoatController>().setCaptain(null);
        }
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

                // détruit la planche
                Destroy(hitColliders[i].gameObject);

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

                // détruit la planche
                Destroy(hitColliders[i].gameObject);

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
            if (hitColliders[i].gameObject.GetComponent<PositionModel>().job != Jobs.Direction)
            {
                currentJob = hitColliders[i].gameObject.GetComponent<PositionModel>().job;
                transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = (Sprite)jobsImages[currentJob];
            }

            // je passe sur le collider Direction, et je suis sur un autre poste ? Je lâche mon poste !
            if (hitColliders[i].gameObject.GetComponent<PositionModel>().job == Jobs.Direction && currentJob != Jobs.Direction && currentJob != Jobs.None)
            {
                currentJob = Jobs.None;
                transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = null;
            }

            i++;
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

    public bool canMove()
    {
        if (currentJob == Jobs.Direction || isInAction == true)
        {
            return false;
        } else
        {
            return true;
        }
    }

}
