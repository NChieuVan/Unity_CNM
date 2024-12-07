using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : MonoBehaviour
{

    public int[] levelsStatus; // Mảng lưu trạng thái màn chơi

    // Constructor mặc định
    public LevelData()
    {
        levelsStatus = new int[10]; // Khởi tạo mặc định với 10 màn chơi, có thể thay đổi
    }

    // Constructor để khởi tạo mảng levelsStatus
    public LevelData(int numberOfLevels)
    {
        levelsStatus = new int[numberOfLevels]; // Khởi tạo mảng với kích thước được chỉ định
    }
}
