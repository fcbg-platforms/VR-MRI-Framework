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


using QualisysRealTime.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(_class_all_references_scene_mri_compatible_googles))]
public class _main_experiment_manager : MonoBehaviour
{

    [Header("Advanced server parameters start experiment")]
    [HideInInspector]
    public bool b_advance_server_check = true;

    [Header("Experiment Sequence")]
    private string load_file_path = "Results/in_experiment.txt";
    private string out_path_logs = "Results/out_experiment.txt";

    //string s_tmp_dot_txt_path = "Results/tmp.txt"; // tmp file for checkpoints
    int i_last_checkpoint_index = 0; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint

    int[] array_i_checkpoints_index; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint
    string[] array_s_checkpoints_names; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint
    float[] array_f_checkpoints_time_elapsed; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint
    int[] array_i_checkpoints_command_number; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint

    int i_current_index_array_checkpoint_index_names = 0;
    int i_current_index_array_checkpoint_index_names_selected_checkpoint; // -> to use Rightcontrol and Leftcontrol when selecting the checkpoint

    int[] array_i_command_number_index_line; // checkpoint number, useless for now, but could be usefull to choose the desired checkpoint

    int i_current_line_input_file = 0;
    string[] array_s_input_file;

    [Header("Others")]
    private bool start_experiment;
    private bool experiment_can_start;
    private string configName;
    string configName_FirstArrayElement;
    [HideInInspector]
    public bool nextCommand;
    bool exists_load_file_path;
    // for feedback
    [HideInInspector]
    public string s_last_input_answer;
    [HideInInspector]
    public string s_last_input_command;

    private IEnumerator coroutineQuestion;
    private IEnumerator coroutineTask;
    private IEnumerator coroutineContinuousScale;
    private IEnumerator coroutineset_display_TV;
    private IEnumerator coroutineNPC;
    private IEnumerator coroutineQuestionWithTV;
    private IEnumerator coroutineQuestionAfterTV;

    private string s_current_command_duration;
    //private bool b_error_connection_cameras;
    private bool b_after_EOF_end_of_exp;

    private void Awake()
    {
        // create instances of class_all_references
        bool b_create_class_all_references_scene_mri_compatible_googles = _class_all_references_scene_mri_compatible_googles.Instance.isActiveAndEnabled;


        // create instances of Functionnalities
        bool b_functionnalities = Functionnalities.Instance.isActiveAndEnabled;
    }

    // Use this for initialization
    void Start()
    {
        //check if directory doesn't exit
        string directoryPath = "Results";
        if (!Directory.Exists(directoryPath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(directoryPath);
        }

        //load_file_path = "Results/in_experiment.txt";
        out_path_logs = "Results/" + System.DateTime.Now.ToString("[yyyy-dd-MM] [HH-mm-ss]") + " out_experiment.txt";


        if (b_build_active_navigation_game_alone)
        {
            b_advance_server_check = false;
        }

        if (b_advance_server_check)
        {
            StartCoroutine(coroutine_start_pipe_server(0.5f));
        }

        Screen.fullScreen = true;
        // force focus on this app window
        ///Cursor.visible = false; // force focus on this app window
        ///Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator coroutine_start_pipe_server(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().start_pipe_on_server();
        StartCoroutine(coroutine_start_pipe_client(0.5f)); //
    }

    private IEnumerator coroutine_start_pipe_client(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().start_pipe_on_client();
        StartCoroutine(coroutine_sent_information_pipe_client(0.5f)); //
    }

    private IEnumerator coroutine_sent_information_pipe_client(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[our_server_started_connect_your_client]");
    }


    // check if app loose focus
    void OnApplicationFocus(bool hasFocus)
    {
        if (b_advance_server_check)
        {
            if (experiment_can_start)
            {
                //if (hasFocus != b_is_app_focused)
                {
                    if (hasFocus == false)
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[mri_app_has_not_focus]");
                    }
                    else if (hasFocus == true)
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[mri_app_has_focus]");
                    }
                }
            }
        }
    }

    public void detect_input_button_and_write_it_in_log()
    {
        if (Input.GetButtonDown("Button1"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 1");
        }
        else if (Input.GetButtonDown("Button2"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 2");
        }
        else if (Input.GetButtonDown("Button3"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 3");
        }
        else if (Input.GetButtonDown("Button6"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 4");
        }
        else if (Input.GetButtonDown("Button7"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 5");
        }
        else if (Input.GetButtonDown("Button8"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 6");
        }
        else if (Input.GetButtonDown("Button9"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [INPUT_RECEIVED_BUTTON_PRESSED] - input :" + " 9");
        }
    }


    [Header("FOR DEBUG PURPOSE ONLY")]
    public bool b_debug_start_experiment_alone; //don't forget to set parameter 'b_debug_discard_named_pipes_start_experiment_alone' to 'true' in <_communicate_with_pipes_for_noob> components of client and server named pipes
    public string s_debug_load_file_path = "C:\\Users\\HNP_VR\\Desktop\\MRI_VR_clean_June\\_build\\_MRI_clean_builds\\_Experiment_Input_Files\\test.txt";

    [Header("FOR GAME PURPOSE")]
    public bool b_build_active_navigation_game_alone;
    bool b_is_build_active_navigation_game_alone;
    GameObject GO_camera_build_active_navigation_game_alone;
    GameObject GO_target_build_active_navigation_game_alone;
    GameObject GO_initialPosition_build_active_navigation_game_alone;
    GameObject GO_target_build_active_navigation_game_alone_feedback;
    List<GameObject> list_GO_trajectory_build_active_navigation_game_alone = new List<GameObject>();


    ////public bool b_debug_save_screenshots;

    // Update is called once per frame
    void Update()
    {
        ////if (b_debug_save_screenshots)
        ////{
        ////    b_debug_save_screenshots = false;
        ////    GameObject.Find("_Active_navigation_game_related/_Screenshots_second_monitor_camera").GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/a");
        ////    StartCoroutine(wait_and_save_taken_screenshots());
        ////}


        /*///////*/
        if (b_build_active_navigation_game_alone)
        {
            if (Application.isEditor)
            {
                GameObject.Find("_Active_navigation_game_related/_Second_monitor_camera").SetActive(false);
            }
            else if (Display.displays.Length > 1)
            {
                Display.displays[1].Activate();

                GameObject.Find("_Active_navigation_game_related/_Canvas_second_monitor").GetComponent<Canvas>().worldCamera = GameObject.Find("_Active_navigation_game_related/_Second_monitor_camera").GetComponent<Camera>();
                GameObject.Find("_Active_navigation_game_related/_Screenshots_second_monitor_camera").SetActive(false);
            }
            Destroy(GO_camera_build_active_navigation_game_alone);
            GO_camera_build_active_navigation_game_alone = GameObject.Instantiate(Resources.Load("GO_camera_build_active_navigation_game_alone") as GameObject);
            GO_target_build_active_navigation_game_alone_feedback = GameObject.Instantiate(Resources.Load("GO_active_navigation_target") as GameObject);
            //GO_target_build_active_navigation_game_alone_feedback.transform.parent = _class_all_references_June_experiment.Instance.GO_June_experiment_anchor.transform;


            b_build_active_navigation_game_alone = false;
            load_file_path = Application.streamingAssetsPath + "\\input_active_navigation_game.txt";
            start_experiment = true;
            Debug.Log(load_file_path);
            b_is_build_active_navigation_game_alone = true;
        }
        else if (b_debug_start_experiment_alone)
        {
            b_debug_start_experiment_alone = false;
            load_file_path = s_debug_load_file_path;
            start_experiment = true;
            Debug.Log("start");
        }

        if (b_is_build_active_navigation_game_alone)
        {
            GO_camera_build_active_navigation_game_alone.transform.position = (_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.position) / 2;
            GO_camera_build_active_navigation_game_alone.transform.position = new Vector3(GO_camera_build_active_navigation_game_alone.transform.position.x, 450, GO_camera_build_active_navigation_game_alone.transform.position.z);
            GO_camera_build_active_navigation_game_alone.transform.rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation;
        }

        if (b_advance_server_check)
        {
            update_received_information_server_pipe();
        }

        update_can_quit_app();
        update_can_move_inside_outside_freely_rb();
        
        if (start_experiment)
        {
            exists_load_file_path = testFileExistance(load_file_path);

            if (exists_load_file_path)
            {
                OpenFile();
                experiment_can_start = true;
                nextCommand = true;
                Debug.Log("Start");


                //_class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[error_connection_to_cameras]");
                //_class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + "before" + " " + 1000);
            }

            start_experiment = false;

            if (b_advance_server_check)
            {
                if (Application.isFocused)
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[mri_app_has_focus]");
                }
            }
        }

        if (experiment_can_start)
        {
            ReadFile();

            if (configName_FirstArrayElement == "question_notime")
            {
                var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                if (question.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    question.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "question_bypasstime")
            {
                var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                if (question.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    StopCoroutine(coroutineQuestion);
                    question.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "question_with_TV_notime")
            {
                var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                if (question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    TVManager.stopDisplayOnTVScreenWithQuestions();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "question_with_TV_bypasstime")
            {
                var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                if (question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    StopCoroutine(coroutineQuestionWithTV);
                    question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    TVManager.stopDisplayOnTVScreenWithQuestions();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "question_after_TV_notime")
            {
                var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                if (question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "question_after_TV_bypasstime")
            {
                var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                if (question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext == true)
                {
                    StopCoroutine(coroutineQuestionAfterTV);
                    question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "text_body_part_notime")
            {
                var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                if (experiment.expyVRTrialRunningEnableInput == false)
                {
                    experiment.ExpyVRStopDisplayTrial();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "text_body_part_bypasstime")
            {
                var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                if (experiment.expyVRTrialRunningEnableInput == false)
                {
                    StopCoroutine(coroutineTask);
                    experiment.ExpyVRStopDisplayTrial();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "continuousscale_notime")
            {
                var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                if (experiment.expyVRTrialRunningEnableInput == false)
                {
                    experiment.ExpyVRStopDisplayTrialContinuousScale();
                    experiment.ExpyVRStopDisplayContinuousScale();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "continuousscale_bypasstime")
            {
                var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                if (experiment.expyVRTrialRunningEnableInput == false)
                {
                    StopCoroutine(coroutineContinuousScale);
                    experiment.ExpyVRStopDisplayTrialContinuousScale();
                    experiment.ExpyVRStopDisplayContinuousScale();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "set_display_TV_notime")
            {
                var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                if (TVManager.TrialRunningEnableInput == false)
                {
                    TVManager.stopDisplayOnTVScreen();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "set_display_TV_bypasstime")
            {
                var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                if (TVManager.TrialRunningEnableInput == false)
                {
                    StopCoroutine(coroutineset_display_TV);
                    TVManager.stopDisplayOnTVScreen();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "NPC_OBT_notime")
            {
                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();
                if (NPCManager.TrialRunningEnableInput == false)
                {
                    NPCManager.ExpyVRStopDisplayTrial();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "NPC_OBT_bypasstime")
            {
                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();
                if (NPCManager.TrialRunningEnableInput == false)
                {
                    StopCoroutine(coroutineNPC);
                    NPCManager.ExpyVRStopDisplayTrial();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "pause")
            {
                if (Input.GetButtonDown("Button9"))
                {
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "forced_pause")
            {
                if (Input.GetButtonDown("Button9"))
                {
                    nextCommand = true;

                    ///Cursor.visible = false; // force focus on this app window
                    ///Cursor.lockState = CursorLockMode.Locked;
                    b_force_pause = false;
                    b_display_menu = false;
                }
            }
            else if (configName_FirstArrayElement == "pause_forced_pb")
            {
                if (Input.GetButtonDown("Button9"))
                {
                    nextCommand = true;
                    b_pause_forced_pb = false;
                    Debug.Log("resume");
                }
                /*if (Input.GetButtonDown("ResetLastCheckpoint"))
                {
                    i_current_line_input_file = i_last_checkpoint_index;

                    Debug.Log("load checkpoint : " + array_s_input_file[i_current_line_input_file]);
                }*/
                if (Input.GetButtonDown("ControlLeft"))
                {
                    if (i_current_index_array_checkpoint_index_names_selected_checkpoint < array_s_checkpoints_names.Length - 1)
                        i_current_index_array_checkpoint_index_names_selected_checkpoint++;

                    Debug.Log("selected checkpoint : " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint]);

                    _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[action]" + " SelectCheckpointChanged " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint] + " " + i_current_index_array_checkpoint_index_names_selected_checkpoint + " " + array_i_checkpoints_index[i_current_index_array_checkpoint_index_names_selected_checkpoint]);

                }
                if (Input.GetButtonDown("ControlRight"))
                {
                    if (i_current_index_array_checkpoint_index_names_selected_checkpoint > 0)
                        i_current_index_array_checkpoint_index_names_selected_checkpoint--;

                    Debug.Log("selected checkpoint : " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint]);

                    _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[action]" + " SelectCheckpointChanged " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint] + " " + i_current_index_array_checkpoint_index_names_selected_checkpoint + " " + array_i_checkpoints_index[i_current_index_array_checkpoint_index_names_selected_checkpoint]);
                }

                if (Input.GetButtonDown("ResetSelectedCheckpoint"))
                {
                    i_current_line_input_file = array_i_checkpoints_index[i_current_index_array_checkpoint_index_names_selected_checkpoint];

                    _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[action]" + " ResetSelectedCheckpoint " + array_i_checkpoints_command_number[i_current_index_array_checkpoint_index_names_selected_checkpoint] + " " + (array_f_checkpoints_time_elapsed[i_current_index_array_checkpoint_index_names_selected_checkpoint] / 1000.0f).ToString() + " " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint]);

                    Debug.Log("load checkpoint : " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint]);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    writeInFilePSpace(timestamp + " - [" + "pause_forced_pb" + "]" + " Back to checkpoint : " + array_s_checkpoints_names[i_current_index_array_checkpoint_index_names_selected_checkpoint] + " - id : i_current_index_array_checkpoint_index_names_selected_checkpoint " + " - line : " + array_i_checkpoints_index[i_current_index_array_checkpoint_index_names_selected_checkpoint]);
                }
            }
            else if (configName_FirstArrayElement == "free_move_mri_table")
            {
                if (Input.GetButtonDown("Button0") || Input.GetButtonDown("Button9"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    b_free_move_mri_table = false;

                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "mri_table_rb_tracked")
            {
                if (Input.GetButtonDown("Button0") || Input.GetButtonDown("Button9"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;

                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "move_inside_rb_notime")
            {
                if (Input.GetButtonDown("Button0") || Input.GetButtonDown("Button9"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;

                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "move_outside_rb_notime")
            {
                if (Input.GetButtonDown("Button0") || Input.GetButtonDown("Button9"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;

                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "pause_wait_for_MRI_pulse")
            {
                if (Input.GetAxisRaw("Command_Pulse_IRM") == 1)
                {
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "set_display_TV_video")
            {
                if (_class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>().b_video_ended == true)
                {
                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                    TVManager.stopDisplayOnTVScreenWithQuestions();
                    nextCommand = true;
                }
            }
            else if (configName_FirstArrayElement == "active_navigation")
            {
                /*if (Input.GetButton("ButtonLeft"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.RotateAround((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, -f_active_navigation_rotation_speed * Time.deltaTime);
                }
                if (Input.GetButton("ButtonRight"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.RotateAround((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, f_active_navigation_rotation_speed * Time.deltaTime);
                }*/
                if (!b_disable_active_navigation_input)
                {
                    if (Input.GetButton("ButtonForward"))
                    {
                        ///
                        float f_limit_constraint_movements = 4.5f;
                        if (f_limit_constraint_movements > 0)
                        {
                            Vector3 v3_old_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition;
                            Vector3 v3_new_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition + _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime; ;
                            Vector3 v3_motion_vector = v3_new_pos - v3_old_pos;

                            if ((v3_new_pos.x > f_limit_constraint_movements) && (v3_motion_vector.x > 0))
                            {

                            }
                            else if ((v3_new_pos.x < -f_limit_constraint_movements) && (v3_motion_vector.x < 0))
                            {

                            }
                            else if ((v3_new_pos.z > f_limit_constraint_movements) && (v3_motion_vector.z > 0))
                            {

                            }
                            else if ((v3_new_pos.z < -f_limit_constraint_movements) && (v3_motion_vector.z < 0))
                            {

                            }
                            else
                            {
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition += _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                            }
                        }
                        else
                        ///
                        {
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition += _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                        }
                    }
                    if (Input.GetButton("ButtonBackward"))
                    {
                        ///
                        float f_limit_constraint_movements = 4.5f;
                        if (f_limit_constraint_movements > 0)
                        {
                            Vector3 v3_old_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition;
                            Vector3 v3_new_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition - _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime; ;
                            Vector3 v3_motion_vector = v3_new_pos - v3_old_pos;

                            if ((v3_new_pos.x > f_limit_constraint_movements) && (v3_motion_vector.x > 0))
                            {

                            }
                            else if ((v3_new_pos.x < -f_limit_constraint_movements) && (v3_motion_vector.x < 0))
                            {

                            }
                            else if ((v3_new_pos.z > f_limit_constraint_movements) && (v3_motion_vector.z > 0))
                            {

                            }
                            else if ((v3_new_pos.z < -f_limit_constraint_movements) && (v3_motion_vector.z < 0))
                            {

                            }
                            else
                            {
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition -= _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                            }
                        }
                        else
                        ///
                        {
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition -= _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                        }
                    }

                    if (Input.GetButtonDown("ButtonStopActiveNavigation"))
                    {
                        b_disable_active_navigation_input = true;

                        if (b_is_build_active_navigation_game_alone)
                        {
                            string s_distance_target = (Vector3.Distance(GO_camera_build_active_navigation_game_alone.transform.position, GO_target_build_active_navigation_game_alone.transform.position) * 100).ToString("F3");
                            float f_signed_angle_target_camera_target_initialpos = Vector3.SignedAngle(GO_target_build_active_navigation_game_alone.transform.position - GO_camera_build_active_navigation_game_alone.transform.position, GO_target_build_active_navigation_game_alone.transform.position - GO_initialPosition_build_active_navigation_game_alone.transform.position, Vector3.up);
                            if ((f_signed_angle_target_camera_target_initialpos < 90) && (f_signed_angle_target_camera_target_initialpos > -90))
                            {
                                s_distance_target = "-" + s_distance_target;
                            }

                            GameObject.Find("_Active_navigation_game_related/_Canvas_second_monitor/_Text_distance_from_target").GetComponent<Text>().text = "Distance from target : " + s_distance_target + "cm";
                            GameObject.Find("_Active_navigation_game_related/_Canvas_second_monitor/_Text_distance_from_target_save").GetComponent<Text>().text += "\nOlder distance from target : " + s_distance_target + "cm";
                            GameObject.Find("_Active_navigation_game_related/_Canvas_first_monitor/_Text_distance_from_target").GetComponent<Text>().text = "Last distance from target : " + s_distance_target + "cm";
                            GameObject.Find("_Active_navigation_game_related/MainCamera/Camera_Left/Camera_Left_Text").GetComponent<TextMesh>().text = "Last distance from target : " + s_distance_target + "cm";
                            GameObject.Find("_Active_navigation_game_related/MainCamera/Camera_Right/Camera_Right_Text").GetComponent<TextMesh>().text = "Last distance from target : " + s_distance_target + "cm";



                            GO_target_build_active_navigation_game_alone_feedback = GameObject.Instantiate(Resources.Load("GO_active_navigation_target") as GameObject);
                            //GO_target_build_active_navigation_game_alone_feedback.transform.parent = _class_all_references_June_experiment.Instance.GO_June_experiment_anchor.transform;
                            GO_target_build_active_navigation_game_alone_feedback.transform.localPosition = new Vector3(GO_target_build_active_navigation_game_alone.transform.localPosition.x, 0, GO_target_build_active_navigation_game_alone.transform.localPosition.z);

                            StartCoroutine(wait_and_nextcommand_and_destroy_GO_target_build_active_navigation_game_alone_feedback(3));

                            if (Display.displays[1].active)
                            {
                                GameObject.Find("_Active_navigation_game_related/_Second_monitor_camera").GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_second_monitor_camera");
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_first_monitor_right_camera");
                            }
                            else
                            {
                                GameObject.Find("_Active_navigation_game_related/_Screenshots_second_monitor_camera").GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_second_monitor_camera");
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_first_monitor_right_camera");
                            }

                            StartCoroutine(wait_and_save_taken_screenshots());
                        }
                        else
                        {
                            nextCommand = true;
                        }

                        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        writeInFilePSpace(timestamp + " - [" + "active_navigation_stopped" + "]" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

                        //nextCommand = true;
                    }
                }
            }
            else if (configName_FirstArrayElement == "active_navigation_with_target")
            {
                if (Input.GetButton("ButtonLeft"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.RotateAround((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, -f_active_navigation_rotation_speed * Time.deltaTime);
                }
                if (Input.GetButton("ButtonRight"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.RotateAround((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2, _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, f_active_navigation_rotation_speed * Time.deltaTime);
                }
                if (Input.GetButton("ButtonForward"))
                {
                    ///
                    float f_limit_constraint_movements = 4.5f;
                    if (f_limit_constraint_movements > 0)
                    {
                        Vector3 v3_old_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition;
                        Vector3 v3_new_pos = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition + _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime; ;
                        Vector3 v3_motion_vector = v3_new_pos - v3_old_pos;

                        if ((v3_new_pos.x > f_limit_constraint_movements) && (v3_motion_vector.x > 0))
                        {

                        }
                        else if ((v3_new_pos.x < -f_limit_constraint_movements) && (v3_motion_vector.x < 0))
                        {

                        }
                        else if ((v3_new_pos.z > f_limit_constraint_movements) && (v3_motion_vector.z > 0))
                        {

                        }
                        else if ((v3_new_pos.z < -f_limit_constraint_movements) && (v3_motion_vector.z < 0))
                        {

                        }
                        else
                        {
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition += _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                        }
                    }
                    else
                    ///
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition += _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                    }
                }
                /*if (Input.GetButton("ButtonBackward"))
                {
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition -= _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.forward * f_active_navigation_translation_speed * Time.deltaTime;
                }*/

                Vector2 v2_center_eye_position = new Vector2((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position.x + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position.x) / 2, (_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position.z + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position.z) / 2);
                Vector2 v2_active_navigation_target_position = new Vector2(GO_active_navigation_target.transform.position.x, GO_active_navigation_target.transform.position.z);

                if (Vector2.Distance(v2_center_eye_position, v2_active_navigation_target_position) < 0.15f)
                {
                    Destroy(GO_active_navigation_target);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    writeInFilePSpace(timestamp + " - [" + "active_navigation_with_target_stopped" + "]" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

                    nextCommand = true;

                    if (b_is_build_active_navigation_game_alone)
                    {
                        if (Display.displays[1].active)
                        {
                            GameObject.Find("_Active_navigation_game_related/_Second_monitor_camera").GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_second_monitor_camera");
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_first_monitor_right_camera");
                        }
                        else
                        {
                            GameObject.Find("_Active_navigation_game_related/_Screenshots_second_monitor_camera").GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_second_monitor_camera");
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().take_screenshot("Results/" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss_fff ", CultureInfo.InvariantCulture) + "_active_navigation_end_first_monitor_right_camera");
                        }

                        StartCoroutine(wait_and_save_taken_screenshots());
                    }
                }

                if (b_is_build_active_navigation_game_alone)
                {
                    GameObject.Find("_Active_navigation_game_related/_Canvas_second_monitor/_Text_distance_from_target").GetComponent<Text>().text = "Distance from target : " + (Vector3.Distance(GO_camera_build_active_navigation_game_alone.transform.position, GO_target_build_active_navigation_game_alone.transform.position) * 100).ToString("F3") + "cm";
                }
            }
        }


        // fade in/out available before/during/after the experiment
        if (Input.GetButtonDown("ButtonFadeIn"))
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().FadeOut(3000);
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_fade_camera>().FadeOut(3000);
        }
        if (Input.GetButtonDown("ButtonFadeOut"))
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().FadeIn(4000);
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_fade_camera>().FadeIn(4000);
        }

        // If reception of MRI pulse
        if (Input.GetAxisRaw("Command_Pulse_IRM") == 1)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [" + "MRI Pulse Received" + "]");
        }

        // detectPressedKeyOrButton();
        // see if a key was pressed and write it to log file
        detect_input_button_and_write_it_in_log();
    }

    bool b_disable_active_navigation_input;

    IEnumerator wait_and_nextcommand_and_destroy_GO_target_build_active_navigation_game_alone_feedback(float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        nextCommand = true;
        b_disable_active_navigation_input = false;
        yield return new WaitForSeconds(1);
        Destroy(GO_target_build_active_navigation_game_alone_feedback);
        GameObject.Find("_Active_navigation_game_related/MainCamera/Camera_Left/Camera_Left_Text").GetComponent<TextMesh>().text = "";
        GameObject.Find("_Active_navigation_game_related/MainCamera/Camera_Right/Camera_Right_Text").GetComponent<TextMesh>().text = "";

    }

    IEnumerator write_active_navigation_recording()
    {
        if (b_is_build_active_navigation_game_alone)
        {
            GameObject GO_curr_trajectory = GameObject.Instantiate(Resources.Load("GO_trajectory") as GameObject);
            GO_curr_trajectory.transform.position = GO_camera_build_active_navigation_game_alone.transform.position;
            GO_curr_trajectory.transform.rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation;
            list_GO_trajectory_build_active_navigation_game_alone.Add(GO_curr_trajectory);
        }

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writeInFilePSpace(timestamp + " - [" + "active_navigation_recording" + "]" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

        yield return new WaitForSeconds(0.25f);

        if (configName_FirstArrayElement == "active_navigation")
        {
            StartCoroutine(write_active_navigation_recording());
        }
    }
    IEnumerator write_active_navigation_with_target_recording()
    {
        if (b_is_build_active_navigation_game_alone)
        {
            GameObject GO_curr_trajectory = GameObject.Instantiate(Resources.Load("GO_trajectory") as GameObject);
            GO_curr_trajectory.transform.position = GO_camera_build_active_navigation_game_alone.transform.position;
            GO_curr_trajectory.transform.rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation;
            list_GO_trajectory_build_active_navigation_game_alone.Add(GO_curr_trajectory);
        }

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writeInFilePSpace(timestamp + " - [" + "active_navigation_with_target_recording" + "]" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

        yield return new WaitForSeconds(0.10f);

        if (configName_FirstArrayElement == "active_navigation_with_target")
        {
            StartCoroutine(write_active_navigation_with_target_recording());
        }
    }

    IEnumerator wait_and_save_taken_screenshots()
    {
        yield return new WaitForSeconds(1);

        if (Display.displays[1].active)
        {
            GameObject.Find("_Active_navigation_game_related/_Second_monitor_camera").GetComponent<_camera_screenshot_threaded>().save_taken_screenshots();
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().save_taken_screenshots();
        }
        else
        {
            GameObject.Find("_Active_navigation_game_related/_Screenshots_second_monitor_camera").GetComponent<_camera_screenshot_threaded>().save_taken_screenshots();
            _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_camera_screenshot_threaded>().save_taken_screenshots();
        }
    }






    float f_active_navigation_translation_speed = 0.9f;
    float f_active_navigation_rotation_speed = 25;
    GameObject GO_active_navigation_target;



    public void OpenFile()
    {
        if (File.Exists(load_file_path))
        {
            array_s_input_file = File.ReadAllLines(load_file_path); // read input files and put it in an array (one read line per array element)

            int i_nb_checkpoints = 0;
            foreach (string s in array_s_input_file)
            {
                if (s.StartsWith("checkpoint"))
                {
                    i_nb_checkpoints++;
                }
            }
            Debug.Log("nb checkpoints : " + i_nb_checkpoints);
            array_i_checkpoints_index = new int[i_nb_checkpoints];
            array_s_checkpoints_names = new string[i_nb_checkpoints];
            array_f_checkpoints_time_elapsed = new float[i_nb_checkpoints];
            array_i_checkpoints_command_number = new int[i_nb_checkpoints];
            int i_init_current_checkpoint_index = 0;

            float f_time_elapsed = 0;
            int i_command_number = 0;
            float f_current_command_time;

            for (int i = 0; i < array_s_input_file.Length; i++)
            {
                if (i > 0)
                {
                    if (array_s_input_file[i] == "")
                    {
                        if (float.TryParse(array_s_input_file[i - 1], out f_current_command_time))
                        {
                            f_time_elapsed += f_current_command_time;
                        }
                        i_command_number++;
                    }
                }

                if (array_s_input_file[i].StartsWith("checkpoint"))
                {
                    array_i_checkpoints_index[i_init_current_checkpoint_index] = i;
                    array_s_checkpoints_names[i_init_current_checkpoint_index] = array_s_input_file[i + 1];
                    array_f_checkpoints_time_elapsed[i_init_current_checkpoint_index] = f_time_elapsed;
                    array_i_checkpoints_command_number[i_init_current_checkpoint_index] = i_command_number; // this command has actually not been counted -> reference the previous command number, which is good as during restart we re-play the "command" checkpoint

                    //Debug.Log(array_s_checkpoints_names[i_init_current_checkpoint_index] + " - " + array_f_checkpoints_time_elapsed[i_init_current_checkpoint_index] + " - " + array_i_checkpoints_command_number[i_init_current_checkpoint_index]);

                    i_init_current_checkpoint_index++;
                }
            }

            // FileChecker command number patch
            array_i_command_number_index_line = new int[array_s_input_file.Length];
            i_command_number = 1;
            for (int i = 0; i < array_s_input_file.Length; i++)
            {
                if (i == 0)
                {
                    array_i_command_number_index_line[i] = 1;
                }
                else if (i > 0)
                {
                    array_i_command_number_index_line[i] = array_i_command_number_index_line[i - 1];
                    if (array_s_input_file[i - 1] == "" || array_s_input_file[i - 1].StartsWith("// "))
                    {
                        i_command_number++;
                        array_i_command_number_index_line[i] = i_command_number;
                    }
                }
            }
            
            nextCommand = true;
            Debug.Log("Start");
        }
        else
        {
            Debug.LogError("Experiment File does not exist - " + load_file_path);
        }
    }

    public void ReadFile()
    {
        if (nextCommand)
        {
            //if (!reader.EndOfStream)
            if (i_current_line_input_file < array_s_input_file.Length)
            {
                //Pause
                if (b_force_pause)
                {
                    nextCommand = false;
                    s_current_command_duration = "-1";

                    configName = "forced_pause";
                    configName_FirstArrayElement = "forced_pause";

                    char[] separators = new char[] { ' ' };
                    string[] result = configName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    writeInFilePSpace(timestamp + " - [" + configName + "]");

                    if (b_advance_server_check)
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + result[0] + " " + s_current_command_duration);
                    }
                }
                //Forced Pause
                else if (b_pause_forced_pb)
                {
                    nextCommand = false;
                    s_current_command_duration = "-1";

                    configName = "pause_forced_pb";
                    configName_FirstArrayElement = "pause_forced_pb";
                    Debug.Log(configName);
                    char[] separators = new char[] { ' ' };
                    string[] result = configName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    writeInFilePSpace(timestamp + " - [" + configName + "]");

                    if (b_advance_server_check)
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[action]" + " " + result[0] + " " + s_current_command_duration);
                    }
                }
                // probably to be deleted
                else if (b_free_move_mri_table)
                {
                    nextCommand = false;
                    s_current_command_duration = "-1";

                    configName = "free_move_mri_table";
                    configName_FirstArrayElement = "free_move_mri_table";

                    char[] separators = new char[] { ' ' };
                    string[] result = configName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    writeInFilePSpace(timestamp + " - [" + configName + "]");

                    if (b_advance_server_check)
                    {
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + result[0] + " " + s_current_command_duration);
                    }
                }
                //Read command
                else
                {

                    string text_to_display_in_command_app = ""; // text to be displayed as information to experimenter in the XPieVRControlApp

                    nextCommand = false;
                    s_current_command_duration = "-1";

                    configName = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;

                    char[] separators = new char[] { ' ' };
                    string[] result = configName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    configName_FirstArrayElement = result[0];

                    //Display commentary in control application
                    if (result.Length == 2)
                    {
                        text_to_display_in_command_app += result[1];
                    }

                    switch (result[0])
                    {
                      
                        case "checkpoint":
                            {
                                i_last_checkpoint_index = i_current_line_input_file - 1;
                                //i_last_checkpoint_input_file_line = i_current_line_input_file;
                                Debug.Log("passed checkpoint : " + array_s_input_file[i_last_checkpoint_index]);

                                // following line -> parameter1 -> checkpoint name
                                {
                                    //string next_read_line = array_s_input_file[i_current_line_input_file];
                                    i_current_line_input_file++;

                                    i_current_index_array_checkpoint_index_names = System.Array.IndexOf(array_i_checkpoints_index, i_last_checkpoint_index);
                                    i_current_index_array_checkpoint_index_names_selected_checkpoint = i_current_index_array_checkpoint_index_names;
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }

                                nextCommand = true;
                            }
                            break;

                        case "//": // user comment in script
                            {
                                nextCommand = true;
                                text_to_display_in_command_app = "";
                            }
                            break;

                        case "move_inside": // move_inside command without tracking rigidbody
                            {

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] -> f_speed move
                                    // result_next_read_line[1] -> f_speed rotate

                                    if ((float.Parse(result_next_read_line[0]) > 0.0f) && (float.Parse(result_next_read_line[1]) > 0.0f))
                                    {
                                        // execute the move_inside command
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().speed = float.Parse(result_next_read_line[0]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle>().speed = float.Parse(result_next_read_line[1]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle>().speed = float.Parse(result_next_read_line[1]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B>().speed = float.Parse(result_next_read_line[0]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().MoveAtoB();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle>().RotateAtoB();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle>().RotateAtoB();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B>().MoveAtoB();
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]" + result_next_read_line[0] + " " + result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "move_outside": // move_outside command (auto) without tracking rigidbody
                            {

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] -> f_speed move
                                    // result_next_read_line[1] -> f_speed rotate

                                    if ((float.Parse(result_next_read_line[0]) > 0.0f) && (float.Parse(result_next_read_line[1]) > 0.0f))
                                    {
                                        // execute the move_inside command
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().speed = float.Parse(result_next_read_line[0]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle>().speed = float.Parse(result_next_read_line[1]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle>().speed = float.Parse(result_next_read_line[1]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B>().speed = float.Parse(result_next_read_line[0]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().MoveBtoA();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle>().RotateBtoA();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle>().RotateBtoA();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B>().MoveBtoA();
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]" + result_next_read_line[0] + " " + result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "move_inside_rb": // move_inside command with tracking rigidbody
                            {
                                // execute the move_inside command
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().this_start_rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.rotation;
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().this_end_rotation = new Quaternion(-0.08722516f,0,0, 0.9961886f);


                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveAtoB();

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");

                                    StartCoroutine(WaitAndNextCommandStopMoveInsideRBOutsideRB(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "move_outside_rb": // move_outside command with tracking rigidbody
                            {
                                // execute the move_outside command
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().this_start_rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.rotation;
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().this_end_rotation = new Quaternion(-0.08722516f, 0, 0, 0.9961886f);
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().this_start_rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.rotation;
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().this_end_rotation = new Quaternion(-0.08722516f, 0, 0, 0.9961886f);

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");

                                    StartCoroutine(WaitAndNextCommandStopMoveInsideRBOutsideRB(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;
                            
                        case "move_inside_rb_notime": // move_inside command (auto) with tracking rigidbody and no time
                            {
                                // execute the move_inside command
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().this_start_rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.rotation;
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().this_end_rotation = new Quaternion(-0.08722516f, 0, 0, 0.9961886f);
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().this_start_rotation = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.transform.rotation;
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().this_end_rotation = new Quaternion(-0.08722516f, 0, 0, 0.9961886f);

                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateAtoB();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveAtoB();

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "move_outside_rb_notime": // move_outside command (auto) with tracking rigidbody and no time
                            {
                                // execute the move_outside command
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveBtoA();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateBtoA();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateBtoA();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveBtoA();

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "init": // init command
                            {
                                // get references
                                var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                                question.setAnswerFilePath(out_path_logs); // set question log file path
                                var question_withTV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                                question_withTV.setAnswerFilePath(out_path_logs); // set question_with_TV log file path
                                var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                                question_after_TV.setAnswerFilePath(out_path_logs); // set question_after_TV log file path
                                var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                experiment.setAnswerFilePath(out_path_logs); // set experiment log file path
                                var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                TVManager.setAnswerFilePath(out_path_logs); // set TV task log file path
                                //var SolveDissolveOverTimeManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<_solve_dissolve_over_time_manager>();
                                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();
                                NPCManager.setAnswerFilePath(out_path_logs); // set NPC_OBT log file path
                                var QualisysConnection = _class_all_references_scene_mri_compatible_googles.Instance.GO_qualisys_connection.GetComponent<_connect_to_qualisys_DHCP_server>();
                                QualisysConnection.ConnectToQDHCPServer(); // connect to Qualisys DHCP server
                                var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_set_transform_tennis_balls>();
                                var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>();
                                var AvatarConfigCharacterSkinColor = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_skin_color>();
                                var AvatarConfigTorsoScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_torso_scale>();
                                var _point_on_sphere_hand_trackingTracking_RH = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_RH.GetComponent<_point_on_sphere_hand_tracking>();
                                var _point_on_sphere_hand_trackingTracking_LH = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_LH.GetComponent<_point_on_sphere_hand_tracking>();
                                
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] = sex
                                    if (result_next_read_line[0] == "Male")
                                    {
                                        AvatarConfigSex.SetMaleSex();
                                        AvatarConfigTennisBall.SetIsMaleTrue();
                                        experiment.SetMaleSex();
                                        _point_on_sphere_hand_trackingTracking_RH.set_avatar_male();
                                        _point_on_sphere_hand_trackingTracking_LH.set_avatar_male();
                                    }
                                    else if (result_next_read_line[0] == "Female")
                                    {
                                        AvatarConfigSex.SetFemaleSex();
                                        AvatarConfigTennisBall.SetIsMaleFalse();
                                        experiment.SetFemaleSex();
                                        _point_on_sphere_hand_trackingTracking_RH.set_avatar_female();
                                        _point_on_sphere_hand_trackingTracking_LH.set_avatar_female();
                                    }
                                    {
                                        AvatarConfigCharacterSkinColor.ButtonGenericColorHex(result_next_read_line[1]);
                                    }
                                    // result_next_read_line[2] = TorsoScaleX
                                    AvatarConfigTorsoScale.UpdateTorsoScaleXFloat(float.Parse(result_next_read_line[2]));
                                    // result_next_read_line[3] = TorsoScaleZ
                                    AvatarConfigTorsoScale.UpdateTorsoScaleZFloat(float.Parse(result_next_read_line[3]));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "configure_avatar_tracking": // init command
                            {
                                // get references
                                /*var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<SetTransformsTennisBalls>();
                                var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<Sex>();
                                var CanvasConfigCanvasManager = GameObject.Find("CanvasConfig").GetComponent<CanvasManager>();
                                var AvatarConfigCharacterSkinColor = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<CharacterSkinColor>();*/
                                var CanvasConfigTorsoScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_torso_scale>();

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] = TorsoScaleX
                                    CanvasConfigTorsoScale.UpdateTorsoScaleXFloat(float.Parse(result_next_read_line[0]) / 0.4f /* scale avatar on X axis (meters)*/);
                                    // result_next_read_line[1] = TorsoScaleZ
                                    CanvasConfigTorsoScale.UpdateTorsoScaleZFloat(float.Parse(result_next_read_line[1]) / 0.22f /* scale avatar on Z axis (meters)*/);
                                    // result_next_read_line[2] = Distance Origine du repère de tracking - milieu poignets-mains
                                    // TO TEST //
                                    if (float.Parse(result_next_read_line[2]) != 0.0f)
                                    {
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.position = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.position.x, float.Parse(result_next_read_line[2]), _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.position.z);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.gameObject.GetComponent<_move_object_from_A_to_B>().pos1.z = -1.234f + float.Parse(result_next_read_line[2]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.gameObject.GetComponent<_move_object_from_A_to_B>().pos2.z = -0.1f + float.Parse(result_next_read_line[2]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.gameObject.GetComponent<_move_object_from_A_to_B_rb>().this_start_position.z = -1.234f + float.Parse(result_next_read_line[2]);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.transform.gameObject.GetComponent<_move_object_from_A_to_B_rb>().this_end_position.z = -0.1f + float.Parse(result_next_read_line[2]);
                                    }
                                    // TO TEST //

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "set_MRI_length_rb":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().distance_between_start_position_and_end_position = new Vector3(0, 0, -float.Parse(result_next_read_line[0]));
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().distance_between_start_position_and_end_position = new Vector3(0, 0, -float.Parse(result_next_read_line[0]));
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().distance_between_start_position_and_end_position = new Vector3(0, 0, -float.Parse(result_next_read_line[0]));

                                    // TO TEST //
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().distance_between_start_position_and_end_position = new Vector3(0, 0, -float.Parse(result_next_read_line[0]));
                                    //_class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_rotate_object_to_angle_rb>().distance_between_start_position_and_end_position = new Vector3(0, 0, -float.Parse(result_next_read_line[0]));
                                    // TO TEST //

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                                    question.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question.expyVRcurrentQuestionAnswered = false;
                                    question.IsAnswerFalse = false;
                                    question.IsAnswerTrue = false;
                                    question.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]));
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearQuestionAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_notime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                                    question.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question.expyVRcurrentQuestionAnswered = false;
                                    question.IsAnswerFalse = false;
                                    question.IsAnswerTrue = false;
                                    question.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]));
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    /*if(int.Parse(result_next_read_line[0]) > 0) {
                                        StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                    }*/
                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_bypasstime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
                                    question.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question.expyVRcurrentQuestionAnswered = false;
                                    question.IsAnswerFalse = false;
                                    question.IsAnswerTrue = false;
                                    question.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]));
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineQuestion = WaitClearQuestionAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_with_TV":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                                    question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_with_TV.expyVRcurrentQuestionAnswered = false;
                                    question_with_TV.IsAnswerFalse = false;
                                    question_with_TV.IsAnswerTrue = false;
                                    question_with_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureWithQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearQuestionWithTVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_with_TV_notime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                                    question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_with_TV.expyVRcurrentQuestionAnswered = false;
                                    question_with_TV.IsAnswerFalse = false;
                                    question_with_TV.IsAnswerTrue = false;
                                    question_with_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureWithQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    /*if(int.Parse(result_next_read_line[0]) > 0) {
                                        StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                    }*/
                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_with_TV_bypasstime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
                                    question_with_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_with_TV.expyVRcurrentQuestionAnswered = false;
                                    question_with_TV.IsAnswerFalse = false;
                                    question_with_TV.IsAnswerTrue = false;
                                    question_with_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureWithQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineQuestionWithTV = WaitClearQuestionWithTVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_after_TV":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                                    question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_after_TV.expyVRcurrentQuestionAnswered = false;
                                    question_after_TV.IsAnswerFalse = false;
                                    question_after_TV.IsAnswerTrue = false;
                                    question_after_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureAfterQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearQuestionAfterTVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_after_TV_notime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                                    question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_after_TV.expyVRcurrentQuestionAnswered = false;
                                    question_after_TV.IsAnswerFalse = false;
                                    question_after_TV.IsAnswerTrue = false;
                                    question_after_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureAfterQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    /*if(int.Parse(result_next_read_line[0]) > 0) {
                                        StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                    }*/
                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "question_after_TV_bypasstime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // display question
                                    //result_next_read_line[0] = question
                                    //result_next_read_line[1] = text left answer
                                    //result_next_read_line[2] = text right answer
                                    //result_next_read_line[3] = Time No Text Before Displaying Question
                                    //result_next_read_line[4] = Time During Which the selected symbol stays visible
                                    //result_next_read_line[5] = TV picture path
                                    var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
                                    question_after_TV.expyVRcurrentQuestionAnsweredcanGoNext = false;
                                    question_after_TV.expyVRcurrentQuestionAnswered = false;
                                    question_after_TV.IsAnswerFalse = false;
                                    question_after_TV.IsAnswerTrue = false;
                                    question_after_TV.ExpyVRStartQuestion(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2], int.Parse(result_next_read_line[3]), int.Parse(result_next_read_line[4]), result_next_read_line[5]);

                                    //result_next_read_line[5] = TV picture path
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    string c_path = result_next_read_line[5];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureAfterQuestion = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineQuestionAfterTV = WaitClearQuestionAfterTVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "text_body_part":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    int number_of_text_words = -2;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }
                                    // get the text string
                                    string task_text = result_next_read_line[2];
                                    for (int i = 2; (i <= number_of_text_words); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntry(result_next_read_line[0], task_text, result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearTaskAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "text_body_part_notime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    int number_of_text_words = -2;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }
                                    // get the text string
                                    string task_text = result_next_read_line[2];
                                    for (int i = 2; (i <= number_of_text_words); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntry(result_next_read_line[0], task_text, result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    /*if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                    }*/
                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "text_body_part_bypasstime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    int number_of_text_words = -2;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }
                                    // get the text string
                                    string task_text = result_next_read_line[2];
                                    for (int i = 2; (i <= number_of_text_words); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntry(result_next_read_line[0], task_text, result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineTask = WaitClearTaskAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "continuousscale":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    /*int number_of_text_words = -5;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }*/
                                    // get the text string
                                    string task_text = result_next_read_line[5];
                                    /*for (int i = 5; (i <= number_of_text_words + 3); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }*/

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntryContinuousScale(result_next_read_line[0], task_text, result_next_read_line[1]);


                                    // + display and randomize scale
                                    if (result_next_read_line[6] == "auto")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                    else if (result_next_read_line[6] == "manual")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale_keyboardControlled(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearContinuousScaleAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "continuousscale_notime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    /*int number_of_text_words = -5;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }*/
                                    // get the text string
                                    string task_text = result_next_read_line[5];
                                    /*for (int i = 5; (i <= number_of_text_words + 3); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }*/

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntryContinuousScale(result_next_read_line[0], task_text, result_next_read_line[1]);


                                    // + display and randomize scale
                                    if (result_next_read_line[6] == "auto")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                    else if (result_next_read_line[6] == "manual")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale_keyboardControlled(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    /*if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        StartCoroutine(WaitClearContinuousScaleAndNextCommand(int.Parse(result_next_read_line[0])));
                                    }*/
                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "continuousscale_bypasstime":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // get the number of words in the text to display
                                    /*int number_of_text_words = -5;
                                    for (int i = 0; (i < result_next_read_line.Length) && (result_next_read_line[i] != null); i++)
                                    {
                                        number_of_text_words++;
                                    }*/
                                    // get the text string
                                    string task_text = result_next_read_line[5];
                                    /*for (int i = 5; (i <= number_of_text_words + 3); i++)
                                    {
                                        task_text += " " + result_next_read_line[i + 1];
                                    }*/

                                    // display task
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    experiment.ExpyVRCheckDisplayTextFullStringsEntryContinuousScale(result_next_read_line[0], task_text, result_next_read_line[1]);


                                    // + display and randomize scale
                                    if (result_next_read_line[6] == "auto")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                    else if (result_next_read_line[6] == "manual")
                                    {
                                        experiment.ExpyVRCheckDisplayContinuousScale_keyboardControlled(float.Parse(result_next_read_line[2]), result_next_read_line[3], result_next_read_line[4]);
                                    }
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineContinuousScale = WaitClearContinuousScaleAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "change_skin":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
                                    var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_set_transform_tennis_balls>();
                                    var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>();
                                    //var CanvasConfigCanvasManager = GameObject.Find("CanvasConfig").GetComponent<CanvasManager>();
                                    var _point_on_sphere_hand_trackingTracking_RH = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_RH.GetComponent<_point_on_sphere_hand_tracking>();
                                    var _point_on_sphere_hand_trackingTracking_LH = _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_LH.GetComponent<_point_on_sphere_hand_tracking>();
                                    if (result_next_read_line[0] == "male")
                                    {
                                        AvatarConfigSex.SetMaleSex();
                                        AvatarConfigTennisBall.SetIsMaleTrue();
                                        //CanvasConfigCanvasManager.ButtonMale();
                                        experiment.SetMaleSex();
                                        _point_on_sphere_hand_trackingTracking_RH.set_avatar_male();
                                        _point_on_sphere_hand_trackingTracking_LH.set_avatar_male();

                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_RH.GetComponent<_point_on_sphere_hand_tracking>().set_avatar_male();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_LH.GetComponent<_point_on_sphere_hand_tracking>().set_avatar_male();
                                    }
                                    else if (result_next_read_line[0] == "female")
                                    {
                                        AvatarConfigSex.SetFemaleSex();
                                        AvatarConfigTennisBall.SetIsMaleFalse();
                                        //CanvasConfigCanvasManager.ButtonFemale();
                                        experiment.SetFemaleSex();
                                        _point_on_sphere_hand_trackingTracking_RH.set_avatar_female();
                                        _point_on_sphere_hand_trackingTracking_LH.set_avatar_female();

                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_right_hand_RH.GetComponent<_point_on_sphere_hand_tracking>().set_avatar_female();
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets_left_hand_LH.GetComponent<_point_on_sphere_hand_tracking>().set_avatar_female();
                                    }
                                    else if (result_next_read_line[0] == "box")
                                    {
                                        AvatarConfigSex.setIsBox();
                                    }
                                    else if (result_next_read_line[0] == "none")
                                    {
                                        AvatarConfigSex.SetNoSkin();
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "activate_TV":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    if (result_next_read_line[0] == "True")
                                    {
                                        TVManager.activateTV = true;
                                    }
                                    else if (result_next_read_line[0] == "False")
                                    {
                                        TVManager.desactivateTV = true;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "activate_tennis_balls":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_set_transform_tennis_balls>();

                                    if (result_next_read_line[0] == "True")
                                    {
                                        AvatarConfigTennisBall.activate_tennis_balls_mesh_renderer();
                                    }
                                    else if (result_next_read_line[0] == "False")
                                    {
                                        AvatarConfigTennisBall.desactivate_tennis_balls_mesh_renderer();
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV_notime":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    c_path = result_next_read_line[0];
                                    TVManager.path = c_path;
                                    TVManager.setTVPicture = true;
                                    TVManager.TrialRunningEnableInput = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV_bypasstime":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    c_path = result_next_read_line[0];
                                    TVManager.path = c_path;
                                    TVManager.setTVPicture = true;
                                    TVManager.TrialRunningEnableInput = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineset_display_TV = WaitClearset_display_TVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    c_path = result_next_read_line[0];
                                    TVManager.path = c_path;
                                    TVManager.setTVPicture = true;
                                    TVManager.TrialRunningEnableInput = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearset_display_TVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV_video":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
                                    c_path = result_next_read_line[0];
                                    TVManager.b_video_ended = false;
                                    TVManager.play_video(c_path);
                                }

                                // check for duration of the command
                                {
                                    //string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    //char[] separators_next_read_line = new char[] { ' ' };
                                    //string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    //s_current_command_duration = result_next_read_line[0];

                                    //StartCoroutine(WaitClearset_display_TVAndNextCommand(int.Parse(result_next_read_line[0])));

                                    //s_current_command_duration = ((float)GameObject.Find("Scene/MRIRoom/Television/TVScreen").GetComponent<VideoPlayer>().clip.length).ToString();
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV_only_notime":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    c_path = result_next_read_line[0];
                                    TVManager.path = c_path;
                                    TVManager.setTVPictureOnly = true;
                                    nextCommand = true;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_display_TV_feedback_last_command_no_input":
                            {
                                string c_path;
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and set new skin
                                    var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();

                                    // result_next_read_line[0] = value of the correct previous answer waited
                                    // result_next_read_line[1] = path of picture corresponding to correct answer
                                    // result_next_read_line[2] = path of picture corresponding to incorrect answer

                                    string s_value_answer;
                                    if (result_next_read_line[0] == (s_last_input_command + "_" + s_last_input_answer))
                                    {
                                        c_path = result_next_read_line[1];
                                        s_value_answer = "CORRECT";
                                    }
                                    else
                                    {
                                        c_path = result_next_read_line[2];
                                        s_value_answer = "INCORRECT";
                                    }

                                    TVManager.path = c_path;
                                    TVManager.setTVPictureFeedbackLastCommand = true;

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + "[" + s_last_input_command + "] - " + "[" + s_value_answer + "] - " + "[answer : " + s_last_input_answer + " / expected : " + result_next_read_line[0] + "]" + " - " + "correct_picture : " + result_next_read_line[1] + " - " + "incorrect_picture : " + result_next_read_line[2]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearset_display_TVAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName + " - " + c_path;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName + " - " + c_path);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "NPC_OBT_notime": // init NPC_OBT command
                            {
                                // get references
                                //var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<SetTransformsTennisBalls>();
                                var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_sex>();
                                //var CanvasConfigCanvasManager = GameObject.Find("CanvasConfig").GetComponent<CanvasManager>();
                                var AvatarConfigCharacterSkinColor = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_character_skin_color>();
                                var CanvasConfigTorsoScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_torso_scale>();
                                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] = sex
                                    if (result_next_read_line[0] == "Male")
                                    {
                                        AvatarConfigSex.SetMaleSex();
                                        NPCManager.i_avatar_sex = 1;
                                    }
                                    else if (result_next_read_line[0] == "Female")
                                    {
                                        AvatarConfigSex.SetFemaleSex();
                                        NPCManager.i_avatar_sex = 2;
                                    }
                                    // result_next_read_line[1] = skin color
                                    {
                                        AvatarConfigCharacterSkinColor.ButtonGenericColorHex(result_next_read_line[1]);
                                    }
                                    // result_next_read_line[2] = TorsoScaleX
                                    CanvasConfigTorsoScale.UpdateTorsoScaleXFloat(float.Parse(result_next_read_line[2]));
                                    // result_next_read_line[3] = TorsoScaleZ
                                    CanvasConfigTorsoScale.UpdateTorsoScaleZFloat(float.Parse(result_next_read_line[3]));

                                    // result_next_read_line[4] = rotation
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.localEulerAngles = new Vector3(0, float.Parse(result_next_read_line[4]), 0);

                                    // result_next_read_line[8] = mode
                                    if (result_next_read_line[8] == "light_mode")
                                    {
                                        NPCManager.b_light_mode = true;
                                        NPCManager.b_bracelet_mode = false;
                                    }
                                    else if (result_next_read_line[8] == "bracelet_mode")
                                    {
                                        NPCManager.b_bracelet_mode = true;
                                        NPCManager.b_light_mode = false;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFileNoSpace(timestamp + " [NPC_OBT] [0] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5] + " - " + result_next_read_line[6] + " - " + result_next_read_line[7] + " - " + result_next_read_line[8]);

                                    // result_next_read_line[5] = part_to_highligh
                                    // result_next_read_line[6] = lights_color
                                    // result_next_read_line[7] = lights_intensity
                                    NPCManager.NPC_highligh_part(result_next_read_line[5], result_next_read_line[6], float.Parse(result_next_read_line[7]));

                                    NPCManager.TrialRunningEnableInput = true;

                                    //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    //writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (int.Parse(result_next_read_line[0]) > 0)
                                    {
                                        Debug.LogError("NoTime means -1 duration !!!! - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "NoTime means -1 duration !!!! - " + configName;
                                    }
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "NPC_OBT": // init NPC_OBT command
                            {
                                // get references
                                //var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<SetTransformsTennisBalls>();
                                var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_sex>();
                                //var CanvasConfigCanvasManager = GameObject.Find("CanvasConfig").GetComponent<CanvasManager>();
                                var AvatarConfigCharacterSkinColor = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_character_skin_color>();
                                var CanvasConfigTorsoScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_torso_scale>();
                                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] = sex
                                    if (result_next_read_line[0] == "Male")
                                    {
                                        AvatarConfigSex.SetMaleSex();
                                        NPCManager.i_avatar_sex = 1;
                                    }
                                    else if (result_next_read_line[0] == "Female")
                                    {
                                        AvatarConfigSex.SetFemaleSex();
                                        NPCManager.i_avatar_sex = 2;
                                    }
                                    // result_next_read_line[1] = skin color
                                    {
                                        AvatarConfigCharacterSkinColor.ButtonGenericColorHex(result_next_read_line[1]);
                                    }
                                    // result_next_read_line[2] = TorsoScaleX
                                    CanvasConfigTorsoScale.UpdateTorsoScaleXFloat(float.Parse(result_next_read_line[2]));
                                    // result_next_read_line[3] = TorsoScaleZ
                                    CanvasConfigTorsoScale.UpdateTorsoScaleZFloat(float.Parse(result_next_read_line[3]));

                                    // result_next_read_line[4] = rotation
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.localEulerAngles = new Vector3(0, float.Parse(result_next_read_line[4]), 0);

                                    // result_next_read_line[8] = mode
                                    if (result_next_read_line[8] == "light_mode")
                                    {
                                        NPCManager.b_light_mode = true;
                                        NPCManager.b_bracelet_mode = false;
                                    }
                                    else if (result_next_read_line[8] == "bracelet_mode")
                                    {
                                        NPCManager.b_bracelet_mode = true;
                                        NPCManager.b_light_mode = false;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFileNoSpace(timestamp + " [NPC_OBT] [0] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5] + " - " + result_next_read_line[6] + " - " + result_next_read_line[7]);

                                    // result_next_read_line[5] = part_to_highligh
                                    // result_next_read_line[6] = lights_color
                                    // result_next_read_line[7] = lights_intensity
                                    NPCManager.NPC_highligh_part(result_next_read_line[5], result_next_read_line[6], float.Parse(result_next_read_line[7]));

                                    NPCManager.TrialRunningEnableInput = true;

                                    //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    //writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitClearNPCAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "NPC_OBT_bypasstime": // init NPC_OBT command
                            {
                                // get references
                                //var AvatarConfigTennisBall = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<SetTransformsTennisBalls>();
                                var AvatarConfigSex = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_sex>();
                                //var CanvasConfigCanvasManager = GameObject.Find("CanvasConfig").GetComponent<CanvasManager>();
                                var AvatarConfigCharacterSkinColor = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_character_skin_color>();
                                var CanvasConfigTorsoScale = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_torso_scale>();
                                var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();

                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // result_next_read_line[0] = sex
                                    if (result_next_read_line[0] == "Male")
                                    {
                                        AvatarConfigSex.SetMaleSex();
                                        NPCManager.i_avatar_sex = 1;
                                    }
                                    else if (result_next_read_line[0] == "Female")
                                    {
                                        AvatarConfigSex.SetFemaleSex();
                                        NPCManager.i_avatar_sex = 2;
                                    }
                                    // result_next_read_line[1] = skin color
                                    {
                                        AvatarConfigCharacterSkinColor.ButtonGenericColorHex(result_next_read_line[1]);
                                    }
                                    // result_next_read_line[2] = TorsoScaleX
                                    CanvasConfigTorsoScale.UpdateTorsoScaleXFloat(float.Parse(result_next_read_line[2]));
                                    // result_next_read_line[3] = TorsoScaleZ
                                    CanvasConfigTorsoScale.UpdateTorsoScaleZFloat(float.Parse(result_next_read_line[3]));

                                    // result_next_read_line[4] = rotation
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.localEulerAngles = new Vector3(0, float.Parse(result_next_read_line[4]), 0);

                                    // result_next_read_line[8] = mode
                                    if (result_next_read_line[8] == "light_mode")
                                    {
                                        NPCManager.b_light_mode = true;
                                        NPCManager.b_bracelet_mode = false;
                                    }
                                    else if (result_next_read_line[8] == "bracelet_mode")
                                    {
                                        NPCManager.b_bracelet_mode = true;
                                        NPCManager.b_light_mode = false;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFileNoSpace(timestamp + " [NPC_OBT] [0] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5] + " - " + result_next_read_line[6] + " - " + result_next_read_line[7]);

                                    // result_next_read_line[5] = part_to_highligh
                                    // result_next_read_line[6] = lights_color
                                    // result_next_read_line[7] = lights_intensity
                                    NPCManager.NPC_highligh_part(result_next_read_line[5], result_next_read_line[6], float.Parse(result_next_read_line[7]));

                                    NPCManager.TrialRunningEnableInput = true;

                                    //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    //writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(coroutineNPC = WaitClearNPCAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "dissolve_object": // dissolve object command
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    var SolveDissolveOverTimeManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<_solve_dissolve_over_time_manager>();

                                    // result_next_read_line[0] = GO_name
                                    // result_next_read_line[1] = value_BurnSize
                                    // result_next_read_line[2] = hex_BurnColor
                                    // result_next_read_line[3] = value_EmissionAmount
                                    // result_next_read_line[4] = path_texture_dissolve_noise
                                    // result_next_read_line[5] = path_texture_burn_ramp
                                    // result_next_read_line[6] = effect_duration
                                    string GO_name = result_next_read_line[0];
                                    float value_BurnSize = float.Parse(result_next_read_line[1]);
                                    string hex_BurnColor = result_next_read_line[2];
                                    float value_EmissionAmount = float.Parse(result_next_read_line[3]);
                                    string path_texture_dissolve_noise = result_next_read_line[4];
                                    string path_texture_burn_ramp = result_next_read_line[5];
                                    float effect_duration = float.Parse(result_next_read_line[6]) / 1000.0f;
                                    SolveDissolveOverTimeManager.dissolve_game_object("dissolve", GO_name, value_BurnSize, hex_BurnColor, value_EmissionAmount, path_texture_dissolve_noise, path_texture_burn_ramp, effect_duration);

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5] + " - " + result_next_read_line[6]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    /*string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    								writeInFilePSpace(timestamp + " - [" + configName + "]");*/

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "solve_object": // solve object command
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    var SolveDissolveOverTimeManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<_solve_dissolve_over_time_manager>();

                                    // result_next_read_line[0] = GO_name
                                    // result_next_read_line[1] = value_BurnSize
                                    // result_next_read_line[2] = hex_BurnColor
                                    // result_next_read_line[3] = value_EmissionAmount
                                    // result_next_read_line[4] = path_texture_dissolve_noise
                                    // result_next_read_line[5] = path_texture_burn_ramp
                                    // result_next_read_line[6] = effect_duration
                                    string GO_name = result_next_read_line[0];
                                    float value_BurnSize = float.Parse(result_next_read_line[1]);
                                    string hex_BurnColor = result_next_read_line[2];
                                    float value_EmissionAmount = float.Parse(result_next_read_line[3]);
                                    string path_texture_dissolve_noise = result_next_read_line[4];
                                    string path_texture_burn_ramp = result_next_read_line[5];
                                    float effect_duration = float.Parse(result_next_read_line[6]) / 1000.0f; ;

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3] + " - " + result_next_read_line[4] + " - " + result_next_read_line[5] + " - " + result_next_read_line[6]);

                                    SolveDissolveOverTimeManager.dissolve_game_object("solve", GO_name, value_BurnSize, hex_BurnColor, value_EmissionAmount, path_texture_dissolve_noise, path_texture_burn_ramp, effect_duration);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    /*string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    								writeInFilePSpace(timestamp + " - [" + configName + "]");*/

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "fade_object_to_transparent": // dissolve object command
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    var SolveDissolveOverTimeManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<_solve_dissolve_over_time_manager>();

                                    // result_next_read_line[0] = GO_name
                                    // result_next_read_line[1] = value_min_alpha
                                    // result_next_read_line[2] = value_max_alpha
                                    // result_next_read_line[3] = effect_duration
                                    string GO_name = result_next_read_line[0];
                                    float value_min_alpha = float.Parse(result_next_read_line[1]);
                                    float value_max_alpha = float.Parse(result_next_read_line[2]);
                                    float effect_duration = float.Parse(result_next_read_line[3]) / 1000.0f;

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3]);

                                    SolveDissolveOverTimeManager.fade_object("dissolve", GO_name, value_min_alpha, value_max_alpha, effect_duration);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "fade_transparent_to_object": // solve object command
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    var SolveDissolveOverTimeManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<_solve_dissolve_over_time_manager>();

                                    // result_next_read_line[0] = GO_name
                                    // result_next_read_line[1] = value_min_alpha
                                    // result_next_read_line[2] = value_max_alpha
                                    // result_next_read_line[3] = effect_duration
                                    string GO_name = result_next_read_line[0];
                                    float value_min_alpha = float.Parse(result_next_read_line[1]);
                                    float value_max_alpha = float.Parse(result_next_read_line[2]);
                                    float effect_duration = float.Parse(result_next_read_line[3]) / 1000.0f;

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2] + " - " + result_next_read_line[3]);

                                    SolveDissolveOverTimeManager.fade_object("solve", GO_name, value_min_alpha, value_max_alpha, effect_duration);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    /*string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");*/

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "mri_table_rb_tracked":
                            {
                                // set pause command
                                configName = "mri_table_rb_tracked";
                                configName_FirstArrayElement = "mri_table_rb_tracked";

                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFreely();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFreely();
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFreely();

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");
                                }
                            }
                            break;

                        case "pause":
                            {
                                // set pause command
                                configName = "pause";
                                configName_FirstArrayElement = "pause";

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");
                                }
                            }
                            break;

                        case "active_navigation":
                            {
                                // check for parameters (translation speed and rotation speed)
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    f_active_navigation_translation_speed = float.Parse(result_next_read_line[0]);
                                    f_active_navigation_rotation_speed = float.Parse(result_next_read_line[1]);

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - TranslationSpeed : " + result_next_read_line[0] + " - RotationSpeed : " + result_next_read_line[1] + " --" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

                                    b_disable_active_navigation_input = false;

                                    StartCoroutine(write_active_navigation_recording());

                                    if (b_is_build_active_navigation_game_alone)
                                    {
                                        foreach (GameObject go in list_GO_trajectory_build_active_navigation_game_alone)
                                        {
                                            Destroy(go);
                                        }
                                        list_GO_trajectory_build_active_navigation_game_alone.Clear();

                                        Destroy(GO_initialPosition_build_active_navigation_game_alone);
                                        GO_initialPosition_build_active_navigation_game_alone = GameObject.Instantiate(Resources.Load("GO_initialPosition_build_active_navigation_game_alone") as GameObject);
                                        GO_initialPosition_build_active_navigation_game_alone.transform.position = GO_camera_build_active_navigation_game_alone.transform.position;
                                    }
                                }


                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");
                                }
                            }
                            break;

                        case "active_navigation_with_target":
                            {
                                // check for parameters (translation speed, rotation speed, target location)
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    f_active_navigation_translation_speed = float.Parse(result_next_read_line[0]);
                                    f_active_navigation_rotation_speed = float.Parse(result_next_read_line[1]);
                                    Vector3 v3_active_navigation_target = StringToVector3(result_next_read_line[2]);

                                    Destroy(GO_active_navigation_target);
                                    GO_active_navigation_target = GameObject.Instantiate(Resources.Load("GO_active_navigation_target") as GameObject);
                                    /*GO_active_navigation_target.name = "GO_active_navigation_target";*/
                                    //GO_active_navigation_target.transform.parent = _class_all_references_June_experiment.Instance.GO_June_experiment_anchor.transform;
                                    GO_active_navigation_target.transform.localPosition = v3_active_navigation_target + new Vector3(0,-1,0);

                                    if (_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().opacity == 0)
                                    {
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().FadeIn(1000);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_fade_camera>().FadeIn(1000);
                                    }

                                    //GO_active_navigation_target.transform.rotation = Quaternion.Euler(90,0,0);
                                    /*GO_active_navigation_target.AddComponent<Light>();
                                    GO_active_navigation_target.GetComponent<Light>().type = LightType.Spot;
                                    GO_active_navigation_target.GetComponent<Light>().intensity = 2;
                                    GameObject GO_active_navigation_target_halo = new GameObject();
                                    GO_active_navigation_target_halo.transform.parent = GO_active_navigation_target.transform;
                                    GO_active_navigation_target_halo.transform.localPosition = new Vector3(0, 0, 0);
                                    GO_active_navigation_target_halo.AddComponent<Light>();
                                    GO_active_navigation_target_halo.GetComponent<Light>().intensity = 0.35f;
                                    Behaviour halo = (Behaviour)GO_active_navigation_target_halo.GetComponent("Halo");
                                    halo.enabled = true;*/
                                    /*var haloComponent = GO_active_navigation_target_halo.GetComponent("Halo");
                                    var haloEnabledProperty = haloComponent.GetType().GetProperty("enabled");
                                    haloEnabledProperty.SetValue(haloComponent, true, null);*/


                                    //GO_active_navigation_target_halo.GetComponent("Halo").GetType().GetProperty("enabled").SetValue(GO_active_navigation_target_halo.GetComponent("Halo"),true,null);
                                    /*GO_active_navigation_target_halo.GetComponent("Halo").GetType().GetProperty("m_Color").SetValue(GO_active_navigation_target_halo.GetComponent("Halo"),Color.blue,null);
                                    GO_active_navigation_target_halo.GetComponent("Halo").GetType().GetProperty("m_Size").SetValue(GO_active_navigation_target_halo.GetComponent("Halo"),0.2f,null);*/


                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - TranslationSpeed : " + result_next_read_line[0] + " - RotationSpeed : " + result_next_read_line[1] + " --" + " ParentCameraPosition : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.position.ToString("F6") + ">; ParentCameraRotation : " + "<" + _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.rotation.ToString("F6") + "> - RealCameraPosition : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.position) / 2).ToString("F6") + ">; RealCameraRotation : " + "<" + ((_class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles + _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.transform.rotation.eulerAngles) / 2).ToString("F6") + ">");

                                    StartCoroutine(write_active_navigation_with_target_recording());

                                    if (b_is_build_active_navigation_game_alone)
                                    {
                                        foreach (GameObject go in list_GO_trajectory_build_active_navigation_game_alone)
                                        {
                                            Destroy(go);
                                        }
                                        list_GO_trajectory_build_active_navigation_game_alone.Clear();

                                        Destroy(GO_target_build_active_navigation_game_alone);
                                        GO_target_build_active_navigation_game_alone = GameObject.Instantiate(Resources.Load("GO_target_build_active_navigation_game_alone") as GameObject);
                                        //GO_target_build_active_navigation_game_alone.transform.parent = _class_all_references_June_experiment.Instance.GO_June_experiment_anchor.transform;
                                        GO_target_build_active_navigation_game_alone.transform.localPosition = v3_active_navigation_target;
                                        GO_target_build_active_navigation_game_alone.transform.position = new Vector3(GO_target_build_active_navigation_game_alone.transform.position.x, 450, GO_target_build_active_navigation_game_alone.transform.position.z);

                                        Destroy(GO_initialPosition_build_active_navigation_game_alone);
                                        GO_initialPosition_build_active_navigation_game_alone = GameObject.Instantiate(Resources.Load("GO_initialPosition_build_active_navigation_game_alone") as GameObject);
                                        GO_initialPosition_build_active_navigation_game_alone.transform.position = GO_camera_build_active_navigation_game_alone.transform.position;
                                    }
                                }


                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");
                                }
                            }
                            break;

                        case "pause_wait_for_MRI_pulse":
                            {
                                // set pause command
                                configName = "pause_wait_for_MRI_pulse";
                                configName_FirstArrayElement = "pause_wait_for_MRI_pulse";
                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "]");
                                }
                            }
                            break;

                        case "idle": 
                            {
                                // check for duration of the command - do nothing during X time
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "fade_camera_to_black_screen": 
                            {
                                // check for duration of the command - do nothing during X time
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().FadeOut(int.Parse(s_current_command_duration));
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_fade_camera>().FadeOut(int.Parse(s_current_command_duration));

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "fade_black_screen_to_camera": 
                            {
                                // check for duration of the command - do nothing during X time
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_fade_camera>().FadeIn(int.Parse(result_next_read_line[0]));
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_fade_camera>().FadeIn(int.Parse(result_next_read_line[0]));

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "comment": 
                            {
                                // 
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // comment

                                    WriteInLogFile("[" + configName + "] - " + next_read_line);
                                    //Debug.LogWarning(next_read_line);
                                    //debug_text.GetComponent<Text>().color = Color.yellow;
                                    //debug_text.GetComponent<Text>().text = next_read_line;
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "change_camera_settings": // change camera settings command
                            {
                                // 
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    // check and change camera settings
                                    var main_camera_Right_change_config = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_change_camera_settings>();
                                    var main_camera_Left_change_config = _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_change_camera_settings>();
                                    main_camera_Right_change_config.change_camera_settings_all_parameters_strings(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2]);
                                    main_camera_Left_change_config.change_camera_settings_all_parameters_strings(result_next_read_line[0], result_next_read_line[1], result_next_read_line[2]);

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " - " + result_next_read_line[1] + " - " + result_next_read_line[2]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "EOF":
                            {
                                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                writeInFilePSpace(timestamp + " - [" + configName + "]");

                                //s_current_command_duration = "-1";

                                b_read_EOF = true;
                            }
                            break;

                        case "set_AvatarsConfig_position_and_rotation":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition = StringToVector3(result_next_read_line[0]);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localRotation = Quaternion.identity;
                                    //_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.Rotate(_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, StringToVector3(result_next_read_line[1]).y);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localRotation = Quaternion.Euler(StringToVector3(result_next_read_line[1]));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " " + result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "set_camera_position_and_rotation":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.localPosition = StringToVector3(result_next_read_line[0]);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.localRotation = Quaternion.identity;
                                    //_class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.Rotate(_class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.right, StringToVector3(result_next_read_line[1]).x);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_character_camera.transform.localRotation = Quaternion.Euler(StringToVector3(result_next_read_line[1]));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " " + result_next_read_line[1]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;

                        case "move_AvatarsConfig_from_A_to_B":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localPosition = StringToVector3(result_next_read_line[0]);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localRotation = Quaternion.identity;
                                    //_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.Rotate(_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.up, StringToVector3(result_next_read_line[1]).y);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.localRotation = Quaternion.Euler(StringToVector3(result_next_read_line[1]));
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.AddComponent<_move_AvatarsConfig_from_A_to_B>();
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_move_AvatarsConfig_from_A_to_B>().pos1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.position;
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_move_AvatarsConfig_from_A_to_B>().pos2 = _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.transform.position + StringToVector3(result_next_read_line[2]);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_move_AvatarsConfig_from_A_to_B>().speed = float.Parse(result_next_read_line[3]);
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_move_AvatarsConfig_from_A_to_B>().MoveAtoB();

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " " + result_next_read_line[1] + " " + result_next_read_line[2] + " " + result_next_read_line[3]);
                                }

                                /*// check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }*/

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;
                        case "rotate_AvatarsConfig_from_A_to_B":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.AddComponent<_rotate_AvatarsConfig_from_A_to_B>();
                                    _class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_rotate_AvatarsConfig_from_A_to_B>().start_rotation(float.Parse(result_next_read_line[0]), float.Parse(result_next_read_line[1]));

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0] + " " + result_next_read_line[1]);
                                }

                                /*// check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }*/

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;
                        case "show_character":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (result_next_read_line[0] == "true" || result_next_read_line[0] == "True")
                                    {
                                        foreach(Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_Character.transform)
                                        {
                                            if (child.name != "Camera")
                                            {
                                                child.transform.localPosition = new Vector3(child.transform.localPosition.x, 0, child.transform.localPosition.z);
                                            }
                                        }
                                       // _class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition = new Vector3(_class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition.x, 0, _class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition.z);

                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.x, 0, _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.z);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_main_experiment_manager.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.x, 0, _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.z);




                                    }
                                    else if (result_next_read_line[0] == "false" || result_next_read_line[0] == "False")
                                    {
                                        foreach (Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_Character.transform)
                                        {
                                            if (child.name != "Camera")
                                            {
                                                child.transform.localPosition = new Vector3(child.transform.localPosition.x, -2000, child.transform.localPosition.z);
                                            }
                                        }
                                        //_class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition = new Vector3(_class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition.x, -4000, _class_all_references_June_experiment.Instance.GO_June_experiment_anchor_character_table.transform.localPosition.z);

                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.x, -2000, _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.z);
                                        _class_all_references_scene_mri_compatible_googles.Instance.GO_main_experiment_manager.transform.localPosition = new Vector3(_class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.x, -2000, _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.transform.localPosition.z);
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.green;
                                        //debug_text.GetComponent<Text>().text = "Current state - " + configName;
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                        //debug_text.GetComponent<Text>().color = Color.red;
                                        //debug_text.GetComponent<Text>().text = "Experiment File does not exist - " + configName;
                                    }
                                }
                            }
                            break;
                        case "enable_mesh_deformation":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    if (result_next_read_line[0] == "true" || result_next_read_line[0] == "True")
                                    {
                                        //_class_all_references_June_experiment.Instance.GO_June_experiment_anchor_stick_touch.SetActive(true);

                                        if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>().s_last_avatar_sex_before_none == "male")
                                        {
                                            foreach (Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male.transform)
                                            {
                                                if (child.transform.GetComponent<_character_mesh_deformer>() != null)
                                                {
                                                    if (child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable != true)
                                                    {
                                                        child.transform.GetComponent<_character_mesh_deformer>().b_debug_get_mesh = true;
                                                        //child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable = true;
                                                    }
                                                }
                                            }
                                        }
                                        else if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>().s_last_avatar_sex_before_none == "female")
                                        {
                                            foreach (Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female.transform)
                                            {
                                                if (child.transform.GetComponent<_character_mesh_deformer>() != null)
                                                {
                                                    if (child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable != true)
                                                    {
                                                        child.transform.GetComponent<_character_mesh_deformer>().b_debug_get_mesh = true;
                                                        //child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (result_next_read_line[0] == "false" || result_next_read_line[0] == "False")
                                    {
                                        //_class_all_references_June_experiment.Instance.GO_June_experiment_anchor_stick_touch.SetActive(false);

                                        if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>().s_last_avatar_sex_before_none == "male")
                                        {
                                            foreach (Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_male.transform)
                                            {
                                                if (child.transform.GetComponent<_character_mesh_deformer>() != null)
                                                {
                                                    if (child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable == true)
                                                    {
                                                        child.transform.GetComponent<_character_mesh_deformer>().b_debug_stop_get_mesh = true;
                                                        //child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable = false;
                                                    }
                                                }
                                            }
                                        }
                                        else if (_class_all_references_scene_mri_compatible_googles.Instance.GO_avatars_config.GetComponent<_avatar_config_sex>().s_last_avatar_sex_before_none == "female")
                                        {
                                            foreach (Transform child in _class_all_references_scene_mri_compatible_googles.Instance.GO_avatar_female.transform)
                                            {
                                                if (child.transform.GetComponent<_character_mesh_deformer>() != null)
                                                {
                                                    if (child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable == true)
                                                    {
                                                        child.transform.GetComponent<_character_mesh_deformer>().b_debug_stop_get_mesh = true;
                                                        //child.transform.GetComponent<_character_mesh_deformer>().b_is_deformable = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "load_scene_single":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    SceneManager.LoadScene(result_next_read_line[0], LoadSceneMode.Single);


                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "load_scene_additive":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    SceneManager.LoadScene(result_next_read_line[0], LoadSceneMode.Additive);

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "unload_scene":
                            {
                                // read next line
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    SceneManager.UnloadSceneAsync(result_next_read_line[0]);

                                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    writeInFilePSpace(timestamp + " - [" + configName + "] - " + result_next_read_line[0]);
                                }

                                // check for duration of the command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++; // duration
                                    char[] separators_next_read_line = new char[] { ' ' };
                                    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                                    s_current_command_duration = result_next_read_line[0];

                                    StartCoroutine(WaitAndNextCommand(int.Parse(result_next_read_line[0])));
                                }

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }
                            }
                            break;

                        case "get_mri_references":
                            {
                                _class_all_references_scene_mri_compatible_googles.Instance.GetMRIReferences();

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }

                                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                writeInFilePSpace(timestamp + " - [" + configName + "] - ");

                                nextCommand = true;
                            }
                            break;
                        case "avatar_inside_mri":
                            {
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.transform.position = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().pos2;

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }

                                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                writeInFilePSpace(timestamp + " - [" + configName + "] - ");

                                nextCommand = true;
                            }
                            break;
                        case "avatar_outside_mri":
                            {
                                _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.transform.position = _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B>().pos1;

                                // check for end of command
                                {
                                    string next_read_line = array_s_input_file[i_current_line_input_file]; i_current_line_input_file++;
                                    if (next_read_line == "")
                                    {
                                        Debug.Log("Current state - " + configName);
                                    }
                                    else
                                    {
                                        Debug.LogError("Experiment File does not exist - " + configName);
                                    }
                                }

                                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                writeInFilePSpace(timestamp + " - [" + configName + "] - ");

                                nextCommand = true;
                            }
                            break;

                    }
                    if (b_advance_server_check)
                    {
                        //_class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + result[0] + " " + s_current_command_duration + " " + text_to_display_in_command_app);
                        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + result[0] + " " + s_current_command_duration + " " + array_i_command_number_index_line[i_current_line_input_file - 1] + " " + text_to_display_in_command_app /*+ " " + array_i_command_number_index_line[i_current_line_input_file - 1]*/);
                    }

                    if (b_read_EOF)
                    {
                        i_current_line_input_file = 0;
                        
                        nextCommand = false;
                        experiment_can_start = false;

                        b_read_EOF = false;
                    }
                }
            }
            else
            {
                // reader.Close();
                i_current_line_input_file = 0;
  
                nextCommand = false;
                experiment_can_start = false;

            }
        }
    }

    bool b_read_EOF = false;

    // write some text to a txt file
    public void writeInFile(string out_path, string s_toWrite)
    {
        StreamWriter writer = new StreamWriter(out_path, true);
        writer.WriteLine(s_toWrite);
        writer.WriteLine("");

        writer.Close();
    }

    // write in log file
    public void WriteInLogFile(string s_log)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writeInFile(out_path_logs, timestamp + " - " + s_log);
    }

    public bool testFileExistance(string filePath)
    {
        if (File.Exists(filePath))
        {
            Debug.Log("File already exists - " + filePath);
            //debug_text.GetComponent<Text>().color = Color.red;
            //debug_text.GetComponent<Text>().text = "File already exists - " + filePath;

            return true;
        }
        else
        {
            return false;
        }
    }

    // Coroutine that wait wait_time ms and start next command
    private IEnumerator WaitAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms and stop move_inside move_outside command
    private IEnumerator WaitAndNextCommandStopMoveInsideRBOutsideRB(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromAToB = false;
        _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFromBToA = false;
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromAToB = false;
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
        _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFromBToA = false;
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms then clear current task (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearTaskAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
        experiment.ExpyVRStopDisplayTrial();
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms then clear current question (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearQuestionAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var question = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions>();
        question.ExpyVRsetanswerNone();
        //nextCommand = true;
        StartCoroutine(WaitOneTickBeforeNextCommand(1));
    }

    // Coroutine that wait wait_time ms then clear current question_with_TV (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearQuestionWithTVAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var question_with_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_with_TV>();
        question_with_TV.ExpyVRsetanswerNone();

        var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
        TVManager.stopDisplayOnTVScreenWithQuestions();

        //nextCommand = true;
        StartCoroutine(WaitOneTickBeforeNextCommand(1));
    }

    // Coroutine that wait wait_time ms then clear current question_with_TV (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearQuestionAfterTVAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var question_after_TV = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_exp.GetComponent<_3d_questions_after_TV>();
        question_after_TV.ExpyVRsetanswerNone();
        //nextCommand = true;
        StartCoroutine(WaitOneTickBeforeNextCommand(1));
    }

    // Coroutine that wait wait_time ms then start the next Command
    // Initialy a corrective for the questions, where answer log line was written one tick too late when the user didn't answer to the question
    private IEnumerator WaitOneTickBeforeNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms then clear current question (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearContinuousScaleAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var experiment = _class_all_references_scene_mri_compatible_googles.Instance.GO_continuous_scale_and_stroop_body.GetComponent<_continuous_scale_and_stroop_body_task>();
        experiment.ExpyVRStopDisplayTrialContinuousScale();
        experiment.ExpyVRStopDisplayContinuousScale();
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms then clear current task (from PosnerTaskExperiment) and start next command
    private IEnumerator WaitClearset_display_TVAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var TVManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>();
        TVManager.stopDisplayOnTVScreen();
        nextCommand = true;
    }

    // Coroutine that wait wait_time ms then clear current NPC_OBT and start next command
    private IEnumerator WaitClearNPCAndNextCommand(float wait_time)
    {
        yield return new WaitForSeconds(wait_time / 1000.0f);
        var NPCManager = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_manager>();
        NPCManager.ExpyVRStopDisplayTrial();
        nextCommand = true;
    }

    // buttons functions - public
    public void StartEntireExperiment()
    {
        start_experiment = true;

        // hide the canvas
        //_class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene.GetComponent<ShowHideCanvas>().hideExperimentStartCanvas();
    }

    /*public void UpdateInputField_load_file_path()
    {
        load_file_path = GameObject.Find("Canvas/HUDAllExperiment/InputFieldinputfile").GetComponent<InputField>().text;
    }

    public void UpdateInputField_out_path_logs()
    {
        out_path_logs = GameObject.Find("Canvas/HUDAllExperiment/InputFieldoutputfile").GetComponent<InputField>().text;
    }*/

    public void writeInFilePSpace(string s_toWrite)
    {
        // Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(out_path_logs, true);
        writer.WriteLine(s_toWrite);
        writer.WriteLine("");

        writer.Close();
    }

    public void writeInFileNoSpace(string s_toWrite)
    {
        // Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(out_path_logs, true);
        writer.WriteLine(s_toWrite);

        writer.Close();
    }

    void update_received_information_server_pipe()
    {
        if (b_advance_server_check)
        {
            if (_class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string.Count > 0)
            //if (_class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().s_received_string != "")
            {
                //Debug.Log(":: new command : ");
                //Debug.Log("::" + _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string[0]);

                //string next_read_line = _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().s_received_string;
                string next_read_line = _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string[0];
                _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string.RemoveAt(0);


                //_class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().b_new_command_waiting_to_be_processed = false;
                _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().s_received_string = "";


                char[] separators_next_read_line = new char[] { ' ' };
                string[] result_next_read_line = new string[0];

                try
                {
                    result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (NullReferenceException)
                {
                    // bro app client disconnectesd -> this app disconnect client + server
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().stop_pipe_client();
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().stop_pipe_server();

                    //_class_all_references_scene_mri_compatible_googles.Instance.GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().b_new_command_waiting_to_be_processed = false;

                    b_advance_server_check = false;
                    b_after_EOF_end_of_exp = true;

                    b_can_quit_app = true;
                }

                if (result_next_read_line.Length > 0)
                {
                    switch (result_next_read_line[0])
                    {
                        case "[our_client_connected_waiting_to_send_variables_values]":
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[waiting_s_experiment_file_data_path]");
                            break;
                        case "[s_experiment_file_data_path]":
                            load_file_path = result_next_read_line[1];
                            for (int i = 2; i < result_next_read_line.Length; i++)
                            {
                                load_file_path += " " + result_next_read_line[i];
                            }
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[waiting_s_log_file_data_path]");
                            //GameObject.Find("Canvas/HUDAllExperiment/InputFieldinputfile").GetComponent<InputField>().text = load_file_path;
                            break;
                        case "[s_log_file_data_path]":
                            out_path_logs = result_next_read_line[1];
                            for (int i = 2; i < result_next_read_line.Length; i++)
                            {
                                out_path_logs += " " + result_next_read_line[i];
                            }
                            _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[all_variables_values_received]");
                            //GameObject.Find("Canvas/HUDAllExperiment/InputFieldoutputfile").GetComponent<InputField>().text = out_path_logs;
                            break;
                        case "[start_experiment]":
                            StartEntireExperiment();
                            break;
                    }
                }
            }
        }
    }





    [HideInInspector]
    public Rect windowRect = new Rect(Mathf.Ceil(Screen.width / 2) - 230 / 2, Mathf.Ceil(Screen.height / 2) - 50 / 2, 230, 50);
    [HideInInspector]
    public Rect windowRect2 = new Rect(Mathf.Ceil(Screen.width / 2) - 120 / 2, Mathf.Ceil(Screen.height / 2) - 50 / 2, 120, 50);
    [HideInInspector]
    public bool b_display_menu;
    bool b_force_pause;
    bool b_pause_forced_pb;
    bool b_can_quit_app;
    bool b_check_can_quit_app;
    bool b_display_menu_2;

    void OnGUI()
    {
        if (b_display_menu)
        {
            DoMyWindow();
        }
        if (b_display_menu_2)
        {
            DoMyWindow2();
        }
    }

    // Make the contents of the window -> forced_pause
    void DoMyWindow()
    {
        if (!b_force_pause)
        {
            // force focus on this app window
            ///Cursor.lockState = CursorLockMode.None;
            ///Cursor.visible = true; // force focus on this app window

            b_force_pause = true;
        }

        if (GUI.Button(new Rect(Mathf.Ceil(Screen.width / 80), Mathf.Ceil(Screen.height / 60), Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 30)), "Resume"))
        {

            if (configName_FirstArrayElement == "forced_pause")
            {
                ///Cursor.visible = false; // force focus on this app window
                ///Cursor.lockState = CursorLockMode.Locked;
                b_force_pause = false;
                b_display_menu = false;

                nextCommand = true;
            }
            if (configName_FirstArrayElement == "pause")
            {
                ///Cursor.visible = false; // force focus on this app window
                ///Cursor.lockState = CursorLockMode.Locked;
                b_force_pause = false;
                b_display_menu = false;

                nextCommand = true;
            }
        }
        if (GUI.Button(new Rect(Mathf.Ceil(Screen.width / 80), Mathf.Ceil(Screen.height / 60 + Screen.height / 60 + Screen.height / 30), Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 30)), "Quit"))
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [" + "forced_stop" + "]");

            // reader.Close();
            i_current_line_input_file = 0;

            nextCommand = false;
            experiment_can_start = false;

            //debug_text.GetComponent<Text>().color = Color.green;
            //debug_text.GetComponent<Text>().text = "Ended";

            s_current_command_duration = "-1";

            if (b_advance_server_check)
            {
                _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[command]" + " " + "forced_stop" + " " + s_current_command_duration);
            }

            b_check_can_quit_app = true;

            StartCoroutine(quit_app(1.0f));
        }
    }

    // Make the contents of the window -> end of file -> quit app
    void DoMyWindow2()
    {
        if (!b_force_pause)
        {
            // force focus on this app window
            ///Cursor.lockState = CursorLockMode.None;
            ///Cursor.visible = true; // force focus on this app window

            b_force_pause = true;
        }

        if (GUI.Button(new Rect(Mathf.Ceil(Screen.width / 80), Mathf.Ceil(Screen.height / 60), Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 30)), "Quit"))
        {
            Application.Quit();
        }
    }

    IEnumerator quit_app(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }


    bool b_free_move_mri_table = false;

    public void update_can_move_inside_outside_freely_rb()
    {
        if (Input.GetButtonDown("ButtonFreeMoveMRITable"))
        {
            if ((configName != "move_inside_rb_notime") && (configName != "move_outside_rb_notime") && (configName != "mri_table_rb_tracked"))
            {
                if (!b_free_move_mri_table)
                {
                    // execute the move_inside command
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_MRI_scene_MRI_room.GetComponent<_move_object_from_A_to_B_rb>().MoveFreely();
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_right_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFreely();
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_camera_left_eye.GetComponent<_rotate_object_to_angle_rb>().RotateFreely();
                    _class_all_references_scene_mri_compatible_googles.Instance.GO_hand_targets.GetComponent<_move_object_from_A_to_B_rb>().MoveFreely();

                    b_free_move_mri_table = true;
                }
            }
        }
    }

    // end of the experiment -> close exp
    public void update_can_quit_app()
    {
        //if (b_advance_server_check)
        //{
        if (b_check_can_quit_app)
        {
            if (b_can_quit_app)
            {
                StartCoroutine(quit_app(1.0f));
            }
        }


        if (!b_after_EOF_end_of_exp)
        {
            if (Input.GetAxisRaw("QuitApp") == 1)
            {
                b_display_menu = true;
            }

            if (Input.GetAxisRaw("PauseForcedPb") == 1)
            {
                b_pause_forced_pb = true;
            }
        }
        //}

        if (!b_advance_server_check)
        {

            if (b_after_EOF_end_of_exp)
            {
                /*if (Input.GetAxisRaw("QuitApp") == 1)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Application.Quit();
                }*/
                if (Input.GetAxisRaw("QuitApp") == 1)
                {
                    b_display_menu_2 = true;
                }
            }
        }
    }

    public void send_error_connection_to_cameras()
    {
        _class_all_references_scene_mri_compatible_googles.Instance.GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[error_connection_to_cameras]");
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
