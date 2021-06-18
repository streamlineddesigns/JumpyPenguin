using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DialogText : MonoBehaviour
{
    public List<Text> Texts = new List<Text>();//List of ui Text
    public List<char[]> textChars = new List<char[]>();//list of string character arrays

    public void SetChars()
    {
        for(int i = 0; i < Texts.Count; i++)
        {
            textChars.Add(Texts[i].text.ToCharArray());
            Texts[i].text = "";
        }
    }

    public abstract IEnumerator WaitForCondition();
}