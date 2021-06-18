using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioByStorm.Scripts
{

    public class Tutorials : MonoBehaviour
    {
        public bool tutorialsPlaying;
        public GameObject Dialog;
        public List<DialogText> DialogTexts = new List<DialogText>();
        public BoxCollider2D upCollider;
        public BoxCollider2D leftCollider;
        public BoxCollider2D rightCollider;

        public void Start()
        {

        }


        //Call the play the tutorials
        public void letTheLessonsBegin()
        {
            if (!tutorialsPlaying) StartCoroutine(BeginTutorials());
        }

        IEnumerator BeginTutorials()
        {
            tutorialsPlaying = true;

            //disable all movements
            upCollider.enabled = false;
            leftCollider.enabled = false;
            rightCollider.enabled = false;

            yield return new WaitForSeconds(2.0f);

            //show dialog
            Dialog.SetActive(true);

            //Iterate over the dialog texts list
            for(int i = 0; i < DialogTexts.Count; i++)
            {
                //set the current dialog to active
                DialogTexts[i].gameObject.SetActive(true);
                DialogTexts[i].SetChars();
                yield return new WaitForSeconds(0.1f);

                //iterate over and print out all the text like a typewriter
                for(int j = 0; j < DialogTexts[i].Texts.Count; j++)
                {
                    string currentTextString = "";

                    foreach (char letter in DialogTexts[i].textChars[j])
                    {
                        AudioController.Singleton.PlayTypeWriterSound();
                        currentTextString += letter;
                        DialogTexts[i].Texts[j].text = currentTextString;      
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                

                //Wait for the wait condition to be true
                yield return StartCoroutine(DialogTexts[i].WaitForCondition());

                //deactivate current dialog
                DialogTexts[i].gameObject.SetActive(false);
            }

            //hide dialog
            Dialog.SetActive(false);

            ZPlayerPrefs.SetInt("InGameTutorial", 1);
        }
    }

}