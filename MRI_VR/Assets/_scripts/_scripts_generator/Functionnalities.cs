/*  
    Copyright (C) <2020>  <Valentin Bourdon>
   
    Author: Valentin Bourdon -- <vr@fcbg.ch>
   
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

public class Functionnalities : _Singleton<Functionnalities>
{
    protected Functionnalities() { }

    private Dictionary<string,string[]> dictionnariFunctionnalities;

    private void Awake()
    {
        dictionnariFunctionnalities = new Dictionary<string, string[]>();

        dictionnariFunctionnalities.Add("//", new string[2] {"Comment : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("init", new string[5] { "Sex : " , "Skin Color : ", "Torso ScaleX : ", "Torso ScaleZ : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("checkpoint", new string[2] { "Comment : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("move_inside", new string[3] { "Speed Move : ", "Speed Rotate : ", "Duration of the command (ms) :" }); // move_inside command without tracking rigidbody
        dictionnariFunctionnalities.Add("move_outside", new string[3] { "Speed Move : ", "Speed Rotate : ", "Duration of the command (ms) :" }); // move_outside command (auto) without tracking rigidbody
        dictionnariFunctionnalities.Add("move_inside_rb", new string[1] { "Duration of the command (ms) :" }); // move_inside command with tracking rigidbody and time
        dictionnariFunctionnalities.Add("move_outside_rb", new string[1] { "Duration of the command (ms) :" }); // move_outside command with tracking rigidbody and time
        dictionnariFunctionnalities.Add("move_inside_rb_notime", new string[1] { "Duration of the command (ms) :" });// move_inside command with tracking rigidbody and no time (-1)
        dictionnariFunctionnalities.Add("move_outside_rb_notime", new string[1] { "Duration of the command (ms) :" });// move_outside command with tracking rigidbody and no time (-1)

        //dictionnariFunctionnalities.Add("configure_avatar_tracking", new string[3] { "Speed Move : ", "Speed Rotate : ", "Duration of the command : " });
        dictionnariFunctionnalities.Add("set_MRI_length_rb", new string[2] { "Length : ", "Duration of the command (ms) :" });
      
        dictionnariFunctionnalities.Add("question", new string[6] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_notime", new string[6] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_bypasstime", new string[6] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_with_TV", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_with_TV_notime", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_with_TV_bypasstime", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_after_TV", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_after_TV_notime", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("question_after_TV_bypasstime", new string[7] { "Question : ", "Left answer : ", "Right answer : ", "Time before display question : ", "Time During Which the selected symbol stays visible : ", "TV picture path : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("text_body_part", new string[4] { "Position on body : ", "Color text : ", "Text : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("text_body_part_notime", new string[4] { "Position on body : ", "Color text : ", "Text : ", "Duration of the command (ms) :" }); 
        dictionnariFunctionnalities.Add("text_body_part_bypasstime", new string[4] { "Position on body : ", "Color text : ", "Text : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("continuousscale", new string[8] { "Position on body : ", "Color text : ", "Speed slider : ", "Left answer : ", "Right answer", "Question :", "Manual or Auto : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("continuousscale_notime", new string[8] { "Position on body : ", "Color text : ", "Speed slider : ", "Left answer : ", "Right answer", "Question :", "Manual or Auto : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("continuousscale_bypasstime", new string[8] { "Position on body : ", "Color text : ", "Speed slider : ", "Left answer : ", "Right answer", "Question :", "Manual or Auto : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("change_skin", new string[2] { "Male or Female : ", "Duration of the command (ms) :" });
       
        dictionnariFunctionnalities.Add("activate_tennis_balls", new string[2] { "True or False : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("activate_TV", new string[2] { "True or False : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_display_TV_notime", new string[2] { "Path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_display_TV_bypasstime", new string[2] { "Path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_display_TV", new string[2] { "Path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_display_TV_video", new string[2] { "Path : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_display_TV_only_notime", new string[2] { "Path : ", "Duration of the command (ms) :" });
        //dictionnariFunctionnalities.Add("set_display_TV_feedback_last_command_no_input", new string[3] { "Speed Move : ", "Speed Rotate : ", "Duration of the command : " });
        
        dictionnariFunctionnalities.Add("NPC_OBT_notime", new string[10] { "Male or Female : ", "Skin Color : ", "Torso ScaleX : ", "Torso ScaleY : ", "Rotation avatar : ", "Body part : ", "Highlight color : ", "Highlight intensity : ", "Mode : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("NPC_OBT", new string[10] { "Male or Female : ", "Skin Color : ", "Torso ScaleX : ", "Torso ScaleY : ", "Rotation avatar : ", "Body part : ", "Highlight color : ", "Highlight intensity : ", "Mode : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("NPC_OBT_bypasstime", new string[10] { "Male or Female : ", "Skin Color : ", "Torso ScaleX : ", "Torso ScaleY : ", "Rotation avatar : ", "Body part : ", "Highlight color : ", "Highlight intensity : ", "Mode : ", "Duration of the command (ms) :" });
       
        dictionnariFunctionnalities.Add("dissolve_object", new string[8] { "GameObject name : ", "Value BurnSize : ", "Burn color : ", "Emission amount value : ", "Path texture dissolve noise : ", "Path texture burn ramp : ", "Effect duration : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("solve_object", new string[8] { "GameObject name : ", "Value BurnSize : ", "Burn color : ", "Emission amount value : ", "Path texture dissolve noise : ", "Path texture burn ramp : ", "Effect duration : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("fade_object_to_transparent", new string[5] { "GameObject name : ", "Min alpha : ", "Max alpha : ", "Effect duration : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("fade_transparent_to_object", new string[5] { "GameObject name : ", "Min alpha : ", "Max alpha : ", "Effect duration : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("mri_table_rb_tracked", new string[1] { "Duration of the command (ms) :" });
        
        dictionnariFunctionnalities.Add("pause", new string[0] { });

        dictionnariFunctionnalities.Add("active_navigation", new string[2] { "Speed move : ", "Speed rotation : " });
        dictionnariFunctionnalities.Add("active_navigation_with_target", new string[3] { "Speed move : ", "Speed rotation : ", "Target position :" });

        dictionnariFunctionnalities.Add("pause_wait_for_MRI_pulse", new string[0] {});

        dictionnariFunctionnalities.Add("idle", new string[1] { "Duration of the command (ms) :" }); //do nothing during X time

        dictionnariFunctionnalities.Add("fade_camera_to_black_screen", new string[1] { "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("fade_black_screen_to_camera", new string[1] { "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("comment", new string[2] { "Comment :" , "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("change_camera_settings", new string[4] { "Clear Flags : ", "Culling Mask : ", "Background Color : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("EOF", new string[0] { });

        dictionnariFunctionnalities.Add("set_AvatarsConfig_position_and_rotation", new string[3] { "Position ('(x,y,z)') : ", "Rotation ('(x,y,z)'): ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("set_camera_position_and_rotation", new string[3] { "Position ('(x,y,z)') : ", "Rotation ('(x,y,z)'): ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("move_AvatarsConfig_from_A_to_B", new string[4] { "Local position ('(x,y,z)') : ", "Local Rotation ('(x,y,z)')  : ", "Target position ('(x,y,z)') : ", "Move speed : " });
        dictionnariFunctionnalities.Add("rotate_AvatarsConfig_from_A_to_B", new string[2] { "Angle : ", "Speed Rotate : "});
        dictionnariFunctionnalities.Add("show_character", new string[2] { "True or False : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("enable_mesh_deformation", new string[2] { "True or False : ", "Duration of the command (ms) :" });
       
        dictionnariFunctionnalities.Add("load_scene_single", new string[2] { "Name of the scene : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("load_scene_additive", new string[2] { "Name of the scene : ", "Duration of the command (ms) :" });
        dictionnariFunctionnalities.Add("unload_scene", new string[2] { "Name of the scene : ", "Duration of the command (ms) :" });

        dictionnariFunctionnalities.Add("get_mri_references", new string[0] {  });

        dictionnariFunctionnalities.Add("avatar_inside_mri", new string[0] { });
        dictionnariFunctionnalities.Add("avatar_outside_mri", new string[0] {  });

    }


    public Dictionary<string, string[]> GetDictionnaryFuncionnalities()
    {
        return dictionnariFunctionnalities;
    }


}
