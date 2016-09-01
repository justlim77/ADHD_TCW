using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultsPopup : MonoBehaviour {

    public Text[] headerText;
    public TextTyper[] resultText;
    public GameObject homeButton;

    public IEnumerator ShowResults()
    {
        //Calculate Results Overall
        headerText[3].text = "Overall Results:";

        headerText[3].GetComponent<AnimatedSlide>().SlideIn();
        yield return new WaitForSeconds(0.7f);

        if (GameManager.CalculateScore(0) <= 14) //Bad
        {
            //headerText[3].color = new Color32(255, 0, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[3].RunTypeText("Uh oh! You are about to enter a boxing match! You are making a mountain out of a molehill. The grass may be greener on the other side but they may be weeds!");
        }
        else if ((GameManager.CalculateScore(0) > 14) && (GameManager.CalculateScore(0) <= 19)) //Mid
        {
            //headerText[3].color = new Color32(255, 100, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[3].RunTypeText("Hmmm... You're almost there! Go on, smell the roses! Being mindful is half the battle won.");
        }
        else if (GameManager.CalculateScore(0) > 19) //Very Good
        {
            //headerText[3].color = new Color32(0, 155, 0, 255);
            resultText[3].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[3].RunTypeText("Hooray! You hit jackpot! You are on your way to being a cool cucumber! Be proud of yourself!");
        }

        headerText[0].GetComponent<AnimatedSlide>().SlideIn();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E1
        if (GameManager.CalculateScore(1) <= 2) //Bad
        {
            //headerText[0].color = new Color32(255, 0, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[0].RunTypeText("You need to work on these:\n\n-> Managing your own emotions\n-> Acknolwedging your child's every effort");
        }
        else if ((GameManager.CalculateScore(1) > 2) && (GameManager.CalculateScore(1) <= 4)) //Mid
        {
            //headerText[0].color = new Color32(255, 100, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[0].RunTypeText("You can improve on these:\n\n-> Using appropriate words\n-> Listening to your child's needs\n-> Giving praise and encouragement");
        }
        else if (GameManager.CalculateScore(1) > 4) //Very Good
        {
            //headerText[0].color = new Color32(0, 155, 0, 255);
            resultText[0].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[0].RunTypeText("You nailed these:\n\n-> Being firm and not harsh\n-> Acknowledging your child's needs");
        }

        headerText[1].GetComponent<AnimatedSlide>().SlideIn();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E2
        if (GameManager.CalculateScore(2) <= 5) //Bad
        {
            //headerText[1].color = new Color32(255, 0, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[1].RunTypeText("You need to work on these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Giving instructions one at a time");
        }
        else if ((GameManager.CalculateScore(2) > 5) && (GameManager.CalculateScore(2) <= 7)) //Mid
        {
            //headerText[1].color = new Color32(255, 100, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[1].RunTypeText("You can improve on these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Keeping instructions short");
        }
        else if (GameManager.CalculateScore(2) > 7) //Very Good
        {
            //headerText[1].color = new Color32(0, 155, 0, 255);
            resultText[1].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[1].RunTypeText("You nailed these:\n\n-> Getting your child's attention first\n-> Stating tasks requirements\n-> Checking for understanding");
        }

        headerText[2].GetComponent<AnimatedSlide>().SlideIn();
        yield return new WaitForSeconds(0.7f);

        //Calculate Results E3
        if (GameManager.CalculateScore(3) <= 5) //Bad
        {
            //headerText[2].color = new Color32(255, 0, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(255, 0, 0, 255);
            yield return resultText[2].RunTypeText("You need to work on these:\n\n-> Ignoring minor misbehavior\n-> Following through with consequences");
        }
        else if ((GameManager.CalculateScore(3) > 5) && (GameManager.CalculateScore(3) <= 7)) //Mid
        {
            //headerText[2].color = new Color32(255, 100, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(255, 100, 0, 255);
            yield return resultText[2].RunTypeText("You can improve on these:\n\n-> Stating expectations and consequences clearly\n-> Following through with consequences");
        }
        else if (GameManager.CalculateScore(3) > 7) //Very Good
        {
            //headerText[2].color = new Color32(0, 155, 0, 255);
            resultText[2].GetComponent<Text>().color = new Color32(0, 155, 0, 255);
            yield return resultText[2].RunTypeText("You nailed these:\n\n-> Being flexible with child's request\n-> Following through with consequences");
        }

        yield return new WaitForSeconds(0.1f);

        homeButton.SetActive(true);
    }
}
