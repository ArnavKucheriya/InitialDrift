///--------------------------------------------------------------------------------------------
/// Simple Car Damage system By Ciorbyn Studio https://www.youtube.com/c/CiorbynStudio
/// Tutorial link: https://youtu.be/l04cw7EChpI
/// -------------------------------------------------------------------------------------------
/// 
using UnityEngine;

public class CarDamage : MonoBehaviour 
{
	public float maxMoveDelta = 1.0f; // maximum distance one vertice moves per explosion (in meters)
	public float maxCollisionStrength = 50.0f;
	public float YforceDamp = 0.1f; // 0.0 - 1.0
	public float demolutionRange = 0.5f;
	public float impactDirManipulator = 0.0f;
	public MeshFilter[] MeshList;
	
	private MeshFilter[] meshfilters;
	private float sqrDemRange;

    //Save Vertex Data
    private struct permaVertsColl
    {
        public Vector3[] permaVerts;
    }
    private permaVertsColl[] originalMeshData;
    int i;

	public void Start()
	{   
        
        if(MeshList.Length>0)
        	meshfilters = MeshList;
        else
        	meshfilters = GetComponentsInChildren<MeshFilter>();
        	
        sqrDemRange = demolutionRange*demolutionRange;

        LoadOriginalMeshData();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Repair();
    }

    void LoadOriginalMeshData()
    {
        originalMeshData = new permaVertsColl[meshfilters.Length];
        for( i = 0; i < meshfilters.Length; i++)
        {
            originalMeshData[i].permaVerts = meshfilters[i].mesh.vertices;
        }
    }
    
    void Repair()
    {
        for (int i = 0; i < meshfilters.Length; i++)
        {
            meshfilters[i].mesh.vertices = originalMeshData[i].permaVerts;
            meshfilters[i].mesh.RecalculateNormals();
            meshfilters[i].mesh.RecalculateBounds();
        }
    }

	public void OnCollisionEnter( Collision collision ) 
	{
		Vector3 colRelVel = collision.relativeVelocity;
		colRelVel.y *= YforceDamp;
		
		Vector3 colPointToMe = transform.position - collision.contacts[0].point;

		// Dot = angle to collision point, frontal = highest damage, strip = lowest damage
		float colStrength = colRelVel.magnitude * Vector3.Dot(collision.contacts[0].normal, colPointToMe.normalized);

		OnMeshForce( collision.contacts[0].point, Mathf.Clamp01(colStrength/maxCollisionStrength) );

	}
	
	// if called by SendMessage(), we only have 1 param
	public void OnMeshForce( Vector4 originPosAndForce )
	{
        OnMeshForce( (Vector3)originPosAndForce, originPosAndForce.w );
	}

	public void OnMeshForce( Vector3 originPos, float force )
	{
        // force should be between 0.0 and 1.0
        force = Mathf.Clamp01(force);
        
        for(int j=0; j<meshfilters.Length; ++j)
        {
        	Vector3 [] verts = meshfilters[j].mesh.vertices;

        	for (int i=0;i<verts.Length;++i)
        	{
        		Vector3 scaledVert = Vector3.Scale( verts[i], transform.localScale );						
        		Vector3 vertWorldPos = meshfilters[j].transform.position + (meshfilters[j].transform.rotation * scaledVert);
                Vector3 originToMeDir = vertWorldPos - originPos;
                Vector3 flatVertToCenterDir = transform.position - vertWorldPos;
                flatVertToCenterDir.y = 0.0f;
                   
                // 0.5 - 1 => 45° to 0°  / current vertice is nearer to exploPos than center of bounds
                if( originToMeDir.sqrMagnitude < sqrDemRange ) //dot > 0.8f )
                {
                	float dist = Mathf.Clamp01(originToMeDir.sqrMagnitude/sqrDemRange);
                    float moveDelta = force * (1.0f-dist) * maxMoveDelta;

					Vector3 moveDir = Vector3.Slerp(originToMeDir, flatVertToCenterDir, impactDirManipulator).normalized * moveDelta;

                    verts[i] += Quaternion.Inverse(transform.rotation)*moveDir;
                }

        	}
       
        	meshfilters[j].mesh.vertices = verts;
        	meshfilters[j].mesh.RecalculateBounds();
        }
	} 
}
