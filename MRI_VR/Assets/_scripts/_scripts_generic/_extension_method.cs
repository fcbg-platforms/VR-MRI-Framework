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
using System.Reflection;
using UnityEngine;

public static class _extension_method {

    public static Component AddComponentExt(this GameObject obj, string scriptName)
    {
        Component cmpnt = null;


        for (int i = 0; i < 10; i++)
        {
            //If call is null, make another call
            cmpnt = _AddComponentExt(obj, scriptName, i);

            //Exit if we are successful
            if (cmpnt != null)
            {
                break;
            }
        }


        //If still null then let user know an exception
        if (cmpnt == null)
        {
            Debug.LogError("Failed to Add Component");
            return null;
        }
        return cmpnt;
    }

    private static Component _AddComponentExt(GameObject obj, string className, int trials)
    {
        //Any script created by user(you)
        const string userMadeScript = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        //Any script/component that comes with Unity such as "Rigidbody"
        const string builtInScript = "UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "Image"
        const string builtInScriptUI = "UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "Networking"
        const string builtInScriptNetwork = "UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "AnalyticsTracker"
        const string builtInScriptAnalytics = "UnityEngine.Analytics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        //Any script/component that comes with Unity such as "AnalyticsTracker"
        const string builtInScriptHoloLens = "UnityEngine.HoloLens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        Assembly asm = null;

        try
        {
            //Decide if to get user script or built-in component
            switch (trials)
            {
                case 0:

                    asm = Assembly.Load(userMadeScript);
                    break;

                case 1:
                    //Get UnityEngine.Component Typical component format
                    className = "UnityEngine." + className;
                    asm = Assembly.Load(builtInScript);
                    break;
                case 2:
                    //Get UnityEngine.Component UI format
                    className = "UnityEngine.UI." + className;
                    asm = Assembly.Load(builtInScriptUI);
                    break;

                case 3:
                    //Get UnityEngine.Component Video format
                    className = "UnityEngine.Video." + className;
                    asm = Assembly.Load(builtInScript);
                    break;

                case 4:
                    //Get UnityEngine.Component Networking format
                    className = "UnityEngine.Networking." + className;
                    asm = Assembly.Load(builtInScriptNetwork);
                    break;
                case 5:
                    //Get UnityEngine.Component Analytics format
                    className = "UnityEngine.Analytics." + className;
                    asm = Assembly.Load(builtInScriptAnalytics);
                    break;

                case 6:
                    //Get UnityEngine.Component EventSystems format
                    className = "UnityEngine.EventSystems." + className;
                    asm = Assembly.Load(builtInScriptUI);
                    break;

                case 7:
                    //Get UnityEngine.Component Audio format
                    className = "UnityEngine.Audio." + className;
                    asm = Assembly.Load(builtInScriptHoloLens);
                    break;

                case 8:
                    //Get UnityEngine.Component SpatialMapping format
                    className = "UnityEngine.VR.WSA." + className;
                    asm = Assembly.Load(builtInScriptHoloLens);
                    break;

                case 9:
                    //Get UnityEngine.Component AI format
                    className = "UnityEngine.AI." + className;
                    asm = Assembly.Load(builtInScript);
                    break;
            }
        }
        catch (Exception e)
        {
            //Debug.Log("Failed to Load Assembly" + e.Message);
        }

        //Return if Assembly is null
        if (asm == null)
        {
            return null;
        }

        //Get type then return if it is null
        Type type = asm.GetType(className);
        if (type == null)
            return null;

        //Finally Add component since nothing is null
        Component cmpnt = obj.AddComponent(type);
        return cmpnt;
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Shuffles the element order of two specified list in the same way.
    /// </summary>
    public static void Shuffle_two_lists_same_way(IList ts_1, IList ts_2)
    {
        var count_1 = ts_1.Count;
        var count_2 = ts_2.Count;
        var count = 0;
        if (count_1 == count_2)
        {
            count = count_1;
        }
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp_1 = ts_1[i];
            ts_1[i] = ts_1[r];
            ts_1[r] = tmp_1;

            var tmp_2 = ts_2[i];
            ts_2[i] = ts_2[r];
            ts_2[r] = tmp_2;
        }
    }
}