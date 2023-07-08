using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UnderlineOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public TMPro.TextMeshProUGUI textTarget;

	public void OnPointerEnter(PointerEventData eventData)
	{
		textTarget.fontStyle = TMPro.FontStyles.Underline;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		textTarget.fontStyle = TMPro.FontStyles.Normal;
	}
}