using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{

	[SerializeField]
	Vector2 mouseStart;
	[SerializeField]
	Vector2 mouseEnd;
	[SerializeField]
	bool isSelecting;

	[SerializeField]
	GameObject selectionRectangle;
	BoxCollider2D selectionCollider;
	SpriteRenderer selectionRenderer;
	Transform selectionTransform;

	[SerializeField]
	Camera cam;


    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main;
		isSelecting = false;

		selectionCollider = selectionRectangle.GetComponent<BoxCollider2D>();
		selectionRenderer = selectionRectangle.GetComponent<SpriteRenderer>();
		selectionTransform = selectionRectangle.transform;

		selectionRectangle.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
		Vector2 mouse = new Vector2(point.x, point.y);

		if (Input.GetMouseButtonDown(0))
		{
			mouseStart = mouse;
			isSelecting = true;

			selectionRectangle.SetActive(true);
		}

		if (isSelecting)
		{
			// obligatory I hate Unity documentation

			selectionTransform.position = (mouse + mouseStart) / 2;

			Vector2 selectSize = new Vector2(Mathf.Abs(mouse.x - mouseStart.x), Mathf.Abs(mouse.y - mouseStart.y));

			selectionRenderer.size = selectSize;
			selectionCollider.size = selectSize;

		}

		if (Input.GetMouseButtonUp(0))
		{
			isSelecting = false;
			selectionRectangle.SetActive(false);
		}
    }
}
