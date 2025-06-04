using UnityEngine;
using System.Collections;

// Полностью новый скрипт для анимации звезд
public class StarEffect : MonoBehaviour
{
    public float lifeTime = 1f; // Время жизни эффекта
    private Vector3 randomDirection; // Направление движения

    void Start()
    {
        // Генерация случайного направления
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        transform.localScale = Vector3.zero; // Начальный размер - 0
        StartCoroutine(Animate()); // Запуск анимации
    }

    IEnumerator Animate()
    {
        float timer = 0;

        // Анимация в течение времени жизни
        while (timer < lifeTime)
        {
            // Плавное увеличение размера
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / lifeTime);

            // Движение в случайном направлении
            transform.position += randomDirection * 2f * Time.deltaTime;

            // Вращение
            transform.Rotate(0, 0, 180 * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // Уничтожение объекта после завершения анимации
        Destroy(gameObject);
    }
}