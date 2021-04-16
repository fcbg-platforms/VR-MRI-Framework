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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0618 //'UnityEngine.GameObject.active' is obsolete
#pragma warning disable 0414 //The private field '' is assigned but its value is never used

public class _continuous_scale_and_stroop_body_task : MonoBehaviour {

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

    [Header("Avatar Male OriginPositions")]
    private Vector3 v3_AvatarMale;
    private Vector3 v3_AvatarMaleHips;
    private Vector3 v3_AvatarMaleLeftUpLeg;
    private Vector3 v3_AvatarMaleLeftLeg;
    private Vector3 v3_AvatarMaleLeftFoot;
    private Vector3 v3_AvatarMaleLeftToeBase;
    private Vector3 v3_AvatarMaleRightUpLeg;
    private Vector3 v3_AvatarMaleRightLeg;
    private Vector3 v3_AvatarMaleRightFoot;
    private Vector3 v3_AvatarMaleRightToeBase;
    private Vector3 v3_AvatarMaleSpine;
    private Vector3 v3_AvatarMaleSpine1;
    private Vector3 v3_AvatarMaleSpine2;
    private Vector3 v3_AvatarMaleLeftShoulder;
    private Vector3 v3_AvatarMaleLeftArm;
    private Vector3 v3_AvatarMaleLeftForeArm;
    private Vector3 v3_AvatarMaleLeftHand;
    private Vector3 v3_AvatarMaleLeftHandIndex1;
    private Vector3 v3_AvatarMaleLeftHandIndex2;
    private Vector3 v3_AvatarMaleLeftHandIndex3;
    private Vector3 v3_AvatarMaleLeftHandMiddle1;
    private Vector3 v3_AvatarMaleLeftHandMiddle2;
    private Vector3 v3_AvatarMaleLeftHandMiddle3;
    private Vector3 v3_AvatarMaleLeftHandPinky1;
    private Vector3 v3_AvatarMaleLeftHandPinky2;
    private Vector3 v3_AvatarMaleLeftHandPinky3;
    private Vector3 v3_AvatarMaleLeftHandRing1;
    private Vector3 v3_AvatarMaleLeftHandRing2;
    private Vector3 v3_AvatarMaleLeftHandRing3;
    private Vector3 v3_AvatarMaleLeftHandThumb1;
    private Vector3 v3_AvatarMaleLeftHandThumb2;
    private Vector3 v3_AvatarMaleLeftHandThumb3;
    private Vector3 v3_AvatarMaleNeck;
    private Vector3 v3_AvatarMaleHead;
    private Vector3 v3_AvatarMaleRightShoulder;
    private Vector3 v3_AvatarMaleRightArm;
    private Vector3 v3_AvatarMaleRightForeArm;
    private Vector3 v3_AvatarMaleRightHand;
    private Vector3 v3_AvatarMaleRightHandIndex1;
    private Vector3 v3_AvatarMaleRightHandIndex2;
    private Vector3 v3_AvatarMaleRightHandIndex3;
    private Vector3 v3_AvatarMaleRightHandMiddle1;
    private Vector3 v3_AvatarMaleRightHandMiddle2;
    private Vector3 v3_AvatarMaleRightHandMiddle3;
    private Vector3 v3_AvatarMaleRightHandPinky1;
    private Vector3 v3_AvatarMaleRightHandPinky2;
    private Vector3 v3_AvatarMaleRightHandPinky3;
    private Vector3 v3_AvatarMaleRightHandRing1;
    private Vector3 v3_AvatarMaleRightHandRing2;
    private Vector3 v3_AvatarMaleRightHandRing3;
    private Vector3 v3_AvatarMaleRightHandThumb1;
    private Vector3 v3_AvatarMaleRightHandThumb2;
    private Vector3 v3_AvatarMaleRightHandThumb3;

    [Header("Avatar Female OriginPositions")]
    private Vector3 v3_AvatarFemale;
    private Vector3 v3_AvatarFemaleHips;
    private Vector3 v3_AvatarFemaleLeftUpLeg;
    private Vector3 v3_AvatarFemaleLeftLeg;
    private Vector3 v3_AvatarFemaleLeftFoot;
    private Vector3 v3_AvatarFemaleLeftToeBase;
    private Vector3 v3_AvatarFemaleRightUpLeg;
    private Vector3 v3_AvatarFemaleRightLeg;
    private Vector3 v3_AvatarFemaleRightFoot;
    private Vector3 v3_AvatarFemaleRightToeBase;
    private Vector3 v3_AvatarFemaleSpine;
    private Vector3 v3_AvatarFemaleSpine1;
    private Vector3 v3_AvatarFemaleSpine2;
    private Vector3 v3_AvatarFemaleLeftShoulder;
    private Vector3 v3_AvatarFemaleLeftArm;
    private Vector3 v3_AvatarFemaleLeftForeArm;
    private Vector3 v3_AvatarFemaleLeftHand;
    private Vector3 v3_AvatarFemaleLeftHandIndex1;
    private Vector3 v3_AvatarFemaleLeftHandIndex2;
    private Vector3 v3_AvatarFemaleLeftHandIndex3;
    private Vector3 v3_AvatarFemaleLeftHandMiddle1;
    private Vector3 v3_AvatarFemaleLeftHandMiddle2;
    private Vector3 v3_AvatarFemaleLeftHandMiddle3;
    private Vector3 v3_AvatarFemaleLeftHandPinky1;
    private Vector3 v3_AvatarFemaleLeftHandPinky2;
    private Vector3 v3_AvatarFemaleLeftHandPinky3;
    private Vector3 v3_AvatarFemaleLeftHandRing1;
    private Vector3 v3_AvatarFemaleLeftHandRing2;
    private Vector3 v3_AvatarFemaleLeftHandRing3;
    private Vector3 v3_AvatarFemaleLeftHandThumb1;
    private Vector3 v3_AvatarFemaleLeftHandThumb2;
    private Vector3 v3_AvatarFemaleLeftHandThumb3;
    private Vector3 v3_AvatarFemaleNeck;
    private Vector3 v3_AvatarFemaleHead;
    private Vector3 v3_AvatarFemaleRightShoulder;
    private Vector3 v3_AvatarFemaleRightArm;
    private Vector3 v3_AvatarFemaleRightForeArm;
    private Vector3 v3_AvatarFemaleRightHand;
    private Vector3 v3_AvatarFemaleRightHandIndex1;
    private Vector3 v3_AvatarFemaleRightHandIndex2;
    private Vector3 v3_AvatarFemaleRightHandIndex3;
    private Vector3 v3_AvatarFemaleRightHandMiddle1;
    private Vector3 v3_AvatarFemaleRightHandMiddle2;
    private Vector3 v3_AvatarFemaleRightHandMiddle3;
    private Vector3 v3_AvatarFemaleRightHandPinky1;
    private Vector3 v3_AvatarFemaleRightHandPinky2;
    private Vector3 v3_AvatarFemaleRightHandPinky3;
    private Vector3 v3_AvatarFemaleRightHandRing1;
    private Vector3 v3_AvatarFemaleRightHandRing2;
    private Vector3 v3_AvatarFemaleRightHandRing3;
    private Vector3 v3_AvatarFemaleRightHandThumb1;
    private Vector3 v3_AvatarFemaleRightHandThumb2;
    private Vector3 v3_AvatarFemaleRightHandThumb3;

    [Header("Avatar Male Text References")]
    private bool TextOnAvatarMaleEmpty;
    private bool TextOnAvatarMale;
    private bool TextOnAvatarMaleHips;
    private bool TextOnAvatarMaleLeftUpLeg;
    private bool TextOnAvatarMaleLeftLeg;
    private bool TextOnAvatarMaleLeftFoot;
    private bool TextOnAvatarMaleLeftToeBase;
    private bool TextOnAvatarMaleRightUpLeg;
    private bool TextOnAvatarMaleRightLeg;
    private bool TextOnAvatarMaleRightFoot;
    private bool TextOnAvatarMaleRightToeBase;
    private bool TextOnAvatarMaleSpine;
    private bool TextOnAvatarMaleSpine1;
    private bool TextOnAvatarMaleSpine2;
    private bool TextOnAvatarMaleLeftShoulder;
    private bool TextOnAvatarMaleLeftArm;
    private bool TextOnAvatarMaleLeftForeArm;
    private bool TextOnAvatarMaleLeftHand;
    private bool TextOnAvatarMaleLeftHandIndex1;
    private bool TextOnAvatarMaleLeftHandIndex2;
    private bool TextOnAvatarMaleLeftHandIndex3;
    private bool TextOnAvatarMaleLeftHandMiddle1;
    private bool TextOnAvatarMaleLeftHandMiddle2;
    private bool TextOnAvatarMaleLeftHandMiddle3;
    private bool TextOnAvatarMaleLeftHandPinky1;
    private bool TextOnAvatarMaleLeftHandPinky2;
    private bool TextOnAvatarMaleLeftHandPinky3;
    private bool TextOnAvatarMaleLeftHandRing1;
    private bool TextOnAvatarMaleLeftHandRing2;
    private bool TextOnAvatarMaleLeftHandRing3;
    private bool TextOnAvatarMaleLeftHandThumb1;
    private bool TextOnAvatarMaleLeftHandThumb2;
    private bool TextOnAvatarMaleLeftHandThumb3;
    private bool TextOnAvatarMaleNeck;
    private bool TextOnAvatarMaleHead;
    private bool TextOnAvatarMaleRightShoulder;
    private bool TextOnAvatarMaleRightArm;
    private bool TextOnAvatarMaleRightForeArm;
    private bool TextOnAvatarMaleRightHand;
    private bool TextOnAvatarMaleRightHandIndex1;
    private bool TextOnAvatarMaleRightHandIndex2;
    private bool TextOnAvatarMaleRightHandIndex3;
    private bool TextOnAvatarMaleRightHandMiddle1;
    private bool TextOnAvatarMaleRightHandMiddle2;
    private bool TextOnAvatarMaleRightHandMiddle3;
    private bool TextOnAvatarMaleRightHandPinky1;
    private bool TextOnAvatarMaleRightHandPinky2;
    private bool TextOnAvatarMaleRightHandPinky3;
    private bool TextOnAvatarMaleRightHandRing1;
    private bool TextOnAvatarMaleRightHandRing2;
    private bool TextOnAvatarMaleRightHandRing3;
    private bool TextOnAvatarMaleRightHandThumb1;
    private bool TextOnAvatarMaleRightHandThumb2;
    private bool TextOnAvatarMaleRightHandThumb3;

    [Header("Avatar Female Text References")]
    private bool TextOnAvatarFemaleEmpty;
    private bool TextOnAvatarFemale;
    private bool TextOnAvatarFemaleHips;
    private bool TextOnAvatarFemaleLeftUpLeg;
    private bool TextOnAvatarFemaleLeftLeg;
    private bool TextOnAvatarFemaleLeftFoot;
    private bool TextOnAvatarFemaleLeftToeBase;
    private bool TextOnAvatarFemaleRightUpLeg;
    private bool TextOnAvatarFemaleRightLeg;
    private bool TextOnAvatarFemaleRightFoot;
    private bool TextOnAvatarFemaleRightToeBase;
    private bool TextOnAvatarFemaleSpine;
    private bool TextOnAvatarFemaleSpine1;
    private bool TextOnAvatarFemaleSpine2;
    private bool TextOnAvatarFemaleLeftShoulder;
    private bool TextOnAvatarFemaleLeftArm;
    private bool TextOnAvatarFemaleLeftForeArm;
    private bool TextOnAvatarFemaleLeftHand;
    private bool TextOnAvatarFemaleLeftHandIndex1;
    private bool TextOnAvatarFemaleLeftHandIndex2;
    private bool TextOnAvatarFemaleLeftHandIndex3;
    private bool TextOnAvatarFemaleLeftHandMiddle1;
    private bool TextOnAvatarFemaleLeftHandMiddle2;
    private bool TextOnAvatarFemaleLeftHandMiddle3;
    private bool TextOnAvatarFemaleLeftHandPinky1;
    private bool TextOnAvatarFemaleLeftHandPinky2;
    private bool TextOnAvatarFemaleLeftHandPinky3;
    private bool TextOnAvatarFemaleLeftHandRing1;
    private bool TextOnAvatarFemaleLeftHandRing2;
    private bool TextOnAvatarFemaleLeftHandRing3;
    private bool TextOnAvatarFemaleLeftHandThumb1;
    private bool TextOnAvatarFemaleLeftHandThumb2;
    private bool TextOnAvatarFemaleLeftHandThumb3;
    private bool TextOnAvatarFemaleNeck;
    private bool TextOnAvatarFemaleHead;
    private bool TextOnAvatarFemaleRightShoulder;
    private bool TextOnAvatarFemaleRightArm;
    private bool TextOnAvatarFemaleRightForeArm;
    private bool TextOnAvatarFemaleRightHand;
    private bool TextOnAvatarFemaleRightHandIndex1;
    private bool TextOnAvatarFemaleRightHandIndex2;
    private bool TextOnAvatarFemaleRightHandIndex3;
    private bool TextOnAvatarFemaleRightHandMiddle1;
    private bool TextOnAvatarFemaleRightHandMiddle2;
    private bool TextOnAvatarFemaleRightHandMiddle3;
    private bool TextOnAvatarFemaleRightHandPinky1;
    private bool TextOnAvatarFemaleRightHandPinky2;
    private bool TextOnAvatarFemaleRightHandPinky3;
    private bool TextOnAvatarFemaleRightHandRing1;
    private bool TextOnAvatarFemaleRightHandRing2;
    private bool TextOnAvatarFemaleRightHandRing3;
    private bool TextOnAvatarFemaleRightHandThumb1;
    private bool TextOnAvatarFemaleRightHandThumb2;
    private bool TextOnAvatarFemaleRightHandThumb3;

    [Header("Other Generics")]
    private Vector3 v3_Other = new Vector3(0,0,0);
    private bool TextOnAvatarOther;

	[Header("Others")]
    private bool isMale;
    private bool StartDisplayTrial;
    private bool StopDisplayTrial;
    private GameObject TextToBeDisplayed;
    private bool TrialRunning;
    private bool NextTrial;

    [Header("Load Experiment Sequence")]
    private string SaveFilePath = "Results/Sequence1Results.txt";
    private string Directory = "Results/";

    private StreamReader reader;
    private bool ReadingExperimentFile;

    private bool TrackingMovingPartLeftHand;
    private bool TrackingMovingPartRightHand;

    [HideInInspector]
    public bool expyVRTrialRunningEnableInput;

    private GameObject continuousScale;
    private bool b_isContinuousScaleActive;
    private float ContinuousScaleSpeed = 250.0f;
    private float randomAdd;
    private float currentContinuousScaleValueSelected = -1;
    private float currentSliderValue;
    private float MaxContinuousScaleValue = 1000;
    private GameObject continuousScaleTextLeftMin;
    private GameObject continuousScaleTextRightMax;
    private GameObject continuousScaleTextDiscreteScale; 

    private Vector3 v3Debug;
    private float DebugFloat;

    private _main_experiment_manager _script_main_experiment_manager;



    // Use this for initialization
    void Start()
    {
        GetAvatarMaleReferences();
        GetAvatarFemaleReferences();
        InitializeOriginsPositionsMale();
        InitializeOriginsPositionsFemale();
        SetMaleSex();

        TextToBeDisplayed = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_text_center;
        TextToBeDisplayed.SetActive(false);
        continuousScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale;
        continuousScaleTextLeftMin = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_text_left_min;
        continuousScaleTextRightMax = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_text_right_max;
        continuousScaleTextDiscreteScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_text_discrete_scale;
        continuousScale.SetActive(false);
		_script_main_experiment_manager = _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager;
    }


    // Update is called once per frame
    void Update()
    {        
        if (TrackingMovingPartRightHand)
        {
            TextToBeDisplayed.transform.position = AvatarMaleRightHand.transform.position + new Vector3(0, 0.150f, 0);
        }
        else if (TrackingMovingPartLeftHand)
        {
            TextToBeDisplayed.transform.position = AvatarMaleLeftHand.transform.position + new Vector3(0, 0.150f, 0);
        }

        // if input allowed (that means between the start of a trial and the end of a trial/first input of trial)
        if (expyVRTrialRunningEnableInput)
        {
            ExpyVRCheckAnswerInputKeyboard();
        }

        if (b_isContinuousScaleActive)
        {
            if (!b_isContinuousScaleKeyboardControlled)
            {
                DebugFloat = Mathf.PingPong(Time.time * ContinuousScaleSpeed + randomAdd, MaxContinuousScaleValue);

                currentSliderValue = DebugFloat;

                continuousScale.transform.Find("Slider").localPosition = new Vector3((DebugFloat - (MaxContinuousScaleValue / 2)) * (0.15f / (MaxContinuousScaleValue / 2)), 0, 0);
            }
            else
            {
                // continuous_scale
                /*if (Input.GetButtonDown("ButtonLeft"))
                {
                    if (currentSliderValue > 0)
                    {
                        DebugFloat = currentSliderValue - Time.deltaTime * ContinuousScaleSpeed;
                        if (DebugFloat < 0)
                        {
                            DebugFloat = 0;
                        }
                        currentSliderValue = DebugFloat;

                        continuousScale.transform.Find("Slider").localPosition = new Vector3((DebugFloat - (MaxContinuousScaleValue / 2)) * (0.15f / (MaxContinuousScaleValue / 2)), 0, 0);
                    }
                }
                if (Input.GetButtonDown("ButtonRight"))
                {
                    if (currentSliderValue < MaxContinuousScaleValue)
                    {
                        DebugFloat = currentSliderValue + Time.deltaTime * ContinuousScaleSpeed;
                        if (DebugFloat > MaxContinuousScaleValue)
                        {
                            DebugFloat = MaxContinuousScaleValue;
                        }
                        currentSliderValue = DebugFloat;

                        continuousScale.transform.Find("Slider").localPosition = new Vector3((DebugFloat - (MaxContinuousScaleValue / 2)) * (0.15f / (MaxContinuousScaleValue / 2)), 0, 0);
                    }
                }*/

                // discrete_scale
                if (Input.GetButtonDown("ButtonLeft"))
                {
                    if (i_indice_f_list_continuousScale_values > 0)
                    {
                        i_indice_f_list_continuousScale_values--;
                        DebugFloat = f_list_continuousScale_values[i_indice_f_list_continuousScale_values];
                        currentSliderValue = DebugFloat;

                        continuousScale.transform.Find("Slider").localPosition = new Vector3((DebugFloat - (MaxContinuousScaleValue / 2)) * (0.15f / (MaxContinuousScaleValue / 2)), 0, 0);
                    }
                }
                if (Input.GetButtonDown("ButtonRight"))
                {
                    if (i_indice_f_list_continuousScale_values < f_list_continuousScale_values.Count)
                    {
                        i_indice_f_list_continuousScale_values++;
                        DebugFloat = f_list_continuousScale_values[i_indice_f_list_continuousScale_values];
                        currentSliderValue = DebugFloat;

                        continuousScale.transform.Find("Slider").localPosition = new Vector3((DebugFloat - (MaxContinuousScaleValue / 2)) * (0.15f / (MaxContinuousScaleValue / 2)), 0, 0);
                    }
                }
            }
        }
    }

    List<float> f_list_continuousScale_values = new List<float>() {0, 166.7f, 333.3f, 500, 666.7f, 833.3f, 1000};
    int i_indice_f_list_continuousScale_values = 3;



    void InitializeOriginsPositionsMale()
    {
        v3_AvatarMale = AvatarMale.transform.position;
        v3_AvatarMaleHips = AvatarMaleHips.transform.position;
        v3_AvatarMaleLeftUpLeg = AvatarMaleLeftUpLeg.transform.position;
        v3_AvatarMaleLeftLeg = AvatarMaleLeftLeg.transform.position;
        v3_AvatarMaleLeftFoot = AvatarMaleLeftFoot.transform.position;
        v3_AvatarMaleLeftToeBase = AvatarMaleLeftToeBase.transform.position;
        v3_AvatarMaleRightUpLeg = AvatarMaleRightUpLeg.transform.position;
        v3_AvatarMaleRightLeg = AvatarMaleRightLeg.transform.position;
        v3_AvatarMaleRightFoot = AvatarMaleRightFoot.transform.position;
        v3_AvatarMaleRightToeBase = AvatarMaleRightToeBase.transform.position;
        v3_AvatarMaleSpine = AvatarMaleSpine.transform.position;
        v3_AvatarMaleSpine1 = AvatarMaleSpine1.transform.position;
        v3_AvatarMaleSpine2 = AvatarMaleSpine2.transform.position;
        v3_AvatarMaleLeftShoulder = AvatarMaleLeftShoulder.transform.position;
        v3_AvatarMaleLeftArm = AvatarMaleLeftArm.transform.position;
        v3_AvatarMaleLeftForeArm = AvatarMaleLeftForeArm.transform.position;
        v3_AvatarMaleLeftHand = AvatarMaleLeftHand.transform.position;
        v3_AvatarMaleLeftHandIndex1 = AvatarMaleLeftHandIndex1.transform.position;
        v3_AvatarMaleLeftHandIndex2 = AvatarMaleLeftHandIndex2.transform.position;
        v3_AvatarMaleLeftHandIndex3 = AvatarMaleLeftHandIndex3.transform.position;
        v3_AvatarMaleLeftHandMiddle1 = AvatarMaleLeftHandMiddle1.transform.position;
        v3_AvatarMaleLeftHandMiddle2 = AvatarMaleLeftHandMiddle2.transform.position;
        v3_AvatarMaleLeftHandMiddle3 = AvatarMaleLeftHandMiddle3.transform.position;
        v3_AvatarMaleLeftHandPinky1 = AvatarMaleLeftHandPinky1.transform.position;
        v3_AvatarMaleLeftHandPinky2 = AvatarMaleLeftHandPinky2.transform.position;
        v3_AvatarMaleLeftHandPinky3 = AvatarMaleLeftHandPinky3.transform.position;
        v3_AvatarMaleLeftHandRing1 = AvatarMaleLeftHandRing1.transform.position;
        v3_AvatarMaleLeftHandRing2 = AvatarMaleLeftHandRing2.transform.position;
        v3_AvatarMaleLeftHandRing3 = AvatarMaleLeftHandRing3.transform.position;
        v3_AvatarMaleLeftHandThumb1 = AvatarMaleLeftHandThumb1.transform.position;
        v3_AvatarMaleLeftHandThumb2 = AvatarMaleLeftHandThumb2.transform.position;
        v3_AvatarMaleLeftHandThumb3 = AvatarMaleLeftHandThumb3.transform.position;
        v3_AvatarMaleNeck = AvatarMaleNeck.transform.position;
        v3_AvatarMaleHead = AvatarMaleHead.transform.position;
        v3_AvatarMaleRightShoulder = AvatarMaleRightShoulder.transform.position;
        v3_AvatarMaleRightArm = AvatarMaleRightArm.transform.position;
        v3_AvatarMaleRightForeArm = AvatarMaleRightForeArm.transform.position;
        v3_AvatarMaleRightHand = AvatarMaleRightHand.transform.position;
        v3_AvatarMaleRightHandIndex1 = AvatarMaleRightHandIndex1.transform.position;
        v3_AvatarMaleRightHandIndex2 = AvatarMaleRightHandIndex2.transform.position;
        v3_AvatarMaleRightHandIndex3 = AvatarMaleRightHandIndex3.transform.position;
        v3_AvatarMaleRightHandMiddle1 = AvatarMaleRightHandMiddle1.transform.position;
        v3_AvatarMaleRightHandMiddle2 = AvatarMaleRightHandMiddle2.transform.position;
        v3_AvatarMaleRightHandMiddle3 = AvatarMaleRightHandMiddle3.transform.position;
        v3_AvatarMaleRightHandPinky1 = AvatarMaleRightHandPinky1.transform.position;
        v3_AvatarMaleRightHandPinky2 = AvatarMaleRightHandPinky2.transform.position;
        v3_AvatarMaleRightHandPinky3 = AvatarMaleRightHandPinky3.transform.position;
        v3_AvatarMaleRightHandRing1 = AvatarMaleRightHandRing1.transform.position;
        v3_AvatarMaleRightHandRing2 = AvatarMaleRightHandRing2.transform.position;
        v3_AvatarMaleRightHandRing3 = AvatarMaleRightHandRing3.transform.position;
        v3_AvatarMaleRightHandThumb1 = AvatarMaleRightHandThumb1.transform.position;
        v3_AvatarMaleRightHandThumb2 = AvatarMaleRightHandThumb2.transform.position;
        v3_AvatarMaleRightHandThumb3 = AvatarMaleRightHandThumb3.transform.position;
    }

    void InitializeOriginsPositionsFemale()
    {
        v3_AvatarFemale = AvatarFemale.transform.position;
        v3_AvatarFemaleHips = AvatarFemaleHips.transform.position;
        v3_AvatarFemaleLeftUpLeg = AvatarFemaleLeftUpLeg.transform.position;
        v3_AvatarFemaleLeftLeg = AvatarFemaleLeftLeg.transform.position;
        v3_AvatarFemaleLeftFoot = AvatarFemaleLeftFoot.transform.position;
        v3_AvatarFemaleLeftToeBase = AvatarFemaleLeftToeBase.transform.position;
        v3_AvatarFemaleRightUpLeg = AvatarFemaleRightUpLeg.transform.position;
        v3_AvatarFemaleRightLeg = AvatarFemaleRightLeg.transform.position;
        v3_AvatarFemaleRightFoot = AvatarFemaleRightFoot.transform.position;
        v3_AvatarFemaleRightToeBase = AvatarFemaleRightToeBase.transform.position;
        v3_AvatarFemaleSpine = AvatarFemaleSpine.transform.position;
        v3_AvatarFemaleSpine1 = AvatarFemaleSpine1.transform.position;
        v3_AvatarFemaleSpine2 = AvatarFemaleSpine2.transform.position;
        v3_AvatarFemaleLeftShoulder = AvatarFemaleLeftShoulder.transform.position;
        v3_AvatarFemaleLeftArm = AvatarFemaleLeftArm.transform.position;
        v3_AvatarFemaleLeftForeArm = AvatarFemaleLeftForeArm.transform.position;
        v3_AvatarFemaleLeftHand = AvatarFemaleLeftHand.transform.position;
        v3_AvatarFemaleLeftHandIndex1 = AvatarFemaleLeftHandIndex1.transform.position;
        v3_AvatarFemaleLeftHandIndex2 = AvatarFemaleLeftHandIndex2.transform.position;
        v3_AvatarFemaleLeftHandIndex3 = AvatarFemaleLeftHandIndex3.transform.position;
        v3_AvatarFemaleLeftHandMiddle1 = AvatarFemaleLeftHandMiddle1.transform.position;
        v3_AvatarFemaleLeftHandMiddle2 = AvatarFemaleLeftHandMiddle2.transform.position;
        v3_AvatarFemaleLeftHandMiddle3 = AvatarFemaleLeftHandMiddle3.transform.position;
        v3_AvatarFemaleLeftHandPinky1 = AvatarFemaleLeftHandPinky1.transform.position;
        v3_AvatarFemaleLeftHandPinky2 = AvatarFemaleLeftHandPinky2.transform.position;
        v3_AvatarFemaleLeftHandPinky3 = AvatarFemaleLeftHandPinky3.transform.position;
        v3_AvatarFemaleLeftHandRing1 = AvatarFemaleLeftHandRing1.transform.position;
        v3_AvatarFemaleLeftHandRing2 = AvatarFemaleLeftHandRing2.transform.position;
        v3_AvatarFemaleLeftHandRing3 = AvatarFemaleLeftHandRing3.transform.position;
        v3_AvatarFemaleLeftHandThumb1 = AvatarFemaleLeftHandThumb1.transform.position;
        v3_AvatarFemaleLeftHandThumb2 = AvatarFemaleLeftHandThumb2.transform.position;
        v3_AvatarFemaleLeftHandThumb3 = AvatarFemaleLeftHandThumb3.transform.position;
        v3_AvatarFemaleNeck = AvatarFemaleNeck.transform.position;
        v3_AvatarFemaleHead = AvatarFemaleHead.transform.position;
        v3_AvatarFemaleRightShoulder = AvatarFemaleRightShoulder.transform.position;
        v3_AvatarFemaleRightArm = AvatarFemaleRightArm.transform.position;
        v3_AvatarFemaleRightForeArm = AvatarFemaleRightForeArm.transform.position;
        v3_AvatarFemaleRightHand = AvatarFemaleRightHand.transform.position;
        v3_AvatarFemaleRightHandIndex1 = AvatarFemaleRightHandIndex1.transform.position;
        v3_AvatarFemaleRightHandIndex2 = AvatarFemaleRightHandIndex2.transform.position;
        v3_AvatarFemaleRightHandIndex3 = AvatarFemaleRightHandIndex3.transform.position;
        v3_AvatarFemaleRightHandMiddle1 = AvatarFemaleRightHandMiddle1.transform.position;
        v3_AvatarFemaleRightHandMiddle2 = AvatarFemaleRightHandMiddle2.transform.position;
        v3_AvatarFemaleRightHandMiddle3 = AvatarFemaleRightHandMiddle3.transform.position;
        v3_AvatarFemaleRightHandPinky1 = AvatarFemaleRightHandPinky1.transform.position;
        v3_AvatarFemaleRightHandPinky2 = AvatarFemaleRightHandPinky2.transform.position;
        v3_AvatarFemaleRightHandPinky3 = AvatarFemaleRightHandPinky3.transform.position;
        v3_AvatarFemaleRightHandRing1 = AvatarFemaleRightHandRing1.transform.position;
        v3_AvatarFemaleRightHandRing2 = AvatarFemaleRightHandRing2.transform.position;
        v3_AvatarFemaleRightHandRing3 = AvatarFemaleRightHandRing3.transform.position;
        v3_AvatarFemaleRightHandThumb1 = AvatarFemaleRightHandThumb1.transform.position;
        v3_AvatarFemaleRightHandThumb2 = AvatarFemaleRightHandThumb2.transform.position;
        v3_AvatarFemaleRightHandThumb3 = AvatarFemaleRightHandThumb3.transform.position;
    }

    public void CheckDisplayText()
    {
        if (StartDisplayTrial)
        {
            TextToBeDisplayed.SetActive(true);
            StartDisplayTrial = false;
            TrialRunning = true;
            expyVRTrialRunningEnableInput = true;
        }
        if (StopDisplayTrial)
        {
            TextToBeDisplayed.SetActive(false);
            StopDisplayTrial = false;
        }
    }

    public void setText(Vector3 position, Vector3 rotation, string text, Color c)
    {
        TextToBeDisplayed.GetComponent<TextMesh>().text = "";
        Char delimiter = '@';
        String[] substrings = text.Split(delimiter);
        foreach (var substring in substrings)
        {
            if (TextToBeDisplayed.GetComponent<TextMesh>().text != "")
            {
                TextToBeDisplayed.GetComponent<TextMesh>().text += Environment.NewLine;
            }
            TextToBeDisplayed.GetComponent<TextMesh>().text += substring;
        }

        TextToBeDisplayed.GetComponent<TextMesh>().color = c;
        TextToBeDisplayed.transform.position = position;
        TextToBeDisplayed.transform.localRotation = Quaternion.Euler(rotation);
    }

    public void StartDisplayText(Vector3 position, Vector3 rotation, string text)
    {
        StartDisplayTrial = true;
    }

    public void StopDisplayText(Vector3 position, Vector3 rotation, string text)
    {
        StopDisplayTrial = true;
    }

    public void SetMaleSex()
    {
        isMale = true;
    }

    public void SetFemaleSex()
    {
        isMale = false;
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


    public void CheckTextOnAvatarMaleEmpty()
    {
        if (TextOnAvatarMaleEmpty)
        {
            setText(AvatarMale.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), "", Color.clear);
            TextOnAvatarMaleEmpty = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleEmpty = false;
                    TrialRunning = false;
                }
            }
        }
    }

    public void CheckTextOnAvatarMale(string s_text, Color c_color)
    {
        if (TextOnAvatarMale)
        {
            setText(AvatarMale.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarMale = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMale = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleHips(string s_text, Color c_color)
    {
        if (TextOnAvatarMaleHips)
        {
            setText(AvatarMaleHips.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarMaleHips = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleHips = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleLeftUpLeg()
    {

    }
    public void CheckTextOnAvatarMaleLeftLeg(string s_text, Color c_color)
    {
        if (TextOnAvatarMaleLeftLeg)
        {
            setText(AvatarMaleLeftLeg.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);

            TextOnAvatarMaleLeftLeg = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleLeftLeg = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleLeftFoot()
    {

    }
    public void CheckTextOnAvatarMaleLeftToeBase()
    {

    }
    public void CheckTextOnAvatarMaleRightUpLeg()
    {

    }
    public void CheckTextOnAvatarMaleRightLeg(string s_text, Color c_color)
    {
        if (TextOnAvatarMaleRightLeg)
        {
            setText(AvatarMaleRightLeg.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarMaleRightLeg = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleRightLeg = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleRightFoot()
    {

    }
    public void CheckTextOnAvatarMaleRightToeBase()
    {

    }
    public void CheckTextOnAvatarMaleSpine()
    {

    }
    public void CheckTextOnAvatarMaleSpine1()
    {

    }
    public void CheckTextOnAvatarMaleSpine2()
    {

    }
    public void CheckTextOnAvatarMaleLeftShoulder()
    {

    }
    public void CheckTextOnAvatarMaleLeftArm()
    {

    }
    public void CheckTextOnAvatarMaleLeftForeArm()
    {

    }
    public void CheckTextOnAvatarMaleLeftHand(string s_text, Color c_color)
    {        
        if (TextOnAvatarMaleLeftHand)
        {
            setText(AvatarMaleLeftHand.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarMaleLeftHand = false;
            TrackingMovingPartLeftHand = true;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleLeftHand = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleLeftHandIndex1()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandIndex2()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandIndex3()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandMiddle1()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandMiddle2()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandMiddle3()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandPinky1()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandPinky2()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandPinky3()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandRing1()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandRing2()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandRing3()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandThumb1()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandThumb2()
    {

    }
    public void CheckTextOnAvatarMaleLeftHandThumb3()
    {

    }
    public void CheckTextOnAvatarMaleNeck()
    {

    }
    public void CheckTextOnAvatarMaleHead()
    {

    }
    public void CheckTextOnAvatarMaleRightShoulder()
    {

    }
    public void CheckTextOnAvatarMaleRightArm()
    {

    }
    public void CheckTextOnAvatarMaleRightForeArm()
    {

    }
    public void CheckTextOnAvatarMaleRightHand(string s_text, Color c_color)
    {
        if (TextOnAvatarMaleRightHand)
        {
            setText(AvatarMaleRightHand.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarMaleRightHand = false;
            TrackingMovingPartRightHand = true;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarMaleRightHand = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarMaleRightHandIndex1()
    {

    }
    public void CheckTextOnAvatarMaleRightHandIndex2()
    {

    }
    public void CheckTextOnAvatarMaleRightHandIndex3()
    {

    }
    public void CheckTextOnAvatarMaleRightHandMiddle1()
    {

    }
    public void CheckTextOnAvatarMaleRightHandMiddle2()
    {

    }
    public void CheckTextOnAvatarMaleRightHandMiddle3()
    {

    }
    public void CheckTextOnAvatarMaleRightHandPinky1()
    {

    }
    public void CheckTextOnAvatarMaleRightHandPinky2()
    {

    }
    public void CheckTextOnAvatarMaleRightHandPinky3()
    {

    }
    public void CheckTextOnAvatarMaleRightHandRing1()
    {

    }
    public void CheckTextOnAvatarMaleRightHandRing2()
    {

    }
    public void CheckTextOnAvatarMaleRightHandRing3()
    {

    }
    public void CheckTextOnAvatarMaleRightHandThumb1()
    {

    }
    public void CheckTextOnAvatarMaleRightHandThumb2()
    {

    }
    public void CheckTextOnAvatarMaleRightHandThumb3()
    {

    }

    public void CheckTextOnAvatarFemaleEmpty()
    {
        if (TextOnAvatarFemaleEmpty)
        {
            setText(AvatarFemale.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), "", Color.clear);
            TextOnAvatarFemaleEmpty = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleEmpty = false;
                    TrialRunning = false;
                }
            }
        }
    }

    public void CheckTextOnAvatarFemale(string s_text, Color c_color)
    {
        if (TextOnAvatarFemale)
        {
            setText(AvatarFemale.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarFemale = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemale = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleHips(string s_text, Color c_color)
    {
        if (TextOnAvatarFemaleHips)
        {
            setText(AvatarFemaleHips.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarFemaleHips = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleHips = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleLeftUpLeg()
    {

    }
    public void CheckTextOnAvatarFemaleLeftLeg(string s_text, Color c_color)
    {
        if (TextOnAvatarFemaleLeftLeg)
        {
            setText(AvatarFemaleLeftLeg.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);

            TextOnAvatarFemaleLeftLeg = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleLeftLeg = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleLeftFoot()
    {

    }
    public void CheckTextOnAvatarFemaleLeftToeBase()
    {

    }
    public void CheckTextOnAvatarFemaleRightUpLeg()
    {

    }
    public void CheckTextOnAvatarFemaleRightLeg(string s_text, Color c_color)
    {
        if (TextOnAvatarFemaleRightLeg)
        {
            setText(AvatarFemaleRightLeg.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarFemaleRightLeg = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleRightLeg = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleRightFoot()
    {

    }
    public void CheckTextOnAvatarFemaleRightToeBase()
    {

    }
    public void CheckTextOnAvatarFemaleSpine()
    {

    }
    public void CheckTextOnAvatarFemaleSpine1()
    {

    }
    public void CheckTextOnAvatarFemaleSpine2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftShoulder()
    {

    }
    public void CheckTextOnAvatarFemaleLeftArm()
    {

    }
    public void CheckTextOnAvatarFemaleLeftForeArm()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHand(string s_text, Color c_color)
    {
        if (TextOnAvatarFemaleLeftHand)
        {
            setText(AvatarFemaleLeftHand.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarFemaleLeftHand = false;
            TrackingMovingPartLeftHand = true;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleLeftHand = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleLeftHandIndex1()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandIndex2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandIndex3()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandMiddle1()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandMiddle2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandMiddle3()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandPinky1()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandPinky2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandPinky3()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandRing1()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandRing2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandRing3()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandThumb1()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandThumb2()
    {

    }
    public void CheckTextOnAvatarFemaleLeftHandThumb3()
    {

    }
    public void CheckTextOnAvatarFemaleNeck()
    {

    }
    public void CheckTextOnAvatarFemaleHead()
    {

    }
    public void CheckTextOnAvatarFemaleRightShoulder()
    {

    }
    public void CheckTextOnAvatarFemaleRightArm()
    {

    }
    public void CheckTextOnAvatarFemaleRightForeArm()
    {

    }
    public void CheckTextOnAvatarFemaleRightHand(string s_text, Color c_color)
    {
        if (TextOnAvatarFemaleRightHand)
        {
            setText(AvatarFemaleRightHand.transform.position + new Vector3(0, 0.150f, 0), new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarFemaleRightHand = false;
            TrackingMovingPartRightHand = true;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarFemaleRightHand = false;
                    TrialRunning = false;
                }
            }
        }
    }
    public void CheckTextOnAvatarFemaleRightHandIndex1()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandIndex2()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandIndex3()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandMiddle1()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandMiddle2()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandMiddle3()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandPinky1()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandPinky2()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandPinky3()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandRing1()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandRing2()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandRing3()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandThumb1()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandThumb2()
    {

    }
    public void CheckTextOnAvatarFemaleRightHandThumb3()
    {

    }
    public void CheckTextOnAvatarOther(string s_text, Color c_color)
    {
        if (TextOnAvatarOther)
        {
            //setText(v3_Other, new Vector3(0, 0, 0), s_text, c_color);
            setText(this.transform.position, new Vector3(0, 0, 0), s_text, c_color);
            TextOnAvatarOther = false;
            if (TrialRunning)
            {
                if (TextToBeDisplayed.active == false)
                {
                    TextOnAvatarOther = false;
                    TrialRunning = false;
                }
            }
        }
    }

	public void ExpyVRCheckDisplayTextFullStringsEntryContinuousScale(string s_position, string s_text, string s_color)
	{
		string s_text_1 = s_text.Replace('_', ' ');
		string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
		expyVRwriteInFile(timestamp + " - [continuousscale] [0] - " + s_position + " - " + s_text_1 + " - " + s_color);

		Color c_color;

		if (s_color == "clear")
		{
			c_color = Color.clear;
		}
		else if (s_color == "red")
		{
			c_color = Color.red;
		}
		else if (s_color == "yellow")
		{
			c_color = Color.yellow;
		}
		else if (s_color == "green")
		{
			c_color = Color.green;
		}
		else if (s_color == "blue")
		{
			c_color = Color.blue;
		}
		else if (s_color == "black")
		{
			c_color = Color.black;
		}
		else if (s_color == "white")
		{
			c_color = Color.white;
		}
		else if (s_color == "cyan")
		{
			c_color = Color.cyan;
		}
		else if (s_color == "gray")
		{
			c_color = Color.gray;
		}
		else if (s_color == "grey")
		{
			c_color = Color.grey;
		}
		else if (s_color == "magenta")
		{
			c_color = Color.magenta;
		}
		else
		{
			c_color = Color.clear;
		}

		ExpyVRCheckDisplayTextFull(s_position, s_text_1, c_color);
	}


	public void ExpyVRCheckDisplayTextFullStringsEntry(string s_position, string s_text, string s_color)
    {
        string s_text_1 = s_text.Replace('_', ' ');
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        expyVRwriteInFile(timestamp + " - [text_body_part] [0] - " + s_position + " - " + s_text_1 + " - " + s_color);

        Color c_color;

        if (s_color == "clear")
        {
            c_color = Color.clear;
        }
        else if (s_color == "red")
        {
            c_color = Color.red;
        }
        else if (s_color == "yellow")
        {
            c_color = Color.yellow;
        }
        else if (s_color == "green")
        {
            c_color = Color.green;
        }
        else if (s_color == "blue")
        {
            c_color = Color.blue;
        }
        else if (s_color == "black")
        {
            c_color = Color.black;
        }
        else if (s_color == "white")
        {
            c_color = Color.white;
        }
        else if (s_color == "cyan")
        {
            c_color = Color.cyan;
        }
        else if (s_color == "gray")
        {
            c_color = Color.gray;
        }
        else if (s_color == "grey")
        {
            c_color = Color.grey;
        }
        else if (s_color == "magenta")
        {
            c_color = Color.magenta;
        }
        else
        {
            c_color = Color.clear;
        }

        ExpyVRCheckDisplayTextFull(s_position, s_text_1, c_color);
    }


    public void ExpyVRCheckDisplayTextFull(string s_position, string s_text, Color c_color)
    {
        if (isMale)
        {
            if (s_position == "Empty")
            {
                TextOnAvatarMaleEmpty = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleEmpty();
            }
            else if (s_position == "LeftKnee")
            {
                TextOnAvatarMaleLeftLeg = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleLeftLeg(s_text, c_color);
            }
            else if (s_position == "RightKnee")
            {
                TextOnAvatarMaleRightLeg = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleRightLeg(s_text, c_color);
            }
            else if (s_position == "RightHand")
            {
                TextOnAvatarMaleRightHand = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleRightHand(s_text, c_color);
            }
            else if (s_position == "LeftHand")
            {
                TextOnAvatarMaleLeftHand = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleLeftHand(s_text, c_color);
            }
            else if (s_position == "Hips")
            {
                TextOnAvatarMaleHips = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarMaleHips(s_text, c_color);
            }
			else if (s_position == "Other")
            {
                TextOnAvatarOther = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarOther(s_text, c_color);
            }
        }
        else
        {
            if (s_position == "Empty")
            {
                TextOnAvatarFemaleEmpty = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleEmpty();
            }
            else if (s_position == "LeftKnee")
            {
                TextOnAvatarFemaleLeftLeg = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleLeftLeg(s_text, c_color);
            }
            else if (s_position == "RightKnee")
            {
                TextOnAvatarFemaleRightLeg = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleRightLeg(s_text, c_color);
            }
            else if (s_position == "RightHand")
            {
                TextOnAvatarFemaleRightHand = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleRightHand(s_text, c_color);
            }
            else if (s_position == "LeftHand")
            {
                TextOnAvatarFemaleLeftHand = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleLeftHand(s_text, c_color);
            }
            else if (s_position == "Hips")
            {
                TextOnAvatarFemaleHips = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarFemaleHips(s_text, c_color);
            }
            else if (s_position == "Other")
            {
                TextOnAvatarOther = true;
                StartDisplayTrial = true;
                CheckDisplayText();
                CheckTextOnAvatarOther(s_text, c_color);
            }
        }
    }




    public void ExpyVRCheckDisplayContinuousScale(float speed, string s_textLeftMin, string s_textRightMax)
    {
        continuousScaleTextLeftMin.GetComponent<TextMesh>().text = s_textLeftMin.Replace('_', ' ');
        continuousScaleTextRightMax.GetComponent<TextMesh>().text = s_textRightMax.Replace('_', ' ');
        continuousScaleTextDiscreteScale.GetComponent<TextMesh>().text = "";
        currentContinuousScaleValueSelected = -1;
        ContinuousScaleSpeed = speed * 250.0f;
        continuousScale.SetActive(true);
        b_isContinuousScaleActive = true;
        randomAdd = UnityEngine.Random.Range(-1000.0f, 1000.0f);
    }

    public void ExpyVRCheckDisplayContinuousScale_keyboardControlled(float speed, string s_textLeftMin, string s_textRightMax)
    {
        continuousScaleTextLeftMin.GetComponent<TextMesh>().text = s_textLeftMin.Replace('_', ' ');
        continuousScaleTextRightMax.GetComponent<TextMesh>().text = s_textRightMax.Replace('_', ' ');
        continuousScaleTextDiscreteScale.GetComponent<TextMesh>().text = "0    1    2    3    4    5    6";
        currentContinuousScaleValueSelected = -1;
        ContinuousScaleSpeed = speed / 25;
        continuousScale.SetActive(true);
        b_isContinuousScaleActive = true;
        b_isContinuousScaleKeyboardControlled = true;
        continuousScale.transform.Find("Slider").localPosition = new Vector3(0, 0, 0);
        currentSliderValue = MaxContinuousScaleValue / 2;
        i_indice_f_list_continuousScale_values = 3;
    }
    bool b_isContinuousScaleKeyboardControlled;

    public void ExpyVRStopDisplayContinuousScale()
    {
        continuousScale.SetActive(false);
        b_isContinuousScaleActive = false;
        b_isContinuousScaleKeyboardControlled = false;

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        expyVRwriteInFilePSpace(timestamp + " - [continuousscale] [1] - value : " + currentContinuousScaleValueSelected);
	}


	public void ExpyVRStopDisplayTrialContinuousScale()
	{
		if (expyVRTrialRunningEnableInput)
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : -1");

			_script_main_experiment_manager.s_last_input_answer = "answer_none";
			_script_main_experiment_manager.s_last_input_command = "continuousscale";
		}

		StopDisplayTrial = true;
		CheckDisplayText();
		TrialRunning = false;
		TrackingMovingPartLeftHand = false;
		TrackingMovingPartRightHand = false;
		expyVRTrialRunningEnableInput = false;
	}


	public void ExpyVRStopDisplayTrial()
    {
        if (expyVRTrialRunningEnableInput)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : -1");

			_script_main_experiment_manager.s_last_input_answer = "answer_none";
			_script_main_experiment_manager.s_last_input_command = "text_body_part";
		}

        StopDisplayTrial = true;
        CheckDisplayText();
        TrialRunning = false;
        TrackingMovingPartLeftHand = false;
        TrackingMovingPartRightHand = false;
        expyVRTrialRunningEnableInput = false;
	}

        
    public void ExpyVRCheckAnswerInputKeyboard()
    {
        /*if (Input.GetButtonDown("Button1")) //(Input.GetKeyDown(KeyCode.Alpha1))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 1");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_1";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 1");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_1";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
        else if (Input.GetButtonDown("Button2"))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 2");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_2";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 2");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_2";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
        else if (Input.GetButtonDown("Button3"))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 3");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_3";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 3");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_3";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
        else*/ if (Input.GetButtonDown("Button6"))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 4");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_4";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 4");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_4";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
        else if (Input.GetButtonDown("Button7"))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 5");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_5";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 5");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_5";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
        else if (Input.GetButtonDown("Button8"))
		{
            if (b_isContinuousScaleActive)
            {
                currentContinuousScaleValueSelected = currentSliderValue;

                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFile(timestamp + " - [continuousscale] [1] - input : 6");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_6";
				_script_main_experiment_manager.s_last_input_command = "continuousscale";
			}
            else
            {
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                expyVRwriteInFilePSpace(timestamp + " - [text_body_part] [1] - input : 6");

                expyVRTrialRunningEnableInput = false;

				_script_main_experiment_manager.s_last_input_answer = "answer_6";
				_script_main_experiment_manager.s_last_input_command = "text_body_part";
			}
        }
    }


    public bool ExpyVRCheckAnswerFilesExistanceAndSetItAsCurrentAnswerFile(string FilePath)
    {
        SaveFilePath = FilePath;
        if (File.Exists(SaveFilePath))
        {
            Debug.Log("Answers File already exists");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void setAnswerFilePath(string FilePath)
    {
        SaveFilePath = FilePath;
    }


    public void expyVRwriteInFile(string s_toWrite)
    {
        // Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(SaveFilePath, true);
        writer.WriteLine(s_toWrite);

        writer.Close();
    }

    public void expyVRwriteInFilePSpace(string s_toWrite)
    {
        // Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(SaveFilePath, true);
        writer.WriteLine(s_toWrite);
        writer.WriteLine("");

        writer.Close();
    }
}
