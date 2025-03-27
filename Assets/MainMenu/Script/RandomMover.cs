using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public float moveSpeed = 1f; // �������� ��������
    public float rotationSpeed = 50f; // �������� ��������
    private Vector2 direction;

    void Start()
    {
        // ������ ��������� �����������
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        // ������� ������
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // ������� ������
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // �������� ������� �������
        RectTransform canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // �������� ������� �������
        RectTransform rectTransform = GetComponent<RectTransform>();
        float objectWidth = rectTransform.rect.width / 2;
        float objectHeight = rectTransform.rect.height / 2;

        // ��������� ������������ � ��������� �������
        if (transform.localPosition.x < -canvasSize.x / 2 + objectWidth || transform.localPosition.x > canvasSize.x / 2 - objectWidth ||
            transform.localPosition.y < -canvasSize.y / 2 + objectHeight || transform.localPosition.y > canvasSize.y / 2 - objectHeight)
        {
            // ����������� � ��������� �����������
            direction = Random.insideUnitCircle.normalized;
            // ���������� ������ ������� ������ �������, ����� �������� �����������
            transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, -canvasSize.x / 2 + objectWidth, canvasSize.x / 2 - objectWidth),
                                                   Mathf.Clamp(transform.localPosition.y, -canvasSize.y / 2 + objectHeight, canvasSize.y / 2 - objectHeight));
        }
    }
}