using Ford.WebApi.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLayoutElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fullName;
    [SerializeField] private Button _removeButton;

    public event Action OnRemoved;

    private void Start()
    {
        _removeButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    public HorseUserDto Initiate(HorseUserDto user, Action onRemoved)
    {
        _fullName.text = $"{user.FirstName} {user.LastName}".Trim();
        OnRemoved += onRemoved;
        return user;
    }

    private void OnDestroy()
    {
        OnRemoved?.Invoke();
        OnRemoved = null;
    }
}