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

#pragma warning disable 0414 //The private field '' is assigned but its value is never used

public class _avatar_config_torso_scale : MonoBehaviour {

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

    [Header("Properties")]
    private float ScaleXTorso = 1;
    private float ScaleZTorso = 1;
    private float ScaleXBelly = 1;
    private float ScaleZBelly = 1;

    private float RatioScaleDistanceTorsoMale = 26.6f;
    private float RatioScaleDistanceTorsoFemale = 25.4f;

    //public bool IsMale = true; // Determine the sex of the character -> To apply correct previous ratio

    // Use this for initialization
    void Start () {
        GetAvatarMaleReferences();
        GetAvatarFemaleReferences();
    }

    public void UpdateTorsoScaleXFloat(float scale)
    {
        ScaleXTorso = scale;
        UpdateTorsoScaleX();
    }

    public void UpdateTorsoScaleZFloat(float scale)
    {
        ScaleZTorso = scale;
        UpdateTorsoScaleZ();
    }

    public void UpdateBellyScaleXFloat(float scale)
    {
        ScaleXBelly = scale;
        UpdateBellyScaleX();
    }

    public void UpdateBellyScaleZFloat(float scale)
    {
        ScaleZBelly = scale;
        UpdateBellyScaleZ();
    }

    public void UpdateTorsoScaleX()
    {
        AvatarMale.transform.localScale = new Vector3(ScaleXTorso, AvatarMale.transform.localScale.y, AvatarMale.transform.localScale.z);
        AvatarMaleLeftUpLeg.transform.localScale = new Vector3(1/ScaleXTorso, AvatarMaleLeftUpLeg.transform.localScale.y, AvatarMaleLeftUpLeg.transform.localScale.z);
        AvatarMaleRightUpLeg.transform.localScale = new Vector3(1 / ScaleXTorso, AvatarMaleRightUpLeg.transform.localScale.y, AvatarMaleRightUpLeg.transform.localScale.z);
        AvatarMaleLeftShoulder.transform.localScale = new Vector3(AvatarMaleLeftShoulder.transform.localScale.x, 1 / ScaleXTorso, AvatarMaleLeftShoulder.transform.localScale.z);
        AvatarMaleRightShoulder.transform.localScale = new Vector3(AvatarMaleRightShoulder.transform.localScale.x, 1 / ScaleXTorso, AvatarMaleRightShoulder.transform.localScale.z);
        AvatarMaleNeck.transform.localScale = new Vector3(1 / ScaleXTorso, AvatarMaleNeck.transform.localScale.y, AvatarMaleNeck.transform.localScale.z);
        AvatarFemale.transform.localScale = new Vector3(ScaleXTorso, AvatarFemale.transform.localScale.y, AvatarFemale.transform.localScale.z);
        AvatarFemaleLeftUpLeg.transform.localScale = new Vector3(1 / ScaleXTorso, AvatarFemaleLeftUpLeg.transform.localScale.y, AvatarFemaleLeftUpLeg.transform.localScale.z);
        AvatarFemaleRightUpLeg.transform.localScale = new Vector3(1 / ScaleXTorso, AvatarFemaleRightUpLeg.transform.localScale.y, AvatarFemaleRightUpLeg.transform.localScale.z);
        AvatarFemaleLeftShoulder.transform.localScale = new Vector3(AvatarFemaleLeftShoulder.transform.localScale.x, 1 / ScaleXTorso, AvatarFemaleLeftShoulder.transform.localScale.z);
        AvatarFemaleRightShoulder.transform.localScale = new Vector3(AvatarFemaleRightShoulder.transform.localScale.x, 1 / ScaleXTorso, AvatarFemaleRightShoulder.transform.localScale.z);
        AvatarFemaleNeck.transform.localScale = new Vector3(1 / ScaleXTorso, AvatarFemaleNeck.transform.localScale.y, AvatarFemaleNeck.transform.localScale.z);
    }

    public void UpdateTorsoScaleZ()
    {
        AvatarMale.transform.localScale = new Vector3(AvatarMale.transform.localScale.x, AvatarMale.transform.localScale.y, ScaleZTorso);
        AvatarMaleLeftUpLeg.transform.localScale = new Vector3(AvatarMaleLeftUpLeg.transform.localScale.x, AvatarMaleLeftUpLeg.transform.localScale.y, 1 / ScaleZTorso);
        AvatarMaleRightUpLeg.transform.localScale = new Vector3(AvatarMaleRightUpLeg.transform.localScale.x, AvatarMaleRightUpLeg.transform.localScale.y, 1 / ScaleZTorso);
        AvatarMaleLeftShoulder.transform.localScale = new Vector3(1 / ScaleZTorso, AvatarMaleLeftShoulder.transform.localScale.y, AvatarMaleLeftShoulder.transform.localScale.z);
        AvatarMaleRightShoulder.transform.localScale = new Vector3(1 / ScaleZTorso, AvatarMaleRightShoulder.transform.localScale.y, AvatarMaleRightShoulder.transform.localScale.z);
        AvatarMaleNeck.transform.localScale = new Vector3(AvatarMaleNeck.transform.localScale.x, AvatarMaleNeck.transform.localScale.y, 1 / ScaleZTorso);
        AvatarFemale.transform.localScale = new Vector3(AvatarFemale.transform.localScale.x, AvatarFemale.transform.localScale.y, ScaleZTorso);
        AvatarFemaleLeftUpLeg.transform.localScale = new Vector3(AvatarFemaleLeftUpLeg.transform.localScale.x, AvatarFemaleLeftUpLeg.transform.localScale.y, 1 / ScaleZTorso);
        AvatarFemaleRightUpLeg.transform.localScale = new Vector3(AvatarFemaleRightUpLeg.transform.localScale.x, AvatarFemaleRightUpLeg.transform.localScale.y, 1 / ScaleZTorso);
        AvatarFemaleLeftShoulder.transform.localScale = new Vector3(1 / ScaleZTorso, AvatarFemaleLeftShoulder.transform.localScale.y, AvatarFemaleLeftShoulder.transform.localScale.z);
        AvatarFemaleRightShoulder.transform.localScale = new Vector3(1 / ScaleZTorso, AvatarFemaleRightShoulder.transform.localScale.y, AvatarFemaleRightShoulder.transform.localScale.z);
        AvatarFemaleNeck.transform.localScale = new Vector3(AvatarFemaleNeck.transform.localScale.x, AvatarFemaleNeck.transform.localScale.y, 1 / ScaleZTorso);
    }

    public void UpdateBellyScaleX()
    {
        AvatarMale.transform.localScale = new Vector3(ScaleXBelly, AvatarMale.transform.localScale.y, AvatarMale.transform.localScale.z);
        AvatarMaleLeftUpLeg.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarMaleLeftUpLeg.transform.localScale.y, AvatarMaleLeftUpLeg.transform.localScale.z);
        AvatarMaleRightUpLeg.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarMaleRightUpLeg.transform.localScale.y, AvatarMaleRightUpLeg.transform.localScale.z);
        AvatarMaleSpine1.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarMaleSpine1.transform.localScale.y, AvatarMaleSpine1.transform.localScale.z);
        AvatarFemale.transform.localScale = new Vector3(ScaleXBelly, AvatarFemale.transform.localScale.y, AvatarFemale.transform.localScale.z);
        AvatarFemaleLeftUpLeg.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarFemaleLeftUpLeg.transform.localScale.y, AvatarFemaleLeftUpLeg.transform.localScale.z);
        AvatarFemaleRightUpLeg.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarFemaleRightUpLeg.transform.localScale.y, AvatarFemaleRightUpLeg.transform.localScale.z);
        AvatarFemaleSpine1.transform.localScale = new Vector3(1 / ScaleXBelly, AvatarFemaleSpine1.transform.localScale.y, AvatarFemaleSpine1.transform.localScale.z);
    }

    public void UpdateBellyScaleZ()
    {
        AvatarMale.transform.localScale = new Vector3(AvatarMale.transform.localScale.x, AvatarMale.transform.localScale.y, ScaleZBelly);
        AvatarMaleLeftUpLeg.transform.localScale = new Vector3(AvatarMaleLeftUpLeg.transform.localScale.x, AvatarMaleLeftUpLeg.transform.localScale.y, 1 / ScaleZBelly);
        AvatarMaleRightUpLeg.transform.localScale = new Vector3(AvatarMaleRightUpLeg.transform.localScale.x, AvatarMaleRightUpLeg.transform.localScale.y, 1 / ScaleZBelly);
        AvatarMaleSpine1.transform.localScale = new Vector3(AvatarMaleSpine1.transform.localScale.x, AvatarMaleSpine1.transform.localScale.y, 1 / ScaleZBelly);
        AvatarFemale.transform.localScale = new Vector3(AvatarFemale.transform.localScale.x, AvatarFemale.transform.localScale.y, ScaleZBelly);
        AvatarFemaleLeftUpLeg.transform.localScale = new Vector3(AvatarFemaleLeftUpLeg.transform.localScale.x, AvatarFemaleLeftUpLeg.transform.localScale.y, 1 / ScaleZBelly);
        AvatarFemaleRightUpLeg.transform.localScale = new Vector3(AvatarFemaleRightUpLeg.transform.localScale.x, AvatarFemaleRightUpLeg.transform.localScale.y, 1 / ScaleZBelly);
        AvatarFemaleSpine1.transform.localScale = new Vector3(AvatarFemaleSpine1.transform.localScale.x, AvatarFemaleSpine1.transform.localScale.y, 1 / ScaleZBelly);
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
}
