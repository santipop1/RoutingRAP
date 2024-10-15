using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class MapboxTouchControls : MonoBehaviour
{
    public AbstractMap map; // El mapa de Mapbox
    public float moveSpeed = 0.0005f; // Velocidad del movimiento
    public float zoomSpeed = 0.01f; // Velocidad del zoom
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private float previousDistanceBetweenTouches;

    void Update()
    {
        // Verificar cuántos toques hay en la pantalla
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                HandleTouchMovement(); // Manejo del movimiento con un toque
            }
            else if (Input.touchCount == 2)
            {
                HandlePinchToZoom(); // Manejo del zoom con dos toques
            }
        }
    }

    void HandleTouchMovement()
    {
        // Obtiene el primer toque
        Touch touch = Input.GetTouch(0);

        // Si el dedo se está moviendo
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 currentTouchPosition = touch.position;

            if (isDragging)
            {
                // Calcula el desplazamiento del dedo
                Vector2 delta = currentTouchPosition - lastTouchPosition;

                // Convertimos el delta del toque en un movimiento en las coordenadas del mapa usando Vector2d
                Vector2d mapMove = new Vector2d(delta.y * moveSpeed, delta.x * moveSpeed);

                // Actualizamos la posición del centro del mapa
                map.UpdateMap(map.CenterLatitudeLongitude + mapMove);
            }

            lastTouchPosition = currentTouchPosition;
            isDragging = true;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;
        }
    }

    void HandlePinchToZoom()
    {
        // Obtén los dos toques
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        // Calcula la distancia actual entre los dos toques
        float currentDistance = Vector2.Distance(touch1.position, touch2.position);

        if (previousDistanceBetweenTouches != 0)
        {
            // Calcula la diferencia de distancia para hacer zoom
            float distanceDifference = currentDistance - previousDistanceBetweenTouches;

            // Actualizamos el zoom del mapa, manteniéndolo dentro de un rango
            float newZoom = Mathf.Clamp(map.Zoom + (distanceDifference * zoomSpeed), 2.0f, 18.0f);

            map.UpdateMap(map.CenterLatitudeLongitude, newZoom);
        }

        previousDistanceBetweenTouches = currentDistance;
    }
}
