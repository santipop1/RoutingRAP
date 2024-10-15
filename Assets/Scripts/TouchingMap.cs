namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;

    public class TouchingMap : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 20)]
        public float _panSpeed = 1.0f;

        [SerializeField]
        float _zoomSpeed = 0.25f;

        [SerializeField]
        public Camera _referenceCamera;

        [SerializeField]
        AbstractMap _mapManager;

        [SerializeField]
        bool _useDegreeMethod;

        private Vector3 _origin;
        private Vector3 _mousePosition;
        private Vector3 _mousePositionPrevious;
        private bool _shouldDrag;
        private bool _isInitialized = false;
        private Plane _groundPlane = new Plane(Vector3.up, 0);
        private bool _dragStartedOnUI = false;

        void Awake()
        {
            if (null == _referenceCamera)
            {
                _referenceCamera = GetComponent<Camera>();
                if (null == _referenceCamera) { Debug.LogErrorFormat("{0}: reference camera not set", this.GetType().Name); }
            }
            _mapManager.OnInitialized += () =>
            {
                _isInitialized = true;
            };
        }

        void Update()
        {
            // En dispositivos móviles, manejar el zoom y el desplazamiento mediante toques
            if (!_isInitialized) { return; }

            if (Input.touchCount > 0)
            {
                HandleTouch();
            }
        }

        void HandleTouch()
        {
            if (Input.touchCount == 1 && !IsTouchingUI())  // Un solo toque para desplazar (pan)
            {
                PanMapUsingTouch();
            }
            else if (Input.touchCount == 2 && !IsTouchingUI())  // Dos toques para hacer zoom (pinch)
            {
                ZoomMapUsingPinch();
            }
        }

        // Método para desplazamiento (pan) usando un solo toque
        void PanMapUsingTouch()
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _origin = GetGroundPlaneHitPoint(touch.position);
                _shouldDrag = true;
            }
            else if (touch.phase == TouchPhase.Moved && _shouldDrag)
            {
                Vector3 touchPosition = GetGroundPlaneHitPoint(touch.position);
                Vector3 offset = _origin - touchPosition;

                if (_mapManager != null && (Mathf.Abs(offset.x) > 0.0f || Mathf.Abs(offset.z) > 0.0f))
                {
                    float factor = _panSpeed * Conversions.GetTileScaleInMeters((float)0, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;
                    var latlongDelta = Conversions.MetersToLatLon(new Vector2d(offset.x * factor, offset.z * factor));
                    var newLatLong = _mapManager.CenterLatitudeLongitude + latlongDelta;
                    _mapManager.UpdateMap(newLatLong, _mapManager.Zoom);
                }
                _origin = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _shouldDrag = false;
            }
        }

        // Método para hacer zoom con dos dedos (pinch)
        void ZoomMapUsingPinch()
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Encuentra la posición de los toques en el cuadro anterior
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Calcula la distancia entre los toques en cada cuadro
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Calcula la diferencia en la distancia entre ambos toques
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Aplicar zoom
            float zoomFactor = deltaMagnitudeDiff * _zoomSpeed * 0.01f;
            ZoomMapUsingFactor(zoomFactor);
        }

        // Método que realiza el zoom en el mapa con un factor de zoom
        void ZoomMapUsingFactor(float zoomFactor)
        {
            float zoom = Mathf.Max(0.0f, Mathf.Min(_mapManager.Zoom - zoomFactor, 21.0f));
            if (Mathf.Abs(zoom - _mapManager.Zoom) > 0.0f)
            {
                _mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, zoom);
            }
        }

        // Método que detecta si el toque es sobre la UI (para evitar interactuar con el mapa cuando tocas la UI)
        bool IsTouchingUI()
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return true;
            }
            return false;
        }

        // Método para obtener el punto de impacto en el plano de suelo a partir de un toque
        Vector3 GetGroundPlaneHitPoint(Vector2 touchPosition)
        {
            Ray ray = _referenceCamera.ScreenPointToRay(touchPosition);
            float distance;
            if (_groundPlane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }
    }
}
