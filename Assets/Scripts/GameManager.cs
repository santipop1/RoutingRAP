using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeSceneCero()
    {
        SceneManager.LoadScene("Campus");
    }
    
    public void ChangeSceneUno()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
