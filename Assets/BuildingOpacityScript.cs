/*
 * Filename: BuildingOpacityScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke, David Spitler
 * 
 * Last Modified: 6/22/2012
 * 
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Building Opacity Script ensures that the Player is visible at all times, by modifying
/// the transparency of wall segments that obstruct the camera's view. To this end, all walls of
/// the model were broken into submeshes within blender, and given a separate texture. A ray is
/// cast from the camera to the Player, and any walls hit receive a new, transparent version of their
/// texture.
/// </summary>
public class BuildingOpacityScript : MonoBehaviour {
	
	/// <summary>
	/// The current Player, which will be used as the destination for the raycast.
	/// </summary>
	GameObject player;
	
	/// <summary>
	/// The renderer of the last wall hit. When a new wall is set as transparent, the old wall is given
	/// its original texture. In future iterations, we will want to make this an array of renderers, for
	/// cases when multiple walls are hit in a single raycast.
	/// </summary>
	MeshRenderer oldRenderer;
	
	/// <summary>
	/// The transparent version of the wall material.
	/// </summary>
	public Material transparentMaterial;
	
	/// <summary>
	/// The non-trasnparent version of the wall material.
	/// </summary>
	public Material originalMaterial;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null)
			player = GameObject.FindGameObjectWithTag("Player");
		
		Vector3 dest = player.transform.position;
		dest.z += 20;
		Ray ray = new Ray(gameObject.transform.position, (player.gameObject.transform.position - gameObject.transform.position).normalized);
		RaycastHit hit;
		
		//Reset old materials. This will be overriden if the renderer is still being hit.
		if(oldRenderer != null)
			oldRenderer.material = originalMaterial;
		
		int layermask = 1 << 10;
		
		if(Physics.Raycast(ray, out hit, 10000, layermask)){
			MeshCollider collider = (MeshCollider) hit.collider;
			if(collider != null){
				MeshRenderer renderer = collider.gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
				renderer.material = transparentMaterial;
				oldRenderer = renderer;
			}
		}
		
	}
}
