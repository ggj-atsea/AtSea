using UnityEngine;

public class FloatController : MonoBehaviour
{
	private GameObject seaPlane;
	private Cloth planeMesh;
	[SerializeField] private int closestVertexIndex = -1;
	
	// Use this for initialization
	void Start () 
	{
		seaPlane = gameObject.transform.parent.gameObject;
		planeMesh = seaPlane.GetComponent<Cloth>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetClosestVertex();
	}

	void GetClosestVertex() 
	{
		for(int i = 0; i < planeMesh.vertices.Length; i++)
		{
			if(closestVertexIndex == -1)
			{
				closestVertexIndex = i;
			}
			
			var distance = Vector3.Distance(planeMesh.vertices[i], transform.position);
			var closestDistance = Vector3.Distance(planeMesh.vertices[closestVertexIndex], transform.position);

			if (distance < closestDistance)
			{
				closestVertexIndex = i;
			}
		}

		transform.localPosition = new Vector3(transform.localPosition.x, planeMesh.vertices[closestVertexIndex].y / seaPlane.transform.localScale.z, transform.localPosition.z);
	}
}
