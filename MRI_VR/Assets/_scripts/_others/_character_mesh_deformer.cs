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

//[RequireComponent(typeof(MeshFilter))]
public class _character_mesh_deformer : MonoBehaviour
{
    [Header("Parameters")]
    public float springForce = 20f;
    public float damping = 5f;
    public float f_attenuatedForce_multiplier = 1;

    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

    float uniformScale = 1000f;


    void LateUpdate()
    {
        if (b_is_deformable)
        {
            uniformScale = transform.localScale.x;
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                UpdateVertex(i);
            }
            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();
        }
    }

        void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time_deltaTime;
        velocity *= 1f - damping * Time_deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * (Time_deltaTime / uniformScale);
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }
    }

    void AddForceToVertex(int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        if (pointToVertex.sqrMagnitude < max_distance_deformation)
        {
            pointToVertex *= uniformScale;
            float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude * f_attenuatedForce_multiplier);
            float velocity = attenuatedForce * Time_deltaTime;
            vertexVelocities[i] += pointToVertex.normalized * velocity;
            //vertexVelocities[i] += pointToVertex.normalized * force * Time_deltaTime;
        }
    }
    public float max_distance_deformation = 0.011f;
    public float Time_deltaTime = 0.017f;

    public void get_mesh()
    {
        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.AddComponent<MeshRenderer>();
        this.gameObject.GetComponent<MeshRenderer>().materials = this.gameObject.GetComponent<SkinnedMeshRenderer>().materials;
        this.gameObject.AddComponent<MeshCollider>();
        this.gameObject.GetComponent<SkinnedMeshRenderer>().BakeMesh(this.gameObject.GetComponent<MeshFilter>().mesh);

        this.gameObject.GetComponent<MeshCollider>().sharedMesh = this.gameObject.GetComponent<MeshFilter>().mesh;

        this.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }
    public void stop_get_mesh()
    {
        Destroy(this.gameObject.GetComponent<MeshRenderer>());
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Destroy(this.gameObject.GetComponent<MeshFilter>());

        this.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    [Header("DEBUG PURPOSE ONLY")]
    public bool b_debug_get_mesh;
    public bool b_debug_stop_get_mesh;
    //[HideInInspector]
    public bool b_is_deformable;
    private void Update()
    {
        if (b_debug_get_mesh)
        {
            b_debug_get_mesh = false;
            get_mesh();
            b_is_deformable = true;

        }
        if (b_debug_stop_get_mesh)
        {
            b_debug_stop_get_mesh = false;
            stop_get_mesh();
            b_is_deformable = false;

        }
    }
}