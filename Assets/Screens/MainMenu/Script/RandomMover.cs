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

        // ��������� ������������ � ��������� ������
        if (transform.position.x < -Screen.width / 2 || transform.position.x > Screen.width / 2 ||
            transform.position.y < -Screen.height / 2 || transform.position.y > Screen.height / 2)
        {
            // ����������� � ��������� �����������
            direction = Random.insideUnitCircle.normalized;
            // ���������� ������ ������� ������ ������, ����� �������� �����������
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -Screen.width / 2 + 1, Screen.width / 2 - 1),
                                              Mathf.Clamp(transform.position.y, -Screen.height / 2 + 1, Screen.height / 2 - 1));
        }
    }
}