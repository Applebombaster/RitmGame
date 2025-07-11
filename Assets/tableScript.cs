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

        // Обновляем только активные записи
        scoreText.text = "Score: " + Date.Records[index].ToString("0000000");

        if (Date.MaxScore > 0)
        {
            float percentage = (float)Date.Records[index] / Date.MaxScore;
            percentText.text = percentage.ToString("#0.##%");
        }
        else
        {
            percentText.text = "0%";
        }
    }
}