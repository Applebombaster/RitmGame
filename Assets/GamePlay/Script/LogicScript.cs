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
        public static LogicScript Instance;

        [Header("UI Elements")]
        public TMP_Text scoreText;
        public TMP_Text comboText;
        public GameObject progressBar;

        [Header("Effects")]
        public GameObject score100Prefab;
        public GameObject score50Prefab;
        public GameObject missXPrefab;
        public List<GameObject> starPrefabs = new List<GameObject>();

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip missSound;

        private GameObject currentEffect;
        private int score = 0;
        private int combo = 0;
        private int maxCombo = 0;
        private bool visualEffectsEnabled = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Загрузка настроек визуальных эффектов
            visualEffectsEnabled = PlayerPrefs.GetInt("VisualEffectsEnabled", 1) == 1;
        }

        public void AddScore(float distance)
        {
            if (distance < 0.7f)
            {
                score += 100 + combo++;
                ShowScoreEffect(score100Prefab);
                CreateStars();
            }
            else
            {
                if (combo > maxCombo)
                    maxCombo = combo;
                combo = 0;
                score += 50;
                ShowScoreEffect(score50Prefab);
            }
            UpdateScore();
        }

        public void ShowMissEffect()
        {
            ReplaceEffect(missXPrefab);

            // Воспроизводим звук
            if (audioSource != null && missSound != null)
            {
                audioSource.PlayOneShot(missSound);
            }
        }

        private void ShowScoreEffect(GameObject prefab)
        {
            ReplaceEffect(prefab);
        }

        // Универсальный метод для замены эффектов
        private void ReplaceEffect(GameObject newEffectPrefab)
        {
            // Удаляем предыдущий эффект, если он есть
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }

            // Создаем новый эффект
            Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
            currentEffect = Instantiate(newEffectPrefab, center, Quaternion.identity);

            // Автоматическое удаление через 0.5 сек
            StartCoroutine(DestroyAfterDelay(currentEffect, 0.5f));
        }

        // Корутина для удаления эффекта
        private IEnumerator DestroyAfterDelay(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (effect != null)
            {
                Destroy(effect);
            }
        }

        // Реализация интерфейсов
        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        public void SetVisualEffectsEnabled(bool enabled)
        {
            visualEffectsEnabled = enabled;
            Debug.Log($"Visual effects set to: {enabled}");
        }

        private void CreateStars()
        {
            if (!visualEffectsEnabled) return;

            int starCount = UnityEngine.Random.Range(1, 12);
            GameObject shield = GameObject.FindGameObjectWithTag("Shield");
            if (shield == null) return;

            Vector3 shieldPos = shield.transform.position;

            for (int i = 0; i < starCount; i++)
            {
                if (starPrefabs.Count == 0) break;

                GameObject starPrefab = starPrefabs[UnityEngine.Random.Range(0, starPrefabs.Count)];
                GameObject star = Instantiate(starPrefab, shieldPos, Quaternion.identity);
                star.AddComponent<StarEffect>();
            }
        }

        public void EndSong()
        {
            // Обновляем рекорды
            bool newRecord = false;
            for (int i = 0; i < Date.Records.Length; i++)
            {
                if (score > Date.Records[i])
                {
                    // Сдвигаем рекорды
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

            // Сохраняем рекорды
            SaveRecords();
            SceneManager.LoadScene("Result");
        }

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

        private void UpdateScore()
        {
            if (scoreText != null)
                scoreText.text = score.ToString("0000000");

            if (comboText != null)
                comboText.text = "X" + combo;
        }

        private void SaveRecords()
        {
            // Сохраняем рекорды через PlayerPrefs
            for (int i = 0; i < Date.Records.Length; i++)
            {
                PlayerPrefs.SetInt("Record_" + i, Date.Records[i]);
            }
            PlayerPrefs.Save();
        }
    }
}