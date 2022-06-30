using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    public CompleteMenu CM;


    private void Start()
    {
        CM = FindObjectOfType<CompleteMenu>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag  == "Player")
        {
            CM.CompleteGame();
        }
    }
}
