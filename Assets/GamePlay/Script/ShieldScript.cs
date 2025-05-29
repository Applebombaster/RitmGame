using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace GamePlay.Script
{
    public class ShieldScript : MonoBehaviour
    {
        public Queue<GameObject> touchsObject = new Queue<GameObject>();
        public LogicScript logic;
        private SpriteRenderer spriteRenderer;

        [Header("Shield Sprites")]
        public Sprite normalShield;
        public Sprite newShield;
        public string[] inputsKey = { "x", "z" };

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.position = new Vector3(Date.RadiusCircle, 0, 0);
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            // Удалено: touch = false; (переменная touch больше не используется)
            spriteRenderer.sprite = normalShield;
        }

        private void Update()
        {
            // Проверяем, есть ли объекты в очереди и было ли нажатие
            if (touchsObject.Count != 0 && (inputsKey.Any(key => Input.GetKeyDown(key)) || Input.GetMouseButtonDown(0)))
            {
                StartCoroutine(ShieldHitEffect());

                // Получаем первый объект из очереди
                GameObject touchObject = touchsObject.Dequeue();

                // Проверяем, что объект не был уничтожен
                if (touchObject == null) return;

                // Рассчитываем дистанцию
                float distance = (transform.position - touchObject.transform.position).magnitude;
                logic.AddScore(distance);
                Destroy(touchObject);
            }
        }

        private IEnumerator ShieldHitEffect()
        {
            spriteRenderer.sprite = newShield;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = normalShield;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            touchsObject.Enqueue(other.gameObject);
            Debug.Log("Note added. Queue count: " + touchsObject.Count);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Создаем временный список для безопасного удаления
            List<GameObject> tempList = new List<GameObject>(touchsObject);

            // Удаляем объект, если он есть в очереди
            if (tempList.Contains(other.gameObject))
            {
                tempList.Remove(other.gameObject);
                touchsObject = new Queue<GameObject>(tempList);
                Debug.Log("Note removed. Queue count: " + touchsObject.Count);
            }
        }
    }
}