using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GamePlay.Script
{
    public class LogicScript : MonoBehaviour
    {
        // Исходные поля (были в первоначальной версии)
        public static LogicScript Instance;
        public TMP_Text scoreText;
        public TMP_Text comboText;
        public GameObject progressBar;
        public AudioSource audioSource;
        public AudioClip missSound;

        // Добавленные поля (новые)
        [Header("Effects")]
        public GameObject score100Prefab;
        public GameObject score50Prefab;
        public GameObject missXPrefab;
        public List<GameObject> starPrefabs = new List<GameObject>();

        [Header("References")]
        public Transform shieldTransform; // Позиция щита для спавна звезд (добавлено)

        // Модифицированные/добавленные приватные поля
        private GameObject currentEffect;
        private int score = 0;
        private int combo = 0;
        private int maxCombo = 0;
        private bool visualEffectsEnabled = true; // Флаг включения визуальных эффектов (добавлено)

        private void Awake()
        {
            // Исходная реализация синглтона
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Добавлено: загрузка состояния визуальных эффектов
            visualEffectsEnabled = PlayerPrefs.GetInt("VisualEffectsEnabled", 1) == 1;

            // Добавлено: автоматический поиск щита, если не назначен
            if (shieldTransform == null)
            {
                GameObject shield = GameObject.FindGameObjectWithTag("Shield");
                if (shield != null) shieldTransform = shield.transform;
            }
        }

        // Основной метод добавления очков (модифицирован)
        public void AddScore(float distance)
        {
            // Исходная логика расчета очков
            if (distance < 0.7f)
            {
                score += 100 + combo++;
                ShowScoreEffect(score100Prefab); // Добавлено: визуальный эффект
                CreateStars(); // Добавлено: создание звезд
            }
            else
            {
                if (combo > maxCombo)
                    maxCombo = combo;
                combo = 0;
                score += 50;
                ShowScoreEffect(score50Prefab); // Добавлено: визуальный эффект
            }
            UpdateScore(); // Исходный метод
        }

        // Добавлено: метод для отображения эффекта промаха
        public void ShowMissEffect()
        {
            ReplaceEffect(missXPrefab);

            // Воспроизведение звука промаха (добавлено)
            if (audioSource != null && missSound != null)
            {
                audioSource.PlayOneShot(missSound);
            }
        }

        // Добавлено: общий метод для эффектов счета
        private void ShowScoreEffect(GameObject prefab)
        {
            ReplaceEffect(prefab);
        }

        // Добавлено: универсальный метод для замены эффектов
        private void ReplaceEffect(GameObject newEffectPrefab)
        {
            // Удаляем предыдущий эффект
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }

            // Создаем новый эффект по центру экрана
            Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
            currentEffect = Instantiate(newEffectPrefab, center, Quaternion.identity);

            // Автоудаление через 0.5 секунды
            StartCoroutine(DestroyAfterDelay(currentEffect, 0.5f));
        }

        // Добавлено: корутина для удаления эффекта
        private IEnumerator DestroyAfterDelay(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (effect != null)
            {
                Destroy(effect);
            }
        }

        // Добавлено: установка громкости звуков (реализация интерфейса)
        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        // Добавлено: установка состояния визуальных эффектов
        public void SetVisualEffectsEnabled(bool enabled)
        {
            visualEffectsEnabled = enabled;
        }

        // Добавлено: создание звезд при идеальном попадании
        private void CreateStars()
        {
            if (!visualEffectsEnabled) return; // Проверка флага

            if (shieldTransform == null) return;

            int starCount = UnityEngine.Random.Range(1, 12);
            Vector3 shieldPos = shieldTransform.position;

            for (int i = 0; i < starCount; i++)
            {
                if (starPrefabs.Count == 0) break;

                GameObject starPrefab = starPrefabs[UnityEngine.Random.Range(0, starPrefabs.Count)];
                GameObject star = Instantiate(starPrefab, shieldPos, Quaternion.identity);
                star.AddComponent<StarEffect>(); // Добавляем компонент эффекта
            }
        }

        // Исходный метод завершения песни (без изменений)
        public void EndSong()
        {
            // Обновление рекордов
            bool newRecord = false;
            for (int i = 0; i < Date.Records.Length; i++)
            {
                if (score > Date.Records[i])
                {
                    for (int j = Date.Records.Length - 1; j > i; j--)
                    {
                        Date.Records[j] = Date.Records[j - 1];
                    }
                    Date.Records[i] = score;
                    newRecord = true;
                    break;
                }
            }

            Date.PreviousScore = score;
            Date.Combo = maxCombo;

            SaveRecords();
            SceneManager.LoadScene("Result");
        }

        // Исходный метод обновления прогресс-бара (без изменений)
        public void UpdateProgressBar(float value)
        {
            if (progressBar != null)
            {
                Slider slider = progressBar.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value = value;
                }
            }
        }

        // Исходный метод обновления UI счета (без изменений)
        private void UpdateScore()
        {
            if (scoreText != null)
                scoreText.text = score.ToString("0000000");

            if (comboText != null)
                comboText.text = "X" + combo;
        }

        // Модифицированный метод сохранения рекордов
        private void SaveRecords()
        {
            // Изменено: сохранение без JSON
            for (int i = 0; i < Date.Records.Length; i++)
            {
                PlayerPrefs.SetInt("Record_" + i, Date.Records[i]);
            }
            PlayerPrefs.Save(); // Добавлено явное сохранение
        }
    }
}