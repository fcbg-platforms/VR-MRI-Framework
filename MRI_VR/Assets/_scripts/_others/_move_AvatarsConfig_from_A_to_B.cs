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

public class _move_AvatarsConfig_from_A_to_B : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 pos1 = new Vector3(0, 0.015f, -1.666f);
    public Vector3 pos2 = new Vector3(0, 0.015f, -2.9f);

    public float speed = 0.002f;
    public float offset = 0.01f;

    public bool MoveFromAToB = false;
    public bool MoveFromBToA = false;

    public void MoveAtoB()
    {
            MoveFromBToA = false;
			MoveFromAToB = true;
	}

    public void MoveBtoA()
    {
			MoveFromAToB = false;
			MoveFromBToA = true;
    }

    void Update()
    {
        if (MoveFromAToB)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, pos2, speed);
        }
        if (MoveFromBToA)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, pos1, speed);
        }

        if (Vector3.Distance(this.transform.position, pos2) < offset)
        {
            MoveFromAToB = false;
            _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.nextCommand = true;
            Destroy(this.GetComponent<_move_AvatarsConfig_from_A_to_B>());
        }

        if (Vector3.Distance(this.transform.position, pos1) < offset)
        {
            MoveFromBToA = false;
        }
    }
}
