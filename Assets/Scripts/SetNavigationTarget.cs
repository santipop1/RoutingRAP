using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown navigationTargetDropDown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();

    [SerializeField] private Camera topDownCamera;
    [SerializeField] private GameObject navTargetObjet;

    private NavMeshPath path; // current calculated path
    private LineRenderer line; // linerenderer to display path
    private Vector3 targetPosition = Vector3.zero;


    private bool lineToggle = false;
    private float baseLineWidth = 0.2f; // Ajusta este valor al grosor deseado
    private float scaleFactor = 2.0f; // Factor de escala fijo
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if(lineToggle && targetPosition != Vector3.zero)
        {
            if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    line.positionCount = path.corners.Length;
                    line.SetPositions(path.corners);

                    // Ajustar el grosor del LineRenderer seg�n la escala del entorno
                    line.startWidth = baseLineWidth * scaleFactor;
                    line.endWidth = baseLineWidth * scaleFactor;
                }
                else
                {
                    Debug.LogWarning("No se pudo encontrar una ruta v�lida.");
                }
            }
            else
            {
                Debug.LogWarning("Error en el c�lculo de la ruta.");
            }
        }
    }
    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(selectedText.ToLower()));

        if (currentTarget != null)
        {
            targetPosition = currentTarget.PositionObject.transform.position;
            Debug.Log("Posici�n objetivo: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("No se encontr� el objetivo de navegaci�n.");
        }
    }

    public void ToogleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
        Debug.Log("Linea de navegaci�n: " + (lineToggle ? "Activada" : "Desactivada"));
    }
}
