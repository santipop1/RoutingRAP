namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		void Start()
		{
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
                //var instance = Instantiate(_markerPrefab);
                _markerPrefab.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                _markerPrefab.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(_markerPrefab);
			}
		}

        private void Update()
        {
            int count = _spawnedObjects.Count;
            float zoomFactor = Mathf.Pow(2, _map.Zoom - 17); // Cálculo del factor basado en el zoom

            for (int i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                var location = _locations[i];

                // Actualizar la posición del objeto en el mapa
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);

                // Actualizar la escala del croquis en función del zoom del mapa
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale) * zoomFactor;
            }
        }

    }
}