using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Models.PreModel;

namespace GroceryStore.MainApp.ViewModels;

public partial class CustomerViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Customer> _customerDS;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasCurrent))]
    private Customer? _selectedCustomer;

    public ObservableCollection<Customer> Customers { get; private set; } = new ObservableCollection<Customer>();

    public CustomerViewModel()
    {
        _customerDS = App.GetService<IDataService<Customer>>();
        _popupService = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Customer);

        AddCommand = new DelegateCommand(AddRecord);
        EditCommand = new DelegateCommand(EditRecord);
        DeleteCommand = new DelegateCommand(DeleteRecord);
        ReloadCommand = new DelegateCommand(Reload);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadData();
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task LoadData()
    {
        SelectedCustomer = null;
        Customers.Clear();
        var data = await _customerDS.GetAll();
        foreach (var item in data)
        {
            Customers.Add(item);
        }
        OnPropertyChanged(nameof(Customers));
    }

    // =============================
    // =============================

    public bool HasCurrent => _selectedCustomer != null;

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ReloadCommand { get; }

    private async void AddRecord(object? param)
    {
        var result = await _popupService.ShowWindow();
        if (result != null && result is PMCustomer pmCustomer)
        {
            // TODO: Display loading screen here
            var insertResult = await pmCustomer.Insert();
            if (insertResult == true)
            {
                Reload(null);
                return;
            }
        }
        // Display system error
    }

    private async void EditRecord(object? obj)
    {
        if (SelectedCustomer == null)
        {
            return;
        }
        var result = await _popupService.ShowWindow(SelectedCustomer);
        if (result != null && result is PMCustomer pmCustomer)
        {
            // TODO: Display loading screen here
            var updateResult = await pmCustomer.Update();
            if (updateResult == true)
            {
                Reload(null);
                return;
            }
        }
        // Display system error
    }

    private async void DeleteRecord(object? obj)
    {
        if (SelectedCustomer == null)
        {
            return;
        }
        var pmOrder = new PMCustomer(SelectedCustomer);
        // Loading
        var checkCascade = await pmOrder.CheckCascade();
        if (checkCascade == true)
        {
            var warning = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Warning);
            var message = "This record is already been used by some Orders. Delete all related Orders?";
            var acceptCascade = await warning.ShowWindow(message);
            // a little bit dirty here, null means decline
            if (acceptCascade is null)
            {
                return;
            }
        }
        var deleteResult = await pmOrder.RawDelete();
        if (deleteResult == true)
        {
            Reload(null);
            return;
        }
        // Error
    }

    private async void Reload(object? param)
    {
        // Use to reset View and Settings
        await LoadData();
    }
}
