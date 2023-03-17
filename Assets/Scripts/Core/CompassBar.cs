using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassBar : MonoBehaviour
{
	public GameObject iconPrefab;
	List<ItemMarker> itemMakers = new List<ItemMarker>();

	public RawImage compassImage;
	public Transform player;
	public Text distToFinalPoint;

	public float maxDistance;
	float compassUnit;

	public ItemMarker finalGoal;

	void Start() 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		compassUnit = compassImage.rectTransform.rect.width / 360f;
		UpdateItemMarkers();
	}

	void Update() 
	{
		compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);
	
		foreach (ItemMarker _itemMarker in itemMakers)
		{
			_itemMarker.image.rectTransform.anchoredPosition = GetPosOnCompass(_itemMarker);

			float dst = Vector2.Distance (new Vector2(player.transform.position.x, player.transform.position.z), _itemMarker.position);
			
			if(_itemMarker.gameObject.tag != "FinalPoint")
			{
				float scale = 0f;
				if(dst < maxDistance)
				scale = 1f - ((dst - 2) / (maxDistance - 5));

				_itemMarker.image.rectTransform.localScale = Vector3.one * scale;
				_itemMarker.image.color = new Color(_itemMarker.image.color.r, _itemMarker.image.color.g, _itemMarker.image.color.b, 1 * scale);
			}
			else 
			{
				distToFinalPoint.text = "~. " + (dst - 5).ToString("00") + "M .~";
			}
		}
	}

	public void UpdateItemMarkers()
	{
		Transform compassTransform = transform.GetChild(0);
		itemMakers.Clear();
		foreach (Transform child in compassTransform)
		{
			Destroy(child.gameObject);
		}
		
		ItemMarker[] _itemMarkers = FindObjectsOfType<ItemMarker>();
		foreach (ItemMarker _itemMarker in _itemMarkers)
		{
			AddItemMarker(_itemMarker);
		}
	}

	public void AddItemMarker (ItemMarker marker)
	{
		GameObject newMarker = Instantiate(iconPrefab, compassImage.transform);
		marker.image = newMarker.GetComponent<Image>();
		newMarker.name = marker.gameObject.name;
		marker.image.sprite = marker.icon;

		itemMakers.Add(marker);
	}

	Vector2 GetPosOnCompass (ItemMarker marker)
	{
		Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
		Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

		float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

		return new Vector2(compassUnit * angle, 0f);
	} 
}
