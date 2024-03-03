using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoPage : Page
{
    [SerializeField] private TextMeshProUGUI _fullNameText;
    [SerializeField] private TextMeshProUGUI _regionAndCityText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private TextMeshProUGUI _loginText;
    [SerializeField] private TextMeshProUGUI _emailText;
    [SerializeField] private TextMeshProUGUI _phoneText;
    [SerializeField] private TextMeshProUGUI _countryText;
    [SerializeField] private TextMeshProUGUI _birthDateText;
    [SerializeField] private TextMeshProUGUI _lastUpdateText;
    [SerializeField] private TextMeshProUGUI _signUpDateText;

    [Space(10)]
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private Button _closeButton;

    public void Start()
    {
        _logoutButton.onClick.AddListener(Logout);
        _editButton.onClick.AddListener(() => { });
        _closeButton.onClick.AddListener(() =>
        {
            PageManager.Instance.ClosePage(this);
            PageManager.Instance.OpenPage(PageManager.Instance.StartPage);
        });
    }

    public override void Open(int popUpLevel = 0)
    {
        base.Open(popUpLevel);

        var data = Player.UserData;

        _fullNameText.text = $"{data.FirstName} {data.LastName}";

        if (string.IsNullOrEmpty(data.Region) && string.IsNullOrEmpty(data.City))
        {
            _regionAndCityText.text = "����������";
        }
        else
        {
            _regionAndCityText.text = $"{data.Region}, {data.City}";
        }

        _loginText.text = data.Login;
        _emailText.text = data.Email;
        _phoneText.text = data.Phone;
        _countryText.text = data.Country;

        if (data.BirthDate.HasValue)
        {
            _birthDateText.text = data.BirthDate.Value.ToString("dd.MM.yyyy");
        }
        else
        {
            _birthDateText.text = "�� ������";
        }

        _lastUpdateText.text = data.LastUpdatedDate.ToString("dd.MM.yyyy");
        _signUpDateText.text = data.CreationDate.ToString("dd.MM.yyyy");
    }

    private void Logout()
    {
        Player.Logout();

        PageManager.Instance.ClosePage(this);
        PageManager.Instance.OpenPage(PageManager.Instance.StartPage);
    }

    public override void Close()
    {
        base.Close();
    }
}
