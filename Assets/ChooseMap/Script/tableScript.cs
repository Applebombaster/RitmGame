using TMPro;
using UnityEngine;
using GamePlay.Script;

public class tableScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text percentText;
    public GameObject activeElements;

    public void SetRecord(int index, bool isActive)
    {
        // Активируем/деактивируем элементы
        activeElements.SetActive(isActive);

        if (!isActive) return;

        // Получаем рекорды для текущего уровня
        if (!Date.LevelRecords.ContainsKey(Date.CurrentLevel))
        {
            Debug.LogWarning($"No records found for: {Date.CurrentLevel}");
            return;
        }

        int[] records = Date.LevelRecords[Date.CurrentLevel];

        if (index >= records.Length)
        {
            Debug.LogWarning($"Index out of range: {index}");
            return;
        }

        // Обновляем записи
        scoreText.text = "Score: " + records[index].ToString("0000000");

        if (Date.MaxScore > 0)
        {
            float percentage = (float)records[index] / Date.MaxScore;
            percentText.text = percentage.ToString("#0.##%");
        }
        else
        {
            percentText.text = "0%";
        }
    }
}