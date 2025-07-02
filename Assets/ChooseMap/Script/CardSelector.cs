using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // ��������� ������������ ���� ��� ������ � �����

public class CardSelector : MonoBehaviour
{
    public Dictionary<string, string> nameVideo = new Dictionary<string, string>()
    {
        { "Card", "Phone1" },
        { "Card (1)", "Phone2" },
        { "Card (2)", "Phone3" },
        { "Card (3)", "Phone4" }
    };

    public GameObject cardPrefab; // ������ �����
    public Transform content; // ��������� ��� ����
    public string[] cardNames; // �������� ����
    public ScrollRect scrollRect; // ������ �� ScrollRect

    [Header("Background Settings")] public GameObject mapBackground; // ������ � ������� ������������ (Map)
    public VideoPlayer videoBackground; // ��������� VideoPlayer �� ������� Video

    private int _firstCardIndex = 0; // ������ ������ �������� (������) � �����, �����


    void Start()
    {
        if (videoBackground != null)
        {
            videoBackground.Stop();
            videoBackground.gameObject.SetActive(false);
        }
    }

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

        var i = 0;
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
            i++;
        }

        // ������������� ������� Scroll View, ����� ������ ������� ��� � ������
        CenterScrollView();
    }

    void Update()
    {
        // ���������, �������������� �� �������� ����
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            scrollRect.verticalNormalizedPosition += scrollInput;
        }
    }

    void CenterScrollView()
    {
        if (content.childCount > 0)
        {
            // �������� RectTransform ������� ��������
            RectTransform firstCardRect = content.GetChild(0).GetComponent<RectTransform>();

            // �������� ������ �������� � ������ ��������
            float contentHeight = content.GetComponent<RectTransform>().rect.height;
            float cardHeight = firstCardRect.rect.height;

            // ��������� ������� ������� ��� �������������
            float targetPosition = (cardHeight / 2) - (contentHeight / 2);
            float scrollPosition = firstCardRect.anchoredPosition.y + targetPosition;

            // ������������� ������� ���������
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollPosition);
        }
    }

    void OnCardSelected(string cardName)
    {
        Debug.Log("������� �����: " + cardName);
    }

    void OnPointerEnter(GameObject card)
    {
        // ����������� ������ ��������
        card.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
        card.transform.localPosition += new Vector3(-180f, 0f, 0f);

        // ���������, �������� �� �������� ������
        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // �������� ����� ������ ��� ������ ��������
            // if (mapBackground != null) mapBackground.SetActive(false);
            // if (videoBackground != null)
            // {
                // videoBackground.gameObject.SetActive(true);
                // videoBackground.Play();
                // videoBackground.clip = Resources.Load<VideoClip>(nameVideo[gameObject.name]);
            // }
        }
    }

    void OnPointerExit(GameObject card)
    {
        // ���������� ������ ��������
        card.transform.localScale = new Vector3(1f, 1f, 1f);
        card.transform.localPosition -= new Vector3(-180f, 0f, 0f);

        // ���������, �������� �� �������� ������
        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // ��������� ����� ������ ��� ������ ��������
            // if (videoBackground != null)
            {
                // videoBackground.Stop();
                // videoBackground.gameObject.SetActive(false);
            }

            // if (mapBackground != null) mapBackground.SetActive(true);
        }
    }
}