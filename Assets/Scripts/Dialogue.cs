using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float typeSpeed;
    private int lineIndex;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue() {
        //Make dialogue box appear
        gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    //Coroutine for typing out the text
    IEnumerator TypeLine() {
        //Type every letter in the line
        foreach (char c in lines[lineIndex]) {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    //Move onto the next line in the lines list
    void NextLine() {
        textComponent.text = string.Empty;

        //if we have not yet reached the end of the list
        if (lineIndex < lines.Length - 1) {
            lineIndex++; //Add 1 to the line index
            StartCoroutine(TypeLine());
        }

        //if we have reached the end of the list
        else {
            gameObject.SetActive(false);
        }
    }

    //CallbackContext class this method 3 times when a key is pressed:
    //1. key is pressed
    //2. key is held
    //3. key is released
    public void ProceedDialogue(InputAction.CallbackContext callbackContext) {
        //if the N key was pressed
        if (callbackContext.performed) {
            //if we have already typed out the full line, proceed to the next line
            if (textComponent.text == lines[lineIndex]) {
                NextLine();
            }

            //if the line is still typing, just display the line in full
            else {
                StopAllCoroutines();
                textComponent.text = lines[lineIndex];
            }
        }
    }
}