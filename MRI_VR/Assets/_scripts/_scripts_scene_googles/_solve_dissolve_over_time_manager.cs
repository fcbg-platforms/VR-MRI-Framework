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

public class _solve_dissolve_over_time_manager : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject GO_mriroom_mri_magnet;
	public GameObject GO_mriroom_mri_table;
	public GameObject GO_mriroom_roomamdothers_ceiling;
	public GameObject GO_mriroom_roomamdothers_chariot;
	public GameObject GO_mriroom_roomamdothers_chariot001;
	public GameObject GO_mriroom_roomamdothers_chariot002;
	public GameObject GO_mriroom_roomamdothers_floor;
	public GameObject GO_mriroom_roomamdothers_frontwalls;
	public GameObject GO_mriroom_roomamdothers_sidewalls;
	public GameObject GO_mrimovingtable;

    public void Start()
	{
		InitializeGO();
	}

	public void InitializeGO()
	{
        GO_mriroom_mri_magnet = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_mri_magnet;
        GO_mriroom_mri_table = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_mri_table;
        GO_mriroom_roomamdothers_ceiling = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_ceiling;
        GO_mriroom_roomamdothers_chariot = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_chariot;
        GO_mriroom_roomamdothers_chariot001 = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_chariot001;
        GO_mriroom_roomamdothers_chariot002 = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_chariot002;
        GO_mriroom_roomamdothers_floor = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_floor;
        GO_mriroom_roomamdothers_frontwalls = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_frontwalls;
        GO_mriroom_roomamdothers_sidewalls = _class_all_references_scene_mri_compatible_googles.Instance.GO_mriroom_roomamdothers_sidewalls;
        GO_mrimovingtable = _class_all_references_scene_mri_compatible_googles.Instance.GO_mrimovingtable;

        /*GO_mriroom_mri_magnet.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_mri_table.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_ceiling.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_chariot.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_chariot001.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_chariot002.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_floor.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_frontwalls.AddComponent<_solve_dissolve_over_time>();
		GO_mriroom_roomamdothers_sidewalls.AddComponent<_solve_dissolve_over_time>();
		GO_mrimovingtable.AddComponent<_solve_dissolve_over_time>();*/
    }

	//function dissolve GameObject
	public void dissolve_game_object(string state,  string GO_name, float value_BurnSize, string hex_BurnColor, float value_EmissionAmount, string path_texture_dissolve_noise, string path_texture_burn_ramp, float f_effect_duration)
	{
		GameObject GO = null;

		switch (GO_name)
		{
			case "mri_magnet":
				GO = GO_mriroom_mri_magnet;
				break;
			case "mri_table":
				GO = GO_mriroom_mri_table;
				break;
			case "mri_movingtable":
				GO = GO_mrimovingtable;
				break;
			case "roomamdothers_ceiling":
				GO = GO_mriroom_roomamdothers_ceiling;
				break;
			case "roomamdothers_chariot":
				GO = GO_mriroom_roomamdothers_chariot;
				break;
			case "roomamdothers_chariot001":
				GO = GO_mriroom_roomamdothers_chariot001;
				break;
			case "roomamdothers_chariot002":
				GO = GO_mriroom_roomamdothers_chariot002;
				break;
			case "roomamdothers_floor":
				GO = GO_mriroom_roomamdothers_floor;
				break;
			case "roomamdothers_frontwalls":
				GO = GO_mriroom_roomamdothers_frontwalls;
				break;
			case "roomamdothers_sidewalls":
				GO = GO_mriroom_roomamdothers_sidewalls;
				break;
			default:
				break;
		}
		
		GO.GetComponent<_solve_dissolve_over_time>().dissolve_game_object(value_BurnSize, hex_BurnColor, value_EmissionAmount, path_texture_dissolve_noise, path_texture_burn_ramp, f_effect_duration);

		switch (state)
		{
			case "dissolve":
				GO.GetComponent<_solve_dissolve_over_time>().dissolve_from_f_float_1_to_f_float_2();
				break;
			case "solve":
				GO.GetComponent<_solve_dissolve_over_time>().solve_from_f_float_2_to_f_float_1();
				break;
			default:
				break;
		}
	}

    //function dissolve GameObject
    public void fade_object(string state, string GO_name, float value_min_alpha, float value_max_alpha, float f_effect_duration)
    {
        GameObject GO = null;

        switch (GO_name)
        {
            case "mri_magnet":
                GO = GO_mriroom_mri_magnet;
                break;
            case "mri_table":
                GO = GO_mriroom_mri_table;
                break;
            case "mri_movingtable":
                GO = GO_mrimovingtable;
                break;
            case "roomamdothers_ceiling":
                GO = GO_mriroom_roomamdothers_ceiling;
                break;
            case "roomamdothers_chariot":
                GO = GO_mriroom_roomamdothers_chariot;
                break;
            case "roomamdothers_chariot001":
                GO = GO_mriroom_roomamdothers_chariot001;
                break;
            case "roomamdothers_chariot002":
                GO = GO_mriroom_roomamdothers_chariot002;
                break;
            case "roomamdothers_floor":
                GO = GO_mriroom_roomamdothers_floor;
                break;
            case "roomamdothers_frontwalls":
                GO = GO_mriroom_roomamdothers_frontwalls;
                break;
            case "roomamdothers_sidewalls":
                GO = GO_mriroom_roomamdothers_sidewalls;
                break;
            default:
                break;
        }

        GO.GetComponent<_solve_dissolve_over_time>().fade_game_object(f_effect_duration, value_min_alpha, value_max_alpha);

        switch (state)
        {
            case "dissolve":
                GO.GetComponent<_solve_dissolve_over_time>().f_float_current = value_max_alpha;
                GO.GetComponent<_solve_dissolve_over_time>().fade_from_f_float_2_to_f_float_1();
                break;
            case "solve":
                GO.GetComponent<_solve_dissolve_over_time>().f_float_current = value_min_alpha;
                GO.GetComponent<_solve_dissolve_over_time>().fade_from_f_float_1_to_f_float_2();
                break;
            default:
                break;
        }
    }
}
