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

public class _change_camera_settings : MonoBehaviour {


    private void change_camera_settings(int clear_flags, int culling_mask, string HEX_color)
    {
        this.GetComponent<Camera>().clearFlags = (CameraClearFlags)clear_flags;
        this.GetComponent<Camera>().cullingMask = culling_mask;
        Color new_color;
        ColorUtility.TryParseHtmlString(HEX_color, out new_color);
        this.GetComponent<Camera>().backgroundColor = new_color;
    }

    public void change_camera_settings_all_parameters_strings(string s_clear_flags, string s_culling_mask, string HEX_color)
    {
        int i_clear_flags;
        int i_culling_mask;

        if (s_clear_flags == "Skybox")
        {
            i_clear_flags = 1;
        }
        else if (s_clear_flags == "Color")
        {
            i_clear_flags = 2;
        }
        else if (s_clear_flags == "Depth")
        {
            i_clear_flags = 3;
        }
        else if (s_clear_flags == "Nothing")
        {
            i_clear_flags = 4;
        }
        else
        {
            i_clear_flags = 1;
        }

        if (s_culling_mask == "Everything")
        {
            i_culling_mask = -1;
        }
        else if (s_culling_mask == "Nothing")
        {
            i_culling_mask = 0;
        }
        else
        {
            i_culling_mask = -1;
        }

        change_camera_settings(i_clear_flags, i_culling_mask, HEX_color);
    }
}
