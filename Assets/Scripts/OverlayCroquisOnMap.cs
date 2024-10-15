using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class OverlayCroquisOnMap : MonoBehaviour
{
    public AbstractMap map;  // Referencia al mapa de Mapbox
    public GameObject croquis;  // GameObject que contiene el croquis (SpriteRenderer)
    public Vector2d croquisCoordinates;  // Coordenadas geogr�ficas donde quieres colocar el croquis
    public float scaleFactor = 1.0f;  // Factor de escala para el croquis

    void Start()
    {
        // Posicionar el croquis en las coordenadas indicadas
        PositionCroquisOnMap();
    }

    void Update()
    {
        // Actualiza la posici�n del croquis si el mapa se mueve o cambia
        PositionCroquisOnMap();
    }

    void PositionCroquisOnMap()
    {
        // Convertir las coordenadas geogr�ficas en una posici�n del mundo en Unity
        Vector3 croquisPosition = map.GeoToWorldPosition(croquisCoordinates, true);

        // Mover el croquis a la posici�n calculada
        croquis.transform.position = croquisPosition;

        // Ajustar la escala del croquis seg�n el zoom del mapa
        float mapZoomFactor = Mathf.Pow(2, (map.Zoom - 16)); // Escalar basado en zoom
        croquis.transform.localScale = Vector3.one * scaleFactor * mapZoomFactor;
    }
}
