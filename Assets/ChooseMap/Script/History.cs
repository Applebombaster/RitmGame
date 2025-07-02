using UnityEngine;
using GamePlay.Script;

public class TableSpawner : MonoBehaviour
{
    public GameObject tablePrefab;
    public int numberOfTables;
    public float spacing = 10f;

    void Start()
    {
        LoadRecords();
        for (var i = 0; i < Date.Records.Length; i++)
            if (Date.Records[i] == 0)
            {
                numberOfTables = i;
                break;
            }

        SpawnTables();
    }

    private void SpawnTables()
    {
        var parentRectTransform = GetComponent<RectTransform>();

        for (var i = 0; i < numberOfTables; i++)
        {
            var position = new Vector3(0, -i * spacing, 0);
            var table = Instantiate(tablePrefab, parentRectTransform);
            table.GetComponent<RectTransform>().anchoredPosition = position;
            table.GetComponent<tableScript>().UpdateScore(i);
        }
    }

    private void LoadRecords()
    {
        var listJson = PlayerPrefs.GetString("sunset");
        Date.Records = JsonUtility.FromJson<SupportClass<int>>(listJson).Item;
    }
}