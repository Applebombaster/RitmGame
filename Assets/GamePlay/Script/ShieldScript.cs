using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

namespace GamePlay.Script
{
    public class ShieldScript : MonoBehaviour
    {
        public Queue<GameObject> touchsObject = new();
        public LogicScript logic;
        private SpriteRenderer spriteRenderer;
        public ControlScript control;

        [Header("Shield Sprites")] public Sprite normalShield;
        public Sprite newShield;
        public string[] inputsKey = { "x", "z" };
        public GameObject touchLongNote;
        public bool pressedLongNote;
        public GameObject touchSpinner;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.position = new Vector3(Date.RadiusCircle, 0, 0);
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            control = GameObject.FindGameObjectWithTag("Center").GetComponent<ControlScript>();
            spriteRenderer.sprite = normalShield;
        }

        private void Update()
        {
            if (touchsObject.Count != 0 && (inputsKey.Any(key => Input.GetKeyDown(key)) || Input.GetMouseButtonDown(0)))
            {
                StartCoroutine(ShieldHitEffect());
                var touchObject = touchsObject.Dequeue();
                if (!touchObject) return;
                var distance = (transform.position - touchObject.transform.position).magnitude;
                logic.AddScore(distance);
                Destroy(touchObject);
            }
            else if (touchLongNote)
            {
                if (inputsKey.Any(Input.GetKeyDown) || Input.GetMouseButtonDown(0))
                {
                    pressedLongNote = true;
                    var distance = (transform.position - touchLongNote.transform.GetChild(2).transform.position)
                        .magnitude;
                    logic.AddScore(distance);
                    StartCoroutine(ShieldHitEffect());
                }

                if (pressedLongNote && (inputsKey.Any(Input.GetKeyUp) || Input.GetMouseButtonUp(0)))
                {
                    var distance = (transform.position - touchLongNote.transform.GetChild(0).transform.position)
                        .magnitude;
                    logic.AddScore(distance);
                    StartCoroutine(ShieldHitEffect());
                    pressedLongNote = false;
                    touchLongNote = null;
                }
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
            if (other.tag.Equals("Note"))
                touchsObject.Enqueue(other.gameObject);
            else if (other.transform.parent.tag.Equals("LongNote") && other.tag.Equals("First"))
                touchLongNote = other.transform.parent.gameObject;
            else if (other.tag.Equals("Spinner"))
                touchSpinner = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Создаем временный список для безопасного удаления
            var tempList = new List<GameObject>(touchsObject);

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