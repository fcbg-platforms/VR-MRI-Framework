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

public class _camera_manager : MonoBehaviour {

    [Header("Parameters")]
    public float f_stereo_convergence;
    public float f_stereo_separation;

	// Use this for initialization
	void Start () {
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.localPosition.x + f_stereo_separation / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.localPosition.y, _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.localPosition.z);
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.localPosition.x - f_stereo_separation / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.localPosition.y, _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.localPosition.z);
    }
}
