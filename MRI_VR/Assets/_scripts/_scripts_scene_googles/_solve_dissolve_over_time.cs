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
using UnityEngine;

public class _solve_dissolve_over_time : MonoBehaviour
{
    [Header("Parameters")]
	public float f_float_current = 0;
    private float f_float_1 = 0; // 0
    private float f_float_2 = 1; // 1

    public float duration = 5.0f;
	public float f_lerp = 0;

	public bool b_float_1_to_float_2 = false;
	public bool b_float_2_to_float_1 = false;

    public bool b_fade_float_1_to_float_2 = false;
    public bool b_fade_float_2_to_float_1 = false;

    public void fade_from_f_float_1_to_f_float_2()
    {
        b_fade_float_1_to_float_2 = true;
        f_lerp = Mathf.Clamp(f_float_current, f_float_1, f_float_2);
    }

    public void fade_from_f_float_2_to_f_float_1()
    {
        b_fade_float_2_to_float_1 = true;
        f_lerp = Mathf.Clamp(f_float_2 - f_float_current, f_float_1, f_float_2);
    }

    public void dissolve_from_f_float_1_to_f_float_2()
	{
		b_float_1_to_float_2 = true;
		f_lerp = Mathf.Clamp(f_float_current, f_float_1, f_float_2);
	}

	public void solve_from_f_float_2_to_f_float_1()
    {
		b_float_2_to_float_1 = true;
		f_lerp = Mathf.Clamp(f_float_2 - f_float_current, f_float_1, f_float_2);
	}

	// function dissolve duration
	public void set_dissolve_effect_duration_time(float dissolve_effect_duration_time) {
		duration = dissolve_effect_duration_time;
	}

	//function dissolve GameObject
	public void dissolve_game_object(float value_BurnSize, string hex_BurnColor, float value_EmissionAmount, string path_texture_dissolve_noise, string path_texture_burn_ramp, float f_effect_duration)
	{
		set_dissolve_effect_duration_time(f_effect_duration);

		GameObject GO = this.gameObject;

		StartCoroutine(InitDissolveGameobject(GO, value_BurnSize, hex_BurnColor, value_EmissionAmount, path_texture_dissolve_noise, path_texture_burn_ramp));
	}

    public void fade_game_object(float f_effect_duration, float f_value_min_alpha, float f_value_max_alpha)
    {
        set_dissolve_effect_duration_time(f_effect_duration);

        GameObject GO = this.gameObject;

        f_float_1 = f_value_min_alpha;
        f_float_2 = f_value_max_alpha;
    }

    private IEnumerator InitDissolveGameobject(GameObject GO, /*float value_Glossiness, float value_OcclusionStrength,*/ float value_BurnSize, string hex_BurnColor, float value_EmissionAmount, string path_texture_dissolve_noise, string path_texture_burn_ramp)
	{
		Color value_BurnColor = GetColorFromHex(hex_BurnColor);

		//GO.GetComponent<Renderer>().material.SetFloat("_Glossiness", value_Glossiness);
		//GO.GetComponent<Renderer>().material.SetFloat("_OcclusionStrengt", value_OcclusionStrength);

		Texture texture_dissolve_noise;
		Texture texture_burn_ramp;

		WWW www = new WWW("file:///" + path_texture_dissolve_noise);
		while (!www.isDone)
			yield return null;
		texture_dissolve_noise = www.texture;

		WWW www_1 = new WWW("file:///" + path_texture_burn_ramp);
		while (!www_1.isDone)
			yield return null;
		texture_burn_ramp = www_1.texture;

		GO.GetComponent<Renderer>().material.SetFloat("_BurnSize", value_BurnSize);
		GO.GetComponent<Renderer>().material.SetColor("_BurnColor", value_BurnColor);
		GO.GetComponent<Renderer>().material.SetFloat("_EmissionAmount", value_EmissionAmount);

		GO.GetComponent<Renderer>().material.SetTexture("_SliceGuide", texture_dissolve_noise);
		GO.GetComponent<Renderer>().material.SetTexture("_BurnRamp", texture_burn_ramp);
	}

	void SetDissolveValueGameObject(GameObject GO, float value_SliceAmount)
	{
		GO.GetComponent<Renderer>().material.SetFloat("_SliceAmount", value_SliceAmount);
	}

	void UpdateDissolveValueGameObjects(GameObject GO, float value_SliceAmount)
	{
		SetDissolveValueGameObject(GO, value_SliceAmount);
	}

    void UpdateFadeValueGameObjects(GameObject GO, float value_newAlpha)
    {
        GO.GetComponent<Renderer>().material.color = new Color(GO.GetComponent<Renderer>().material.color.r, GO.GetComponent<Renderer>().material.color.g, GO.GetComponent<Renderer>().material.color.b, value_newAlpha);
    }

    public Color GetColorFromHex(string Hex)
	{

		Color Body_Color_Selected = new Color();

		if (Hex[0] == '#')
		{
			if (Hex.Length == 7)
				ColorUtility.TryParseHtmlString(Hex, out Body_Color_Selected);
		}
		else
		{
			if (Hex.Length == 6)
				ColorUtility.TryParseHtmlString("#" + Hex, out Body_Color_Selected);
		}

		return Body_Color_Selected;
	}

	void Update()
	{
		if (b_float_1_to_float_2)
		{
			f_lerp = Mathf.Clamp(f_lerp + Time.deltaTime / duration, f_float_1, f_float_2);
			f_float_current = (float)Mathf.Lerp(f_float_1, f_float_2, Mathf.Clamp(f_lerp, f_float_1, f_float_2));

			UpdateDissolveValueGameObjects(this.gameObject, f_float_current);
		}
		if (b_float_2_to_float_1)
		{
			f_lerp = Mathf.Clamp(f_lerp + Time.deltaTime / duration, f_float_1, f_float_2);
			f_float_current = (float)Mathf.Lerp(f_float_1, f_float_2, f_float_2 - Mathf.Clamp(f_lerp, f_float_1, f_float_2));

			UpdateDissolveValueGameObjects(this.gameObject, f_float_current);
		}

		if ((b_float_1_to_float_2) || (b_float_2_to_float_1))
		{
			if ((f_float_current == f_float_1) || (f_float_current == f_float_2))
			{
				b_float_1_to_float_2 = false;
				b_float_2_to_float_1 = false;
				f_lerp = 0;
			}
		}


        if (b_fade_float_1_to_float_2)
        {
            f_lerp = Mathf.Clamp(f_lerp + Time.deltaTime / duration, f_float_1, f_float_2);
            f_float_current = (float)Mathf.Lerp(f_float_1, f_float_2, Mathf.Clamp(f_lerp, f_float_1, f_float_2));

            UpdateFadeValueGameObjects(this.gameObject, f_float_current);
        }
        if (b_fade_float_2_to_float_1)
        {
            f_lerp = Mathf.Clamp(f_lerp + Time.deltaTime / duration, f_float_1, f_float_2);
            f_float_current = (float)Mathf.Lerp(f_float_1, f_float_2, f_float_2 - Mathf.Clamp(f_lerp, f_float_1, f_float_2));

            UpdateFadeValueGameObjects(this.gameObject, f_float_current);
        }

        if ((b_fade_float_1_to_float_2) || (b_fade_float_2_to_float_1))
        {
            if ((f_float_current == f_float_1) || (f_float_current == f_float_2))
            {
                b_fade_float_1_to_float_2 = false;
                b_fade_float_2_to_float_1 = false;
                f_lerp = 0;
            }
        }
    }
}
