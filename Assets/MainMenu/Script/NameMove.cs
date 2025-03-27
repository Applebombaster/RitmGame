using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMover : MonoBehaviour
{
    public float moveDistance = 10f; // ������������ ���������� �����������
    public float moveSpeed = 2f; // �������� ��������
    private Vector2 startPosition;

    void Start()
    {
        // ��������� ��������� �������
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // ��������� ����� �������� �� ������ ���������
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        // ��������� ������� �������
        transform.localPosition = new Vector2(startPosition.x, newY);
    }
}
