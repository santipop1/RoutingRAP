using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastFloor : MonoBehaviour
{
    public GameObject sueloCubo;       // Instancia del suelo
    public GameObject card;            // Referencia a la tarjeta única en la escena
    private Canvas canvas;             // Referencia al Canvas

    // Sprites para las imágenes de las diferentes zonas
    [SerializeField] Sprite cieSprite, unoSprite, biblioSprite, cedetecSprite, dosSprite, surSprite;

    private GraphicRaycaster graphicRaycaster; // Para detectar clics en el UI
    private PointerEventData pointerEventData; // Para almacenar la información del toque/clic


    // Referencias a los textos e imagen de la tarjeta
    private TMP_Text tituloText, descripcion1Text, descripcion2Text;
    private Image imagenUI;

    void Start()
    {
        // Encontrar el Canvas en la escena
        canvas = FindObjectOfType<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();

        if (canvas == null)
        {
            Debug.LogError("No se encontró un Canvas en la escena.");
            return;
        }

        // Encontrar los componentes de la tarjeta
        tituloText = card.transform.GetChild(4).GetComponent<TMP_Text>();
        descripcion1Text = card.transform.GetChild(2).GetComponent<TMP_Text>();
        descripcion2Text = card.transform.GetChild(3).GetComponent<TMP_Text>();
        imagenUI = card.transform.GetChild(5).GetComponent<Image>();

        // Asegurarse de que la tarjeta está oculta al inicio
        card.SetActive(false);
    }

    void Update()
    {
        // Solo se evalúa si hay un toque en pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Solo procesamos el toque cuando ha comenzado
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;

                // Verificar si tocaste un elemento interactivo del UI
                if (IsPointerOverUIElement(touchPosition))
                {
                    Debug.Log("Tocaste un elemento interactivo del UI.");
                    return;
                }

                // Procesar el raycast sobre la escena
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Verifica si tocó el suelo
                    if (hit.collider.gameObject == sueloCubo)
                    {
                        // Convertir la posición global del punto de colisión a coordenadas locales del cubo
                        Vector3 localHitPosition = sueloCubo.transform.InverseTransformPoint(hit.point);
                        Debug.Log("Posición local tocada: " + localHitPosition);

                        // Detectar la zona tocada y actualizar la tarjeta
                        DetectarArea(localHitPosition);
                    }
                    else
                    {
                        // Si no tocó el suelo, ocultar la tarjeta
                        card.SetActive(false);
                    }
                }
            }
        }
    }
    void DetectarArea(Vector3 localPosition)
    {
        // Dependiendo de la posición tocada, actualizar los valores de la tarjeta
        if (localPosition.x > .403 && localPosition.x < .488 && localPosition.z > 0.016 && localPosition.z < 0.318)
        {
            Debug.Log("Tocaste CIE");
            ActualizarTarjeta("CIE", "Centro de Innovación", "Edificio principal", cieSprite);
        }
        else if (localPosition.x > -0.135 && localPosition.x < .046 && localPosition.z > -.36 && localPosition.z < -.201)
        {
            Debug.Log("Tocaste UNO");
            ActualizarTarjeta("Aulas 1", "Universidad", "Primer edificio", unoSprite);
        }
        else if (localPosition.x > -0.225 && localPosition.x < -0.112 && localPosition.z > -.128 && localPosition.z < 0.062)
        {
            Debug.Log("Tocaste BIBLIO");
            ActualizarTarjeta("Biblioteca", "Biblioteca central", "Material académico", biblioSprite);
        }
        else if (localPosition.x > -0.222 && localPosition.x < -0.120 && localPosition.z > 0.105 && localPosition.z < 0.306)
        {
            Debug.Log("Tocaste CEDETEC");
            ActualizarTarjeta("CEDETEC", "Centro de tecnología", "Laboratorios y oficinas", cedetecSprite);
        }
        else if (localPosition.x > -0.385 && localPosition.x < -0.226 && localPosition.z > -0.295 && localPosition.z < -0.166)
        {
            Debug.Log("Tocaste DOS");
            ActualizarTarjeta("Aulas 2", "Edificio dos", "Salones de clase", dosSprite);
        }
        else if (localPosition.x > -0.418 && localPosition.x < -0.276 && localPosition.z > -0.114 && localPosition.z < 0.078)
        {
            Debug.Log("Tocaste SUR");
            ActualizarTarjeta("Edificio SUR", "Área sur", "Zona de recreación", surSprite);
        }
        else
        {
            Debug.Log("Tocaste el centro o alguna otra parte");
            card.SetActive(false);  // Ocultar la tarjeta si no hay información relevante
            canvas.transform.GetChild(1).gameObject.SetActive(false); 

        }
    }

    void ActualizarTarjeta(string titulo, string descripcion1, string descripcion2, Sprite imagen)
    {
        // Actualizar el texto y la imagen de la tarjeta
        tituloText.text = titulo;
        descripcion1Text.text = descripcion1;
        descripcion2Text.text = descripcion2;
        imagenUI.sprite = imagen;

        // Mostrar la tarjeta
        canvas.transform.GetChild(1).gameObject.SetActive(true);  // Mostrar el panel
        card.SetActive(true);
    }

    // Detectar si el clic fue sobre un elemento interactivo específico de UI
    bool IsPointerOverUIElement(Vector2 touchPosition)
    {
        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            // Verificar si el objeto clicado es interactivo (como un botón o imagen importante)
            if (result.gameObject.CompareTag("Interactivo"))  // Asegúrate de que tus botones tengan la etiqueta "Interactivo"
            {
                return true; // Si se tocó un objeto interactivo, no cerrar la tarjeta
            }
        }
        return false;  // Si no tocaste nada interactivo, permitir cerrar la tarjeta
    }
}
