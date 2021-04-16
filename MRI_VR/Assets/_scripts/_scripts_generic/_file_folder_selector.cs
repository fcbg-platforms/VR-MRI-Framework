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

using Crosstales.FB;
using UnityEngine;
using UnityEngine.UI;

public class _file_folder_selector : MonoBehaviour {

    public GameObject GO_load_path;
    public GameObject GO_save_path;
    
    private void Start()
    {
        GO_load_path = GameObject.Find("Canvas/HUDAllExperiment/InputFieldinputfile");
        GO_save_path = GameObject.Find("Canvas/HUDAllExperiment/InputFieldoutputfile");
    }

    public void OpenSingleFile()
    {
        /*var extensions = new[] {
            new ExtensionFilter("Text Files", "txt" ),
            new ExtensionFilter("All Files", "*" ),
        };*/

        string extensions = "txt";

        string path = FileBrowser.OpenSingleFile("Open File", "", extensions);

        GO_load_path.GetComponent<InputField>().text = path;
    }


	public void SaveFile()
    {
        string extensions = "txt";

        string path = FileBrowser.SaveFile("Save File", "", System.DateTime.Now.ToString("[yyyy-dd-MM] [HH-mm-ss]") + " out_experiment", extensions);

        GO_save_path.GetComponent<InputField>().text = path;
    }
}
