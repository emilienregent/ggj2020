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
    private string _currentAnimationParameter = string.Empty;
    public ContactFilter2D fishContactFilter;
    public ContactFilter2D actionContactFilter;
    Hashtable jobsImages = new Hashtable();
    private float actionStartTime = 0;
    private bool isInAction = false;
    public float actionDuration = 1.5f;
    private Sprite missingPlankImage;

    private Animator _animator = null;
    private SpriteRenderer _sprite = null;

    public Animator animator { get { return _animator; } }
    public SpriteRenderer sprite { get { return _sprite; } }

    private PlayerAudioController _audioController;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _audioController = GetComponentInChildren<PlayerAudioController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GameObject boat = GameObject.Find("Ship");
        transform.SetParent(boat.transform);
        Sprite fishSpriteImage = Resources.Load<Sprite>("fishPositionImage");
        Sprite directionSpriteImage = Resources.Load<Sprite>("directionPositionImage");
        Sprite repairSpriteImage = Resources.Load<Sprite>("repairPositionImage");
        Sprite bailOutSpriteImage = Resources.Load<Sprite>("bailOutPositionImage");

        jobsImages.Add(Jobs.Fish1, fishSpriteImage);
        jobsImages.Add(Jobs.Fish2, fishSpriteImage);
        jobsImages.Add(Jobs.Direction, directionSpriteImage);
        jobsImages.Add(Jobs.Repair, repairSpriteImage);
        jobsImages.Add(Jobs.BailOut, bailOutSpriteImage);

        missingPlankImage = Resources.Load<Sprite>("missingPlankImage");
    }

    // Update is called once per frame
    private void Update()
    {
        if (isInAction == true)
        {
            // le bloc suivant gère la gauge
            float actionTime = (Time.time - actionStartTime);
            float progress = actionTime / actionDuration;
            transform.Find("Gauge/gaugeFill").GetComponent<Transform>().localScale = new Vector3(125.0f * progress, 10.0f, 1.0f);
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

        if (currentJob == Jobs.Repair)
        {
            if(ResourcesModel.getStock() <= 0)
            {
                return;
            }
            Collider2D myBoxCollider = GetComponent<Collider2D>();

            // liste des colliders sur lesquels je suis
            List<Collider2D> hitColliders = new List<Collider2D>();
            myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

            int i = 0;
            while (i < hitColliders.Count)
            {
                Tile hitTile = hitColliders[i].transform.gameObject.GetComponent<Tile>();
                if (!!hitTile) 
                {
                    hitTile.SetUnderRepair(true);
                }
                i++;
            }
        }

        if (currentJob != Jobs.None)
        {
            PlayJobAnimation(true);
            isInAction = true;
        } else // j'ai appuyé sur A, j'ai pas de taff, et je suis sur la zone du gouvernail, je deviens captain !
        {
            BoatController boatController = transform.parent.GetComponent<BoatController>();

            if(boatController.captainPlayer != null)
            {
                return;
            } 

            Collider2D myBoxCollider = GetComponent<Collider2D>();

            // liste des colliders sur lesquels je suis
            List<Collider2D> hitColliders = new List<Collider2D>();
            myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

            // Pour chaque collider sur lequel je suis...
            int i = 0;
            
            while (i < hitColliders.Count)
            {
                if (hitColliders[i].gameObject.GetComponent<PositionModel>().job == Jobs.Direction)
                {
                    setCurrentJob(Jobs.Direction);
                    setBubbleIcon((Sprite)jobsImages[currentJob]);
                    boatController.setCaptain(gameObject);
                    PlayJobAnimation(true);
                    break;
                }
                i++;
            }
        }

    }

    // relache du bouton a
    public void actionStop()
    {
        transform.Find("Gauge/gaugeFill").GetComponent<Transform>().localScale = new Vector3(0.0f, 10.0f, 1.0f);
        isInAction = false;
        actionStartTime = 0;
        if (currentJob == Jobs.Direction) // je lâche la barre
        {
            setCurrentJob(Jobs.None);
            setBubbleIcon(null);
            transform.parent.GetComponent<BoatController>().setCaptain(null);
        }
        if (currentJob == Jobs.Repair)
        {
            Collider2D myBoxCollider = GetComponent<Collider2D>();

            // liste des colliders sur lesquels je suis
            List<Collider2D> hitColliders = new List<Collider2D>();
            myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

            int i = 0;
            while (i < hitColliders.Count)
            {
                Tile hitTile = hitColliders[i].transform.gameObject.GetComponent<Tile>();
                if (!!hitTile) 
                {
                  hitTile.SetUnderRepair(false);
                }
                i++;
            }
        }

        StopCurrentAnimation();
    }

    public void setBubbleIcon(Sprite icon)
    {
        if (icon == null)
        {
            transform.Find("CurrentAction/Bubble").gameObject.SetActive(false);
            transform.Find("CurrentAction/Bubble/Icon").GetComponent<SpriteRenderer>().sprite = null;
        } else {
            transform.Find("CurrentAction/Bubble").gameObject.SetActive(true);
            transform.Find("CurrentAction/Bubble/Icon").GetComponent<SpriteRenderer>().sprite = icon;
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
                ResourcesModel.Add(1);              
                // détruit la planche
                Destroy(hitColliders[i].gameObject);

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
                ResourcesModel.Add(1);              
                // détruit la planche
                Destroy(hitColliders[i].gameObject);

                i++;
            }
            isInAction = true;
        }

        if (currentJob == Jobs.Repair || currentJob == Jobs.BailOut)
        {
            Collider2D myBoxCollider = GetComponent<Collider2D>();

            // liste des colliders sur lesquels je suis
            List<Collider2D> hitColliders = new List<Collider2D>();
            myBoxCollider.OverlapCollider(actionContactFilter, hitColliders);

            // Pour chaque collider dans la fishing zone ...
            int i = 0;
            while (i < hitColliders.Count)
            {
                Tile hitTile = hitColliders[i].transform.gameObject.GetComponent<Tile>();
                if (hitTile != null && hitTile.type != TileType.EMPTY)
                {
                    int res = 0;
                    if (currentJob == Jobs.Repair) 
                    {
                        res = ResourcesModel.Use(1);
                    }
                    if(res != -1) 
                    {
                        // détruit la planche
                        hitColliders[i].transform.gameObject.GetComponent<Tile>().doRepair();
                        break;
                    }
                }
                i++;
            }
            isInAction = true;
        }
        checkJob();
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
            Jobs colliderJob = hitColliders[i].gameObject.GetComponent<PositionModel>().job;
            if (colliderJob != Jobs.Direction && colliderJob != Jobs.None)
            {
                // on marche sur une position
                setCurrentJob(colliderJob);
                setBubbleIcon((Sprite)jobsImages[currentJob]);
                if (currentJob == Jobs.Repair && ResourcesModel.getStock() == 0)
                {
                    setBubbleIcon(missingPlankImage);
                }
                break;
            }

            // je passe sur le collider Direction, et je suis sur un autre poste ? Je lâche mon poste !
            if (colliderJob == Jobs.None || (colliderJob == Jobs.Direction && currentJob != Jobs.Direction && currentJob != Jobs.None))
            {
                setCurrentJob(Jobs.None);
                setBubbleIcon(null);
            }

            if (colliderJob == Jobs.Direction && currentJob != Jobs.Direction && transform.parent.GetComponent<BoatController>().captainPlayer == null)
            {
                setBubbleIcon((Sprite)jobsImages[colliderJob]);
            }

            i++;
        }

        if (hitColliders.Count == 0)
        {
            setCurrentJob(Jobs.None);
            setBubbleIcon(null);
        }

    }

    private void setCurrentJob(Jobs job)
    {
        currentJob = job;
    }

    private void PlayJobAnimation(bool enable)
    {
        if (currentJob == Jobs.Fish1 || currentJob == Jobs.Fish2)
        {
            PlayAnimation("Fishing", enable);
        }
        else if (currentJob == Jobs.Repair)
        {
            PlayAnimation("Repairing", enable);
        }
        else if (currentJob == Jobs.Direction)
        {
            PlayAnimation("Directing", enable);
            SetAnimationValue("Direction", 0f);
        }
        else if (currentJob == Jobs.BailOut)
        {
            PlayAnimation("BailingOut", enable);
        }

        _audioController.Play(currentJob);
    }

    private void StopCurrentAnimation()
    {
        if(_currentAnimationParameter != string.Empty)
        {
            _animator.SetBool(_currentAnimationParameter, false);
        }
    }

    public void PlayAnimation(string parameter, bool enable)
    {
        StopCurrentAnimation();

        _animator.SetBool(parameter, enable);

        _currentAnimationParameter = enable == true ? parameter : string.Empty;

    }

    public void SetAnimationValue(string parameter, float value)
    {
        _animator.SetFloat(parameter, value);
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

    public void setColorFromIndex(int index) {
        switch (index) {
            case 0:
                 _sprite.color = new Color32(255, 223, 81, 255);
              break;
            case 1:
                 _sprite.color = new Color32(248, 154, 183, 255);
                break;
            case 2:
                _sprite.color = new Color32(0, 195, 14, 255);
                break;
            case 3:
                _sprite.color = new Color32(248, 154, 183, 255);
                break;
        }
    }
}
