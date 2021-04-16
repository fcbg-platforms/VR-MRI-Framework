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

#pragma warning disable 0414 //The private field '' is assigned but its value is never used

public class _avatar_config_IK : MonoBehaviour {

    public bool b_track_avatar;

    [Header("Avatar Male References")]
    private GameObject AvatarMale;
    private GameObject AvatarMaleHips;
    private GameObject AvatarMaleLeftUpLeg;
    private GameObject AvatarMaleLeftLeg;
    private GameObject AvatarMaleLeftFoot;
    private GameObject AvatarMaleLeftToeBase;
    private GameObject AvatarMaleRightUpLeg;
    private GameObject AvatarMaleRightLeg;
    private GameObject AvatarMaleRightFoot;
    private GameObject AvatarMaleRightToeBase;
    private GameObject AvatarMaleSpine;
    private GameObject AvatarMaleSpine1;
    private GameObject AvatarMaleSpine2;
    private GameObject AvatarMaleLeftShoulder;
    private GameObject AvatarMaleLeftArm;
    private GameObject AvatarMaleLeftForeArm;
    private GameObject AvatarMaleLeftHand;
    private GameObject AvatarMaleLeftHandIndex1;
    private GameObject AvatarMaleLeftHandIndex2;
    private GameObject AvatarMaleLeftHandIndex3;
    private GameObject AvatarMaleLeftHandMiddle1;
    private GameObject AvatarMaleLeftHandMiddle2;
    private GameObject AvatarMaleLeftHandMiddle3;
    private GameObject AvatarMaleLeftHandPinky1;
    private GameObject AvatarMaleLeftHandPinky2;
    private GameObject AvatarMaleLeftHandPinky3;
    private GameObject AvatarMaleLeftHandRing1;
    private GameObject AvatarMaleLeftHandRing2;
    private GameObject AvatarMaleLeftHandRing3;
    private GameObject AvatarMaleLeftHandThumb1;
    private GameObject AvatarMaleLeftHandThumb2;
    private GameObject AvatarMaleLeftHandThumb3;
    private GameObject AvatarMaleNeck;
    private GameObject AvatarMaleHead;
    private GameObject AvatarMaleRightShoulder;
    private GameObject AvatarMaleRightArm;
    private GameObject AvatarMaleRightForeArm;
    private GameObject AvatarMaleRightHand;
    private GameObject AvatarMaleRightHandIndex1;
    private GameObject AvatarMaleRightHandIndex2;
    private GameObject AvatarMaleRightHandIndex3;
    private GameObject AvatarMaleRightHandMiddle1;
    private GameObject AvatarMaleRightHandMiddle2;
    private GameObject AvatarMaleRightHandMiddle3;
    private GameObject AvatarMaleRightHandPinky1;
    private GameObject AvatarMaleRightHandPinky2;
    private GameObject AvatarMaleRightHandPinky3;
    private GameObject AvatarMaleRightHandRing1;
    private GameObject AvatarMaleRightHandRing2;
    private GameObject AvatarMaleRightHandRing3;
    private GameObject AvatarMaleRightHandThumb1;
    private GameObject AvatarMaleRightHandThumb2;
    private GameObject AvatarMaleRightHandThumb3;

    [Header("Avatar Female References")]
    private GameObject AvatarFemale;
    private GameObject AvatarFemaleHips;
    private GameObject AvatarFemaleLeftUpLeg;
    private GameObject AvatarFemaleLeftLeg;
    private GameObject AvatarFemaleLeftFoot;
    private GameObject AvatarFemaleLeftToeBase;
    private GameObject AvatarFemaleRightUpLeg;
    private GameObject AvatarFemaleRightLeg;
    private GameObject AvatarFemaleRightFoot;
    private GameObject AvatarFemaleRightToeBase;
    private GameObject AvatarFemaleSpine;
    private GameObject AvatarFemaleSpine1;
    private GameObject AvatarFemaleSpine2;
    private GameObject AvatarFemaleLeftShoulder;
    private GameObject AvatarFemaleLeftArm;
    private GameObject AvatarFemaleLeftForeArm;
    private GameObject AvatarFemaleLeftHand;
    private GameObject AvatarFemaleLeftHandIndex1;
    private GameObject AvatarFemaleLeftHandIndex2;
    private GameObject AvatarFemaleLeftHandIndex3;
    private GameObject AvatarFemaleLeftHandMiddle1;
    private GameObject AvatarFemaleLeftHandMiddle2;
    private GameObject AvatarFemaleLeftHandMiddle3;
    private GameObject AvatarFemaleLeftHandPinky1;
    private GameObject AvatarFemaleLeftHandPinky2;
    private GameObject AvatarFemaleLeftHandPinky3;
    private GameObject AvatarFemaleLeftHandRing1;
    private GameObject AvatarFemaleLeftHandRing2;
    private GameObject AvatarFemaleLeftHandRing3;
    private GameObject AvatarFemaleLeftHandThumb1;
    private GameObject AvatarFemaleLeftHandThumb2;
    private GameObject AvatarFemaleLeftHandThumb3;
    private GameObject AvatarFemaleNeck;
    private GameObject AvatarFemaleHead;
    private GameObject AvatarFemaleRightShoulder;
    private GameObject AvatarFemaleRightArm;
    private GameObject AvatarFemaleRightForeArm;
    private GameObject AvatarFemaleRightHand;
    private GameObject AvatarFemaleRightHandIndex1;
    private GameObject AvatarFemaleRightHandIndex2;
    private GameObject AvatarFemaleRightHandIndex3;
    private GameObject AvatarFemaleRightHandMiddle1;
    private GameObject AvatarFemaleRightHandMiddle2;
    private GameObject AvatarFemaleRightHandMiddle3;
    private GameObject AvatarFemaleRightHandPinky1;
    private GameObject AvatarFemaleRightHandPinky2;
    private GameObject AvatarFemaleRightHandPinky3;
    private GameObject AvatarFemaleRightHandRing1;
    private GameObject AvatarFemaleRightHandRing2;
    private GameObject AvatarFemaleRightHandRing3;
    private GameObject AvatarFemaleRightHandThumb1;
    private GameObject AvatarFemaleRightHandThumb2;
    private GameObject AvatarFemaleRightHandThumb3;

    [Header("Hands Goals")]
    private GameObject HandTargets_RightHand;
    private GameObject HandTargets_LeftHand;
    private GameObject HandTargets_RightHand_RightHandWrist;
    private GameObject HandTargets_LeftHand_LeftHandWrist;

    // Use this for initialization
    void Start () {
        GetAvatarMaleReferences();
        GetAvatarFemaleReferences();
        GetHandTargetsReferences();
        SetIKMale();
        SetIKFemale();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void GetAvatarMaleReferences()
    {
        AvatarMale = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male;
        AvatarMaleHips = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Hips;
        AvatarMaleLeftUpLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftUpLeg;
        AvatarMaleLeftLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftLeg;
        AvatarMaleLeftFoot = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftFoot;
        AvatarMaleLeftToeBase = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftToeBase;
        AvatarMaleRightUpLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightUpLeg;
        AvatarMaleRightLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightLeg;
        AvatarMaleRightFoot = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightFoot;
        AvatarMaleRightToeBase = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightToeBase;
        AvatarMaleSpine = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Spine;
        AvatarMaleSpine1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Spine1;
        AvatarMaleSpine2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Spine2;
        AvatarMaleLeftShoulder = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftShoulder;
        AvatarMaleLeftArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftArm;
        AvatarMaleLeftForeArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftForeArm;
        AvatarMaleLeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHand;
        AvatarMaleLeftHandIndex1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandIndex1;
        AvatarMaleLeftHandIndex2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandIndex2;
        AvatarMaleLeftHandIndex3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandIndex3;
        AvatarMaleLeftHandMiddle1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandMiddle1;
        AvatarMaleLeftHandMiddle2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandMiddle2;
        AvatarMaleLeftHandMiddle3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandMiddle3;
        AvatarMaleLeftHandPinky1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandPinky1;
        AvatarMaleLeftHandPinky2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandPinky2;
        AvatarMaleLeftHandPinky3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandPinky3;
        AvatarMaleLeftHandRing1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandRing1;
        AvatarMaleLeftHandRing2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandRing2;
        AvatarMaleLeftHandRing3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandRing3;
        AvatarMaleLeftHandThumb1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandThumb1;
        AvatarMaleLeftHandThumb2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandThumb2;
        AvatarMaleLeftHandThumb3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_LeftHandThumb3;
        AvatarMaleNeck = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Neck;
        AvatarMaleHead = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_Head;
        AvatarMaleRightShoulder = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightShoulder;
        AvatarMaleRightArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightArm;
        AvatarMaleRightForeArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightForeArm;
        AvatarMaleRightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHand;
        AvatarMaleRightHandIndex1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandIndex1;
        AvatarMaleRightHandIndex2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandIndex2;
        AvatarMaleRightHandIndex3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandIndex3;
        AvatarMaleRightHandMiddle1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandMiddle1;
        AvatarMaleRightHandMiddle2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandMiddle2;
        AvatarMaleRightHandMiddle3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandMiddle3;
        AvatarMaleRightHandPinky1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandPinky1;
        AvatarMaleRightHandPinky2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandPinky2;
        AvatarMaleRightHandPinky3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandPinky3;
        AvatarMaleRightHandRing1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandRing1;
        AvatarMaleRightHandRing2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandRing2;
        AvatarMaleRightHandRing3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandRing3;
        AvatarMaleRightHandThumb1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandThumb1;
        AvatarMaleRightHandThumb2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandThumb2;
        AvatarMaleRightHandThumb3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male_RightHandThumb3;
    }

    void GetAvatarFemaleReferences()
    {
        AvatarFemale = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female;
        AvatarFemaleHips = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Hips;
        AvatarFemaleLeftUpLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftUpLeg;
        AvatarFemaleLeftLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftLeg;
        AvatarFemaleLeftFoot = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftFoot;
        AvatarFemaleLeftToeBase = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftToeBase;
        AvatarFemaleRightUpLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightUpLeg;
        AvatarFemaleRightLeg = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightLeg;
        AvatarFemaleRightFoot = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightFoot;
        AvatarFemaleRightToeBase = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightToeBase;
        AvatarFemaleSpine = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Spine;
        AvatarFemaleSpine1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Spine1;
        AvatarFemaleSpine2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Spine2;
        AvatarFemaleLeftShoulder = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftShoulder;
        AvatarFemaleLeftArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftArm;
        AvatarFemaleLeftForeArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftForeArm;
        AvatarFemaleLeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHand;
        AvatarFemaleLeftHandIndex1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandIndex1;
        AvatarFemaleLeftHandIndex2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandIndex2;
        AvatarFemaleLeftHandIndex3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandIndex3;
        AvatarFemaleLeftHandMiddle1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandMiddle1;
        AvatarFemaleLeftHandMiddle2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandMiddle2;
        AvatarFemaleLeftHandMiddle3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandMiddle3;
        AvatarFemaleLeftHandPinky1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandPinky1;
        AvatarFemaleLeftHandPinky2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandPinky2;
        AvatarFemaleLeftHandPinky3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandPinky3;
        AvatarFemaleLeftHandRing1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandRing1;
        AvatarFemaleLeftHandRing2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandRing2;
        AvatarFemaleLeftHandRing3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandRing3;
        AvatarFemaleLeftHandThumb1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandThumb1;
        AvatarFemaleLeftHandThumb2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandThumb2;
        AvatarFemaleLeftHandThumb3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_LeftHandThumb3;
        AvatarFemaleNeck = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Neck;
        AvatarFemaleHead = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_Head;
        AvatarFemaleRightShoulder = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightShoulder;
        AvatarFemaleRightArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightArm;
        AvatarFemaleRightForeArm = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightForeArm;
        AvatarFemaleRightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHand;
        AvatarFemaleRightHandIndex1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandIndex1;
        AvatarFemaleRightHandIndex2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandIndex2;
        AvatarFemaleRightHandIndex3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandIndex3;
        AvatarFemaleRightHandMiddle1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandMiddle1;
        AvatarFemaleRightHandMiddle2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandMiddle2;
        AvatarFemaleRightHandMiddle3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandMiddle3;
        AvatarFemaleRightHandPinky1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandPinky1;
        AvatarFemaleRightHandPinky2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandPinky2;
        AvatarFemaleRightHandPinky3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandPinky3;
        AvatarFemaleRightHandRing1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandRing1;
        AvatarFemaleRightHandRing2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandRing2;
        AvatarFemaleRightHandRing3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandRing3;
        AvatarFemaleRightHandThumb1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandThumb1;
        AvatarFemaleRightHandThumb2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandThumb2;
        AvatarFemaleRightHandThumb3 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female_RightHandThumb3;
    }

    void GetHandTargetsReferences()
    {
        if (!b_track_avatar)
        {
            return;
        }

        HandTargets_RightHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.gameObject.transform.GetChild(0).gameObject;
        HandTargets_LeftHand = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.gameObject.transform.GetChild(1).gameObject;
        HandTargets_RightHand_RightHandWrist = HandTargets_RightHand.gameObject.transform.GetChild(1).gameObject;
        HandTargets_LeftHand_LeftHandWrist = HandTargets_LeftHand.gameObject.transform.GetChild(1).gameObject;
    }

    void SetIKMale()
    {
        if (!b_track_avatar)
        {
            return;
        }

        // add VRIK component
        AvatarMale.gameObject.AddComponent<VRIK>();

        AvatarMale.GetComponent<VRIK>().references.root = AvatarMale.transform;
        AvatarMale.GetComponent<VRIK>().references.pelvis = AvatarMaleHips.transform;
        AvatarMale.GetComponent<VRIK>().references.spine = AvatarMaleSpine.transform;
        AvatarMale.GetComponent<VRIK>().references.chest = AvatarMaleSpine1.transform;
        AvatarMale.GetComponent<VRIK>().references.neck = AvatarMaleNeck.transform;
        AvatarMale.GetComponent<VRIK>().references.head = AvatarMaleHead.transform;
        AvatarMale.GetComponent<VRIK>().references.leftShoulder = AvatarMaleLeftShoulder.transform;
        AvatarMale.GetComponent<VRIK>().references.leftUpperArm = AvatarMaleLeftArm.transform;
        AvatarMale.GetComponent<VRIK>().references.leftForearm = AvatarMaleLeftForeArm.transform;
        AvatarMale.GetComponent<VRIK>().references.leftHand = AvatarMaleLeftHand.transform;
        AvatarMale.GetComponent<VRIK>().references.rightShoulder = AvatarMaleRightShoulder.transform;
        AvatarMale.GetComponent<VRIK>().references.rightUpperArm = AvatarMaleRightArm.transform;
        AvatarMale.GetComponent<VRIK>().references.rightForearm = AvatarMaleRightForeArm.transform;
        AvatarMale.GetComponent<VRIK>().references.rightHand = AvatarMaleRightHand.transform;
        AvatarMale.GetComponent<VRIK>().references.leftThigh = AvatarMaleLeftUpLeg.transform;
        AvatarMale.GetComponent<VRIK>().references.leftCalf = AvatarMaleLeftLeg.transform;
        AvatarMale.GetComponent<VRIK>().references.leftFoot = AvatarMaleLeftFoot.transform;
        AvatarMale.GetComponent<VRIK>().references.leftToes = AvatarMaleLeftToeBase.transform;
        AvatarMale.GetComponent<VRIK>().references.rightThigh = AvatarMaleRightUpLeg.transform;
        AvatarMale.GetComponent<VRIK>().references.rightCalf = AvatarMaleRightLeg.transform;
        AvatarMale.GetComponent<VRIK>().references.rightFoot = AvatarMaleRightFoot.transform;
        AvatarMale.GetComponent<VRIK>().references.rightToes = AvatarMaleRightToeBase.transform;


        AvatarMale.GetComponent<VRIK>().solver.IKPositionWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.plantFeet = false;

        AvatarMale.GetComponent<VRIK>().solver.spine.headTarget = null;
        AvatarMale.GetComponent<VRIK>().solver.spine.pelvisTarget = null;
        AvatarMale.GetComponent<VRIK>().solver.spine.positionWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.rotationWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.pelvisPositionWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.pelvisRotationWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.chestGoal = null;
        AvatarMale.GetComponent<VRIK>().solver.spine.chestGoalWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.minHeadHeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.bodyPosStiffness = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.bodyRotStiffness = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.neckStiffness = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.rotateChestByHands = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.chestClampWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.headClampWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.moveBodyBackWhenCrouching = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.maintainPelvisPosition = 0;
        AvatarMale.GetComponent<VRIK>().solver.spine.maxRootAngle = 0;

        AvatarMale.GetComponent<VRIK>().solver.leftArm.target = HandTargets_LeftHand_LeftHandWrist.transform;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.bendGoal = null;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.positionWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.rotationWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.YawPitch;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.shoulderRotationWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.bendGoalWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.swivelOffset = 0;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.wristToPalmAxis = new Vector3(0, 1, 0);
        AvatarMale.GetComponent<VRIK>().solver.leftArm.palmToThumbAxis = new Vector3(1, 0, 0);
        AvatarMale.GetComponent<VRIK>().solver.leftArm.armLengthMlp = 1;
        AvatarMale.GetComponent<VRIK>().solver.leftArm.stretchCurve = new AnimationCurve();

        AvatarMale.GetComponent<VRIK>().solver.rightArm.target = HandTargets_RightHand_RightHandWrist.transform;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.bendGoal = null;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.positionWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.rotationWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.YawPitch;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.shoulderRotationWeight = 1;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.bendGoalWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.swivelOffset = 0;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.wristToPalmAxis = new Vector3(0, 1, 0);
        AvatarMale.GetComponent<VRIK>().solver.rightArm.palmToThumbAxis = new Vector3(1, 0, 0);
        AvatarMale.GetComponent<VRIK>().solver.rightArm.armLengthMlp = 1;
        AvatarMale.GetComponent<VRIK>().solver.rightArm.stretchCurve = new AnimationCurve();

        AvatarMale.GetComponent<VRIK>().solver.leftLeg.target = null;
        AvatarMale.GetComponent<VRIK>().solver.leftLeg.bendGoal = null;
        AvatarMale.GetComponent<VRIK>().solver.leftLeg.positionWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.leftLeg.rotationWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.leftLeg.bendGoalWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.leftLeg.swivelOffset = 0;

        AvatarMale.GetComponent<VRIK>().solver.rightLeg.target = null;
        AvatarMale.GetComponent<VRIK>().solver.rightLeg.bendGoal = null;
        AvatarMale.GetComponent<VRIK>().solver.rightLeg.positionWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.rightLeg.rotationWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.rightLeg.bendGoalWeight = 0;
        AvatarMale.GetComponent<VRIK>().solver.rightLeg.swivelOffset = 0;

        AvatarMale.GetComponent<VRIK>().solver.locomotion.weight = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.footDistance = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.stepThreshold = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.angleThreshold = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.comAngleMlp = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.maxVelocity = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.velocityFactor = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.maxLegStretch = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.rootSpeed = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.stepSpeed = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.relaxLegTwistMinAngle = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.relaxLegTwistSpeed = 0;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.stepInterpolation = RootMotion.InterpolationMode.InOutSine;
        AvatarMale.GetComponent<VRIK>().solver.locomotion.offset = new Vector3(0, 0, 0);


        // add twist relaxer modified components
        AvatarMaleLeftForeArm.gameObject.AddComponent<TwistRelaxer>();
        AvatarMaleLeftForeArm.GetComponent<TwistRelaxer>().ik = AvatarMale.GetComponent<VRIK>();
        AvatarMaleLeftForeArm.GetComponent<TwistRelaxer>().weight = 1;
        AvatarMaleLeftForeArm.GetComponent<TwistRelaxer>().parentChildCrossfade = 0.5f;
        AvatarMaleLeftForeArm.GetComponent<TwistRelaxer>().twistAngleOffset = 0;
        AvatarMaleLeftForeArm.GetComponent<TwistRelaxer>().twistAxis = Vector3.up;

        AvatarMaleRightForeArm.gameObject.AddComponent<TwistRelaxer>();
        AvatarMaleRightForeArm.GetComponent<TwistRelaxer>().ik = AvatarMale.GetComponent<VRIK>();
        AvatarMaleRightForeArm.GetComponent<TwistRelaxer>().weight = 1;
        AvatarMaleRightForeArm.GetComponent<TwistRelaxer>().parentChildCrossfade = 0.5f;
        AvatarMaleRightForeArm.GetComponent<TwistRelaxer>().twistAngleOffset = 0;
        AvatarMaleRightForeArm.GetComponent<TwistRelaxer>().twistAxis = Vector3.up;
    }

    void SetIKFemale()
    {
        if (!b_track_avatar)
        {
            return;
        }

        // add VRIK component
        AvatarFemale.gameObject.AddComponent<VRIK>();

        AvatarFemale.GetComponent<VRIK>().references.root = AvatarFemale.transform;
        AvatarFemale.GetComponent<VRIK>().references.pelvis = AvatarFemaleHips.transform;
        AvatarFemale.GetComponent<VRIK>().references.spine = AvatarFemaleSpine.transform;
        AvatarFemale.GetComponent<VRIK>().references.chest = AvatarFemaleSpine1.transform;
        AvatarFemale.GetComponent<VRIK>().references.neck = AvatarFemaleNeck.transform;
        AvatarFemale.GetComponent<VRIK>().references.head = AvatarFemaleHead.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftShoulder = AvatarFemaleLeftShoulder.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftUpperArm = AvatarFemaleLeftArm.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftForearm = AvatarFemaleLeftForeArm.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftHand = AvatarFemaleLeftHand.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightShoulder = AvatarFemaleRightShoulder.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightUpperArm = AvatarFemaleRightArm.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightForearm = AvatarFemaleRightForeArm.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightHand = AvatarFemaleRightHand.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftThigh = AvatarFemaleLeftUpLeg.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftCalf = AvatarFemaleLeftLeg.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftFoot = AvatarFemaleLeftFoot.transform;
        AvatarFemale.GetComponent<VRIK>().references.leftToes = AvatarFemaleLeftToeBase.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightThigh = AvatarFemaleRightUpLeg.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightCalf = AvatarFemaleRightLeg.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightFoot = AvatarFemaleRightFoot.transform;
        AvatarFemale.GetComponent<VRIK>().references.rightToes = AvatarFemaleRightToeBase.transform;


        AvatarFemale.GetComponent<VRIK>().solver.IKPositionWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.plantFeet = false;

        AvatarFemale.GetComponent<VRIK>().solver.spine.headTarget = null;
        AvatarFemale.GetComponent<VRIK>().solver.spine.pelvisTarget = null;
        AvatarFemale.GetComponent<VRIK>().solver.spine.positionWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.rotationWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.pelvisPositionWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.pelvisRotationWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.chestGoal = null;
        AvatarFemale.GetComponent<VRIK>().solver.spine.chestGoalWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.minHeadHeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.bodyPosStiffness = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.bodyRotStiffness = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.neckStiffness = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.rotateChestByHands = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.chestClampWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.headClampWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.moveBodyBackWhenCrouching = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.maintainPelvisPosition = 0;
        AvatarFemale.GetComponent<VRIK>().solver.spine.maxRootAngle = 0;

        AvatarFemale.GetComponent<VRIK>().solver.leftArm.target = HandTargets_LeftHand_LeftHandWrist.transform;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.bendGoal = null;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.positionWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.rotationWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.YawPitch;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.shoulderRotationWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.bendGoalWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.swivelOffset = 0;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.wristToPalmAxis = new Vector3(0, 1, 0);
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.palmToThumbAxis = new Vector3(1, 0, 0);
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.armLengthMlp = 1;
        AvatarFemale.GetComponent<VRIK>().solver.leftArm.stretchCurve = new AnimationCurve();

        AvatarFemale.GetComponent<VRIK>().solver.rightArm.target = HandTargets_RightHand_RightHandWrist.transform;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.bendGoal = null;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.positionWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.rotationWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.YawPitch;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.shoulderRotationWeight = 1;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.bendGoalWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.swivelOffset = 0;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.wristToPalmAxis = new Vector3(0, 1, 0);
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.palmToThumbAxis = new Vector3(1, 0, 0);
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.armLengthMlp = 1;
        AvatarFemale.GetComponent<VRIK>().solver.rightArm.stretchCurve = new AnimationCurve();

        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.target = null;
        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.bendGoal = null;
        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.positionWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.rotationWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.bendGoalWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.leftLeg.swivelOffset = 0;

        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.target = null;
        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.bendGoal = null;
        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.positionWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.rotationWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.bendGoalWeight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.rightLeg.swivelOffset = 0;

        AvatarFemale.GetComponent<VRIK>().solver.locomotion.weight = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.footDistance = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.stepThreshold = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.angleThreshold = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.comAngleMlp = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.maxVelocity = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.velocityFactor = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.maxLegStretch = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.rootSpeed = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.stepSpeed = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.relaxLegTwistMinAngle = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.relaxLegTwistSpeed = 0;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.stepInterpolation = RootMotion.InterpolationMode.InOutSine;
        AvatarFemale.GetComponent<VRIK>().solver.locomotion.offset = new Vector3(0, 0, 0);


        // add twist relaxer modified components
        AvatarFemaleLeftForeArm.gameObject.AddComponent<TwistRelaxer>();
        AvatarFemaleLeftForeArm.GetComponent<TwistRelaxer>().ik = AvatarFemale.GetComponent<VRIK>();
        AvatarFemaleLeftForeArm.GetComponent<TwistRelaxer>().weight = 1;
        AvatarFemaleLeftForeArm.GetComponent<TwistRelaxer>().parentChildCrossfade = 0.5f;
        AvatarFemaleLeftForeArm.GetComponent<TwistRelaxer>().twistAngleOffset = 0;
        AvatarFemaleLeftForeArm.GetComponent<TwistRelaxer>().twistAxis = Vector3.up;

        AvatarFemaleRightForeArm.gameObject.AddComponent<TwistRelaxer>();
        AvatarFemaleRightForeArm.GetComponent<TwistRelaxer>().ik = AvatarFemale.GetComponent<VRIK>();
        AvatarFemaleRightForeArm.GetComponent<TwistRelaxer>().weight = 1;
        AvatarFemaleRightForeArm.GetComponent<TwistRelaxer>().parentChildCrossfade = 0.5f;
        AvatarFemaleRightForeArm.GetComponent<TwistRelaxer>().twistAngleOffset = 0;
        AvatarFemaleRightForeArm.GetComponent<TwistRelaxer>().twistAxis = Vector3.up;
    }
}
