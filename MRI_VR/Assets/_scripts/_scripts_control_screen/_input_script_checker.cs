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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class _input_script_checker : MonoBehaviour {

	[Header("Advanced server parameters start experiment")]
    private bool b_advance_server_check = true;
    private GameObject DebugTextZone;
    private int i_debug_num_lines_debugtextzone;
	public string s_scene_second_control_screen = "scene_second_control_screen";
	public string s_experiment_file_location = "_Datas";
	public string s_experiment_file_name = "_run_mri_experiment.bat";

	[Header("Experiment Sequence")]
    public string load_file_path = "Results/in_experiment.txt";
	public string save_file_path = "Results/out_experiment.txt";

	[Header("Others")]
    private StreamReader reader;
    private bool start_experiment;
    //private bool experiment_can_start;
    private bool reading_file;
    private string configName;
    private bool nextCommand;
    bool exists_load_file_path;
    int nb_line;
    int tot_nb_line;
    bool startChecking;
    bool gostartChecking;
    [HideInInspector]
    public bool advanced_check = true; // check for command pb
	bool is_TV_displayed; // is the TV displayed
	bool is_MRI_table_inside; // is the MRI table inside (means camera looking from the MRI to the front MRI wall)
	bool is_avatar_initialised; // has the <init> command be called
	string s_last_answer_asked_command;
    private int i_number_of_commands;
    private float f_total_commands_no_time_duration;

	// Use this for initialization
	void Start () {

        load_file_path = "Results/in_experiment.txt";
        GameObject.Find("Canvas/HUDAllExperiment/InputFieldinputfile").GetComponent<InputField>().text = load_file_path;
		_static_informations.s_experiment_file_data_path = load_file_path;

		save_file_path = "Results/" + System.DateTime.Now.ToString("[yyyy-dd-MM] [HH-mm-ss]") + " out_experiment.txt";
		GameObject.Find("Canvas/HUDAllExperiment/InputFieldoutputfile").GetComponent<InputField>().text = save_file_path;
		_static_informations.s_log_file_data_path = save_file_path;

		if (b_advance_server_check)
		{
			DebugTextZone = GameObject.Find("Canvas/DebugTextScrollView/Viewport/Content/DebugTextZone");
			_static_informations.s_experiment_exe_data_path = "\\" + s_experiment_file_location + "\\" + s_experiment_file_name;
		}
	}
	
	// Update is called once per frame
	void Update () {
        //check_cheat_codes();
        if (reading_file)
        {
            CheckFile();
        }
    }

    System.DateTime startTime;
    string currentCode = "";

    /*void check_cheat_codes()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                if ((System.DateTime.UtcNow - startTime).Seconds >= 1)
                {
                    currentCode = "";
                }
                startTime = System.DateTime.UtcNow;
                currentCode += vKey.ToString();

                if (currentCode.Contains("THEREISNOURFLEVEL")) // ref : league of legend to bypass launcher when in maintenance and servers still accessibles
                {
                    StartCoroutine(CheckFileCoroutine());
                    Debug.LogError("clear_logs");
                    Debug.LogWarning("File checker bypassed");
                    StartCoroutine(WaitForMsWait3s(1000));
                }
            }
        }
    }*/

    public String computeHash(String message, String algo)
    {
        byte[] sourceBytes = System.Text.Encoding.Default.GetBytes(message);
        byte[] hashBytes = null;
        switch (algo.Trim().ToUpper())
        {
            case "MD5":
                hashBytes = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(sourceBytes);
                break;
            case "SHA1":
                hashBytes = System.Security.Cryptography.SHA1Managed.Create().ComputeHash(sourceBytes);
                break;
            case "SHA256":
                hashBytes = System.Security.Cryptography.SHA256Managed.Create().ComputeHash(sourceBytes);
                break;
            case "SHA384":
                hashBytes = System.Security.Cryptography.SHA384Managed.Create().ComputeHash(sourceBytes);
                break;
            case "SHA512":
                hashBytes = System.Security.Cryptography.SHA512Managed.Create().ComputeHash(sourceBytes);
                break;
            default:
                break;
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; (hashBytes != null) && (i < hashBytes.Length); i++)
        {
            sb.AppendFormat("{0:x2}", hashBytes[i]);
        }
        return sb.ToString();
    }


    public static string reverse_string(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    bool check_hash_file(string s_input_load_file_path)
    {
        if (File.Exists(s_input_load_file_path))
        {
            //StreamReader reader = new StreamReader(s_input_load_file_path);

            string[] a_s_all_lines = File.ReadAllLines(s_input_load_file_path);
            string[] a_s_all_lines_slice_first_elem = new string[a_s_all_lines.Length - 1];
            string s_all_lines = "";

            if (a_s_all_lines.Length > 1)
            {
                for (int i = 1; i < a_s_all_lines.Length; i++)
                {
                    s_all_lines += a_s_all_lines[i];
                    if (i < a_s_all_lines.Length - 1)
                    {
                        s_all_lines += "\n";

                        a_s_all_lines_slice_first_elem[i] = a_s_all_lines[i + 1];
                    }
                }

                string s_first_element_text_file_in = a_s_all_lines[0];

                // verify first line is sha512 hash format
                if (s_first_element_text_file_in.Length > 3)
                {
                    if (s_first_element_text_file_in.Length == (128 + 32 + 96 + 40 + 64 + 128 + 3)) // sha512.length + md5.length + sha384.length + sha1.length + sha256.length + sha512.length + "// ".length
                    {
                        string[] input_array = s_all_lines.Split('\n');

                        List<String> l_modified_input_array_256 = new List<String>();
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 2) == 0)
                            {
                                l_modified_input_array_256.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 5) == 0)
                            {
                                l_modified_input_array_256.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 6) == 0)
                            {
                                l_modified_input_array_256.Add(input_array[i]);
                            }
                        }

                        string s_modified_input_256 = "";
                        for (int i = 0; i < l_modified_input_array_256.Count; i++)
                        {
                            s_modified_input_256 += l_modified_input_array_256[i];
                            if (i < l_modified_input_array_256.Count - 1)
                            {
                                s_modified_input_256 += "\n";
                            }
                        }
                        string s_sha256_modified_input = computeHash(s_modified_input_256, "SHA256");

                        List<String> l_modified_input_array_512 = new List<String>();
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 5) == 0)
                            {
                                l_modified_input_array_512.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 1) == 0)
                            {
                                l_modified_input_array_512.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 2) == 0)
                            {
                                l_modified_input_array_512.Add(input_array[i]);
                            }
                        }

                        string s_modified_input_512 = "";
                        for (int i = 0; i < l_modified_input_array_512.Count; i++)
                        {
                            s_modified_input_512 += l_modified_input_array_512[i];
                            if (i < l_modified_input_array_512.Count - 1)
                            {
                                s_modified_input_512 += "\n";
                            }
                        }
                        string s_sha512_modified_input = computeHash(s_modified_input_512, "SHA512");

                        List<String> l_modified_input_array_384 = new List<String>();
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 3) == 0)
                            {
                                l_modified_input_array_384.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 8) == 0)
                            {
                                l_modified_input_array_384.Add(input_array[i]);
                            }
                        }
                        for (int i = 0; i < input_array.Length; i++)
                        {
                            if ((i % 4) == 0)
                            {
                                l_modified_input_array_384.Add(input_array[i]);
                            }
                        }

                        string s_modified_input_384 = "";
                        for (int i = 0; i < l_modified_input_array_384.Count; i++)
                        {
                            s_modified_input_384 += l_modified_input_array_384[i];
                            if (i < l_modified_input_array_384.Count - 1)
                            {
                                s_modified_input_384 += "\n";
                            }
                        }
                        string s_sha384_modified_input = computeHash(s_modified_input_384, "SHA384");

                        string s_sha1_input = computeHash(s_all_lines, "SHA1");
                        string s_md5_input = computeHash(s_all_lines, "MD5");

                        string s_final_input = s_sha512_modified_input + s_md5_input + s_sha384_modified_input + s_sha1_input + s_sha256_modified_input;

                        string s_final_hash = s_final_input + computeHash(computeHash(computeHash(computeHash(computeHash(s_all_lines, "MD5"), "SHA1"), "SHA256"), "SHA384"), "SHA512"); ;
                        
                        if (s_final_hash == s_first_element_text_file_in.Substring(3, s_first_element_text_file_in.Length - 3))
                        {
                            //Debug.Log("All is good");
                        }
                        else
                        {
                            Debug.LogError("File not validated : corrupted key !");
                            return false;
                        }

                    }
                    else
                    {
                        Debug.LogError("File not validated : first line in wrong format");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("File not validated : first line has " + s_first_element_text_file_in.Length + " characters");
                    return false;
                }
            }
            else
            {
                Debug.LogError("Input file contains " + a_s_all_lines.Length + " line");
                return false;
            }
        }
        else
        {
            Debug.LogError("Experiment File does not exist - " + s_input_load_file_path);
            return false;
        }
        return true;
    }


    public void StartCheckFile()
    {
        nb_line = 0;
        tot_nb_line = 0;
		i_number_of_commands = 0;
		f_total_commands_no_time_duration = 0;

		is_TV_displayed = false;
		is_MRI_table_inside = false;
		is_avatar_initialised = false;

		s_last_answer_asked_command = "";

        gostartChecking = true;
        startChecking = false;

		OpenFile();
    }

    public void OpenFile()
    {
        if (File.Exists(load_file_path))
        {
            tot_nb_line = GetNumberOfLinesOfFile(load_file_path);

            reader = new StreamReader(load_file_path);
            reading_file = true;
            nextCommand = true;
            gostartChecking = true;

            Debug.Log("Checking File - " + load_file_path);
        }
        else
        {
            Debug.LogError("Experiment File does not exist - " + load_file_path);
        }
    }

    public int GetNumberOfLinesOfFile(string load_file_path)
    {
        int count = 0;
        TextReader t_reader = new StreamReader(load_file_path);
        while ((t_reader.ReadLine()) != null)
        {
            count++;
        }
        t_reader.Close();

        return count;
    }

    private IEnumerator CheckFileCoroutine()
    {
        while (startChecking)
        {
			if (nextCommand)
			{
				i_number_of_commands++;

				if (!reader.EndOfStream)
				{
					configName = reader.ReadLine(); nb_line++;

					char[] separators = new char[] { ' ' };
					string[] result = configName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

					if (result.Length == 0) // blank space -> no command found
					{
						Debug.LogError("[Line " + nb_line + "] - " + configName + " : Unexpected empty line");
					}
					else if (result[0] == "checkpoint")
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <checkpoint>, followed by an argument representing a text to display in command app, no other following arguments");
							}
						}

						// read next line
						{
							reader.ReadLine(); nb_line++;
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "//") // user comment in script
					{

					}
					else if (result[0] == "move_inside") // move_inside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_inside>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == true)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already inside the MRI");
								}
								else if (is_MRI_table_inside == false)
								{
									is_MRI_table_inside = true;
								}
							}

						}

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check init 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <f_speed_translation> <f_speed_rotation>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    // result_next_read_line[0] = f_speed_translation
                                    float out_float_1;
                                    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the f_speed_translation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                    if (out_float_1 <= 0.0f)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the f_speed_translation (value > 0) - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    // result_next_read_line[1] = f_speed_rotation
                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the f_speed_rotation - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                    if (out_float_2 <= 0.0f)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the f_speed_rotation (value > 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);


							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{

								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "move_outside") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_outside>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already outside the MRI");
								}
								else if (is_MRI_table_inside == true)
								{
									is_MRI_table_inside = false;
								}
							}
						}

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check init 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <f_speed_translation> <f_speed_rotation>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    // result_next_read_line[0] = f_speed_translation
                                    float out_float_1;
                                    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the f_speed_translation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                    if (out_float_1 <= 0.0f)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the f_speed_translation (value > 0) - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    // result_next_read_line[1] = f_speed_rotation
                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the f_speed_rotation - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                    if (out_float_2 <= 0.0f)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the f_speed_rotation (value > 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "move_inside_rb") // move_inside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_inside_rb>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == true)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already inside the MRI");
								}
								else if (is_MRI_table_inside == false)
								{
									is_MRI_table_inside = true;
								}
							}

						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "move_outside_rb") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_outside_rb>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already outside the MRI");
								}
								else if (is_MRI_table_inside == true)
								{
									is_MRI_table_inside = false;
								}
							}

						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "move_inside_rb_notime") // move_inside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_inside_rb_notime>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == true)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already inside the MRI");
								}
								else if (is_MRI_table_inside == false)
								{
									is_MRI_table_inside = true;
								}
							}

						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
								}
								else if (int.Parse(result_next_read_line[0]) != -1)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "move_outside_rb_notime") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_outside_rb_notime>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
								}

								if (is_MRI_table_inside == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[USELESS]" + " - " + "MRI table already outside the MRI");
								}
								else if (is_MRI_table_inside == true)
								{
									is_MRI_table_inside = false;
								}
							}

						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
								}
								else if (int.Parse(result_next_read_line[0]) != -1)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "init") // init command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <init>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								if (is_avatar_initialised == true)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command has already been called");
								}
								else if (is_avatar_initialised == false)
								{
									is_avatar_initialised = true;
								}
							}

						}

						// read next line
						{
							string next_read_line = reader.ReadLine(); nb_line++;
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							//Check init 1st line arguments
							{
								if (result_next_read_line.Length != 4)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 4 arguments (" + result_next_read_line.Length + " arguments detected) - format : <sex> <color> <torsoscaleX> <torsoscaleZ>");
								}
								else if (result_next_read_line.Length == 4)
								{
									// result_next_read_line[0] = sex
									if ((result_next_read_line[0] != "Male") && (result_next_read_line[0] != "Female"))
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to sex : <Male> or <Female> - (<" + result_next_read_line[0] + "> detected)");
									}

									// result_next_read_line[1] = color
									/*if ((result_next_read_line[1] != "3C2E28") && (result_next_read_line[1] != "4B3932") && (result_next_read_line[1] != "5A453C") && (result_next_read_line[1] != "695046") && (result_next_read_line[1] != "785C50")
                                         && (result_next_read_line[1] != "87675A") && (result_next_read_line[1] != "967264") && (result_next_read_line[1] != "A57E6E") && (result_next_read_line[1] != "B48A78") && (result_next_read_line[1] != "C39582")
                                          && (result_next_read_line[1] != "D2A18C") && (result_next_read_line[1] != "E1AC96") && (result_next_read_line[1] != "F0B8A0") && (result_next_read_line[1] != "FFC3AA") && (result_next_read_line[1] != "FFCEB4")
                                           && (result_next_read_line[1] != "FFDABE") && (result_next_read_line[1] != "FFE5C8"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <3C2E28> or <4B3932> or <5A453C> or <695046> or <785C50> or <87675A> or <967264> or <A57E6E> or <B48A78> or <C39582> or <D2A18C> or <E1AC96> or <F0B8A0> or <FFC3AA> or <FFCEB4> or <FFDABE> or <FFE5C8> - (<" + result_next_read_line[1] + "> detected)");
                                    }*/

									// result_next_read_line[1] = color
									string Hex = result_next_read_line[1];
									Color c;
									bool result_hex_color_parsing = false;
									if (Hex[0] == '#')
									{
										if (Hex.Length == 7)
											result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
									}
									else
									{
										if (Hex.Length == 6)
											result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
									}
									if (!result_hex_color_parsing)
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[1] + "> detected)");
									}

									// result_next_read_line[2] = TorsoScaleX
									float out_float_1;
									if (float.TryParse(result_next_read_line[2], out out_float_1) == false)
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
									}
									if ((out_float_1 < 0.5) || (out_float_1 > 1.5))
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
									}

									// result_next_read_line[3] = TorsoScaleZ
									float out_float_2;
									if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar - (<" + result_next_read_line[3] + "> detected)");
									}
									if ((out_float_2 < 0.5) || (out_float_2 > 1.5))
									{
										Debug.LogError("[Line " + nb_line + "] - " + configName + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
									}
								}
							}
						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "configure_avatar_tracking") // init command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <configure_avatar_tracking>, followed by an argument representing a text to display in command app, no other following arguments");
							}

							if (advanced_check)
							{
								
							}

						}

						// read next line
						{
							string next_read_line = reader.ReadLine(); nb_line++;
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);


                            if (result_next_read_line.Length != 3)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <torsoscaleX> <torsoscaleZ> <distanceOriginHand>  ");
                            }
                            else if (result_next_read_line.Length == 3)


                            //Check init 1st line arguments
                            {
							// result_next_read_line[2] = TorsoScaleX
							float out_float_1;
							if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the torsoXscale of the avatar (value in m E [0 ; 1]) - (<" + result_next_read_line[0] + "> detected)");
							}
							if ((out_float_1 < 0) || (out_float_1 > 1))
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the torsoXscale of the avatar (value in m E [0 ; 1]) - (<" + result_next_read_line[0] + "> detected)");
							}

							// result_next_read_line[3] = TorsoScaleZ
							float out_float_2;
							if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the torsoZscale of the avatar (value in m E [0 ; 1]) - (<" + result_next_read_line[1] + "> detected)");
							}
							if ((out_float_2 < 0) || (out_float_2 > 1))
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should be of type <float>, representing the torsoZscale of the avatar (value in m E [0 ; 1])  - (<" + result_next_read_line[1] + "> detected)");
							}

							// result_next_read_line[3] = TorsoScaleZ
							float out_float_3;
							if (float.TryParse(result_next_read_line[2], out out_float_3) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should be of type <float>, representing the distance between the origin of the Tracking system and the middle of the wrist/hand  - (<" + result_next_read_line[2] + "> detected)");
							}
						}
					}

					// check for duration of the command
					{
						string next_read_line = reader.ReadLine(); nb_line++; // duration
						char[] separators_next_read_line = new char[] { ' ' };
						string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						int out_int;
						if (result_next_read_line.Length != 1)
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
						}
						else if (result_next_read_line.Length == 1)
						{
							if (int.TryParse(result_next_read_line[0], out out_int) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							}
							f_total_commands_no_time_duration += out_int;
							if (out_int < 0)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							}
						}
					}

					// check for end of command
					{
						string next_read_line = reader.ReadLine(); nb_line++;

						if (next_read_line != "")
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						}
					}
				}
				    else if (result[0] == "question")
				{
					s_last_answer_asked_command = "s_answers_question";

					//Check that configname contains only one argument
					{
						if (result.Length > 2)
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question>, followed by an argument representing a text to display in command app, no other following arguments");
						}

						if (advanced_check)
						{
							if (is_avatar_initialised == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							}

							if (is_MRI_table_inside == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							}
						}

					}

					// read next line
					{
						string next_read_line = reader.ReadLine(); nb_line++;
						char[] separators_next_read_line = new char[] { ' ' };
						string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						if (result_next_read_line.Length != 5)
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 5 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						}
						else if (result_next_read_line.Length == 5)
						{
							//result_next_read_line[0] = question
							//result_next_read_line[1] = text left answer
							//result_next_read_line[2] = text right answer
							//result_next_read_line[3] = Time No Text Before Displaying Question
							//result_next_read_line[4] = Time During Which the selected symbol stays visible
							int out_int;
							if (int.TryParse(result_next_read_line[3], out out_int) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							}

							int out_int2;
							if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							}
						}
					}

					// check for duration of the command
					{
						string next_read_line = reader.ReadLine(); nb_line++; // duration
						char[] separators_next_read_line = new char[] { ' ' };
						string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						int out_int;
						if (result_next_read_line.Length != 1)
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
						}
						else if (result_next_read_line.Length == 1)
						{
							if (int.TryParse(result_next_read_line[0], out out_int) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							}
							f_total_commands_no_time_duration += out_int;
							if (out_int < 0)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							}
						}
					}

					// check for end of command
					{
						string next_read_line = reader.ReadLine(); nb_line++;

						if (next_read_line != "")
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						}
					}
				}
				    else if (result[0] == "question_notime")
				    {
					    s_last_answer_asked_command = "s_answers_question";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 5)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 5 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 5)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }
						    }
					    }

					// check for duration of the command
					{
						string next_read_line = reader.ReadLine(); nb_line++; // duration
						char[] separators_next_read_line = new char[] { ' ' };
						string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						int out_int;
						if (result_next_read_line.Length != 1)
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
						}
						else if (result_next_read_line.Length == 1)
						{
							if (int.TryParse(result_next_read_line[0], out out_int) == false)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							}
							else if (int.Parse(result_next_read_line[0]) != -1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							}
						}
					}

					// check for end of command
					{
						string next_read_line = reader.ReadLine(); nb_line++;

						if (next_read_line != "")
						{
							Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						}
					}
				}
				    else if (result[0] == "question_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_question";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 5)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 5 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 5)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_with_TV")
				    {
					    s_last_answer_asked_command = "s_answers_question_with_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_with_TV>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_with_TV_notime")
				    {
					    s_last_answer_asked_command = "s_answers_question_with_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_with_TV_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }

						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_with_TV_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_question_with_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_with_TV_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_after_TV")
				    {
					    s_last_answer_asked_command = "s_answers_question_after_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_after_TV>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }
							    if (out_int <= 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be > 0, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_after_TV_notime")
				    {
					    s_last_answer_asked_command = "s_answers_question_after_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_after_TV_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }
							    if (out_int <= 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be > 0, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "question_after_TV_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_question_after_TV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <question_after_TV_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    if (result_next_read_line.Length != 6)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <question> <text_left_answer> <text_right_answer> <Time_No_Text_Before_Displaying_Question_> <Time_During_Which_the_selected_symbol_stays_visible> <path_displayed_picture>. Use <_> to make space. In the displayed question, <_> of string will be replaced by < > and <@> by a returnline <>");
						    }
						    else if (result_next_read_line.Length == 6)
						    {
							    //result_next_read_line[0] = question
							    //result_next_read_line[1] = text left answer
							    //result_next_read_line[2] = text right answer
							    //result_next_read_line[3] = Time No Text Before Displaying Question
							    //result_next_read_line[4] = Time During Which the selected symbol stays visible
							    //result_next_read_line[5] = Path Displayed picture
							    int out_int;
							    if (int.TryParse(result_next_read_line[3], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }
							    if (out_int <= 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be > 0, representing the Time No Text Before Displaying Question - (<" + result_next_read_line[3] + "> detected)");
							    }

							    int out_int2;
							    if (int.TryParse(result_next_read_line[4], out out_int2) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <int>, representing the Time During Which the selected symbol stays visible - (<" + result_next_read_line[4] + "> detected)");
							    }

							    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 6th argument : (<" + result_next_read_line[5] + "> detected)");
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "text_body_part")
				    {
					    s_last_answer_asked_command = "s_answers_text_body_part";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <text_body_part>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check task 1st line arguments
						    {
							    if (result_next_read_line.Length != 3)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <text>");
							    }
							    else if (result_next_read_line.Length == 3)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = text
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "text_body_part_notime")
				    {
					    s_last_answer_asked_command = "s_answers_text_body_part";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <text_body_part_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <text_body_part_notime> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check task 1st line arguments
						    {
							    if (result_next_read_line.Length != 3)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <text>");
							    }
							    else if (result_next_read_line.Length == 3)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = text
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "text_body_part_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_text_body_part";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <text_body_part_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check task 1st line arguments
						    {
							    if (result_next_read_line.Length != 3)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <text>");
							    }
							    else if (result_next_read_line.Length == 3)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = text
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "continuousscale")
				    {
					    s_last_answer_asked_command = "s_answers_continuousscale";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <continuousscale>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 7)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <speed> <textleftmin> <textrightmax> <text> <auto_or_manual>");
							    }
							    else if (result_next_read_line.Length == 7)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = speed
								    float out_float;
								    if (float.TryParse(result_next_read_line[2], out out_float) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale (value E [0 ; +oo[) - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if (out_float < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale (value E [0 ; +oo[) - (<" + result_next_read_line[2] + "> detected)");
								    }

                                    if ((result_next_read_line[6] != "auto") && (result_next_read_line[6] != "manual"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <string>, representing the constinuous scale mode (<auto> for automatic move, <manual> to control sliedr with keyboard) - (<" + result_next_read_line[6] + "> detected)");
                                    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "continuousscale_notime")
				    {
					    s_last_answer_asked_command = "s_answers_continuousscale";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <continuousscale_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 7)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <speed> <textleftmin> <textrightmax> <text> <auto_or_manual>");
							    }
							    else if (result_next_read_line.Length == 7)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = speed
								    float out_float;
								    if (float.TryParse(result_next_read_line[2], out out_float) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if (out_float < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale (value E [0 ; +oo[) - (<" + result_next_read_line[2] + "> detected)");
								    }

                                    if ((result_next_read_line[6] != "auto") && (result_next_read_line[6] != "manual"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <string>, representing the constinuous scale mode (<auto> for automatic move, <manual> to control sliedr with keyboard) - (<" + result_next_read_line[6] + "> detected)");
                                    }
                                }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "continuousscale_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_continuousscale";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <continuousscale_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 7)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 6 arguments (" + result_next_read_line.Length + " arguments detected) - format : <position> <color> <speed> <textleftmin> <textrightmax> <text> <auto_or_manual>");
							    }
							    else if (result_next_read_line.Length == 7)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "LeftKnee") && (result_next_read_line[0] != "RightKnee") && (result_next_read_line[0] != "LeftHand") && (result_next_read_line[0] != "RightHand") && (result_next_read_line[0] != "Hips") && (result_next_read_line[0] != "Other"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the position : <LeftKnee> or <RightKnee> or <LeftHand> or <RightHand> or <Hips> or <Other> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    if ((result_next_read_line[1] != "clear") && (result_next_read_line[1] != "red") && (result_next_read_line[1] != "yellow") && (result_next_read_line[1] != "green") && (result_next_read_line[1] != "blue")
									     && (result_next_read_line[1] != "black") && (result_next_read_line[1] != "white") && (result_next_read_line[1] != "cyan") && (result_next_read_line[1] != "gray") && (result_next_read_line[1] != "grey")
									      && (result_next_read_line[1] != "magenta"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <clear> or <red> or <yellow> or <green> or <blue> or <black> or <white> or <cyan> or <gray> or <grey> or <magenta> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = speed
								    float out_float;
								    if (float.TryParse(result_next_read_line[2], out out_float) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if (out_float < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the speed of the cursor on the displayed constinuous scale (value E [0 ; +oo[) - (<" + result_next_read_line[2] + "> detected)");
								    }

                                    if ((result_next_read_line[6] != "auto") && (result_next_read_line[6] != "manual"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <string>, representing the constinuous scale mode (<auto> for automatic move, <manual> to control sliedr with keyboard) - (<" + result_next_read_line[6] + "> detected)");
                                    }
                                }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "change_skin")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <change_skin>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <skin>");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "male") && (result_next_read_line[0] != "female") && (result_next_read_line[0] != "box") && (result_next_read_line[0] != "none"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should correspond to the skin : <male> or <female> or <box> or <none> - (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "set_MRI_length_rb")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_MRI_length_rb>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    /*if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }*/

							    if (is_MRI_table_inside == true)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called after seting the length of the mri moving table displacement");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <length of the mri moving table displacement (float)>");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <float>, representing the lengh of the MRI moving table displacement (for tracked rb command) (value E [0 ; +oo[) - (<" + result_next_read_line[0] + "> detected)");
								    }
								    if ((out_float_1 < 0))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <float>, , representing the lengh of the MRI moving table displacement (for tracked rb command) (value E [0 ; +oo[) - (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "activate_TV")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <activate_TV>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    // advanced_check is present on next line parsing too
						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check activate_TVstate (1st line argument)
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <bool>");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "true") && (result_next_read_line[0] != "false"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should correspond to a bool : <true> to activate the TV or <false> to desactivate the TV - (<" + result_next_read_line[0] + "> detected)");
								    }

								    else if (advanced_check)
								    {
									    if ((result_next_read_line[0] == "true"))
									    {
										    if (is_TV_displayed == true)
										    {
											    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> command with <true> parameter has already been called. TV is currently activated");
										    }
										    else if (is_TV_displayed == false)
										    {
											    is_TV_displayed = true;
										    }
									    }
									    else if ((result_next_read_line[0] == "false"))
									    {
										    if (is_TV_displayed == false)
										    {
											    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> command with <false> parameter has already been called. TV is currently desactivated");
										    }
										    else if (is_TV_displayed == true)
										    {
											    is_TV_displayed = false;
										    }
									    }
								    }

							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "activate_tennis_balls")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <activate_tennis_balls>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    // advanced_check is present on next line parsing too
						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check activate_TVstate (1st line argument)
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <bool>");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    // result_next_read_line[0] = position
								    if ((result_next_read_line[0] != "true") && (result_next_read_line[0] != "false"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should correspond to a bool : <true> to activate the tennis balls mesh renderer or <false> to desactivate the tennis balls mesh renderer - (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "set_display_TV_notime")
				    {
					    s_last_answer_asked_command = "s_answers_displayTV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <string> - path of the image to be displayed");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[0]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "set_display_TV_bypasstime")
				    {
					    s_last_answer_asked_command = "s_answers_displayTV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <string> - path of the image to be displayed");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[0]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line should contain one argument of type <int> corresponding to the duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "set_display_TV")
				    {
					    s_last_answer_asked_command = "s_answers_displayTV";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <string> - path of the image to be displayed");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[0]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
                    else if (result[0] == "set_display_TV_video")
                    {                    
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV_video>, followed by an argument representing a text to display in command app, no other following arguments");
                            }

                            if (advanced_check)
                            {
                                if (is_avatar_initialised == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
                                }

                                if (is_MRI_table_inside == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
                                }

                                if (is_TV_displayed == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
                                }
                            }

                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 1)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <string> - path of the video to be played");
                                }
                                else if (result_next_read_line.Length == 1)
                                {
                                    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[0]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "VIDEO NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[0] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        /*{
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }*/

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "set_display_TV_only_notime")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV_only_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <string> - path of the image to be displayed");
							    }
							    else if (result_next_read_line.Length == 1)
							    {
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[0]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[0] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "mri_table_rb_tracked")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <mri_table_rb_tracked>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "pause")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <pause>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "pause_wait_for_MRI_pulse")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <pause_wait_for_MRI_pulse>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "idle") // move_outside command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <idle>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "fade_camera_to_black_screen") // move_outside command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <fade_camera_to_black_screen>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "fade_black_screen_to_camera") // move_outside command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <fade_black_screen_to_camera>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "comment") // move_outside command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <comment>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // Comment
					    {
						    reader.ReadLine(); nb_line++; // comment
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "change_camera_settings") // change camera settings command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <change_camera_settings>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 3)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <clear_flag> <culling_mask> <background_color>");
							    }
							    else if (result_next_read_line.Length == 3)
							    {
								    // result_next_read_line[0] = clear_flag
								    if ((result_next_read_line[0] != "Skybox") && (result_next_read_line[0] != "Color") && (result_next_read_line[0] != "Depth") && (result_next_read_line[0] != "Nothing"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to the clear_flag : <Skybox> or <Color> or <Depth> or <Nothing> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = culling_mask
								    if ((result_next_read_line[1] != "Everything") && (result_next_read_line[1] != "Nothing"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to culling_mask : <Everything> or <Nothing> - (<" + result_next_read_line[1] + "> detected)");
								    }


								    if (result_next_read_line[2].Length != 7)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should correspond to background_color, in a HEX format, that means a # followed by 6 characters. ex : <#000000> for black or <#FFFFFF> for white - (<" + result_next_read_line[2] + "> detected)");
								    }
								    else if (result_next_read_line[2].Length == 7)
								    {
									    if (result_next_read_line[2][0] != '#')
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should correspond to background_color, in a HEX format, that means a # followed by 6 characters. ex : <#000000> for black or <#FFFFFF> for white - No # character detected (<" + result_next_read_line[2] + "> detected)");
									    }
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "NPC_OBT_bypasstime") // init command
				    {
					    s_last_answer_asked_command = "s_answers_NPC_OBT";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <NPC_OBT_bypasstime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 9)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 9 arguments (" + result_next_read_line.Length + " arguments detected) - format : <sex> <color> <torsoscaleX> <torsoscaleZ> <AvatarUpAxisRotation> <AvatarPartToHighlight> <highlight_color> <highlight_intensity_transparency> <highlight_mode>");
							    }
							    else if (result_next_read_line.Length == 9)
							    {
								    // result_next_read_line[0] = sex
								    if ((result_next_read_line[0] != "Male") && (result_next_read_line[0] != "Female"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to sex : <Male> or <Female> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    string Hex = result_next_read_line[1];
								    Color c;
								    bool result_hex_color_parsing = false;
								    if (Hex[0] == '#')
								    {
									    if (Hex.Length == 7)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
								    }
								    else
								    {
									    if (Hex.Length == 6)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
								    }
								    if (!result_hex_color_parsing)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = TorsoScaleX
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[2], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if ((out_float_1 < 0.5) || (out_float_1 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }

								    // result_next_read_line[3] = TorsoScaleZ
								    float out_float_2;
								    if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }
								    if ((out_float_2 < 0.5) || (out_float_2 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }

								    // result_next_read_line[4] = AvatarUpAxisRotation
								    float out_float_3;
								    if (float.TryParse(result_next_read_line[4], out out_float_3) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }
								    if ((out_float_3 <= -180) || (out_float_3 >= 360))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }

								    // result_next_read_line[5] = PartToHighlight
								    if ((result_next_read_line[5] != "none") && (result_next_read_line[5] != "right_hand") && (result_next_read_line[5] != "left_hand"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 6th argument should correspond to the part of the avatar to highlight : <none> or <right_hand> or <left_hand> - (<" + result_next_read_line[5] + "> detected)");
								    }

								    // result_next_read_line[6] = lights_color
								    string Hex_1 = result_next_read_line[6];
								    Color c_1;
								    bool result_hex_color_parsing_1 = false;
								    if (Hex_1[0] == '#')
								    {
									    if (Hex_1.Length == 7)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString(Hex_1, out c_1);
								    }
								    else
								    {
									    if (Hex_1.Length == 6)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString("#" + Hex_1, out c_1);
								    }
								    if (!result_hex_color_parsing_1)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 7th argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters), representing the highlight lights/bracelet color : Example <> - (<" + result_next_read_line[6] + "> detected)");
								    }

								    // result_next_read_line[7] = lights_intensity
								    float out_float_4;
								    if (float.TryParse(result_next_read_line[7], out out_float_4) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity - (<" + result_next_read_line[7] + "> detected)");
								    }
								    if (out_float_4 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity (value E [0 ; +oo[) or the bracelet transparency (value E [0 ; 1])  - (<" + result_next_read_line[7] + "> detected)");
								    }

								    // result_next_read_line[8] = mode
								    if ((result_next_read_line[8] != "light_mode") && (result_next_read_line[8] != "bracelet_mode"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 9th argument should be <light_mode> or <bracelet_mode>, representing the mode of hand highlight - (<" + result_next_read_line[8] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "NPC_OBT") // init command
				    {
					    s_last_answer_asked_command = "s_answers_NPC_OBT";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <NPC_OBT>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 9)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 9 arguments (" + result_next_read_line.Length + " arguments detected) - format : <sex> <color> <torsoscaleX> <torsoscaleZ> <AvatarUpAxisRotation> <AvatarPartToHighlight> <highlight_color> <highlight_intensity_transparency> <highlight_mode>");
							    }
							    else if (result_next_read_line.Length == 9)
							    {
								    // result_next_read_line[0] = sex
								    if ((result_next_read_line[0] != "Male") && (result_next_read_line[0] != "Female"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to sex : <Male> or <Female> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    /*if ((result_next_read_line[1] != "3C2E28") && (result_next_read_line[1] != "4B3932") && (result_next_read_line[1] != "5A453C") && (result_next_read_line[1] != "695046") && (result_next_read_line[1] != "785C50")
									     && (result_next_read_line[1] != "87675A") && (result_next_read_line[1] != "967264") && (result_next_read_line[1] != "A57E6E") && (result_next_read_line[1] != "B48A78") && (result_next_read_line[1] != "C39582")
									      && (result_next_read_line[1] != "D2A18C") && (result_next_read_line[1] != "E1AC96") && (result_next_read_line[1] != "F0B8A0") && (result_next_read_line[1] != "FFC3AA") && (result_next_read_line[1] != "FFCEB4")
									       && (result_next_read_line[1] != "FFDABE") && (result_next_read_line[1] != "FFE5C8"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <3C2E28> or <4B3932> or <5A453C> or <695046> or <785C50> or <87675A> or <967264> or <A57E6E> or <B48A78> or <C39582> or <D2A18C> or <E1AC96> or <F0B8A0> or <FFC3AA> or <FFCEB4> or <FFDABE> or <FFE5C8> - (<" + result_next_read_line[1] + "> detected)");
								    }*/

								    // result_next_read_line[1] = color
								    string Hex = result_next_read_line[1];
								    Color c;
								    bool result_hex_color_parsing = false;
								    if (Hex[0] == '#')
								    {
									    if (Hex.Length == 7)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
								    }
								    else
								    {
									    if (Hex.Length == 6)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
								    }
								    if (!result_hex_color_parsing)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = TorsoScaleX
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[2], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if ((out_float_1 < 0.5) || (out_float_1 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }

								    // result_next_read_line[3] = TorsoScaleZ
								    float out_float_2;
								    if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }
								    if ((out_float_2 < 0.5) || (out_float_2 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }

								    // result_next_read_line[4] = AvatarUpAxisRotation
								    float out_float_3;
								    if (float.TryParse(result_next_read_line[4], out out_float_3) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }
								    if ((out_float_3 <= -180) || (out_float_3 >= 360))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }

								    // result_next_read_line[5] = PartToHighlight
								    if ((result_next_read_line[5] != "none") && (result_next_read_line[5] != "right_hand") && (result_next_read_line[5] != "left_hand"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 6th argument should correspond to the part of the avatar to highlight : <none> or <right_hand> or <left_hand> - (<" + result_next_read_line[5] + "> detected)");
								    }

								    // result_next_read_line[6] = lights_color
								    string Hex_1 = result_next_read_line[6];
								    Color c_1;
								    bool result_hex_color_parsing_1 = false;
								    if (Hex_1[0] == '#')
								    {
									    if (Hex_1.Length == 7)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString(Hex_1, out c_1);
								    }
								    else
								    {
									    if (Hex_1.Length == 6)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString("#" + Hex_1, out c_1);
								    }
								    if (!result_hex_color_parsing_1)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 7th argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters), representing the highlight lights/bracelet color : Example <> - (<" + result_next_read_line[6] + "> detected)");
								    }

								    // result_next_read_line[7] = lights_intensity
								    float out_float_4;
								    if (float.TryParse(result_next_read_line[7], out out_float_4) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity - (<" + result_next_read_line[7] + "> detected)");
								    }
								    if (out_float_4 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity (value E [0 ; +oo[) or the bracelet transparency (value E [0 ; 1])  - (<" + result_next_read_line[7] + "> detected)");
								    }

								    // result_next_read_line[8] = mode
								    if ((result_next_read_line[8] != "light_mode") && (result_next_read_line[8] != "bracelet_mode"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 9th argument should be <light_mode> or <bracelet_mode>, representing the mode of hand highlight - (<" + result_next_read_line[8] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "NPC_OBT_notime") // init command
				    {
					    s_last_answer_asked_command = "s_answers_NPC_OBT";

					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <NPC_OBT_notime>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 9)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 9 arguments (" + result_next_read_line.Length + " arguments detected) - format : <sex> <color> <torsoscaleX> <torsoscaleZ> <AvatarUpAxisRotation> <AvatarPartToHighlight> <highlight_color> <highlight_intensity_transparency> <highlight_mode>");
							    }
							    else if (result_next_read_line.Length == 9)
							    {
								    // result_next_read_line[0] = sex
								    if ((result_next_read_line[0] != "Male") && (result_next_read_line[0] != "Female"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to sex : <Male> or <Female> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = color
								    /*if ((result_next_read_line[1] != "3C2E28") && (result_next_read_line[1] != "4B3932") && (result_next_read_line[1] != "5A453C") && (result_next_read_line[1] != "695046") && (result_next_read_line[1] != "785C50")
									     && (result_next_read_line[1] != "87675A") && (result_next_read_line[1] != "967264") && (result_next_read_line[1] != "A57E6E") && (result_next_read_line[1] != "B48A78") && (result_next_read_line[1] != "C39582")
									      && (result_next_read_line[1] != "D2A18C") && (result_next_read_line[1] != "E1AC96") && (result_next_read_line[1] != "F0B8A0") && (result_next_read_line[1] != "FFC3AA") && (result_next_read_line[1] != "FFCEB4")
									       && (result_next_read_line[1] != "FFDABE") && (result_next_read_line[1] != "FFE5C8"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color : <3C2E28> or <4B3932> or <5A453C> or <695046> or <785C50> or <87675A> or <967264> or <A57E6E> or <B48A78> or <C39582> or <D2A18C> or <E1AC96> or <F0B8A0> or <FFC3AA> or <FFCEB4> or <FFDABE> or <FFE5C8> - (<" + result_next_read_line[1] + "> detected)");
								    }*/

								    // result_next_read_line[1] = color
								    string Hex = result_next_read_line[1];
								    Color c;
								    bool result_hex_color_parsing = false;
								    if (Hex[0] == '#')
								    {
									    if (Hex.Length == 7)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
								    }
								    else
								    {
									    if (Hex.Length == 6)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
								    }
								    if (!result_hex_color_parsing)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 2nd argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = TorsoScaleX
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[2], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }
								    if ((out_float_1 < 0.5) || (out_float_1 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the torsoXscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[2] + "> detected)");
								    }

								    // result_next_read_line[3] = TorsoScaleZ
								    float out_float_2;
								    if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }
								    if ((out_float_2 < 0.5) || (out_float_2 > 1.5))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the torsoZscale of the avatar (value E [0.5 ; 1.5]) - (<" + result_next_read_line[3] + "> detected)");
								    }

								    // result_next_read_line[4] = AvatarUpAxisRotation
								    float out_float_3;
								    if (float.TryParse(result_next_read_line[4], out out_float_3) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }
								    if ((out_float_3 <= -180) || (out_float_3 >= 360))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 5th argument should be of type <float>, representing the avatur up axis rotation (value E ]-180 ; 180] or [0 ; 360[) - (<" + result_next_read_line[4] + "> detected)");
								    }

								    // result_next_read_line[5] = PartToHighlight
								    if ((result_next_read_line[5] != "none") && (result_next_read_line[5] != "right_hand") && (result_next_read_line[5] != "left_hand"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 6th argument should correspond to the part of the avatar to highlight : <none> or <right_hand> or <left_hand> - (<" + result_next_read_line[5] + "> detected)");
								    }

								    // result_next_read_line[6] = lights_color
								    string Hex_1 = result_next_read_line[6];
								    Color c_1;
								    bool result_hex_color_parsing_1 = false;
								    if (Hex_1[0] == '#')
								    {
									    if (Hex_1.Length == 7)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString(Hex_1, out c_1);
								    }
								    else
								    {
									    if (Hex_1.Length == 6)
										    result_hex_color_parsing_1 = ColorUtility.TryParseHtmlString("#" + Hex_1, out c_1);
								    }
								    if (!result_hex_color_parsing_1)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 7th argument should correspond to color in Hex format (a # (not obligatory) followed by 6 characters), representing the highlight lights/bracelet color : Example <> - (<" + result_next_read_line[6] + "> detected)");
								    }

								    // result_next_read_line[7] = lights_intensity
								    float out_float_4;
								    if (float.TryParse(result_next_read_line[7], out out_float_4) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity - (<" + result_next_read_line[7] + "> detected)");
								    }
								    if (out_float_4 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 8th argument should be of type <float>, representing the highlight lights intensity (value E [0 ; +oo[) or the bracelet transparency (value E [0 ; 1])  - (<" + result_next_read_line[7] + "> detected)");
								    }

								    // result_next_read_line[8] = mode
								    if ((result_next_read_line[8] != "light_mode") && (result_next_read_line[8] != "bracelet_mode"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 9th argument should be <light_mode> or <bracelet_mode>, representing the mode of hand highlight - (<" + result_next_read_line[8] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
							    else if (int.Parse(result_next_read_line[0]) != -1)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be <-1>, representing the wanted duration of the command - here : <-1> cause notime");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "set_display_TV_feedback_last_command_no_input")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_display_TV_feedback_last_command_no_input>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_avatar_initialised == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<init> command should be called before any other experience avatar related command");
							    }

							    if (is_MRI_table_inside == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_inside> command should be called at the begining, before <move_inside> command");
							    }

							    if (is_TV_displayed == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <true> parameter should be called before this command. No pictures will be displayed here");
							    }
						    }

					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 3)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 argument (" + result_next_read_line.Length + " arguments detected) - format : <waited_correct_previous_answer> <path_picture_correct_ answer> <path_picture_incorrect_answer>");
							    }
							    else if (result_next_read_line.Length == 3)
							    {
								    // result_next_read_line[0] = value of the correct previous answer waited

								    string[] s_answers_NPC_OBT = new string[] { "NPC_OBT_answer_none", "NPC_OBT_answer_1", "NPC_OBT_answer_2", "NPC_OBT_answer_3", "NPC_OBT_answer_4", "NPC_OBT_answer_5", "NPC_OBT_answer_6" };
								    string[] s_answers_continuousscale = new string[] { "continuousscale_answer_none", "continuousscale_answer_1", "continuousscale_answer_2", "continuousscale_answer_3", "continuousscale_answer_4", "continuousscale_answer_5", "continuousscale_answer_6" };
								    string[] s_answers_text_body_part = new string[] { "text_body_part_answer_none", "text_body_part_answer_1", "text_body_part_answer_2", "text_body_part_answer_3", "text_body_part_answer_4", "text_body_part_answer_5", "text_body_part_answer_6" };
								    string[] s_answers_displayTV = new string[] { "displayTV_answer_none", "displayTV_answer_1", "displayTV_answer_2", "displayTV_answer_3", "displayTV_answer_4", "displayTV_answer_5", "displayTV_answer_6" };
								    string[] s_answers_question = new string[] { "question_answer_none", "question_answer_left", "question_answer_right" };
								    string[] s_answers_question_with_TV = new string[] { "question_with_TV_answer_none", "question_with_TV_answer_left", "question_with_TV_answer_right" };
								    string[] s_answers_question_after_TV = new string[] { "question_after_TV_answer_none", "question_after_TV_answer_left", "question_after_TV_answer_right" };

								    //string[][] s_answers = new string[][] { s_answers_NPC_OBT, s_answers_continuousscale, s_answers_text_body_part, s_answers_displayTV, s_answers_question, s_answers_question_with_TV, s_answers_question_after_TV };


								    bool does_answer_exists = false;
								    if (s_last_answer_asked_command == "s_answers_NPC_OBT")
								    {
									    for (int a = 0; a < s_answers_NPC_OBT.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_NPC_OBT[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <NPC_OBT_answer_none>, <NPC_OBT_answer_1>, <NPC_OBT_answer_2>, <NPC_OBT_answer_3>, <NPC_OBT_answer_4>, <NPC_OBT_answer_5>, <NPC_OBT_answer_6>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_continuousscale")
								    {
									    for (int a = 0; a < s_answers_continuousscale.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_continuousscale[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <continuousscale_answer_none>, <continuousscale_answer_1>, <continuousscale_answer_2>, <continuousscale_answer_3>, <continuousscale_answer_4>, <continuousscale_answer_5>, <continuousscale_answer_6>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_text_body_part")
								    {
									    for (int a = 0; a < s_answers_text_body_part.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_text_body_part[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <text_body_part_answer_none>, <text_body_part_answer_1>, <text_body_part_answer_2>, <text_body_part_answer_3>, <text_body_part_answer_4>, <text_body_part_answer_5>, <text_body_part_answer_6>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_displayTV")
								    {
									    for (int a = 0; a < s_answers_displayTV.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_displayTV[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <displayTV_answer_none>, <displayTV_answer_1>, <displayTV_answer_2>, <displayTV_answer_3>, <displayTV_answer_4>, <displayTV_answer_5>, <displayTV_answer_6>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_question")
								    {
									    for (int a = 0; a < s_answers_question.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_question[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <question_answer_none>, <question_answer_left>, <question_answer_right>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_question_with_TV")
								    {
									    for (int a = 0; a < s_answers_question_with_TV.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_question_with_TV[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <question_with_TV_answer_none>, <question_with_TV_answer_left>, <question_with_TV_answer_right>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else if (s_last_answer_asked_command == "s_answers_question_after_TV")
								    {
									    for (int a = 0; a < s_answers_question_after_TV.Length; a++)
									    {
										    if (result_next_read_line[0] == s_answers_question_after_TV[a])
										    {
											    does_answer_exists = true;
										    }
									    }

									    if (!does_answer_exists)
									    {
										    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + "> - possibilities : <question_after_TV_answer_none>, <question_after_TV_answer_left>, <question_after_TV_answer_right>" + " - <" + result_next_read_line[0] + "> detected");
									    }
								    }
								    else
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of the type of the last command asking an anwer <" + s_last_answer_asked_command + ">");
								    }


								    /*for (int i = 0; i < s_answers.Length; i++)
								    {
									    string[] innerArray = s_answers[i];
									    for (int a = 0; a < innerArray.Length; a++)
									    {
										    Console.Write(innerArray[a] + " ");
									    }
									    Console.WriteLine();
								    }*/

								    //NPC_OBT, continuousscale, text_body_part, displayTV, question, question_with_TV, question_after_TV

								    // result_next_read_line[1] = path of picture corresponding to correct answer
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[1]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 2nd argument : (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = path of picture corresponding to incorrect answer
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[2]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - 3rd argument : (<" + result_next_read_line[2] + "> detected)");
								    }

							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "solve_object") // solve_object command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <solve_object>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 7)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 7 arguments (" + result_next_read_line.Length + " arguments detected) - format : <GO_name> <value_BurnSize> <hex_BurnColor> <value_EmissionAmount> <path_texture_dissolve_noise> <path_texture_burn_ramp> <effect_duration>");
							    }
							    else if (result_next_read_line.Length == 7)
							    {
								    // result_next_read_line[0] = GO_name
								    if ((result_next_read_line[0] != "mri_magnet") &&
								    (result_next_read_line[0] != "mri_table") &&
								    (result_next_read_line[0] != "mri_movingtable") &&
								    (result_next_read_line[0] != "roomamdothers_ceiling") &&
								    (result_next_read_line[0] != "roomamdothers_chariot") &&
								    (result_next_read_line[0] != "roomamdothers_chariot001") &&
								    (result_next_read_line[0] != "roomamdothers_chariot002") &&
								    (result_next_read_line[0] != "roomamdothers_floor") &&
								    (result_next_read_line[0] != "roomamdothers_frontwalls") &&
								    (result_next_read_line[0] != "roomamdothers_sidewalls"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to GO_name : <mri_magnet> or <mri_table> or <mri_movingtable> or <roomamdothers_ceiling> or <roomamdothers_chariot> or <roomamdothers_chariot001> or <roomamdothers_chariot002> or <roomamdothers_floor> or <roomamdothers_frontwalls> or <roomamdothers_sidewalls> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = value_BurnSize
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[1], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_BurnSize (value E [0 ; 1]) - (<" + result_next_read_line[1] + "> detected)");
								    }
								    if ((out_float_1 < 0) || (out_float_1 > 1))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_BurnSize (value E [0 ; 1]) - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = hex_BurnColor
								    string Hex = result_next_read_line[2];
								    Color c;
								    bool result_hex_color_parsing = false;
								    if (Hex[0] == '#')
								    {
									    if (Hex.Length == 7)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
								    }
								    else
								    {
									    if (Hex.Length == 6)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
								    }
								    if (!result_hex_color_parsing)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should correspond to the burn color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[2] + "> detected)");
								    }

								    // result_next_read_line[3] = value_EmissionAmount
								    float out_float_2;
								    if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the value_EmissionAmount (value E [0 ; +oo[) - (<" + result_next_read_line[3] + "> detected)");
								    }
								    if (out_float_2 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the value_EmissionAmount (value E [0 ; +oo[) - (<" + result_next_read_line[3] + "> detected)");
								    }

								    // result_next_read_line[4] = path_texture_dissolve_noise
								    // result_next_read_line[5] = path_texture_burn_ramp
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[4]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[4] + "> detected)");
								    }
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[5] + "> detected)");
								    }

								    // result_next_read_line[6] = effect_duration
								    int out_int_1;
								    if (int.TryParse(result_next_read_line[6], out out_int_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 7th argument should be of type <int>, representing the effect_duration in ms - (<" + result_next_read_line[6] + "> detected)");
								    }
								    if (out_int_1 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 7th argument should be of type <int>, representing the effect_duration in ms (>= 0) - (<" + result_next_read_line[6] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
				    else if (result[0] == "dissolve_object") // dissolve_object command
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <dissolve_object>, followed by an argument representing a text to display in command app, no other following arguments");
						    }
					    }

					    // read next line
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    //Check 1st line arguments
						    {
							    if (result_next_read_line.Length != 7)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 7 arguments (" + result_next_read_line.Length + " arguments detected) - format : <GO_name> <value_BurnSize> <hex_BurnColor> <value_EmissionAmount> <path_texture_dissolve_noise> <path_texture_burn_ramp> <effect_duration>");
							    }
							    else if (result_next_read_line.Length == 7)
							    {
								    // result_next_read_line[0] = GO_name
								    if ((result_next_read_line[0] != "mri_magnet") &&
								    (result_next_read_line[0] != "mri_table") &&
								    (result_next_read_line[0] != "mri_movingtable") &&
								    (result_next_read_line[0] != "roomamdothers_ceiling") &&
								    (result_next_read_line[0] != "roomamdothers_chariot") &&
								    (result_next_read_line[0] != "roomamdothers_chariot001") &&
								    (result_next_read_line[0] != "roomamdothers_chariot002") &&
								    (result_next_read_line[0] != "roomamdothers_floor") &&
								    (result_next_read_line[0] != "roomamdothers_frontwalls") &&
								    (result_next_read_line[0] != "roomamdothers_sidewalls"))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to GO_name : <mri_magnet> or <mri_table> or <mri_movingtable> or <roomamdothers_ceiling> or <roomamdothers_chariot> or <roomamdothers_chariot001> or <roomamdothers_chariot002> or <roomamdothers_floor> or <roomamdothers_frontwalls> or <roomamdothers_sidewalls> - (<" + result_next_read_line[0] + "> detected)");
								    }

								    // result_next_read_line[1] = value_BurnSize
								    float out_float_1;
								    if (float.TryParse(result_next_read_line[1], out out_float_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_BurnSize (value E [0 ; 1]) - (<" + result_next_read_line[1] + "> detected)");
								    }
								    if ((out_float_1 < 0) || (out_float_1 > 1))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_BurnSize (value E [0 ; 1]) - (<" + result_next_read_line[1] + "> detected)");
								    }

								    // result_next_read_line[2] = hex_BurnColor
								    string Hex = result_next_read_line[2];
								    Color c;
								    bool result_hex_color_parsing = false;
								    if (Hex[0] == '#')
								    {
									    if (Hex.Length == 7)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString(Hex, out c);
								    }
								    else
								    {
									    if (Hex.Length == 6)
										    result_hex_color_parsing = ColorUtility.TryParseHtmlString("#" + Hex, out c);
								    }
								    if (!result_hex_color_parsing)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " : 3rd argument should correspond to the burn color in Hex format (a # (not obligatory) followed by 6 characters) : Example <> - (<" + result_next_read_line[2] + "> detected)");
								    }

								    // result_next_read_line[3] = value_EmissionAmount
								    float out_float_2;
								    if (float.TryParse(result_next_read_line[3], out out_float_2) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the value_EmissionAmount (value E [0 ; +oo[) - (<" + result_next_read_line[3] + "> detected)");
								    }
								    if (out_float_2 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the value_EmissionAmount (value E [0 ; +oo[) - (<" + result_next_read_line[3] + "> detected)");
								    }

								    // result_next_read_line[4] = path_texture_dissolve_noise
								    // result_next_read_line[5] = path_texture_burn_ramp
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[4]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[4] + "> detected)");
								    }
								    if (!System.IO.File.Exists(s_experiment_file_location + "//" + result_next_read_line[5]))
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "PICTURE NOT FOUND AT GIVEN PATH - argument : (<" + result_next_read_line[5] + "> detected)");
								    }

								    // result_next_read_line[6] = effect_duration
								    int out_int_1;
								    if (int.TryParse(result_next_read_line[6], out out_int_1) == false)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 7th argument should be of type <int>, representing the effect_duration in ms - (<" + result_next_read_line[6] + "> detected)");
								    }
								    if (out_int_1 < 0)
								    {
									    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 7th argument should be of type <int>, representing the effect_duration in ms (>= 0) - (<" + result_next_read_line[6] + "> detected)");
								    }
							    }
						    }
					    }

					    // check for duration of the command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++; // duration
						    char[] separators_next_read_line = new char[] { ' ' };
						    string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

						    int out_int;
						    if (result_next_read_line.Length != 1)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
						    }
						    else if (result_next_read_line.Length == 1)
						    {
							    if (int.TryParse(result_next_read_line[0], out out_int) == false)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
							    f_total_commands_no_time_duration += out_int;
							    if (out_int < 0)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
							    }
						    }
					    }

					    // check for end of command
					    {
						    string next_read_line = reader.ReadLine(); nb_line++;

						    if (next_read_line != "")
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
						    }
					    }
				    }
                    else if (result[0] == "fade_object_to_transparent") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <fade_object_to_transparent>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 4)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 4 arguments (" + result_next_read_line.Length + " arguments detected) - format : <GO_name> <value_min_alpha> <value_max_alpha> <effect_duration>");
                                }
                                else if (result_next_read_line.Length == 4)
                                {
                                    // result_next_read_line[0] = GO_name
                                    if ((result_next_read_line[0] != "mri_magnet") &&
                                    (result_next_read_line[0] != "mri_table") &&
                                    (result_next_read_line[0] != "mri_movingtable") &&
                                    (result_next_read_line[0] != "roomamdothers_ceiling") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot001") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot002") &&
                                    (result_next_read_line[0] != "roomamdothers_floor") &&
                                    (result_next_read_line[0] != "roomamdothers_frontwalls") &&
                                    (result_next_read_line[0] != "roomamdothers_sidewalls"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to GO_name : <mri_magnet> or <mri_table> or <mri_movingtable> or <roomamdothers_ceiling> or <roomamdothers_chariot> or <roomamdothers_chariot001> or <roomamdothers_chariot002> or <roomamdothers_floor> or <roomamdothers_frontwalls> or <roomamdothers_sidewalls> - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    // result_next_read_line[1] = value_BurnSize
                                    float out_float_1;
                                    if (float.TryParse(result_next_read_line[1], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_min_alpha (value E [0 ; 1] - default : 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                    if ((out_float_1 < 0) || (out_float_1 > 1))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_max alpha (value E [0 ; 1] - default : 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }

                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[2], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the value_min_alpha (value E [0 ; 1] - default : 1) - (<" + result_next_read_line[2] + "> detected)");
                                    }
                                    if ((out_float_1 < 0) || (out_float_1 > 1))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the value_max_alpha (value E [0 ; 1] - default : 1) - (<" + result_next_read_line[2] + "> detected)");
                                    }

                                    // result_next_read_line[6] = effect_duration
                                    int out_int_1;
                                    if (int.TryParse(result_next_read_line[3], out out_int_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the effect_duration in ms - (<" + result_next_read_line[3] + "> detected)");
                                    }
                                    if (out_int_1 < 0)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the effect_duration in ms (>= 0) - (<" + result_next_read_line[3] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "fade_transparent_to_object") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <fade_transparent_to_object>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 4)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 4 arguments (" + result_next_read_line.Length + " arguments detected) - format : <GO_name> <value_min_alpha> <value_max_alpha> <effect_duration>");
                                }
                                else if (result_next_read_line.Length == 4)
                                {
                                    // result_next_read_line[0] = GO_name
                                    if ((result_next_read_line[0] != "mri_magnet") &&
                                    (result_next_read_line[0] != "mri_table") &&
                                    (result_next_read_line[0] != "mri_movingtable") &&
                                    (result_next_read_line[0] != "roomamdothers_ceiling") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot001") &&
                                    (result_next_read_line[0] != "roomamdothers_chariot002") &&
                                    (result_next_read_line[0] != "roomamdothers_floor") &&
                                    (result_next_read_line[0] != "roomamdothers_frontwalls") &&
                                    (result_next_read_line[0] != "roomamdothers_sidewalls"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should correspond to GO_name : <mri_magnet> or <mri_table> or <mri_movingtable> or <roomamdothers_ceiling> or <roomamdothers_chariot> or <roomamdothers_chariot001> or <roomamdothers_chariot002> or <roomamdothers_floor> or <roomamdothers_frontwalls> or <roomamdothers_sidewalls> - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    // result_next_read_line[1] = value_BurnSize
                                    float out_float_1;
                                    if (float.TryParse(result_next_read_line[1], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_min_alpha (value E [0 ; 1] - default : 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                    if ((out_float_1 < 0) || (out_float_1 > 1))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the value_max alpha (value E [0 ; 1] - default : 0) - (<" + result_next_read_line[1] + "> detected)");
                                    }

                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[2], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the value_min_alpha (value E [0 ; 1] - default : 1) - (<" + result_next_read_line[2] + "> detected)");
                                    }
                                    if ((out_float_1 < 0) || (out_float_1 > 1))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <float>, representing the value_max_alpha (value E [0 ; 1] - default : 1) - (<" + result_next_read_line[2] + "> detected)");
                                    }

                                    // result_next_read_line[6] = effect_duration
                                    int out_int_1;
                                    if (int.TryParse(result_next_read_line[3], out out_int_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the effect_duration in ms - (<" + result_next_read_line[3] + "> detected)");
                                    }
                                    if (out_int_1 < 0)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <int>, representing the effect_duration in ms (>= 0) - (<" + result_next_read_line[3] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "EOF")
				    {
					    //Check that configname contains only one argument
					    {
						    if (result.Length > 2)
						    {
							    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <EOF>, followed by an argument representing a text to display in command app, no other following arguments");
						    }

						    if (advanced_check)
						    {
							    if (is_MRI_table_inside == true)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<move_outside> command may have been called before this command. MRI_table is currently inside the MRI");
							    }

							    if (is_TV_displayed == true)
							    {
								    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + "[WARNING]" + " - " + "<activate_TV> with <false> parameter may have been called before this command. TV is actually activated in scene");
							    }
						    }

					    }

					    if (tot_nb_line != nb_line)
					    {
						    Debug.LogError("" + configName + " : <EOF> command, read <" + nb_line + "/" + tot_nb_line + " lines>");
					    }

					    nb_line = 0;
					    tot_nb_line = 0;
					    reader.Close();
					    reading_file = false;
					    nextCommand = false;
					    //experiment_can_start = false;
					    startChecking = false;
					    Debug.Log("End of Checking File - " + load_file_path);

					    _static_informations.i_number_of_commands = i_number_of_commands;
					    _static_informations.f_total_commands_time = f_total_commands_no_time_duration;

					    //if (b_advance_server_check)
					    {
						    i_debug_num_lines_debugtextzone = DebugTextZone.GetComponent<Text>().text.Split('\n').Length;
						    if (i_debug_num_lines_debugtextzone == 3)
						    {
							    if (File.Exists(s_experiment_file_location + "/" + s_experiment_file_name))
							    {
                                    //UnityEngine.SceneManagement.SceneManager.LoadScene(s_scene_named_pipes_name, UnityEngine.SceneManagement.LoadSceneMode.Single);
                                    //Debug.LogError("clear_logs");
                                    StartCoroutine(WaitForMsWait3s(1000));
                                }
							    else
							    {
								    Debug.LogError("");
								    Debug.LogError("Could not find app at path : " + s_experiment_file_location + "/" + s_experiment_file_name);
							    }
						    }
						    else
						    {
							    Debug.Log("Inconsistencies detected in file");
						    }
					    }

				    }
                    else if (result[0] == "set_AvatarsConfig_position_and_rotation") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_AvatarsConfig_position_and_rotation>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <v3_position> <v3_rotation>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    if (!StringToVector3(result_next_read_line[0]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <Vector3>, representing the desired position - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    if (!StringToVector3(result_next_read_line[1]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <Vector3>, representing the desired rotation - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "set_camera_position_and_rotation") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <set_Camera_position_and_rotation>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <v3_position> <v3_rotation>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    if (!StringToVector3(result_next_read_line[0]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <Vector3>, representing the desired position - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    if (!StringToVector3(result_next_read_line[1]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <Vector3>, representing the desired rotation - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "move_AvatarsConfig_from_A_to_B") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <move_AvatarsConfig_from_A_to_B>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 4)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 4 arguments (" + result_next_read_line.Length + " arguments detected) - format : <v3_position_start> <v3_rotation_start> <v3_position_end> <f_speed>");
                                }
                                else if (result_next_read_line.Length == 4)
                                {
                                    if (!StringToVector3(result_next_read_line[0]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <Vector3>, representing the start position - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    if (!StringToVector3(result_next_read_line[1]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <Vector3>, representing the start rotation - (<" + result_next_read_line[1] + "> detected)");
                                    }

                                    if (!StringToVector3(result_next_read_line[2]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <Vector3>, representing the end rotation - (<" + result_next_read_line[2] + "> detected)");
                                    }

                                    float f_;
                                    if (!float.TryParse(result_next_read_line[3], out f_))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 4th argument should be of type <float>, representing the speed - (<" + result_next_read_line[3] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "rotate_AvatarsConfig_from_A_to_B") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <rotate_AvatarsConfig_from_A_to_B>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <f_angle> <f_speed>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    float out_float_1;
                                    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the desired rotation angle - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : 1st argument should be of type <float>, representing the desired rotation speed - (<" + result_next_read_line[1] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "show_character")
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <show_character>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check activate_TVstate (1st line argument)
                            {
                                if (result_next_read_line.Length != 1)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <bool>");
                                }
                                else if (result_next_read_line.Length == 1)
                                {
                                    // result_next_read_line[0] = position
                                    if ((result_next_read_line[0] != "true") && (result_next_read_line[0] != "false"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should correspond to a bool : <true> to show characters or <false> to not show characters - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "enable_mesh_deformation")
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <enable_mesh_deformation>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check activate_TVstate (1st line argument)
                            {
                                if (result_next_read_line.Length != 1)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 argument (" + result_next_read_line.Length + " arguments detected) - format : <bool>");
                                }
                                else if (result_next_read_line.Length == 1)
                                {
                                    // result_next_read_line[0] = position
                                    if ((result_next_read_line[0] != "true") && (result_next_read_line[0] != "false"))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " : argument should correspond to a bool : <true> to show characters or <false> to enable mesh deformation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "load_scene_single") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <" + result[0] + ">, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 1)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 arguments (" + result_next_read_line.Length + " arguments detected) - format : <string>");
                                }  
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
					else if (result[0] == "load_scene_additive") // solve_object command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <" + result[0] + ">, followed by an argument representing a text to display in command app, no other following arguments");
							}
						}

						// read next line
						{
							string next_read_line = reader.ReadLine(); nb_line++;
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							//Check 1st line arguments
							{
								if (result_next_read_line.Length != 1)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 arguments (" + result_next_read_line.Length + " arguments detected) - format : <string>");
								}
								
							}
						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "unload_scene") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <" + result[0] + ">, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 1)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 1 arguments (" + result_next_read_line.Length + " arguments detected) - format : <string>");
                                }
                            }
                        }

                        // check for duration of the command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++; // duration
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            int out_int;
                            if (result_next_read_line.Length != 1)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
                            }
                            else if (result_next_read_line.Length == 1)
                            {
                                if (int.TryParse(result_next_read_line[0], out out_int) == false)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                                f_total_commands_no_time_duration += out_int;
                                if (out_int < 0)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "active_navigation") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <active_navigation>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 2)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 2 arguments (" + result_next_read_line.Length + " arguments detected) - format : <f_translation_speed> <f_rotation_speed>");
                                }
                                else if (result_next_read_line.Length == 2)
                                {
                                    float out_float_1;
                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <float>, representing the translation speed during active navigation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                    if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2bd argument should be of type <float>, representing the rotation speed during active navigation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
                    else if (result[0] == "active_navigation_with_target") // solve_object command
                    {
                        //Check that configname contains only one argument
                        {
                            if (result.Length > 2)
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <active_navigation_with_target>, followed by an argument representing a text to display in command app, no other following arguments");
                            }
                        }

                        // read next line
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;
                            char[] separators_next_read_line = new char[] { ' ' };
                            string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

                            //Check 1st line arguments
                            {
                                if (result_next_read_line.Length != 3)
                                {
                                    Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain 3 arguments (" + result_next_read_line.Length + " arguments detected) - format : <f_translation_speed> <f_rotation_speed> <v3_target_position>");
                                }
                                else if (result_next_read_line.Length == 3)
                                {
                                    float out_float_1;
                                    float out_float_2;
                                    if (float.TryParse(result_next_read_line[0], out out_float_1) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 1st argument should be of type <float>, representing the translation speed during active navigation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                    if (float.TryParse(result_next_read_line[1], out out_float_2) == false)
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 2nd argument should be of type <float>, representing the rotation speed during active navigation - (<" + result_next_read_line[0] + "> detected)");
                                    }

                                    if (!StringToVector3(result_next_read_line[2]))
                                    {
                                        Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : 3rd argument should be of type <Vector3>, representing the target position of active navigation - (<" + result_next_read_line[0] + "> detected)");
                                    }
                                }
                            }
                        }

                        // check for end of command
                        {
                            string next_read_line = reader.ReadLine(); nb_line++;

                            if (next_read_line != "")
                            {
                                Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
                            }
                        }
                    }
					else if (result[0] == "get_mri_references") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <get_mri_references>, followed by an argument representing a text to display in command app, no other following arguments");
							}
						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "avatar_inside_mri") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <avatar_inside_mri>, followed by an argument representing a text to display in command app, no other following arguments");
							}
						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}
					else if (result[0] == "avatar_outside_mri") // move_outside command
					{
						//Check that configname contains only one argument
						{
							if (result.Length > 2)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " : line must only contain <avatar_inside_mri>, followed by an argument representing a text to display in command app, no other following arguments");
							}
						}

						// check for duration of the command
						{
							string next_read_line = reader.ReadLine(); nb_line++; // duration
							char[] separators_next_read_line = new char[] { ' ' };
							string[] result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);

							int out_int;
							if (result_next_read_line.Length != 1)
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : line should contain one argument of type <int> corresponding to the duration of the command");
							}
							else if (result_next_read_line.Length == 1)
							{
								if (int.TryParse(result_next_read_line[0], out out_int) == false)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
								f_total_commands_no_time_duration += out_int;
								if (out_int < 0)
								{
									Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + " : argument should be of type <int> E [0 ; +oo[, representing the wanted duration of the command - argument : (<" + result_next_read_line[0] + "> detected)");
								}
							}
						}

						// check for end of command
						{
							string next_read_line = reader.ReadLine(); nb_line++;

							if (next_read_line != "")
							{
								Debug.LogError("[Line " + nb_line + "] - " + configName + " - " + next_read_line + " : line should be empty - end of command");
							}
						}
					}

					else
                    {
					Debug.LogError("[Line " + nb_line + "] - " + configName + " : not a command");
				    }
                }
                else
                {
                    reader.Close();
                    reading_file = false;
                    nextCommand = false;
                    //experiment_can_start = false;
                    startChecking = false;

                    Debug.LogError("End Of File command missing - syntax : <EOF>");

                    Debug.Log("End of Checking File - " + load_file_path);

					_static_informations.i_number_of_commands = i_number_of_commands;
				}
            }
        }
        yield return true;
    }


    public void CheckFile()
    {
        if (gostartChecking && !startChecking)
        {
            startChecking = true;
            gostartChecking = false;

            bool b_can_check_file = true; // check_hash_file(load_file_path);
            if (b_can_check_file)
            {
                StartCoroutine(CheckFileCoroutine());
            }
            else
            {
                Debug.Log("End of Checking File - " + load_file_path);
            }
        }
    }

    public void UpdateInputField_load_file_path()
    {
        load_file_path = GameObject.Find("Canvas/HUDAllExperiment/InputFieldinputfile").GetComponent<InputField>().text;
		_static_informations.s_experiment_file_data_path = load_file_path;
	}

	public void UpdateInputField_save_file_path()
	{
		save_file_path = GameObject.Find("Canvas/HUDAllExperiment/InputFieldoutputfile").GetComponent<InputField>().text;
		_static_informations.s_log_file_data_path = save_file_path;
	}



    // FUN WITH CHEAT CODE
   /* IEnumerator WaitForMs(int i_time_ms)
    {
        yield return new WaitForSeconds(i_time_ms / 1000);
    }*/

    IEnumerator WaitForMsWait3s(int i_time_ms)
    {
        yield return new WaitForSeconds(i_time_ms / 1000);
        Debug.LogError("clear_logs");
        Debug.LogWarning("Experiment will start in 3 s");
        StartCoroutine(WaitForMsWait2s(1000));
    }

    IEnumerator WaitForMsWait2s(int i_time_ms)
    {
        yield return new WaitForSeconds(i_time_ms / 1000);
        Debug.LogError("clear_logs");
        Debug.LogWarning("Experiment will start in 2 s");
        StartCoroutine(WaitForMsWait1s(1000));
    }

    IEnumerator WaitForMsWait1s(int i_time_ms)
    {
        yield return new WaitForSeconds(i_time_ms / 1000);
        Debug.LogError("clear_logs");
        Debug.LogWarning("Experiment will start in 1 s");
        StartCoroutine(WaitForMsWaitStart(1000));
    }

    IEnumerator WaitForMsWaitStart(int i_time_ms)
    {
        yield return new WaitForSeconds(i_time_ms / 1000);
        Debug.LogError("clear_logs");
        // DO SOMETHING

        if (File.Exists(s_experiment_file_location + "/" + s_experiment_file_name))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(s_scene_second_control_screen, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("");
            Debug.LogError("Could not find app at path : " + s_experiment_file_location + "/" + s_experiment_file_name);
        }
    }

	

    public static bool StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        bool b_ = false;
        // store as a Vector3
        float f_1, f_2, f_3;
        if (float.TryParse(sArray[0], out f_1) && float.TryParse(sArray[1], out f_2) && float.TryParse(sArray[2], out f_3))
        {
            b_ = true;
        }

        return b_;
    }
}
