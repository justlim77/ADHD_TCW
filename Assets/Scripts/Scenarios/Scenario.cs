using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario : MonoBehaviour
{
    public enum ScenarioType
    {
        Bag,
        Homework,
        PC
    }

    public BoxCollider2D bcObj;

    public ScenarioType scenarioType;

    public ChatPanel chatPanel;

    public string synopsis;
    public float synopsisDuration = 2.0f;
    public float responseDelay = 1.0f;

    public ParentResponse parent1;
    public Feedback feedback1;

    public ParentResponse parent2;
    public Feedback feedback2A;
    public Feedback feedback2B;

    public ParentResponse parent3A; // Cont.
    public ParentResponse parent3B; // Lose
    public Feedback feedback3A;     // Cont
    public Feedback feedback3B;     // Lose

    public ParentResponse parent4;
    public Feedback feedback4A;
    public Feedback feedback4B;

    public ParentResponse parent5A;
    public ParentResponse parent5B;
    public Feedback feedback5A;     // Lose
    public Feedback feedback5B;     // Done

    public List<Response> chosenResponses = new List<Response>();

    public static Response CurrentResponse = null;
    public static Response PrevResponse = null;

    Feedback m_CurrentFeedback;
    ParentResponse m_NextResponse;

    public void SetBoxCollider(bool isActive)
    {
        if (!GameManager.isTempPause)
            bcObj.enabled = isActive;
    }

    void Start ()
    {
        Initialize();
    }

    void Initialize()
    {
        parent1.response1.feedback = feedback1;
        parent1.response2.feedback = feedback1;

        parent2.response1.feedback = feedback2A;
        parent2.response2.feedback = feedback2B;

        parent3A.response1.feedback = feedback3A;   // Cont
        parent3A.response2.feedback = feedback3A;   // Cont
        parent3B.response1.feedback = feedback3A;   // Cont
        parent3B.response2.feedback = feedback3B;   // Lose

        parent4.response1.feedback = feedback4A;
        parent4.response2.feedback = feedback4B;

        parent5A.response1.feedback = feedback5A;   // Lose
        parent5A.response2.feedback = feedback5B;   // Done
        parent5B.response1.feedback = feedback5B;   // Done
        parent5B.response2.feedback = feedback5B;   // Done
    }

    public void BeginScenario()
    {
        if (!GameManager.isTempPause)
        {
            GameManager.isTempPause = true;
            switch (scenarioType)
            {
                case ScenarioType.Bag:
                    StartCoroutine(BagScenario());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator BagScenario()
    {
        chatPanel.Initialize();

        // Show synopsis
        yield return chatPanel.TypeSynopsis(synopsis);
        yield return new WaitForSeconds(synopsisDuration);
        chatPanel.synopsisText.FadeText();
        yield return chatPanel.ShowParentIcon();

        // Show parent response 1
        chatPanel.PrepareResponse();
        m_NextResponse = parent1;
        yield return chatPanel.ShowParentResponses(m_NextResponse.response1.response, m_NextResponse.response2.response);
        yield return chatPanel.WaitForResponse();
        CurrentResponse = GetCurrentResponse(m_NextResponse);
        AddPotentialScore();

        // Show child response 1
        yield return new WaitForSeconds(responseDelay);
        yield return chatPanel.ShowChildIcon();
        yield return chatPanel.ShowChildResponse(m_NextResponse.response1.feedback.feedback);
        yield return new WaitForSeconds(responseDelay);
        m_NextResponse = parent2;

        // Show parent response 2
        chatPanel.PrepareResponse();
        yield return chatPanel.ShowParentResponses(parent2.response1.response, parent2.response2.response);
        yield return chatPanel.WaitForResponse();
        AddPotentialScore();

        switch (chatPanel.GetResponseIndex) //Obtain Possible Reward for Parent2 Selection & Proceed Next
        {
            case 1:
                m_CurrentFeedback = m_NextResponse.response1.feedback; 
                m_NextResponse = parent3A;
                break;
            case 2:
                m_CurrentFeedback = m_NextResponse.response2.feedback;
                m_NextResponse = parent3B;
                break;
        }
        CurrentResponse = GetCurrentResponse(m_NextResponse);

        // Show child response 2A/2B
        yield return new WaitForSeconds(responseDelay);
        yield return chatPanel.ShowChildResponse(m_CurrentFeedback.feedback);
        yield return new WaitForSeconds(responseDelay);

        // Show parent response 3A/3B
        chatPanel.PrepareResponse();
        yield return chatPanel.ShowParentResponses(m_NextResponse.response1.response, m_NextResponse.response2.response);
        yield return chatPanel.WaitForResponse();
        AddPotentialScore();

        switch (chatPanel.GetResponseIndex)
        {
            case 1:
                m_CurrentFeedback = parent3A.response1.feedback;
                CurrentResponse = GetCurrentResponse(m_NextResponse);
                m_NextResponse = parent4;
                break;
            case 2:
                if (m_NextResponse == parent3B)
                {
                    m_CurrentFeedback = parent3B.response2.feedback;
                    m_CurrentFeedback.gameOver = true;  // Lose 
                    break;                  
                }

                m_CurrentFeedback = parent3A.response1.feedback;
                m_NextResponse = parent4;
                break;
        }
        CurrentResponse = chatPanel.GetResponseIndex == 1 ? parent1.response1 : parent1.response2;

        // Show child response 3A/3B
        yield return new WaitForSeconds(responseDelay);
        yield return chatPanel.ShowChildResponse(m_CurrentFeedback.feedback);
        yield return new WaitForSeconds(responseDelay);
        yield return CheckState(m_CurrentFeedback);

        // Show parent response 4
        chatPanel.PrepareResponse();
        yield return chatPanel.ShowParentResponses(m_NextResponse.response1.response, m_NextResponse.response2.response);
        yield return chatPanel.WaitForResponse();
        AddPotentialScore();

        switch (chatPanel.GetResponseIndex)
        {
            case 1:
                m_CurrentFeedback = parent4.response1.feedback;
                m_NextResponse = parent5A;
                break;
            case 2:
                m_CurrentFeedback = parent4.response2.feedback;
                m_NextResponse = parent5B;
                break;
        }

        // Show child response 4A/4B
        yield return new WaitForSeconds(responseDelay);
        yield return chatPanel.ShowChildResponse(m_CurrentFeedback.feedback);
        yield return new WaitForSeconds(responseDelay);
        yield return CheckState(m_CurrentFeedback);

        // Show parent response 5A/5B
        chatPanel.PrepareResponse();
        yield return chatPanel.ShowParentResponses(m_NextResponse.response1.response, m_NextResponse.response2.response);
        yield return chatPanel.WaitForResponse();
        AddPotentialScore();

        switch (chatPanel.GetResponseIndex)
        {
            case 1:
                if (m_NextResponse == parent5A)
                {
                    m_CurrentFeedback = parent5A.response1.feedback;
                    m_NextResponse = null;
                    m_CurrentFeedback.gameOver = true;  // Lose
                    break;
                }

                m_CurrentFeedback = feedback5B;
                m_CurrentFeedback.done = true;  // Done
                break;
            case 2:
                m_CurrentFeedback = feedback5B;
                m_NextResponse = null;
                m_CurrentFeedback.done = true;  // Done
                break;
        }

        // Show child response 5A/5B
        yield return new WaitForSeconds(responseDelay);
        yield return chatPanel.ShowChildResponse(m_CurrentFeedback.feedback);
        yield return new WaitForSeconds(responseDelay);
        yield return CheckState(m_CurrentFeedback);

        yield return null;
    }

    IEnumerator CheckState(Feedback feedback)
    {
        if (feedback.done)
        {
            chatPanel.Done();
            gameObject.SetActive(false);
            GameManager.isTempPause = false;
            GameManager.isBagDone = true;

            //If Score is 8 (Max score for Bag Story && Achievement Not Completed Then
            if((GameManager.CalculateScore(0) == 8) && (DataManager.ReadIntData(DataManager.acOne) == 0))
                DataManager.StoreIntData(DataManager.acOne, 1);

            yield break;
        }

        if (feedback.gameOver)
        {
            chatPanel.Lose();
            gameObject.SetActive(false);
            GameManager.isTempPause = false;
            GameManager.isBagDone = true;
            GameManager.lvlPositivity--; //Deduct 1
            GameManager.UpdateNegativeBar();
            GameManager.CheckifGameOver();

            yield break;
        }
    }

    Response GetCurrentResponse(ParentResponse parentResponse)
    {
        Response response;
        response = chatPanel.GetResponseIndex == 1 ? parentResponse.response1 : parentResponse.response2;
        return response;
    }

    void AddPotentialScore()
    {
        switch (chatPanel.GetResponseIndex) //Obtain Possible Reward for Parent1 Selection
        {
            case 1:
                for (int i = 0; i < m_NextResponse.response1.skillsEarned.Length; i++)
                    GameManager.gameReward[m_NextResponse.response1.skillsEarned[i] - 1] += 1;
                break;
            case 2:
                for (int i = 0; i < m_NextResponse.response2.skillsEarned.Length; i++)
                    GameManager.gameReward[m_NextResponse.response2.skillsEarned[i] - 1] += 1;
                break;
        }
    }
}
