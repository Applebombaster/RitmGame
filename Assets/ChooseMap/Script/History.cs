using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableSpawner : MonoBehaviour
{
    public GameObject tablePrefab; // ������ table
    public int numberOfTables = 5; // ���������� ��������
    public float spacing = 10f; // ���������� ����� ���������

    void Start()
    {
        SpawnTables();
    }

    void SpawnTables()
    {
        // �������� RectTransform �������� �������
        RectTransform parentRectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < numberOfTables; i++)
        {
            // ������������ ������� ��� ������� �������
            Vector3 position = new Vector3(0, -i * spacing, 0); // �������� �������� �� ���� �����, ����� ���������� ����
            GameObject tableInstance = Instantiate(tablePrefab, parentRectTransform);
            tableInstance.GetComponent<RectTransform>().anchoredPosition = position; // ������������� �������
        }
    }
}