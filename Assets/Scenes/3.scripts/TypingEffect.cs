using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;   // Reference to the TextMeshPro component
    public float typingSpeed = 0.05f;     // Time delay between each character
    public string fullText =
        "> Project: Code L`ife\n" +
        "> Terminal\n" +
        "> Terminal\n" +
        "> Terminal\n" +
        "> Day 185468678\n" +
        "> This captain ****** Space station 1729\n" +
        "> Contact with planet X35\n" +
        "> Deploying Nav AI\n" +
        "> Deploying X20 pilot ship\n" +
        "> Entering atmosphere\n" +
        "> X20 condition 100%\n" +
        "> Nav AI condition 100%\n" +
        "> Probability of life form on X35: 0%\n" +
        "> Success rate: 0%";
    
    private string currentText = "";      // To store the current progress of the typed text
    private bool isCursorVisible = true;  // Cursor visibility
    public float cursorBlinkSpeed = 0.5f; // Blink speed of the cursor

    void Start()
    {
        StartCoroutine(TypeText());
        StartCoroutine(CursorBlink());
    }

    // Coroutine to type the text one character at a time
    IEnumerator TypeText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            textMeshPro.text = currentText + (isCursorVisible ? "_" : ""); // Add blinking cursor
            yield return new WaitForSeconds(typingSpeed);
        }
        
        // After typing is done, keep the cursor blinking
        textMeshPro.text = currentText + (isCursorVisible ? "_" : "");
    }

    // Coroutine to handle the blinking cursor effect
    IEnumerator CursorBlink()
    {
        while (true)
        {
            isCursorVisible = !isCursorVisible;
            textMeshPro.text = currentText + (isCursorVisible ? "_" : "");
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }
}
