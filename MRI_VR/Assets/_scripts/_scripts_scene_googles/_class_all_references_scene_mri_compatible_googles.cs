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

public class _class_all_references_scene_mri_compatible_googles : _Singleton<_class_all_references_scene_mri_compatible_googles> {

    // (Optional) Prevent non-singleton constructor use
    protected _class_all_references_scene_mri_compatible_googles() { }

    // Then add whatver code to the class you need as you normally would

    // configuration parameters
    private int targetFPS = 60;

    // GameObject references
    public GameObject GO_MRI_scene;
    public GameObject GO_MRI_scene_MRI_room;
    public GameObject GO_television;
    public GameObject GO_television_TVScreen;
    public GameObject GO_camera_right_eye;
    public GameObject GO_camera_left_eye;
    public GameObject GO_character_camera;
    public GameObject GO_rb_RBTable;
    public _main_experiment_manager script_main_experiment_manager;
    public GameObject GO_main_experiment_manager;


    // character references
    public GameObject GO_Character;

    [Header("Avatar Male References")]
    public GameObject GO_avatar_male;
    public GameObject GO_avatar_male_Hips;
    public GameObject GO_avatar_male_LeftUpLeg;
    public GameObject GO_avatar_male_LeftLeg;
    public GameObject GO_avatar_male_LeftFoot;
    public GameObject GO_avatar_male_LeftToeBase;
    public GameObject GO_avatar_male_RightUpLeg;
    public GameObject GO_avatar_male_RightLeg;
    public GameObject GO_avatar_male_RightFoot;
    public GameObject GO_avatar_male_RightToeBase;
    public GameObject GO_avatar_male_Spine;
    public GameObject GO_avatar_male_Spine1;
    public GameObject GO_avatar_male_Spine2;
    public GameObject GO_avatar_male_LeftShoulder;
    public GameObject GO_avatar_male_LeftArm;
    public GameObject GO_avatar_male_LeftForeArm;
    public GameObject GO_avatar_male_LeftHand;
    public GameObject GO_avatar_male_LeftHandIndex1;
    public GameObject GO_avatar_male_LeftHandIndex2;
    public GameObject GO_avatar_male_LeftHandIndex3;
    public GameObject GO_avatar_male_LeftHandMiddle1;
    public GameObject GO_avatar_male_LeftHandMiddle2;
    public GameObject GO_avatar_male_LeftHandMiddle3;
    public GameObject GO_avatar_male_LeftHandPinky1;
    public GameObject GO_avatar_male_LeftHandPinky2;
    public GameObject GO_avatar_male_LeftHandPinky3;
    public GameObject GO_avatar_male_LeftHandRing1;
    public GameObject GO_avatar_male_LeftHandRing2;
    public GameObject GO_avatar_male_LeftHandRing3;
    public GameObject GO_avatar_male_LeftHandThumb1;
    public GameObject GO_avatar_male_LeftHandThumb2;
    public GameObject GO_avatar_male_LeftHandThumb3;
    public GameObject GO_avatar_male_Neck;
    public GameObject GO_avatar_male_Head;
    public GameObject GO_avatar_male_RightShoulder;
    public GameObject GO_avatar_male_RightArm;
    public GameObject GO_avatar_male_RightForeArm;
    public GameObject GO_avatar_male_RightHand;
    public GameObject GO_avatar_male_RightHandIndex1;
    public GameObject GO_avatar_male_RightHandIndex2;
    public GameObject GO_avatar_male_RightHandIndex3;
    public GameObject GO_avatar_male_RightHandMiddle1;
    public GameObject GO_avatar_male_RightHandMiddle2;
    public GameObject GO_avatar_male_RightHandMiddle3;
    public GameObject GO_avatar_male_RightHandPinky1;
    public GameObject GO_avatar_male_RightHandPinky2;
    public GameObject GO_avatar_male_RightHandPinky3;
    public GameObject GO_avatar_male_RightHandRing1;
    public GameObject GO_avatar_male_RightHandRing2;
    public GameObject GO_avatar_male_RightHandRing3;
    public GameObject GO_avatar_male_RightHandThumb1;
    public GameObject GO_avatar_male_RightHandThumb2;
    public GameObject GO_avatar_male_RightHandThumb3;

    [Header("Avatar Female References")]
    public GameObject GO_avatar_female;
    public GameObject GO_avatar_female_Hips;
    public GameObject GO_avatar_female_LeftUpLeg;
    public GameObject GO_avatar_female_LeftLeg;
    public GameObject GO_avatar_female_LeftFoot;
    public GameObject GO_avatar_female_LeftToeBase;
    public GameObject GO_avatar_female_RightUpLeg;
    public GameObject GO_avatar_female_RightLeg;
    public GameObject GO_avatar_female_RightFoot;
    public GameObject GO_avatar_female_RightToeBase;
    public GameObject GO_avatar_female_Spine;
    public GameObject GO_avatar_female_Spine1;
    public GameObject GO_avatar_female_Spine2;
    public GameObject GO_avatar_female_LeftShoulder;
    public GameObject GO_avatar_female_LeftArm;
    public GameObject GO_avatar_female_LeftForeArm;
    public GameObject GO_avatar_female_LeftHand;
    public GameObject GO_avatar_female_LeftHandIndex1;
    public GameObject GO_avatar_female_LeftHandIndex2;
    public GameObject GO_avatar_female_LeftHandIndex3;
    public GameObject GO_avatar_female_LeftHandMiddle1;
    public GameObject GO_avatar_female_LeftHandMiddle2;
    public GameObject GO_avatar_female_LeftHandMiddle3;
    public GameObject GO_avatar_female_LeftHandPinky1;
    public GameObject GO_avatar_female_LeftHandPinky2;
    public GameObject GO_avatar_female_LeftHandPinky3;
    public GameObject GO_avatar_female_LeftHandRing1;
    public GameObject GO_avatar_female_LeftHandRing2;
    public GameObject GO_avatar_female_LeftHandRing3;
    public GameObject GO_avatar_female_LeftHandThumb1;
    public GameObject GO_avatar_female_LeftHandThumb2;
    public GameObject GO_avatar_female_LeftHandThumb3;
    public GameObject GO_avatar_female_Neck;
    public GameObject GO_avatar_female_Head;
    public GameObject GO_avatar_female_RightShoulder;
    public GameObject GO_avatar_female_RightArm;
    public GameObject GO_avatar_female_RightForeArm;
    public GameObject GO_avatar_female_RightHand;
    public GameObject GO_avatar_female_RightHandIndex1;
    public GameObject GO_avatar_female_RightHandIndex2;
    public GameObject GO_avatar_female_RightHandIndex3;
    public GameObject GO_avatar_female_RightHandMiddle1;
    public GameObject GO_avatar_female_RightHandMiddle2;
    public GameObject GO_avatar_female_RightHandMiddle3;
    public GameObject GO_avatar_female_RightHandPinky1;
    public GameObject GO_avatar_female_RightHandPinky2;
    public GameObject GO_avatar_female_RightHandPinky3;
    public GameObject GO_avatar_female_RightHandRing1;
    public GameObject GO_avatar_female_RightHandRing2;
    public GameObject GO_avatar_female_RightHandRing3;
    public GameObject GO_avatar_female_RightHandThumb1;
    public GameObject GO_avatar_female_RightHandThumb2;
    public GameObject GO_avatar_female_RightHandThumb3;

    [Header("NPC Male References")]
    public GameObject GO_NPC_male;
    public GameObject GO_NPC_male_Hips;
    public GameObject GO_NPC_male_LeftUpLeg;
    public GameObject GO_NPC_male_LeftLeg;
    public GameObject GO_NPC_male_LeftFoot;
    public GameObject GO_NPC_male_LeftToeBase;
    public GameObject GO_NPC_male_RightUpLeg;
    public GameObject GO_NPC_male_RightLeg;
    public GameObject GO_NPC_male_RightFoot;
    public GameObject GO_NPC_male_RightToeBase;
    public GameObject GO_NPC_male_Spine;
    public GameObject GO_NPC_male_Spine1;
    public GameObject GO_NPC_male_Spine2;
    public GameObject GO_NPC_male_LeftShoulder;
    public GameObject GO_NPC_male_LeftArm;
    public GameObject GO_NPC_male_LeftForeArm;
    public GameObject GO_NPC_male_LeftHand;
    public GameObject GO_NPC_male_LeftHandIndex1;
    public GameObject GO_NPC_male_LeftHandIndex2;
    public GameObject GO_NPC_male_LeftHandIndex3;
    public GameObject GO_NPC_male_LeftHandMiddle1;
    public GameObject GO_NPC_male_LeftHandMiddle2;
    public GameObject GO_NPC_male_LeftHandMiddle3;
    public GameObject GO_NPC_male_LeftHandPinky1;
    public GameObject GO_NPC_male_LeftHandPinky2;
    public GameObject GO_NPC_male_LeftHandPinky3;
    public GameObject GO_NPC_male_LeftHandRing1;
    public GameObject GO_NPC_male_LeftHandRing2;
    public GameObject GO_NPC_male_LeftHandRing3;
    public GameObject GO_NPC_male_LeftHandThumb1;
    public GameObject GO_NPC_male_LeftHandThumb2;
    public GameObject GO_NPC_male_LeftHandThumb3;
    public GameObject GO_NPC_male_Neck;
    public GameObject GO_NPC_male_Head;
    public GameObject GO_NPC_male_RightShoulder;
    public GameObject GO_NPC_male_RightArm;
    public GameObject GO_NPC_male_RightForeArm;
    public GameObject GO_NPC_male_RightHand;
    public GameObject GO_NPC_male_RightHandIndex1;
    public GameObject GO_NPC_male_RightHandIndex2;
    public GameObject GO_NPC_male_RightHandIndex3;
    public GameObject GO_NPC_male_RightHandMiddle1;
    public GameObject GO_NPC_male_RightHandMiddle2;
    public GameObject GO_NPC_male_RightHandMiddle3;
    public GameObject GO_NPC_male_RightHandPinky1;
    public GameObject GO_NPC_male_RightHandPinky2;
    public GameObject GO_NPC_male_RightHandPinky3;
    public GameObject GO_NPC_male_RightHandRing1;
    public GameObject GO_NPC_male_RightHandRing2;
    public GameObject GO_NPC_male_RightHandRing3;
    public GameObject GO_NPC_male_RightHandThumb1;
    public GameObject GO_NPC_male_RightHandThumb2;
    public GameObject GO_NPC_male_RightHandThumb3;

    [Header("NPC Female References")]
    public GameObject GO_NPC_female;
    public GameObject GO_NPC_female_Hips;
    public GameObject GO_NPC_female_LeftUpLeg;
    public GameObject GO_NPC_female_LeftLeg;
    public GameObject GO_NPC_female_LeftFoot;
    public GameObject GO_NPC_female_LeftToeBase;
    public GameObject GO_NPC_female_RightUpLeg;
    public GameObject GO_NPC_female_RightLeg;
    public GameObject GO_NPC_female_RightFoot;
    public GameObject GO_NPC_female_RightToeBase;
    public GameObject GO_NPC_female_Spine;
    public GameObject GO_NPC_female_Spine1;
    public GameObject GO_NPC_female_Spine2;
    public GameObject GO_NPC_female_LeftShoulder;
    public GameObject GO_NPC_female_LeftArm;
    public GameObject GO_NPC_female_LeftForeArm;
    public GameObject GO_NPC_female_LeftHand;
    public GameObject GO_NPC_female_LeftHandIndex1;
    public GameObject GO_NPC_female_LeftHandIndex2;
    public GameObject GO_NPC_female_LeftHandIndex3;
    public GameObject GO_NPC_female_LeftHandMiddle1;
    public GameObject GO_NPC_female_LeftHandMiddle2;
    public GameObject GO_NPC_female_LeftHandMiddle3;
    public GameObject GO_NPC_female_LeftHandPinky1;
    public GameObject GO_NPC_female_LeftHandPinky2;
    public GameObject GO_NPC_female_LeftHandPinky3;
    public GameObject GO_NPC_female_LeftHandRing1;
    public GameObject GO_NPC_female_LeftHandRing2;
    public GameObject GO_NPC_female_LeftHandRing3;
    public GameObject GO_NPC_female_LeftHandThumb1;
    public GameObject GO_NPC_female_LeftHandThumb2;
    public GameObject GO_NPC_female_LeftHandThumb3;
    public GameObject GO_NPC_female_Neck;
    public GameObject GO_NPC_female_Head;
    public GameObject GO_NPC_female_RightShoulder;
    public GameObject GO_NPC_female_RightArm;
    public GameObject GO_NPC_female_RightForeArm;
    public GameObject GO_NPC_female_RightHand;
    public GameObject GO_NPC_female_RightHandIndex1;
    public GameObject GO_NPC_female_RightHandIndex2;
    public GameObject GO_NPC_female_RightHandIndex3;
    public GameObject GO_NPC_female_RightHandMiddle1;
    public GameObject GO_NPC_female_RightHandMiddle2;
    public GameObject GO_NPC_female_RightHandMiddle3;
    public GameObject GO_NPC_female_RightHandPinky1;
    public GameObject GO_NPC_female_RightHandPinky2;
    public GameObject GO_NPC_female_RightHandPinky3;
    public GameObject GO_NPC_female_RightHandRing1;
    public GameObject GO_NPC_female_RightHandRing2;
    public GameObject GO_NPC_female_RightHandRing3;
    public GameObject GO_NPC_female_RightHandThumb1;
    public GameObject GO_NPC_female_RightHandThumb2;
    public GameObject GO_NPC_female_RightHandThumb3;

    public GameObject GO_hand_targets;
    public GameObject GO_hand_targets_right_hand_RH;
    public GameObject GO_hand_targets_left_hand_LH;
    public GameObject GO_foot_targets;

    public GameObject GO_tennis_ball_left_hand;
    public GameObject GO_tennis_ball_right_hand;
    public GameObject GO_target_male_tennis_ball_left_hand;
    public GameObject GO_target_male_tennis_ball_right_hand;
    public GameObject GO_target_female_tennis_ball_left_hand;
    public GameObject GO_target_female_tennis_ball_right_hand;

    public GameObject GO_hand_targets_right_hand;
    public GameObject GO_hand_targets_right_hand_right_hand_wrist;
    public GameObject GO_hand_targets_left_hand;
    public GameObject GO_hand_targets_left_hand_left_hand_wrist;

    public GameObject GO_avatars_config;

    public GameObject GO_NPC_Character;

    //3d questions mechanism
    [Header("3D Questions")]
    public GameObject GO_questions_exp;
    public GameObject GO_question_scene_text;
    public GameObject GO_questions_selector_mecanism_AnswerTrue;
    public GameObject GO_questions_selector_mecanism_AnswerFalse;

    //continuous scale
    [Header("Continuous Scale")]
    public GameObject GO_continuous_scale;
    public GameObject GO_continuous_scale_and_stroop_body;
    public GameObject GO_continuous_scale_text_center;
    public GameObject GO_continuous_scale_text_left_min;
    public GameObject GO_continuous_scale_text_right_max;
    public GameObject GO_continuous_scale_text_discrete_scale;

    //named pipe references
    [Header("Named Pipes")]
    public GameObject GO_server_pipe;
    public GameObject GO_client_pipe;

    [Header("Qualisys")]
    public GameObject GO_qualisys_connection;

    [Header("Fade GameObjects")]
    public GameObject GO_mriroom_mri_magnet;
    public GameObject GO_mriroom_mri_table;
    public GameObject GO_mriroom_roomamdothers_ceiling;
    public GameObject GO_mriroom_roomamdothers_chariot;
    public GameObject GO_mriroom_roomamdothers_chariot001;
    public GameObject GO_mriroom_roomamdothers_chariot002;
    public GameObject GO_mriroom_roomamdothers_floor;
    public GameObject GO_mriroom_roomamdothers_frontwalls;
    public GameObject GO_mriroom_roomamdothers_sidewalls;
    public GameObject GO_mrimovingtable;


    // Init all references
    private void Awake()
    {
        // target framerate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
        //

        // get references
        //GO_MRI_scene = GameObject.Find("_MRI_scene");
       // GO_MRI_scene_MRI_room = GameObject.Find("_MRI_scene/MRIRoom");
        //GO_television = GameObject.Find("_MRI_scene/MRIRoom/Television");
       // GO_television_TVScreen = GameObject.Find("_MRI_scene/MRIRoom/Television/TVScreen");
       // GO_television_TVScreen.SetActive(false);
        GO_camera_right_eye = GameObject.Find("_AvatarsConfig/_Character/Camera/Camera_Right");
        GO_camera_left_eye = GameObject.Find("_AvatarsConfig/_Character/Camera/Camera_Left");
        //GO_rb_RBTable = GameObject.Find("_MRI_scene/RBTable");
        script_main_experiment_manager = GameObject.Find("_MainExperimentManager").GetComponent<_main_experiment_manager>();
        GO_main_experiment_manager = GameObject.Find("_MainExperimentManager");

  
        GO_Character = GameObject.Find("_AvatarsConfig/_Character");
        GO_character_camera = GameObject.Find("_AvatarsConfig/_Character/Camera");
        GetAvatarMaleReferences();
        GetAvatarFemaleReferences();

        GO_hand_targets = GameObject.Find("_AvatarsConfig/_HandTargets");
        GO_hand_targets_right_hand_RH = GameObject.Find("_AvatarsConfig/_HandTargets/RightHand/RH");
        GO_hand_targets_left_hand_LH = GameObject.Find("_AvatarsConfig/_HandTargets/LeftHand/LH");
        GO_foot_targets = GameObject.Find("_AvatarsConfig/_FootTargets");
        GO_hand_targets_right_hand = GameObject.Find("_AvatarsConfig/_HandTargets/RightHand");
        GO_hand_targets_right_hand_right_hand_wrist = GameObject.Find("_AvatarsConfig/_HandTargets/RightHand/RightHandWrist");
        GO_hand_targets_left_hand = GameObject.Find("_AvatarsConfig/_HandTargets/LeftHand");
        GO_hand_targets_left_hand_left_hand_wrist = GameObject.Find("_AvatarsConfig/_HandTargets/LeftHand/LeftHandWrist");

        GO_tennis_ball_left_hand = GameObject.Find("_AvatarsConfig/_Character/TennisBallLeftHand");
        GO_tennis_ball_right_hand = GameObject.Find("_AvatarsConfig/_Character/TennisBallRightHand");
        GO_target_male_tennis_ball_left_hand = GO_avatar_male_LeftHand.gameObject.transform.GetChild(5).gameObject;
        GO_target_male_tennis_ball_right_hand = GO_avatar_male_RightHand.gameObject.transform.GetChild(5).gameObject;
        GO_target_female_tennis_ball_left_hand = GO_avatar_female_LeftHand.gameObject.transform.GetChild(5).gameObject;
        GO_target_female_tennis_ball_right_hand = GO_avatar_female_RightHand.gameObject.transform.GetChild(5).gameObject;

        GO_questions_exp = GameObject.Find("_QuestionsExp");
        GO_question_scene_text = GameObject.Find("_QuestionsExp/CurrentQuestion");
        GO_questions_selector_mecanism_AnswerTrue = GameObject.Find("_QuestionsExp/SelectorMecanism/ItemTrue");
        GO_questions_selector_mecanism_AnswerFalse = GameObject.Find("_QuestionsExp/SelectorMecanism/ItemFalse");

        GO_continuous_scale = GameObject.Find("_MainExperimentManager/SliderGameObject");
        GO_continuous_scale_and_stroop_body = GameObject.Find("_ContinuousScaleAndStroopBody");
        GO_continuous_scale_text_center = GameObject.Find("_ContinuousScaleAndStroopBody/TextToBeDisplayed");
        GO_continuous_scale_text_left_min = GameObject.Find("_MainExperimentManager/SliderGameObject/TextLeftMin");
        GO_continuous_scale_text_right_max = GameObject.Find("_MainExperimentManager/SliderGameObject/TextRightMax");
        GO_continuous_scale_text_discrete_scale = GameObject.Find("_MainExperimentManager/SliderGameObject/TextDiscreteScale");

        GO_avatars_config = GameObject.Find("_AvatarsConfig");

        GO_server_pipe = GameObject.Find("_NamedPipe/GO_server_pipe");
        GO_client_pipe = GameObject.Find("_NamedPipe/GO_client_pipe");

        GO_NPC_Character = GameObject.Find("_NPC_Character");
        GetNPCMaleReferences();
        GetNPCFemaleReferences();

        GO_qualisys_connection = GameObject.Find("_QualisysConnection");

        script_main_experiment_manager = GameObject.Find("_MainExperimentManager").GetComponent<_main_experiment_manager>();

        getFadeGameObjectsReferences();
    }

    public void GetMRIReferences()
    {
        GO_MRI_scene = GameObject.Find("_MRI_scene");
        GO_MRI_scene_MRI_room = GameObject.Find("_MRI_scene/MRIRoom");
        GO_television = GameObject.Find("_MRI_scene/MRIRoom/Television");
        GO_television_TVScreen = GameObject.Find("_MRI_scene/MRIRoom/Television/TVScreen");
        GO_television_TVScreen.SetActive(false);

        GO_rb_RBTable = GameObject.Find("_MRI_scene/RBTable");
    }

    void getFadeGameObjectsReferences()
    {
        GO_mriroom_mri_magnet = GameObject.Find("_MRI_scene/MRIRoom/MRI/Magnet");
        GO_mriroom_mri_table = GameObject.Find("_MRI_scene/MRIRoom/MRI/Table");
        GO_mriroom_roomamdothers_ceiling = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/Ceiling");
        GO_mriroom_roomamdothers_chariot = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/Chariot");
        GO_mriroom_roomamdothers_chariot001 = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/Chariot001");
        GO_mriroom_roomamdothers_chariot002 = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/Chariot002");
        GO_mriroom_roomamdothers_floor = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/Floor");
        GO_mriroom_roomamdothers_frontwalls = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/FrontWalls");
        GO_mriroom_roomamdothers_sidewalls = GameObject.Find("_MRI_scene/MRIRoom/RoomAndOthers/SideWalls");
        GO_mrimovingtable = GameObject.Find("_MRI_scene/MRIMovingTable");
    }

    void GetAvatarMaleReferences()
    {
        GO_avatar_male = GO_Character.transform.GetChild(0).gameObject;
        GO_avatar_male_Hips = GO_avatar_male.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftUpLeg = GO_avatar_male_Hips.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftLeg = GO_avatar_male_LeftUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftFoot = GO_avatar_male_LeftLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftToeBase = GO_avatar_male_LeftFoot.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightUpLeg = GO_avatar_male_Hips.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_male_RightLeg = GO_avatar_male_RightUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightFoot = GO_avatar_male_RightLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightToeBase = GO_avatar_male_RightFoot.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_Spine = GO_avatar_male_Hips.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_male_Spine1 = GO_avatar_male_Spine.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_Spine2 = GO_avatar_male_Spine1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftShoulder = GO_avatar_male_Spine2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftArm = GO_avatar_male_LeftShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftForeArm = GO_avatar_male_LeftArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHand = GO_avatar_male_LeftForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandIndex1 = GO_avatar_male_LeftHand.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandIndex2 = GO_avatar_male_LeftHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandIndex3 = GO_avatar_male_LeftHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandMiddle1 = GO_avatar_male_LeftHand.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_male_LeftHandMiddle2 = GO_avatar_male_LeftHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandMiddle3 = GO_avatar_male_LeftHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandPinky1 = GO_avatar_male_LeftHand.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_male_LeftHandPinky2 = GO_avatar_male_LeftHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandPinky3 = GO_avatar_male_LeftHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandRing1 = GO_avatar_male_LeftHand.gameObject.transform.GetChild(3).gameObject;
        GO_avatar_male_LeftHandRing2 = GO_avatar_male_LeftHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandRing3 = GO_avatar_male_LeftHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandThumb1 = GO_avatar_male_LeftHand.gameObject.transform.GetChild(4).gameObject;
        GO_avatar_male_LeftHandThumb2 = GO_avatar_male_LeftHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_LeftHandThumb3 = GO_avatar_male_LeftHandThumb2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_Neck = GO_avatar_male_Spine2.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_male_Head = GO_avatar_male_Neck.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightShoulder = GO_avatar_male_Spine2.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_male_RightArm = GO_avatar_male_RightShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightForeArm = GO_avatar_male_RightArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHand = GO_avatar_male_RightForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandIndex1 = GO_avatar_male_RightHand.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandIndex2 = GO_avatar_male_RightHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandIndex3 = GO_avatar_male_RightHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandMiddle1 = GO_avatar_male_RightHand.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_male_RightHandMiddle2 = GO_avatar_male_RightHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandMiddle3 = GO_avatar_male_RightHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandPinky1 = GO_avatar_male_RightHand.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_male_RightHandPinky2 = GO_avatar_male_RightHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandPinky3 = GO_avatar_male_RightHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandRing1 = GO_avatar_male_RightHand.gameObject.transform.GetChild(3).gameObject;
        GO_avatar_male_RightHandRing2 = GO_avatar_male_RightHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandRing3 = GO_avatar_male_RightHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandThumb1 = GO_avatar_male_RightHand.gameObject.transform.GetChild(4).gameObject;
        GO_avatar_male_RightHandThumb2 = GO_avatar_male_RightHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_male_RightHandThumb3 = GO_avatar_male_RightHandThumb2.gameObject.transform.GetChild(0).gameObject;
    }

    void GetAvatarFemaleReferences()
    {
        GO_avatar_female = GO_Character.transform.GetChild(1).gameObject;
        GO_avatar_female_Hips = GO_avatar_female.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftUpLeg = GO_avatar_female_Hips.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftLeg = GO_avatar_female_LeftUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftFoot = GO_avatar_female_LeftLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftToeBase = GO_avatar_female_LeftFoot.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightUpLeg = GO_avatar_female_Hips.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_female_RightLeg = GO_avatar_female_RightUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightFoot = GO_avatar_female_RightLeg.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightToeBase = GO_avatar_female_RightFoot.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_Spine = GO_avatar_female_Hips.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_female_Spine1 = GO_avatar_female_Spine.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_Spine2 = GO_avatar_female_Spine1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftShoulder = GO_avatar_female_Spine2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftArm = GO_avatar_female_LeftShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftForeArm = GO_avatar_female_LeftArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHand = GO_avatar_female_LeftForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandIndex1 = GO_avatar_female_LeftHand.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandIndex2 = GO_avatar_female_LeftHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandIndex3 = GO_avatar_female_LeftHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandMiddle1 = GO_avatar_female_LeftHand.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_female_LeftHandMiddle2 = GO_avatar_female_LeftHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandMiddle3 = GO_avatar_female_LeftHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandPinky1 = GO_avatar_female_LeftHand.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_female_LeftHandPinky2 = GO_avatar_female_LeftHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandPinky3 = GO_avatar_female_LeftHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandRing1 = GO_avatar_female_LeftHand.gameObject.transform.GetChild(3).gameObject;
        GO_avatar_female_LeftHandRing2 = GO_avatar_female_LeftHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandRing3 = GO_avatar_female_LeftHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandThumb1 = GO_avatar_female_LeftHand.gameObject.transform.GetChild(4).gameObject;
        GO_avatar_female_LeftHandThumb2 = GO_avatar_female_LeftHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_LeftHandThumb3 = GO_avatar_female_LeftHandThumb2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_Neck = GO_avatar_female_Spine2.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_female_Head = GO_avatar_female_Neck.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightShoulder = GO_avatar_female_Spine2.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_female_RightArm = GO_avatar_female_RightShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightForeArm = GO_avatar_female_RightArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHand = GO_avatar_female_RightForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandIndex1 = GO_avatar_female_RightHand.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandIndex2 = GO_avatar_female_RightHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandIndex3 = GO_avatar_female_RightHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandMiddle1 = GO_avatar_female_RightHand.gameObject.transform.GetChild(1).gameObject;
        GO_avatar_female_RightHandMiddle2 = GO_avatar_female_RightHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandMiddle3 = GO_avatar_female_RightHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandPinky1 = GO_avatar_female_RightHand.gameObject.transform.GetChild(2).gameObject;
        GO_avatar_female_RightHandPinky2 = GO_avatar_female_RightHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandPinky3 = GO_avatar_female_RightHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandRing1 = GO_avatar_female_RightHand.gameObject.transform.GetChild(3).gameObject;
        GO_avatar_female_RightHandRing2 = GO_avatar_female_RightHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandRing3 = GO_avatar_female_RightHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandThumb1 = GO_avatar_female_RightHand.gameObject.transform.GetChild(4).gameObject;
        GO_avatar_female_RightHandThumb2 = GO_avatar_female_RightHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_avatar_female_RightHandThumb3 = GO_avatar_female_RightHandThumb2.gameObject.transform.GetChild(0).gameObject;
    }

    void GetNPCMaleReferences()
    {
        GO_NPC_male = GO_NPC_Character.transform.GetChild(0).gameObject;
        GO_NPC_male_Hips = GO_NPC_male.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftUpLeg = GO_NPC_male_Hips.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftLeg = GO_NPC_male_LeftUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftFoot = GO_NPC_male_LeftLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftToeBase = GO_NPC_male_LeftFoot.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightUpLeg = GO_NPC_male_Hips.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_male_RightLeg = GO_NPC_male_RightUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightFoot = GO_NPC_male_RightLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightToeBase = GO_NPC_male_RightFoot.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_Spine = GO_NPC_male_Hips.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_male_Spine1 = GO_NPC_male_Spine.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_Spine2 = GO_NPC_male_Spine1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftShoulder = GO_NPC_male_Spine2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftArm = GO_NPC_male_LeftShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftForeArm = GO_NPC_male_LeftArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHand = GO_NPC_male_LeftForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandIndex1 = GO_NPC_male_LeftHand.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandIndex2 = GO_NPC_male_LeftHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandIndex3 = GO_NPC_male_LeftHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandMiddle1 = GO_NPC_male_LeftHand.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_male_LeftHandMiddle2 = GO_NPC_male_LeftHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandMiddle3 = GO_NPC_male_LeftHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandPinky1 = GO_NPC_male_LeftHand.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_male_LeftHandPinky2 = GO_NPC_male_LeftHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandPinky3 = GO_NPC_male_LeftHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandRing1 = GO_NPC_male_LeftHand.gameObject.transform.GetChild(3).gameObject;
        GO_NPC_male_LeftHandRing2 = GO_NPC_male_LeftHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandRing3 = GO_NPC_male_LeftHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandThumb1 = GO_NPC_male_LeftHand.gameObject.transform.GetChild(4).gameObject;
        GO_NPC_male_LeftHandThumb2 = GO_NPC_male_LeftHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_LeftHandThumb3 = GO_NPC_male_LeftHandThumb2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_Neck = GO_NPC_male_Spine2.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_male_Head = GO_NPC_male_Neck.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightShoulder = GO_NPC_male_Spine2.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_male_RightArm = GO_NPC_male_RightShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightForeArm = GO_NPC_male_RightArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHand = GO_NPC_male_RightForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandIndex1 = GO_NPC_male_RightHand.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandIndex2 = GO_NPC_male_RightHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandIndex3 = GO_NPC_male_RightHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandMiddle1 = GO_NPC_male_RightHand.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_male_RightHandMiddle2 = GO_NPC_male_RightHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandMiddle3 = GO_NPC_male_RightHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandPinky1 = GO_NPC_male_RightHand.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_male_RightHandPinky2 = GO_NPC_male_RightHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandPinky3 = GO_NPC_male_RightHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandRing1 = GO_NPC_male_RightHand.gameObject.transform.GetChild(3).gameObject;
        GO_NPC_male_RightHandRing2 = GO_NPC_male_RightHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandRing3 = GO_NPC_male_RightHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandThumb1 = GO_NPC_male_RightHand.gameObject.transform.GetChild(4).gameObject;
        GO_NPC_male_RightHandThumb2 = GO_NPC_male_RightHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_male_RightHandThumb3 = GO_NPC_male_RightHandThumb2.gameObject.transform.GetChild(0).gameObject;
    }

    void GetNPCFemaleReferences()
    {
        GO_NPC_female = GO_NPC_Character.transform.GetChild(1).gameObject;
        GO_NPC_female_Hips = GO_NPC_female.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftUpLeg = GO_NPC_female_Hips.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftLeg = GO_NPC_female_LeftUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftFoot = GO_NPC_female_LeftLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftToeBase = GO_NPC_female_LeftFoot.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightUpLeg = GO_NPC_female_Hips.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_female_RightLeg = GO_NPC_female_RightUpLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightFoot = GO_NPC_female_RightLeg.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightToeBase = GO_NPC_female_RightFoot.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_Spine = GO_NPC_female_Hips.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_female_Spine1 = GO_NPC_female_Spine.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_Spine2 = GO_NPC_female_Spine1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftShoulder = GO_NPC_female_Spine2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftArm = GO_NPC_female_LeftShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftForeArm = GO_NPC_female_LeftArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHand = GO_NPC_female_LeftForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandIndex1 = GO_NPC_female_LeftHand.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandIndex2 = GO_NPC_female_LeftHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandIndex3 = GO_NPC_female_LeftHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandMiddle1 = GO_NPC_female_LeftHand.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_female_LeftHandMiddle2 = GO_NPC_female_LeftHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandMiddle3 = GO_NPC_female_LeftHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandPinky1 = GO_NPC_female_LeftHand.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_female_LeftHandPinky2 = GO_NPC_female_LeftHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandPinky3 = GO_NPC_female_LeftHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandRing1 = GO_NPC_female_LeftHand.gameObject.transform.GetChild(3).gameObject;
        GO_NPC_female_LeftHandRing2 = GO_NPC_female_LeftHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandRing3 = GO_NPC_female_LeftHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandThumb1 = GO_NPC_female_LeftHand.gameObject.transform.GetChild(4).gameObject;
        GO_NPC_female_LeftHandThumb2 = GO_NPC_female_LeftHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_LeftHandThumb3 = GO_NPC_female_LeftHandThumb2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_Neck = GO_NPC_female_Spine2.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_female_Head = GO_NPC_female_Neck.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightShoulder = GO_NPC_female_Spine2.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_female_RightArm = GO_NPC_female_RightShoulder.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightForeArm = GO_NPC_female_RightArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHand = GO_NPC_female_RightForeArm.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandIndex1 = GO_NPC_female_RightHand.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandIndex2 = GO_NPC_female_RightHandIndex1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandIndex3 = GO_NPC_female_RightHandIndex2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandMiddle1 = GO_NPC_female_RightHand.gameObject.transform.GetChild(1).gameObject;
        GO_NPC_female_RightHandMiddle2 = GO_NPC_female_RightHandMiddle1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandMiddle3 = GO_NPC_female_RightHandMiddle2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandPinky1 = GO_NPC_female_RightHand.gameObject.transform.GetChild(2).gameObject;
        GO_NPC_female_RightHandPinky2 = GO_NPC_female_RightHandPinky1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandPinky3 = GO_NPC_female_RightHandPinky2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandRing1 = GO_NPC_female_RightHand.gameObject.transform.GetChild(3).gameObject;
        GO_NPC_female_RightHandRing2 = GO_NPC_female_RightHandRing1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandRing3 = GO_NPC_female_RightHandRing2.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandThumb1 = GO_NPC_female_RightHand.gameObject.transform.GetChild(4).gameObject;
        GO_NPC_female_RightHandThumb2 = GO_NPC_female_RightHandThumb1.gameObject.transform.GetChild(0).gameObject;
        GO_NPC_female_RightHandThumb3 = GO_NPC_female_RightHandThumb2.gameObject.transform.GetChild(0).gameObject;
    }
}
