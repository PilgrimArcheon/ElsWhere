using UnityEngine;
using UnityEngine.UI;

public class ItemMarker : MonoBehaviour
{
	public Sprite icon;
	public Image image;

	public Vector2 position 
	{
		get { return new Vector2(transform.position.x, transform.position.z); }
	}
}
