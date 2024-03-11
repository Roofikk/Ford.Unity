using Ford.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class SavesPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect _savesScrollRect;
    [SerializeField] private SaveElementUI _savePrefab;

    [Space(15)]
    [SerializeField] private SavePanel _editSavePage;

    private ToggleGroup _toggleGroup;
    public SaveData SelectedHorseSave { get; private set; }

    private void Awake()
    {
        if (_toggleGroup == null)
            _toggleGroup = GetComponent<ToggleGroup>();
    }

    public void FillSaves(SaveData[] saves)
    {
        if (_toggleGroup == null)
            _toggleGroup = GetComponent<ToggleGroup>();

        CleanSavesObject();

        foreach (var saveData in saves)
        {
            SaveElementUI saveObject = Instantiate(_savePrefab.gameObject, _savesScrollRect.content.transform).GetComponent<SaveElementUI>();
            saveObject.Initiate(saveData, _toggleGroup);
            _editSavePage.HorseUpdated += saveObject.UpdateInfo;
        }
    }

    private void OpenEditSavePage(SaveData saveData)
    {
        PageManager.Instance.OpenPage(_editSavePage, saveData, 1);

    }

    public void CleanSavesObject()
    {
        foreach (Transform t in _savesScrollRect.content.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
