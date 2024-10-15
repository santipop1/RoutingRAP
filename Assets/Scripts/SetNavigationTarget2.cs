using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget2 : MonoBehaviour
{

    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();

    [SerializeField] private GameObject navTargetObjet;

    private NavMeshPath path; // current calculated path
    private LineRenderer line; // linerenderer to display path
    private Vector3 targetPosition = Vector3.zero;
    private Canvas canvas;             // Referencia al Canvas
    public GameObject card;            // Referencia a la tarjeta única en la escena
    public GameObject NavigationMesh;  // Referencia al objeto NavigationMesh

    [SerializeField] private NavMeshSurface navMeshSurface;



    private bool lineToggle = false;
    private float baseLineWidth = 0.2f; // Ajusta este valor al grosor deseado
    private float scaleFactor = 2.0f; // Factor de escala fijo
    private string TargetText;
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
        canvas = FindObjectOfType<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {

        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);

            // Ajustar el grosor del LineRenderer según la escala del entorno
            line.startWidth = baseLineWidth * scaleFactor;
            line.endWidth = baseLineWidth * scaleFactor;
        }
    }
    public void SetCurrentNavigationTarget()
    {
        targetPosition = Vector3.zero;
        //string selectedText = navigationTargetDropDown.options[selectedValue].text;
        canvas = FindObjectOfType<Canvas>();
        TargetText = canvas.transform.GetChild(2).gameObject.transform.GetChild(4).GetComponent<TMP_Text>().text;
        Debug.Log("Target position: " + TargetText);
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(TargetText.ToLower()));
        if (currentTarget != null)
        {
            targetPosition = currentTarget.PositionObject.transform.position;
            Debug.Log("Target position: " + targetPosition);
        }
    }
    public void ToogleVisibility()
    {
        lineToggle = true;
        line.enabled = lineToggle;
        Debug.Log("Line visibility: " + lineToggle);
        card.SetActive(false);  // Ocultar la tarjeta si no hay información relevante
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        //NavigationMesh.GetComponent<NavMeshSurface>.build;
        navMeshSurface.BuildNavMesh();

    }
}
