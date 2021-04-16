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

public class _rotate_object_to_angle_rb : MonoBehaviour {
    [Header("Parameters")]
    public Vector3 distance_between_start_position_and_end_position = new Vector3(0, 0, -1.234f);
    public Quaternion this_start_rotation; // virtual start position
    public Quaternion this_end_rotation; // virtual end position

    private Vector3 rb_start_position;
    private Vector3 rb_end_position;

	private Vector3 rb_start_position_outside_to_go_inside;
	//private Vector3 rb_start_position_inside_to_go_outside;

    public bool RotateFromAToB = false;
    public bool RotateFromBToA = false;

    public void RotateFreely()
	{
		if (rb_start_position == new Vector3(0, 0, 0))
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

		RotateFromBToA = true;
	}



	public void RotateAtoB()
    {
        rb_start_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
        //rb_start_position_outside_to_go_inside = rb_start_position; // for the rotate freely
        rb_end_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position - distance_between_start_position_and_end_position;

        RotateFromBToA = false;
		RotateFromAToB = true;
    }

    public void RotateBtoA()
    {
        rb_start_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
        //rb_start_position_inside_to_go_outside = rb_start_position; // for the rotate freely
        rb_end_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position + distance_between_start_position_and_end_position;

        RotateFromAToB = false;
		RotateFromBToA = true;
    }

    void Update()
    {
        if (RotateFromAToB)
        {
            Vector3 rb_current_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
            float t = Vector3.Distance(rb_start_position, rb_current_position) / Vector3.Distance(rb_start_position, rb_end_position);

            this.transform.rotation = Quaternion.Lerp(this_start_rotation, this_end_rotation, t);
        }
        if (RotateFromBToA)
        {
            Vector3 rb_current_position = _class_all_references_scene_mri_compatible_googles.Instance.GO_rb_RBTable.transform.position;
            float t = Vector3.Distance(rb_start_position, rb_current_position) / Vector3.Distance(rb_start_position, rb_end_position);

            this.transform.rotation = Quaternion.Lerp(this_end_rotation, this_start_rotation, t);
        }
    }
}
