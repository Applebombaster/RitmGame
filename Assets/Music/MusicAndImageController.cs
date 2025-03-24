using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicAndImageController : MonoBehaviour
{
    public AudioSource audioSource; // ���������� ��� �����
    public Image imageToShow; // ���������� ��� �����������

    void Start()
    {
        // ��������� ������
        audioSource.Play();
        // ��������� �������� ��� ������ �����������
        StartCoroutine(ShowImageAfterDelay(46f));
    }

    IEnumerator ShowImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ���������� �����������
        imageToShow.gameObject.SetActive(true);
    }
}