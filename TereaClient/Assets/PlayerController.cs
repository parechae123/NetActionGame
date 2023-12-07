using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical")!= 0||Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector3 tempVec = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
            transform.position += tempVec*Time.deltaTime;
        }
    }
}
