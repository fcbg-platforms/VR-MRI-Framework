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

public class _rotate_AvatarsConfig_from_A_to_B : MonoBehaviour
{
    [Header("Parameters")]
    public float f_speed = 0.1f;
    public bool b_is_rotating;

    Vector3 v3_end_direction = new Vector3(0, 0.2f, 0.8f); // Vector3.forward
    
    void Update()
    {
        if (b_is_rotating)
        {
            float f_step = f_speed * Time.deltaTime;
            Vector3 v3_newDir = Vector3.RotateTowards(transform.forward, v3_end_direction, f_step, 0);
            if (Vector3.RotateTowards(transform.forward, v3_end_direction, f_step, 0) != Vector3.RotateTowards(transform.forward, v3_end_direction, 2 * f_step, 0))
            {
                this.transform.rotation = Quaternion.LookRotation(v3_newDir);
            }
            else
            {
                this.transform.rotation = Quaternion.LookRotation(v3_end_direction);
                b_is_rotating = false;
                _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.nextCommand = true;
                Destroy(this.GetComponent<_rotate_AvatarsConfig_from_A_to_B>());
            }
        }
    }

    public void start_rotation(float f_angle_in, float f_speed_in)
    {
        v3_end_direction = Quaternion.AngleAxis(f_angle_in, transform.up) * transform.forward;
        f_speed = f_speed_in;
        b_is_rotating = true;
    }
}
