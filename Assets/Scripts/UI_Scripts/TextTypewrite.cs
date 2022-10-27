using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
// attach to UI Text component (with the full text already there)

public class TextTypewrite: MonoBehaviour
{

	private TextMeshProUGUI text;
	private string textWhole;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		textWhole = text.text;
		text.text = "";

		// TODO: add optional delay when to start
		StartCoroutine("PlayText");
	}

	private IEnumerator PlayText()
	{
		foreach (char c in textWhole)
		{
			text.text += c;
			yield return new WaitForSeconds(0.05f);
		}
	}

}