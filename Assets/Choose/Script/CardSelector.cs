using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    public GameObject cardPrefab; // ������ �����
    public Transform content; // ��������� ��� ����
    public string[] cardNames; // �������� ����

    void LoadCards()
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card Prefab �� ��������!");
            return;
        }

        if (content == null)
        {
            Debug.LogError("Content �� ��������!");
            return;
        }

        foreach (var cardName in cardNames)
        {
            GameObject card = Instantiate(cardPrefab, content);

            // ��������� �������� ��������� Text
            Text cardText = card.GetComponentInChildren<Text>();
            if (cardText != null)
            {
                cardText.text = cardName; // ������������� �������� �����, ���� ��������� ������
            }
            else
            {
                Debug.LogWarning("��������� Text �� ������ �� ��������: " + cardName);
            }

            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnCardSelected(cardName)); // ��������� ���������� �������
            }

            // ��������� ����������� ������� ��� ��������� ����
            EventTrigger eventTrigger = card.AddComponent<EventTrigger>();
            EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
            entryPointerEnter.eventID = EventTriggerType.PointerEnter;
            entryPointerEnter.callback.AddListener((data) => { OnPointerEnter(card); });
            eventTrigger.triggers.Add(entryPointerEnter);

            EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
            entryPointerExit.eventID = EventTriggerType.PointerExit;
            entryPointerExit.callback.AddListener((data) => { OnPointerExit(card); });
            eventTrigger.triggers.Add(entryPointerExit);
        }
    }

    void OnCardSelected(string cardName)
    {
        Debug.Log("������� �����: " + cardName);
    }

    void OnPointerEnter(GameObject card)
    {
        card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); // ����������� ������
    }

    void OnPointerExit(GameObject card)
    {
        card.transform.localScale = new Vector3(1f, 1f, 1f); // ���������� ������
    }
}