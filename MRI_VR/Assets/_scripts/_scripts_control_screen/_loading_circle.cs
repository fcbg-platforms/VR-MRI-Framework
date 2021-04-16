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

using UnityEngine;

public class _loading_circle : MonoBehaviour
{
	private RectTransform rectComponent;
	public float rotateSpeed = - 350f;

	private void Start()
	{
		rectComponent = GetComponent<RectTransform>();
	}

	private void Update()
	{
		rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}
}