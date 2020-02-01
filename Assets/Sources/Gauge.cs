using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
      Quaternion rotation;
  void Awake()
  {
       rotation = transform.rotation;
  }
  void LateUpdate()
  {
        transform.rotation = rotation;
  }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
