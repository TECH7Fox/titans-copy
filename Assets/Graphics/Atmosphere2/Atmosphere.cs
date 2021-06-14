using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Atmosphere : CustomImageEffect {

	public Transform planet;
	public float planetRadius;
	public float waterRadius;

	[Range (0, 1)]
	public float atmosphereScale = 0.2f;

	public Color color;
	public Vector4 testParams;
	ComputeBuffer buffer;
	Texture2D falloffTex;
	public Gradient falloff;
	public int gradientRes = 10;
	public int numSteps = 10;
	public Texture2D blueNoise;

	public struct Sphere {
		public Vector3 centre;
		public float radius;
		public float waterRadius;

		public static int Size {
			get {
				return sizeof (float) * 5;
			}
		}
	}

	public override Material GetMaterial () {

		// Validate inputs
		if (material == null || material.shader != shader) {
			if (shader == null) {
				shader = Shader.Find ("Unlit/Texture");
			}
			material = new Material (shader);
		}

		// Set
		Sphere sphere = new Sphere () {
			centre = planet.position,
			radius = (1 + atmosphereScale) * planetRadius,
			waterRadius = waterRadius
		};

		buffer = new ComputeBuffer (1, Sphere.Size);
		buffer.SetData (new Sphere[] { sphere });
		material.SetBuffer ("spheres", buffer);
		material.SetVector ("params", testParams);
		material.SetColor ("_Color", color);
		material.SetFloat ("planetRadius", planetRadius);

		TextureFromGradient (ref falloffTex, gradientRes, falloff);
		material.SetTexture ("_Falloff", falloffTex);
		material.SetTexture ("_BlueNoise", blueNoise);
		material.SetInt ("numSteps", numSteps);
		return material;
	}

	public static void TextureFromGradient(ref Texture2D texture, int width, Gradient gradient, FilterMode filterMode = FilterMode.Bilinear)
	{
		if (texture == null)
		{
			texture = new Texture2D(width, 1);
		}
		else if (texture.width != width)
		{
			texture.Resize(width, 1);
		}
		if (gradient == null)
		{
			gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.black, 1) },
				new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
			);
		}
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = filterMode;

		Color[] cols = new Color[width];
		for (int i = 0; i < cols.Length; i++)
		{
			float t = i / (cols.Length - 1f);
			cols[i] = gradient.Evaluate(t);
		}
		texture.SetPixels(cols);
		texture.Apply();
	}

	public override void Release () {
		buffer.Release ();
	}


}