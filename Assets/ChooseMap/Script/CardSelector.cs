using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // Добавляем пространство имен для работы с видео

public class CardSelector : MonoBehaviour
{
    public GameObject cardPrefab; // Префаб карты
    public Transform content; // Контейнер для карт
    public string[] cardNames; // Названия карт
    public ScrollRect scrollRect; // Ссылка на ScrollRect

    [Header("Background Settings")]
    public GameObject mapBackground; // Объект с фоновым изображением (Map)
    public VideoPlayer videoBackground; // Компонент VideoPlayer на объекте Video

    private int _firstCardIndex = 0; // Индекс первой карточки (уровня) а зачем, шутка


    void Start()
    {
        // ... существующий код ...

        // Инициализация видео
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
            Debug.LogError("Card Prefab не назначен!");
            return;
        }

        if (content == null)
        {
            Debug.LogError("Content не назначен!");
            return;
        }

        foreach (var cardName in cardNames)
        {
            GameObject card = Instantiate(cardPrefab, content);

            // Попробуем получить компонент Text
            Text cardText = card.GetComponentInChildren<Text>();
            if (cardText != null)
            {
                cardText.text = cardName; // Устанавливаем название карты, если компонент найден
            }
            else
            {
                Debug.LogWarning("Компонент Text не найден на карточке: " + cardName);
            }

            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnCardSelected(cardName)); // Добавляем обработчик нажатия
            }

            // Добавляем обработчики событий для наведения мыши
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

        // Устанавливаем позицию Scroll View, чтобы первый элемент был в центре
        CenterScrollView();
    }

    void Update()
    {
        // Проверяем, прокручивается ли колесико мыши
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // Прокручиваем ScrollRect в зависимости от ввода
            scrollRect.verticalNormalizedPosition += scrollInput;
        }
    }

    void CenterScrollView()
    {
        if (content.childCount > 0)
        {
            // Получаем RectTransform первого элемента
            RectTransform firstCardRect = content.GetChild(0).GetComponent<RectTransform>();

            // Получаем высоту контента и высоту карточки
            float contentHeight = content.GetComponent<RectTransform>().rect.height;
            float cardHeight = firstCardRect.rect.height;

            // Вычисляем целевую позицию для центрирования
            float targetPosition = (cardHeight / 2) - (contentHeight / 2);
            float scrollPosition = firstCardRect.anchoredPosition.y + targetPosition;

            // Устанавливаем позицию прокрутки
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollPosition);
        }
    }

    void OnCardSelected(string cardName)
    {
        Debug.Log("Выбрана карта: " + cardName);
    }

    void OnPointerEnter(GameObject card)
    {
        // Увеличиваем размер карточки
        card.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
        card.transform.localPosition += new Vector3(-180f, 0f, 0f);

        // Проверяем, является ли карточка первой
        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // Включаем видео только для первой карточки
            if (mapBackground != null) mapBackground.SetActive(false);
            if (videoBackground != null)
            {
                videoBackground.gameObject.SetActive(true);
                videoBackground.Play();
            }
        }
    }

    void OnPointerExit(GameObject card)
    {
        // Возвращаем размер карточки
        card.transform.localScale = new Vector3(1f, 1f, 1f);
        card.transform.localPosition -= new Vector3(-180f, 0f, 0f);

        // Проверяем, является ли карточка первой
        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // Выключаем видео только для первой карточки
            if (videoBackground != null)
            {
                videoBackground.Stop();
                videoBackground.gameObject.SetActive(false);
            }
            if (mapBackground != null) mapBackground.SetActive(true);
        }
    }
}