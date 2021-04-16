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
using System.Collections;

public class _fade_camera : MonoBehaviour
{
	[Range (0f, 1f)]
	public float opacity = 1;
	public Color color = Color.black;

	private Material material;
	private float startTime = 0;
	private float startOpacity = 1;
	private int endOpacity = 1;
	private float duration = 0;
	private bool isFading = false;

	public void FadeIn (float duration = 1)
	{
		this.duration = duration / 1000;
		this.startTime = Time.time;
		this.startOpacity = opacity;
		this.endOpacity = 1;
		this.isFading = true;
	}

	public void FadeOut (float duration = 1)
	{
		this.duration = duration / 1000;
		this.startTime = Time.time;
		this.startOpacity = opacity;
		this.endOpacity = 0;
		this.isFading = true;
	}

	void Awake ()
	{
		material = new Material (Shader.Find ("Hidden/FadeCameraShader"));
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (isFading && duration > 0) {
			opacity = Mathf.Lerp (startOpacity, endOpacity, (Time.time - startTime) / duration);
			isFading = opacity != endOpacity;
		}

		if (opacity == 1f) {
			Graphics.Blit (source, destination);
			return;
		}

		material.color = color;
		material.SetFloat ("_opacity", opacity);
		Graphics.Blit (source, destination, material);
	}
}
