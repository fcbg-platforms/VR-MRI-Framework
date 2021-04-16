/*  
    Copyright (C) <2020>  <Louis Albert>
   
    Author: Louis Albert -- <vr@fcbg.ch>
   
    This file is part of VR-MRI Framework.

    VR-MRI Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VR-MRI Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see<https://www.gnu.org/licenses/>.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_mesh_deformation : MonoBehaviour
{
	public float f_lenght = 100;

	//public float force = 10f;
	public float forceOffset = 0.1f;


	void Update()
	{
		/*if (Input.GetMouseButton(0))
		{
			HandleInput();
		}*/
		_handle_input_mesh_deformation();
	}

	/*void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit))
		{
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer)
			{
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);

				Debug.Log(hit.point);
			}
		}
	}*/

	void _handle_input_mesh_deformation()
	{
		Ray inputRay = new Ray(transform.position,Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit, f_lenght))
		{
			f_current_force = f_lenght - hit.distance;

            _character_mesh_deformer deformer = hit.collider.GetComponent<_character_mesh_deformer>();
			if (deformer)
			{				
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, f_current_force * f_current_force_multiplier);
			}
		}
	}


    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector3.down * f_lenght);
	}

	//public float f_debug_distance_hit;
	public float f_current_force;
	public float f_current_force_multiplier = 1;
}
