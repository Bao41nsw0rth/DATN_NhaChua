using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextWriter : MonoBehaviour
{
    private TextWriterSingle textWriterSingle;
    public void AddWriter(Text uiText, string textToWrite, float timePerCharacter, bool invinsibleCharacters)
    {
        textWriterSingle = new TextWriterSingle(uiText, textToWrite, timePerCharacter, invinsibleCharacters);

    }
    private void Update()
    {
        if (textWriterSingle != null)
        {
            textWriterSingle.Update();
        }
    }
    //them class
    public class TextWriterSingle
    {
        private Text uiText;
        private string textToWrite;
        private int characterIndex;
        private float timePerCharacter;
        private float timer;
        private bool invinsibleCharacters;
        public TextWriterSingle(Text uiText, string textToWrite, float timePerCharacter, bool invinsibleCharacters)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.invinsibleCharacters = invinsibleCharacters;
            characterIndex = 0;
        }
        public void Update()
        {
            if (uiText != null)
            {
                timer -= Time.deltaTime;
                while (timer <= 0f)
                {
                    timer += timePerCharacter;
                    characterIndex++;

                    string text = textToWrite.Substring(0, characterIndex);
                    if (invinsibleCharacters)
                    {
                        text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    }
                    uiText.text = text;
                    if (characterIndex >= textToWrite.Length)
                    {
                        uiText = null;
                        return;
                    }
                }
            }
        }
    }
}
