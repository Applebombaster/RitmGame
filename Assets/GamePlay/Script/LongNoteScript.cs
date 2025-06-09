using UnityEngine;

namespace GamePlay.Script
{
    public class LongNoteScript : MonoBehaviour
    {
        public Rigidbody2D myRigidbody2D;
        private float speed = 6;
        private float time;
        private int stage;
        private Transform[] children;
        private float targetScale = 1;
        private float rot;

        private void Start()
        {
            transform.localScale = new Vector3(0, 0, 0);
            children = new[] { transform.GetChild(0), transform.GetChild(1), transform.GetChild(2) };
            var rotation = GameObject.FindGameObjectWithTag("Center").transform.rotation.eulerAngles.z;
            var rotate = rotation + Random.Range(-60, 60);
            rot = rotate / 180 * Mathf.PI;
            transform.rotation = Quaternion.Euler(0, 0, rotate);
        }

        private void Update()
        {
            switch (stage)
            {
                case 0:
                    Emerging();
                    break;
                case 1:
                    Scaling();
                    break;
                case 2:
                    Moving();
                    break;
            }
        }

        private void Emerging()
        {
            if (targetScale > transform.localScale.x)
                transform.localScale += (new Vector3(1f, 1f, 11f) * Time.deltaTime);
            else
                stage = 1;
        }

        private void Scaling()
        {
            
            children[2].localPosition += (Vector3.right * (Time.deltaTime * speed));
            var scale = children[2].localPosition.x / 4;
            children[1].localPosition += (Vector3.right * (Time.deltaTime * speed / 2));
            children[1].localScale = new Vector3(scale, 1, 1);
            if (scale > 3)
                stage = 2;

        }

        private void Moving()
        {
            myRigidbody2D.velocity = new Vector2(Mathf.Cos(rot) * speed, Mathf.Sin(rot) * speed);
        }
    }
}