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

using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTMRealTimeSDK;
using QualisysRealTime.Unity;

public class _avatar_config_foot_IK : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        update_IK_knee();
    }

    void update_IK_knee()
    {
        float f_angle_left_knee = Vector3.SignedAngle(new Vector3(this.transform.GetChild(0).transform.up.x, this.transform.GetChild(0).transform.up.y, 0), new Vector3(0, 1, 0) /* up axis */, new Vector3(0, 0, 1)/* forward axis */);
        float f_angle_right_knee = Vector3.SignedAngle(new Vector3(this.transform.GetChild(1).transform.up.x, this.transform.GetChild(1).transform.up.y, 0), new Vector3(0, 1, 0) /* up axis */, new Vector3(0, 0, 1)/* forward axis */);

        if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female.GetComponent<VRIK>() != null)
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female.GetComponent<VRIK>().solver.rightLeg.swivelOffset = f_angle_right_knee;
            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female.GetComponent<VRIK>().solver.leftLeg.swivelOffset = f_angle_left_knee;
        }

        if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male.GetComponent<VRIK>() != null)
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male.GetComponent<VRIK>().solver.rightLeg.swivelOffset = f_angle_right_knee;
            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male.GetComponent<VRIK>().solver.leftLeg.swivelOffset = f_angle_left_knee;
        }
    }
}
