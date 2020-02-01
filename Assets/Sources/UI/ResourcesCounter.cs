using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesCounter : MonoBehaviour
{
    public Text countText;
    // Start is called before the first frame update
    void Start()
    {
       countText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {   
        int count = ResourcesModel.getStock();
        countText.text = count.ToString();
    }
}
