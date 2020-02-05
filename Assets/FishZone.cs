using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Plank")
        {
            //col.gameObject.GetComponent<Animator>().enabled = true;
            col.gameObject.GetComponent<Animator>().SetBool("Shining", true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Plank")
        {
            col.gameObject.GetComponent<Animator>().SetBool("Shining", false);
            //col.gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
