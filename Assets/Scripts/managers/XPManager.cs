using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    [SerializeField] AnimationCurve experienceCurve;
    int currentLevel, totalExperience;
    int previousLevelExperience, nextLevelsExperience;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceBar;
    public static XPManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one");

        }
        Instance = this;
    }

    public void Start()
    {
        UpdateLevel();
        UpdateInterface();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) //TODO REMOVE
        {
            AddExperience(10); // Add 10 XP when E is pressed
        }
    }
    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    private void CheckForLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
        }
    }

    public void UpdateLevel()
    {
        previousLevelExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    public void UpdateInterface()
    {
        int start = totalExperience - previousLevelExperience;
        int end = nextLevelsExperience - previousLevelExperience;

        levelText.text = currentLevel.ToString();
        experienceText.text = $"{start} / {end}";
        experienceBar.fillAmount = (float)start / (float)end;
    }
}
