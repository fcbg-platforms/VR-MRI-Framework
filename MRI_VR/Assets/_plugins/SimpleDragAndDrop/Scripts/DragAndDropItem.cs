using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;
	public static GameObject icon;                                                  // Icon of dragged item
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 100;

	public GameObject dragItem;
	public int keyFunctionnality;

	List<DragAndDropCell> experimentationCells;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
		}
	}

	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (dragDisabled == false && transform.parent.parent.GetComponent<DragAndDropCell>().isFixed == false)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = this;                                                     // Set as dragged item
																					// Create item's icon
			icon =  Instantiate(dragItem, canvas.transform);
			//icon = new dragItem;
			icon.transform.SetParent(canvas.transform);
			//icon.transform.SetParent(transform.parent.parent.parent.transform);
			icon.name = "Icon";

			if(GetComponent<Image>() != null)
            {
				Image myImage = transform.parent.transform.parent.GetComponent<Image>();
				myImage.raycastTarget = false;                                          // Disable icon's raycast for correct drop handling
				Image iconImage = icon.AddComponent<Image>();
				iconImage.raycastTarget = false;
				iconImage.sprite = myImage.sprite;
			}
			else if(GetComponent<TextMeshProUGUI>() != null)
            {
				TextMeshProUGUI myText = GetComponent<TextMeshProUGUI>();

				myText.raycastTarget = false;                                          // Disable icon's raycast for correct drop handling
				TextMeshProUGUI iconText = icon.transform.Find("Header").transform.Find("Text").GetComponent<TextMeshProUGUI>();
				iconText.raycastTarget = false;
				iconText.text = myText.text;
				iconText.color = Color.red;
				iconText.alignment = GetComponent<TextMeshProUGUI>().alignment;
			}

			RectTransform iconRect = transform.parent.transform.parent.GetComponent<RectTransform>();// icon.GetComponent<RectTransform>();
			
			if (iconRect != null)
            {
				
				// Set icon's dimensions
				RectTransform myRect = GetComponent<RectTransform>();
				iconRect.pivot = new Vector2(0.5f, 0.5f);
				iconRect.anchorMin = new Vector2(0.5f, 0.5f);
				iconRect.anchorMax = new Vector2(0.5f, 0.5f);
				iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);
			}
			
			if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                			// Notify all items about drag start for raycast disabling
			}

			experimentationCells = new List<DragAndDropCell>();
			DragAndDropCell[] cells = GameObject.FindObjectsOfType<DragAndDropCell>();
			Debug.Log(cells.Length);

            for (int i = 0; i < cells.Length; i++)
            {
				if(cells[i].cellType == DragAndDropCell.CellType.Swap)
                {
					cells[i].cellType = DragAndDropCell.CellType.DragOnly;
					experimentationCells.Add(cells[i]);
				}
            }
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (icon != null)
		{
			//transform.parent.parent.position = Input.mousePosition;
			transform.parent.parent.parent.GetComponent<VerticalLayoutGroup>().spacing += 0.01f;
			transform.parent.parent.parent.GetComponent<VerticalLayoutGroup>().spacing -= 0.01f;
			
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
		}
	}


	

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		if(experimentationCells != null)
        {
			for (int i = 0; i < experimentationCells.Count; i++)
			{
				experimentationCells[i].cellType = DragAndDropCell.CellType.Swap;
			}

		}

		ResetConditions();
	}


    /// <summary>
    /// Resets all temporary conditions.
    /// </summary>
    private void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;

        
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		if(GetComponent<Image>() != null)
        {
			Image image = GetComponent<Image>();
			if (image != null)
			{
				image.raycastTarget = condition;
			}
		}
		else if (GetComponent<TextMeshProUGUI>() != null)
        {
			TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
			if (text != null)
			{
				text.raycastTarget = condition;
			}
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}

	public DragAndDropItem GetDraggedItem()
    {
		return draggedItem;
    }
}
