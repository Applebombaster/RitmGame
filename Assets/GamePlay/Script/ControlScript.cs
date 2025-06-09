using UnityEngine;

namespace GamePlay.Script
{
    public class ControlScript : MonoBehaviour
    {
        public static bool isPause;
        public static bool isCount = false;
        public bool First;
        public float? previousRotate = null;
        public bool isClockwise;
        public bool flag;

        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (isPause) return;
            var position = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
            var rotation = Mathf.Atan(position.y / position.x) * 180 / Mathf.PI;
            if (position.x < 0)
                rotation += 180;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        public bool CountRotation()
        {
            var rotation = transform.rotation.eulerAngles.z;
            if (First)
            {
                if (previousRotate is null)
                {
                    previousRotate = rotation;
                    return true;
                }

                isClockwise = previousRotate < rotation;
                First = false;
            }
            else
            {
                if (isClockwise)
                {
                    if (Mathf.Abs(Mathf.Abs((float)previousRotate) - Mathf.Abs(rotation)) < 10)
                    {
                        flag = true;
                        return flag;
                    }
                    flag = flag && previousRotate < rotation;
                }
                else
                {
                    if (Mathf.Abs(Mathf.Abs((float)previousRotate) - Mathf.Abs(rotation)) < 10)
                    {
                        flag = true;
                        return flag;
                    }
                    flag = flag && previousRotate > rotation;
                }
            }

            return flag;
        }
    }
}