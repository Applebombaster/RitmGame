using UnityEngine;
using UnityEngine.UI;

public class UICursorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Vector2 cursorOffset = new Vector2(10, -10);

    private void Awake()
    {
        // ������ Canvas ��������������
        DontDestroyOnLoad(gameObject);

        // ������� ��������� Image ������ �������
        if (cursorImage == null)
        {
            cursorImage = GetComponentInChildren<Image>();
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        // ��������� �������
        cursorImage.rectTransform.position = Input.mousePosition + (Vector3)cursorOffset;

        // ������������� �������� ��������� ������ ������ ����
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}