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
using System.IO;
using UnityEngine;

public class _input_file_generator_experiment : MonoBehaviour {

    public int i_total_nb_subject = 5;

    public string s_MRI_entrance_exit_animation_type = "automatic"; // 'automatic' ; 'rb_tracked'
    private string s_avatar_sex = "male"; // 'male' ; 'female'

    // Use this for initialization
    void Start () {

        if (!System.IO.Directory.Exists("_Generated_experiment_files"))
        {
            System.IO.Directory.CreateDirectory("_Generated_experiment_files");
        }

        for (int i = 1; i <= i_total_nb_subject; i++)
        {
            if (!System.IO.Directory.Exists("_Generated_experiment_files\\" + "S_" + i))
            {
                System.IO.Directory.CreateDirectory("_Generated_experiment_files\\" + "S_" + i);
            }

            generate_experiment_file_string("_Generated_experiment_files\\" + "S_" + i + "\\" + "S_" + i + "_" + s_avatar_sex + ".txt");

        }
    }


    void generate_experiment_file_string(string s_path)
    {
        StreamWriter SW_writer = new StreamWriter(s_path, false);


        // comment in logs : Start of the experiment
        write_comment(SW_writer, "Start of the experiment");

        // init avatar + connect to Qualisys Motion Tracking system
        write_init_avatar(SW_writer, s_avatar_sex);
        write_init_MRI_length(SW_writer, 1.234f);

        // add a pause before the move inside
        write_pause_comment(SW_writer, "move_inside");

        // MRI entrance animation
        write_MRI_entrance(SW_writer, s_MRI_entrance_exit_animation_type);

        // add a pause after the move inside
        write_pause_comment(SW_writer, "end_move_inside");

        //add checkpoint
        write_checkpoint(SW_writer, "ckpt_move_inside");

        // fade to black
        write_fade_camera_to_black_screen(SW_writer);

        //Wait MRI pulse before continue
        write_pause_wait_for_MRI_pulse(SW_writer);

        //fade to camera
        write_fade_black_screen_to_camera(SW_writer);

        //Question
        write_continuous_scale_no_time(SW_writer, "How_was_the_example?", "very_bad", "very_good");

        // fade to black
        write_fade_camera_to_black_screen(SW_writer);

        // MRI exit animation
        write_MRI_exit(SW_writer, s_MRI_entrance_exit_animation_type);

        write_EOF(SW_writer);

        SW_writer.Close();

    }

    /// <summary>
    /// Function to activate the entrance animation inside the MRI
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_animation_type"></param> : 'automatic' ; 'rb_tracked'
    void write_MRI_entrance(StreamWriter SW_writer, string s_animation_type)
    {
        if (s_animation_type == "automatic")
        {
            write_MRI_entrance_automatic(SW_writer);
        }
        else if (s_animation_type == "rb_tracked")
        {
            write_MRI_entrance_rb_tracked(SW_writer);
        }
    }
    void write_MRI_entrance_automatic(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("move_inside");
        SW_writer.WriteLine("0.004 0.0045");
        SW_writer.WriteLine("7000");
        SW_writer.WriteLine("");
    }
    void write_MRI_entrance_rb_tracked(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("move_inside_rb_notime");
        SW_writer.WriteLine("-1");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to activate the exit animation inside the MRI
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_animation_type"></param> : 'automatic' ; 'rb_tracked'
    void write_MRI_exit(StreamWriter SW_writer, string s_animation_type)
    {
        if (s_animation_type == "automatic")
        {
            write_MRI_exit_automatic(SW_writer);
        }
        else if (s_animation_type == "rb_tracked")
        {
            write_MRI_exit_rb_tracked(SW_writer);
        }
    }
    void write_MRI_exit_automatic(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("move_outside");
        SW_writer.WriteLine("0.004 0.0045");
        SW_writer.WriteLine("7000");
        SW_writer.WriteLine("");
    }
    void write_MRI_exit_rb_tracked(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("move_outside_rb_notime");
        SW_writer.WriteLine("-1");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to write a comment in log files
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_comment"></param>
    void write_comment(StreamWriter SW_writer, string s_comment)
    {
        SW_writer.WriteLine("comment");
        SW_writer.WriteLine(s_comment);
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to init the avatar + connect to the Qualisys Motion Capture system
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_sex"></param> 'male' ; 'female'
    void write_init_avatar(StreamWriter SW_writer, string s_sex)
    {
        if (s_sex == "male")
        {
            write_init_avatar_male(SW_writer);
        }
        else if (s_sex == "female")
        {
            write_init_avatar_female(SW_writer);
        }
    }
    void write_init_avatar_male(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("init");
        SW_writer.WriteLine("Male #959595 1 1");
        SW_writer.WriteLine("5000");
        SW_writer.WriteLine("");
    }
    void write_init_avatar_female(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("init");
        SW_writer.WriteLine("Female #FFCEB4 1 1");
        SW_writer.WriteLine("5000");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to show or not the avatar
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="show"></param>
    void write_show_avatar(StreamWriter SW_writer, string show)
    {
        SW_writer.WriteLine("show_character");
        SW_writer.WriteLine(show);
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");
    }


    void write_init_MRI_length(StreamWriter SW_writer, float f_distance)
    {
        SW_writer.WriteLine("set_MRI_length_rb");
        SW_writer.WriteLine(f_distance);
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");
    }

    void write_configure_avatar_tracking(StreamWriter SW_writer, float f_size_belly_y, float f_size_belly_x, float f_offset)
    {
        SW_writer.WriteLine("set_MRI_length_rb");
        SW_writer.WriteLine(f_size_belly_y + " " + f_size_belly_x + " " + f_offset);
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to add a pause command
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_pause(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("pause");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to add a pause command
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_pause_comment(StreamWriter SW_writer, string s_comment)
    {
        SW_writer.WriteLine("pause " + s_comment);
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to add a pause_wait_for_MRI_pulse command
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_pause_wait_for_MRI_pulse(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("pause_wait_for_MRI_pulse");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to fade the camera view to a black screen
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_fade_camera_to_black_screen(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("fade_camera_to_black_screen");
        SW_writer.WriteLine("1000");
        SW_writer.WriteLine("");
    }
    /// <summary>
    /// Function to fade a black screen view to a camera view
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_fade_black_screen_to_camera(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("fade_black_screen_to_camera");
        SW_writer.WriteLine("1000");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to load a scene + anchor character to this scene (this function also unload previous loaded scenes)
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_scene"></param> 'indoor' ; 'outdoor' ; 'MRI_room'
    void write_load_scene(StreamWriter SW_writer, string s_scene)
    {
        if (s_scene == "scene_1")
        {
            // unload outdoor scene (in case it was loaded)
            SW_writer.WriteLine("unload_scene");
            SW_writer.WriteLine("1");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // load indoor_scene
            SW_writer.WriteLine("load_scene");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // anchor character to the loaded scene
            SW_writer.WriteLine("anchor_AvatarsConfig_to_Scene_1_anchor");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");
        }
        else if (s_scene == "scene_2")
        {
            // unload indoor scene (in case it was loaded)
            SW_writer.WriteLine("unload_scene");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // load outdoor_scene
            SW_writer.WriteLine("load_scene");
            SW_writer.WriteLine("1");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // anchor character to the loaded scene
            SW_writer.WriteLine("anchor_AvatarsConfig_to_Scene_2_anchor");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");
        }
        else if (s_scene == "MRI_room")
        {
            // unload indoor scene (in case it was loaded)
            SW_writer.WriteLine("unload_scene");
            SW_writer.WriteLine("1");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // unload outdoor scene (in case it was loaded)
            SW_writer.WriteLine("unload_scene");
            SW_writer.WriteLine("1");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");

            // anchor character to the loaded scene
            SW_writer.WriteLine("anchor_AvatarsConfig_to_nothing");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");
        }
    }

    /// <summary>
    /// Fuction to init the camera position and rotation to rotate around the scene (to give participant an overview of the scene)
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_scene"></param>
    void write_init_camera_rotate_around_scene(StreamWriter SW_writer, string s_scene)
    {
        SW_writer.WriteLine("set_AvatarsConfig_position_and_rotation");
        SW_writer.WriteLine("(0,0,0) (0,0,0)");
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");

        SW_writer.WriteLine("show_character");
        SW_writer.WriteLine("false");
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");

        /*SW_writer.WriteLine("show_table_under_character");
        SW_writer.WriteLine("false");
        SW_writer.WriteLine("0");
        SW_writer.WriteLine("");*/

        if (s_scene == "scene_1")
        {
            // load indoor_scene
            SW_writer.WriteLine("set_camera_position_and_rotation");
            SW_writer.WriteLine("(0,1,-4.5) (10,0,0)");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");
        }
        else if (s_scene == "scene_2")
        {
            // unload indoor scene (in case it was loaded)
            SW_writer.WriteLine("set_camera_position_and_rotation");
            //SW_writer.WriteLine("(0,200,-200) (55,0,0)");
            SW_writer.WriteLine("(0,1,-4.5) (10,0,0)");
            SW_writer.WriteLine("0");
            SW_writer.WriteLine("");
        }
    }

    /// <summary>
    /// Fuction to rotate the camera around the scene (to give participant an overview of the scene)
    /// </summary>
    /// <param name="SW_writer"></param>
    void write_camera_rotate_around_scene(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("rotate_AvatarsConfig_from_A_to_B");
        SW_writer.WriteLine("90 0.1");
        SW_writer.WriteLine("");

        SW_writer.WriteLine("rotate_AvatarsConfig_from_A_to_B");
        SW_writer.WriteLine("90 0.1");
        SW_writer.WriteLine("");

        SW_writer.WriteLine("rotate_AvatarsConfig_from_A_to_B");
        SW_writer.WriteLine("90 0.1");
        SW_writer.WriteLine("");

        SW_writer.WriteLine("rotate_AvatarsConfig_from_A_to_B");
        SW_writer.WriteLine("90 0.1");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to add an idle
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="i_time_ms"></param>
    void write_idle(StreamWriter SW_writer, int i_time_ms)
    {
        SW_writer.WriteLine("idle");
        SW_writer.WriteLine(i_time_ms);
        SW_writer.WriteLine("");
    }
    
    /// <summary>
    /// Function to write the EOF command
    /// </summary>
    void write_EOF(StreamWriter SW_writer)
    {
        SW_writer.WriteLine("EOF");
    }
    
    /// <summary>
    /// Function to add a checkpoint
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_checkpoint_name"></param>
    void write_checkpoint(StreamWriter SW_writer, string s_checkpoint_name)
    {
        SW_writer.WriteLine("checkpoint");
        SW_writer.WriteLine(s_checkpoint_name);
        SW_writer.WriteLine("");
    }

   
    /// <summary>
    /// Function to display a continuous scale
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_question"></param> in degree
    /// <param name="s_left_choice"></param>
    /// <param name="s_right_choice"></param>
    void write_continuous_scale_no_time(StreamWriter SW_writer, string s_question, string s_left_choice, string s_right_choice)
    {
       
        SW_writer.WriteLine("continuousscale_notime");
        SW_writer.WriteLine("Other" + " " + "white" + " " + "1" + " " + s_left_choice + " " + s_right_choice + " " + s_question + " " + "manual");
        SW_writer.WriteLine("-1");
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to display a text to the user
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_text_to_display"></param> in degree
    /// <param name="i_duration"></param>
    void write_text_black_screen(StreamWriter SW_writer, string s_text_to_display, int i_duration)
    {  
        SW_writer.WriteLine("text_body_part");
        SW_writer.WriteLine("Other" + " " + "white" + " " + s_text_to_display);
        SW_writer.WriteLine(i_duration);
        SW_writer.WriteLine("");
    }

    /// <summary>
    /// Function to display a text to the user. Text color : white ; background color : black. Press Button1 to continue
    /// </summary>
    /// <param name="SW_writer"></param>
    /// <param name="s_text_to_display"></param> in degree
    void write_text_black_screen_notime(StreamWriter SW_writer, string s_text_to_display)
    {     
        SW_writer.WriteLine("text_body_part_notime");
        SW_writer.WriteLine("Other" + " " + "white" + " " + s_text_to_display);
        SW_writer.WriteLine("-1");
        SW_writer.WriteLine("");
    }

   /// <summary>
   /// Function to display a question 
   /// </summary>
   /// <param name="SW_writer"></param>
   /// <param name="question"></param>
   /// <param name="left_answer"></param>
   /// <param name="right_answer"></param>
   /// <param name="time_before"></param>
   /// <param name="time_during"></param>
    void write_question_noTime(StreamWriter SW_writer, string question, string left_answer, string right_answer, string time_before, string time_during)
    {
        SW_writer.WriteLine("question_notime");
        SW_writer.WriteLine(question + " " + left_answer + " " + right_answer + " " + time_before + " " + time_during);
        SW_writer.WriteLine("");
    }
}
