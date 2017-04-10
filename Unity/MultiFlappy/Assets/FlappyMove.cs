using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bird;

public class FlappyMove : MonoBehaviour
{
    public float zspeed = 0.015f;

    public float yspeed = -0.025f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CSharpClient.Instance.starting)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                yspeed = 0.025f;
            }
            else
            {
                yspeed = -0.025f;
            }

            transform.Translate(new Vector3(0, yspeed, zspeed));

            Camera.main.transform.Translate(new Vector3(zspeed, 0, 0));

            CSharpClient.Instance.SyncPos(transform.localPosition);
        }
    }
}
