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

public class _avatar_config_set_transform_tennis_balls : MonoBehaviour {

    private GameObject TennisBallLeftHand;
    private GameObject TennisBallRightHand;
    private GameObject TargetMaleTennisBallLeftHand;
    private GameObject TargetMaleTennisBallRightHand;
    private GameObject TargetFemaleTennisBallLeftHand;
    private GameObject TargetFemaleTennisBallRightHand;
    private bool IsMale = true;

    // Use this for initialization
    void Start () {
        TennisBallLeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_tennis_ball_left_hand;
        TennisBallRightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_tennis_ball_right_hand;
        TargetMaleTennisBallLeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_target_male_tennis_ball_left_hand;
        TargetMaleTennisBallRightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_target_male_tennis_ball_right_hand;
        TargetFemaleTennisBallLeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_target_female_tennis_ball_left_hand;
        TargetFemaleTennisBallRightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_target_female_tennis_ball_right_hand;

    }
	
	// Update is called once per frame
	void Update () {
        if (IsMale)
        {
            TennisBallLeftHand.transform.position = TargetMaleTennisBallLeftHand.transform.position;
            TennisBallLeftHand.transform.rotation = TargetMaleTennisBallLeftHand.transform.rotation;
            TennisBallRightHand.transform.position = TargetMaleTennisBallRightHand.transform.position;
            TennisBallRightHand.transform.rotation = TargetMaleTennisBallRightHand.transform.rotation;
        }
        else
        {
            TennisBallLeftHand.transform.position = TargetFemaleTennisBallLeftHand.transform.position;
            TennisBallLeftHand.transform.rotation = TargetFemaleTennisBallLeftHand.transform.rotation;
            TennisBallRightHand.transform.position = TargetFemaleTennisBallRightHand.transform.position;
            TennisBallRightHand.transform.rotation = TargetFemaleTennisBallRightHand.transform.rotation;

        }

    }

    // If sex choosen = Male
    public void SetIsMaleTrue()
    {
        IsMale = true;
    }

    // If sex choosen = Female
    public void SetIsMaleFalse()
    {
        IsMale = false;
    }

	public void activate_tennis_balls_mesh_renderer()
	{
		TennisBallLeftHand.gameObject.GetComponent<MeshRenderer>().enabled = true;
		TennisBallRightHand.gameObject.GetComponent<MeshRenderer>().enabled = true;
	}
	
	public void desactivate_tennis_balls_mesh_renderer()
	{
		TennisBallLeftHand.gameObject.GetComponent<MeshRenderer>().enabled = false;
		TennisBallRightHand.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
}
