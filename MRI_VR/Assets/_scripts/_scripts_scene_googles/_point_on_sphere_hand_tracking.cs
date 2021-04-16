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

public class _point_on_sphere_hand_tracking : MonoBehaviour {

    private GameObject GO_sphere_center; // elbow
    private GameObject GO_current_hand_wrist;

    private GameObject GO_hand_wrist;
    private float f_sphere_radius;

    [Header("Parameters")]
    public bool b_right_hand;

    // Use this for initialization
    void Start () {
        set_avatar_male();
    }

    public void set_avatar_male()
    {
        if (b_right_hand)
        {
            GO_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHand;
            GO_sphere_center = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightForeArm;
            GO_current_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_right_hand_wrist;
        }
        else
        {
            GO_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHand;
            GO_sphere_center = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftForeArm;
            GO_current_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_left_hand_wrist;
        }
    }

    public void set_avatar_female()
    {
        if (b_right_hand)
        {
            GO_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHand;
            GO_sphere_center = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightForeArm;
            GO_current_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_right_hand_wrist;
        }

        else
        {
            GO_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHand;
            GO_sphere_center = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftForeArm;
            GO_current_hand_wrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_left_hand_wrist;
        }
    }

    // Update is called once per frame
    void Update () {

        f_sphere_radius = Vector3.Distance(GO_sphere_center.transform.position, GO_hand_wrist.transform.position);
        float f_x_position = Mathf.Clamp(GO_current_hand_wrist.transform.position.x - GO_sphere_center.transform.position.x, -f_sphere_radius, f_sphere_radius);
        float f_y_position = Mathf.Clamp(GO_current_hand_wrist.transform.position.y - GO_sphere_center.transform.position.y, -f_sphere_radius, f_sphere_radius);

        float z_position_deduced = 0;
        z_position_deduced = Mathf.Sqrt(f_sphere_radius * f_sphere_radius - f_x_position * f_x_position - f_y_position * f_y_position);
        if (!float.IsNaN(z_position_deduced))
        {
            this.transform.position = new Vector3(GO_sphere_center.transform.position.x + f_x_position, GO_sphere_center.transform.position.y + f_y_position, GO_sphere_center.transform.position.z + z_position_deduced);
        }
        
	}

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GO_sphere_center.transform.position, f_sphere_radius);
    }*/
}
