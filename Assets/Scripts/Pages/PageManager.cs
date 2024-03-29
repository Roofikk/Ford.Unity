using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private Page _startPage;
    [SerializeField] private WarningPage _warningPage;

    public Page StartPage { get { return _startPage; } }
    public static PageManager Instance { get; private set; }
    public Page CurrentPage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (_startPage != null)
            (CurrentPage = _startPage).Open();
    }

    public void OpenStartPage()
    {
        OpenPage(StartPage);
    }

    public void OpenPage(Page page, int popUpLevel = 0)
    { 
        if (popUpLevel == 0)
            CurrentPage?.Close();

        page.Open(popUpLevel);
        CurrentPage = page;
    }

    public void ClosePage(Page page)
    {
        page.Close();
    }

    public void OpenPage<T>(Page page, T param, int popUpLevel = 0)
    {
        page.Open(param, popUpLevel);
    }

    public void OpenWarningPage(WarningData data, int popUpLevel = 1)
    {
        _warningPage.Open(data, popUpLevel);
    }

    public void CloseWarningPage()
    {
        _warningPage.Close();
    }
}
