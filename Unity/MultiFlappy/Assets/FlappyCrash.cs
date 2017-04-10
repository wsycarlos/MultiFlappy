using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bird;

public class FlappyCrash : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Crash!");
        GameObject.Destroy(col.gameObject);
        CSharpClient.Instance.ReportCrash();
    }
}
