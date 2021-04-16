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

public class _move_object_from_A_to_B_rb : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 distance_between_start_position_and_end_position;
    public Vector3 this_start_position; // virtual start position
    public Vector3 this_end_position; // virtual end position

    public Vector3 rb_start_position;
    public Vector3 rb_end_position;

	private Vector3 rb_start_position_outside_to_go_inside;
	//private Vector3 rb_start_position_inside_to_go_outside;

    public bool MoveFromAToB = false;
    public bool MoveFromBToA = false;

	public void MoveFreely()
	{
		if (rb_start_position == new Vector3(0,0,0))
		{
			rb_start_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
			rb_start_position_outside_to_go_inside = rb_start_position;
		}
		else
		{
			rb_start_position = rb_start_position_outside_to_go_inside;
		}

		rb_end_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position - distance_between_start_position_and_end_position;
		//rb_start_position_inside_to_go_outside = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position - distance_between_start_position_and_end_position;

		MoveFromAToB = true;
	}





	public void MoveAtoB()
    {
        rb_start_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
		rb_start_position_outside_to_go_inside = rb_start_position; // for the move freely
		rb_end_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position - distance_between_start_position_and_end_position;

		MoveFromBToA = false;
		MoveFromAToB = true;
    }

    public void MoveBtoA()
    {
        rb_start_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
		//rb_start_position_inside_to_go_outside = rb_start_position; // for the move freely
		rb_end_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position + distance_between_start_position_and_end_position;

		MoveFromAToB = false;
		MoveFromBToA = true;
    }

    void Update()
    {
        if (MoveFromAToB)
        {
            Vector3 rb_current_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
            float t = Vector3.Distance(rb_start_position, rb_current_position)/ Vector3.Distance(rb_start_position, rb_end_position);
            this.transform.position = Vector3.Lerp(this_start_position, this_end_position, t);
        }
        if (MoveFromBToA)
        {
            Vector3 rb_current_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
            float t = Vector3.Distance(rb_start_position, rb_current_position) / Vector3.Distance(rb_start_position, rb_end_position);
            this.transform.position = Vector3.Lerp(this_end_position, this_start_position, t);
        }
    }
}
