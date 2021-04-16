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
using UnityEngine.UI;

public class _avatar_config_add_track_hand_rigid_bodies_real_wrist_offset : MonoBehaviour {

    [Header("Avatar Male References")]
    private GameObject HandTargets_RightHand;
    private GameObject HandTargets_RightHand_RightHandWrist;
    private GameObject HandTargets_LeftHand;
    private GameObject HandTargets_LeftHand_LeftHandWrist;

    [Header("Parameters")]
    public float OffsetHandRigidBodyRealWrist = 15; //15cm

    // Use this for initialization
    void Start () {
        InitialiseGameObjects();
        SetWristOffset();
        ToogleDebug(false);
    }

    public void ToogleDebug(bool show)
    {
        HandTargets_RightHand.GetComponent<MeshRenderer>().enabled = show;
        HandTargets_RightHand_RightHandWrist.GetComponent<MeshRenderer>().enabled = show;
        HandTargets_LeftHand.GetComponent<MeshRenderer>().enabled = show;
        HandTargets_LeftHand_LeftHandWrist.GetComponent<MeshRenderer>().enabled = show;
    }

    void InitialiseGameObjects()
    {
        HandTargets_RightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand;
        HandTargets_RightHand_RightHandWrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_right_hand_wrist;
        HandTargets_LeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand;
        HandTargets_LeftHand_LeftHandWrist = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_left_hand_wrist;
    }

    void SetWristOffset()
    {
        HandTargets_RightHand_RightHandWrist.transform.localPosition = new Vector3(0, OffsetHandRigidBodyRealWrist, 0);
        HandTargets_LeftHand_LeftHandWrist.transform.localPosition = new Vector3(0, OffsetHandRigidBodyRealWrist, 0);
    }
}
