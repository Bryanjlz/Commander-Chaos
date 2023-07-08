using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HelpScreenController : MonoBehaviour
{

	const int LEFT = 0;
	const int RIGHT = 1;

	public GameObject leftButton;
	public GameObject rightButton;
	public GameObject pageParent;

	List<GameObject> buttons;
	List<GameObject> pages;


	int currentPage;


	// Start is called before the first frame update
	void Start()
	{
		buttons = new List<GameObject>();
		buttons.Add(leftButton);
		buttons.Add(rightButton);

		pages = new List<GameObject>();
		foreach (Transform child in pageParent.transform)
		{
			child.gameObject.SetActive(false);
			pages.Add(child.gameObject);
		}

		GoToPage(0);
	}
	public void UpdatePageButtons()
	{
		switch (currentPage)
		{
			case 0:
				SetButtonText(LEFT, "< Back");
				SetButtonText(RIGHT, "Next >");
				break;
			case 1:
				SetButtonText(LEFT, "< Back");
				SetButtonText(RIGHT, "Next >");
				break;
			case 2:
				SetButtonText(LEFT, "< Back");
				SetButtonText(RIGHT, "Play! >");
				break;
			default:
				return;
		}
	}

	public void LeftButtonPress()
	{
		switch (currentPage)
		{
			case 0:
				GoToScene("Start");
				break;
			case 1:
				GoToPrevPage();
				break;
			case 2:
				GoToPrevPage();
				break;
			default:
				Debug.Log("Something went wrong");
				return;
		}
	}

	public void RightButtonPress()
	{
		switch (currentPage)
		{
			case 0:
				GoToNextPage();
				break;
			case 1:
				GoToNextPage();
				break;
			case 2:
				GoToScene("BryanScene");
				break;
			default:
				Debug.Log("Something went wrong");
				return;
		}
	}

	public void SetButtonText(int index, string text)
	{
		buttons[index].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = text;
	}

	public void SetButtonVisibility(int index, bool visibility)
	{
		buttons[index].SetActive(visibility);
	}

	public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void GoToPage(int page)
	{
		pages[currentPage].SetActive(false);
		currentPage = page;
		pages[currentPage].SetActive(true);
		UpdatePageButtons();
	}

	public void GoToPrevPage()
	{
		GoToPage(currentPage - 1);
	}

	public void GoToNextPage()
	{
		GoToPage(currentPage + 1);
	}

}
