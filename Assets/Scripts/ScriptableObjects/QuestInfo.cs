using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest Info", order = 1)]
public class QuestInfo : ScriptableObject
{
    [field : SerializeField] public string id { get; private set; }
    public string displayName;
    public int levelRequired;
    public QuestInfo[] questRequirements;
    public GameObject[] questStepPrefab;
    public int goldReward;
    public int expReward;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
