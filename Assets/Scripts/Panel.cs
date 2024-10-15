using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] GameObject Panell;


    

   

    public void onClickButon()
    {
        map.SetActive(true);
        Panell.SetActive(true);
    }

    public void DesClickButon()
    {
        map.SetActive(false);
        Panell.SetActive(false);
    }
}
