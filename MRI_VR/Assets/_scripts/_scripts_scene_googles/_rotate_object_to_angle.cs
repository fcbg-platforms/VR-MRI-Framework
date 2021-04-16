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

public class _rotate_object_to_angle : MonoBehaviour {
    [Header("Parameters")]
    public Vector3 rot1 = Vector3.up;
    public Vector3 rot2 = new Vector3(0,0.2f, 0.8f); // Vector3.forward

    public float speed = 0.0022f; //0.0022f
    public float offset = 0.02f; // 0.02f

    public float curO1;
    public float curO2;

    public bool RotateFromAToB = false;
    public bool RotateFromBToA = false;

    public void RotateAtoB()
    {
		//if (Vector3.Angle(this.transform.forward, rot1) < offset)
		RotateFromBToA = false;
		RotateFromAToB = true;
    }

    public void RotateBtoA()
    {
		//if (Vector3.Angle(this.transform.forward, rot2) < offset)
		RotateFromAToB = false;
		RotateFromBToA = true;
    }

    void Update()
    {
        if (RotateFromAToB)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, rot2, speed, speed));
        }
        if (RotateFromBToA)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, rot1, speed, speed));
		}

        curO1 = Vector3.Angle(this.transform.forward, rot2);

        if (Vector3.Angle(this.transform.forward, rot2) < offset)
        {
            RotateFromAToB = false;
        }

        curO2 = Vector3.Angle(this.transform.forward, rot1);

        if (Vector3.Angle(this.transform.forward, rot1) < offset)
        {
            RotateFromBToA = false;
        }        
    }
}
