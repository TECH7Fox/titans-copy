using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereEffect {

	public Light light;
	protected Material material;

	public void UpdateSettings (Transform planet, float planetRadius, float waterRadius, AtmosphereSettings atmosphereSettings) {

		Shader shader = atmosphereSettings.atmosphereShader;

		if (material == null || material.shader != shader) {
			material = new Material (shader);
		}

		if (light == null) {
			light = GameObject.FindGameObjectWithTag("Directional Light").GetComponent<Light> ();
		}

		//generator.shading.SetAtmosphereProperties (material);
		atmosphereSettings.SetProperties (material, planetRadius);

		material.SetVector ("planetCentre", planet.position);
		//material.SetFloat ("atmosphereRadius", (1 + 0.5f) * generator.BodyScale);
		material.SetFloat ("oceanRadius", waterRadius);

		if (light) {
			Vector3 dirFromPlanetToSun = (light.transform.position - planet.position).normalized;
			//Debug.Log(dirFromPlanetToSun);
			material.SetVector ("dirToSun", dirFromPlanetToSun);
		} else {
			material.SetVector ("dirToSun", Vector3.up);
			Debug.Log ("No SunShadowCaster found");
		}
	}

	public Material GetMaterial () {
		return material;
	}
}