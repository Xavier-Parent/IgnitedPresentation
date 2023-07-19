using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private Transform m_PlayerPosition;
    [SerializeField]
    private float m_Distance;
    [SerializeField]
    private NPCData m_Data;
    [SerializeField]
    private TMP_Text m_NpcName;
    [SerializeField]
    private List<string> m_Sentences;

    [SerializeField]
    private TMP_Text m_DialogueText;

    [SerializeField]
    private GameObject m_Indicator;
    [SerializeField]
    private GameObject m_Window;
    [SerializeField]
    private float m_TypingSpeed;

    private int m_Index;
    private int m_CharacterIndex;
    private bool m_DialogueStarted;

    private void Start()
    {
        m_NpcName.text = m_Data.NpcName;
        m_Sentences = m_Data.Sentences;
    }
    private void Update()
    {
        DistanceBetween();
        if (Input.GetKeyDown(KeyCode.Space) && m_DialogueStarted == true)
        {
            m_Index++;
            if (m_Index < m_Sentences.Count)
            {
                GetDialogue(m_Index);
            }
            else
            {
                EndDialogue();
            }
        }
    }
    private void DistanceBetween()
    {
        if (Vector3.Distance(transform.position, m_PlayerPosition.transform.position) <= m_Distance)
        {
            m_Indicator.transform.LookAt(m_Indicator.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            m_Indicator.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
        }
        else
        {
            m_Indicator.SetActive(false);
        }
    }

    public void ToggleWindow(bool m_Show)
    {
        m_Window.SetActive(m_Show);
    }

    public void StartDialogue()
    {
        if (m_DialogueStarted)
            return;

        m_DialogueStarted = true;
        ToggleWindow(true);
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
        m_Index = i;
        m_CharacterIndex = 0;
        StartCoroutine(TypingText());
        m_DialogueText.text = "";
    }

    public void EndDialogue()
    {
        m_DialogueStarted = false;
        StopCoroutine(TypingText());
        ToggleWindow(false);
    }

    IEnumerator TypingText()
    {
        string m_CurrentDialogue = m_Sentences[m_Index];
        m_DialogueText.text += m_CurrentDialogue[m_CharacterIndex];
        m_CharacterIndex++;

        if (m_CharacterIndex < m_CurrentDialogue.Length)
        {
            yield return new WaitForSeconds(m_TypingSpeed);
            StartCoroutine(TypingText());
        }
    }

}
