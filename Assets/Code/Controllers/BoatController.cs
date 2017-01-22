using UnityEngine;

public class BoatController : MonoBehaviour, IInteractable
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

    public void OnTouchDown()
    {
		FlipBoat();
    }

    public void OnTouchUp()
    {
    }

    public void OnTouchStay()
    {
    }

    public void OnTouchExit()
    {
    }

	public void FlipBoat()
	{
		iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(transform.position.x, transform.position.y, 0f), "easetype", iTween.EaseType.easeInOutSine, "time", 1.3f));
	}
}
