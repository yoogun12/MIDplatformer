using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySkip2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SSSkip()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("End");
    }

}