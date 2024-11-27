using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TagertLevel : MonoBehaviour
{
    public List<GameObject> Tagert; // Danh sách các cube

    void Start()
    {
        // Ẩn tất cả các cube khi bắt đầu
        SetTagertActive(false);

        // Lấy cấp độ đã chọn và hiển thị cube tương ứng
        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        ShowCubesForLevel(selectedLevel);

    }



    public void ShowCubesForLevel(int level)
    {
        // Ẩn tất cả các cube
        SetTagertActive(false);

        // Hiển thị cube tương ứng với level
        if (level > 0 && level <= Tagert.Count)
        {
            Tagert[level - 1].SetActive(true); // Hiển thị cube cho cấp độ đã chọn
        }
    }

    private void SetTagertActive(bool isActive)
    {
        foreach (var cube in Tagert)
        {
            cube.SetActive(isActive);
        }
    }
}