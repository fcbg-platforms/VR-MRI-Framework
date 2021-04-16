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


using QTMRealTimeSDK.Data;
using QualisysRealTime.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class _connect_to_qualisys_DHCP_server : MonoBehaviour {

    private List<DiscoveryResponse> discoveryResponses;
    private DiscoveryResponse response;
    private bool b_errorConnectionQualisys;
	
	void Update () {
		if (b_errorConnectionQualisys)
		{
			b_errorConnectionQualisys = false;
            _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.send_error_connection_to_cameras();
        }
	}

    public void ConnectToQDHCPServer()
    {
        Thread thread = new Thread(Connect);
        thread.Start();
    }

    void Connect()
    {
        discoveryResponses = RTClient.GetInstance().GetServers();

        if (!RTClient.GetInstance().Connect(discoveryResponses[0], discoveryResponses[0].Port, true, true))
        {
            Debug.LogError("Could not connect to this server");
			b_errorConnectionQualisys = true;
		}
    }
}
