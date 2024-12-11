using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private string filePath;
    private LevelData levelData; // Biến lưu trạng thái các màn chơi
    public GameObject lv1,lv2,lv3,lv4,lv5,lv6,lv7,lv8,lv9,lv10;
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "levelData.json");
        Debug.Log($"Đường dẫn file JSON: {filePath}");
        print("Start ....");
        LoadLevels();
        PrintLevelsStatus();
        SetLevelActive();
    }

    public void SetLevelActive()
    {
        //int[] lv = GetLevelsStatus();
        if (levelData.levelsStatus[0]== 1)
        {
            lv1.SetActive(false);
        }
        if (levelData.levelsStatus[1] == 1)
        {
            lv2.SetActive(false);
        }
        if (levelData.levelsStatus[2] == 1)
        {
            lv3.SetActive(false);
        }
        if (levelData.levelsStatus[3] == 1)
        {
            lv4.SetActive(false);
        }
        if (levelData.levelsStatus[4] == 1)
        {
            lv5.SetActive(false);
        }
        if (levelData.levelsStatus[5] == 1)
        {
            lv6.SetActive(false);
        }
        if (levelData.levelsStatus[6] == 1)
        {
            lv7.SetActive(false);
        }
        if (levelData.levelsStatus[7] == 1)
        {
            lv8.SetActive(false);
        }
        if (levelData.levelsStatus[8] == 1)
        {
            lv9.SetActive(false);
        }
        if (levelData.levelsStatus[9] == 1)
        {
            lv10.SetActive(false);
        }
    }

    public void SaveLevels()
    {
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadLevels()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"JSON Read: {json}"); // Kiểm tra nội dung JSON

            // Khởi tạo levelData nếu chưa có
            if (levelData == null)
            {
                Debug.Log("File null");
                levelData = new LevelData(); // Hoặc khởi tạo với số lượng màn chơi
            }

            JsonUtility.FromJsonOverwrite(json, levelData);
            Debug.Log("Đọc thành công");
        }
        else
        {
            Debug.Log("File không tồn tại, khởi tạo mặc định.");
            int numberOfLevels = 10; // Thay đổi số lượng màn chơi nếu cần
            levelData = new LevelData(numberOfLevels);
            SaveLevels(); // Lưu trạng thái khởi tạo

        }
    }

    public void MarkLevelAsCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelData.levelsStatus.Length)
        {
            // Đánh dấu màn hiện tại là hoàn thành
            levelData.levelsStatus[levelIndex] = 1;

            // Lưu lại trạng thái vào file JSON
            SaveLevels();
            PrintLevelsStatus();
            

           // Debug.Log($"Level {levelIndex + 1} completed.");
        }
        else
        {
            Debug.LogWarning($"Invalid level index: {levelIndex}");
        }
    }


    public int[] GetLevelsStatus()
    {
        return levelData.levelsStatus; // Trả về trạng thái các màn chơi
    }

    public void PrintLevelsStatus()
    {
        Debug.Log("IN File JSon HT");
        for (int i = 0; i < levelData.levelsStatus.Length; i++)
        {
            Debug.Log($"Level {i + 1}: {(levelData.levelsStatus[i] == 1 ? "Completed" : "Not Completed")}");
        }
    }
}