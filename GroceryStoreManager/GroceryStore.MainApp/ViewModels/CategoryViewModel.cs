using System.Collections.ObjectModel;
using System.Windows.Input;
using GroceryStore.MainApp.ViewModels.CategoryVMs;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Models.PreModel;
using Microsoft.UI.Xaml;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Contracts.Services;

namespace GroceryStore.MainApp.ViewModels
{
    public partial class CategoryViewModel : ObservableRecipient, INavigationAware
    {
        private readonly IDataService<ProductType> _productTypeDS;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasCurrent))]
        private ProductType? _selectedProductType;

        public ObservableCollection<ProductType> Source { get; } = new();

        public CategoryViewModel()
        {
            _productTypeDS = App.GetService<IDataService<ProductType>>();
            _onAccept = async () =>
            {
                await LoadData();
                CurrentForm = new FormVM(_onAccept!, _onCancel!);
            };
            _onCancel = () =>
            {
                CurrentForm = new FormVM(_onAccept!, _onCancel!); ;
            };
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
            SelectedProductType = null;
            CurrentForm = new FormVM(_onAccept!, _onCancel!); ;
            Source.Clear();
            var data = await _productTypeDS.GetAll();
            foreach (var item in data)
            {
                Source.Add(item);
            }
            OnPropertyChanged(nameof(Source));
        }

        // =============================
        // =============================

        public bool HasCurrent => _selectedProductType != null;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ReloadCommand { get; }

        [ObservableProperty]
        private FormVM? _currentForm;

        private Action _onAccept;
        private Action _onCancel;

        private void AddRecord(object? param)
        {
            CurrentForm = new AddFormVM(_onAccept, _onCancel);
        }

        private void EditRecord(object? obj)
        {
            if (SelectedProductType == null)
            {
                return;
            }
            CurrentForm = new EditFormVM(_onAccept, _onCancel, SelectedProductType);
        }

        private void DeleteRecord(object? obj)
        {
            if (SelectedProductType == null)
            {
                return;
            }
            CurrentForm = new DeleteFormVM(_onAccept, _onCancel, SelectedProductType);
        }

        private async void Reload(object? param)
        {
            // Use to reset View and Settings
            await LoadData();
        }
    }
}

namespace GroceryStore.MainApp.ViewModels.CategoryVMs
{
    // Lazy implement VM
    public partial class FormVM : ObservableRecipient
    {
        protected readonly Action _successCB;
        protected readonly Action _cancelCB;
        protected int? _id = null;

        public bool IsBase = true;

        public FormVM(Action successCB, Action cancelCB)
        {
            _successCB = successCB;
            _cancelCB = cancelCB;
            TitleText = string.Empty;
            Readonly = false;
        }

        [ObservableProperty]
        private string? _name;

        public string TitleText { get; protected set; }
        public bool Readonly { get; protected set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShouldDisplayError))]
        private string? _errorMessage = null;

        public Visibility ShouldDisplayError
        {
            get
            {
                if (ErrorMessage != null || ErrorMessage == string.Empty)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        protected bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    ErrorMessage = "Can't have an empty name";
                }
                else
                {
                    ErrorMessage = null;
                }
                return ErrorMessage == null;
            }
        }

        public virtual ICommand ConfirmCommand => new DelegateCommand((obj) => { });
        public virtual ICommand CancelCommand => new DelegateCommand((obj) => { _cancelCB.Invoke(); });
    }

    public partial class AddFormVM : FormVM
    {
        public AddFormVM(Action successCB, Action cancelCB) : base(successCB, cancelCB)
        {
            IsBase = false;
            TitleText = "Add a Category";
        }

        public override ICommand ConfirmCommand => new DelegateCommand(async (obj) =>
        {
            if (Valid == true)
            {
                var productType = new ProductType();
                productType.Name = Name!;
                productType.Id = null;
                var pmProductType = new PMProductType(productType);
                var result = await pmProductType.Insert();
                if (result == true)
                {
                    _successCB.Invoke();
                }
                else
                {
                    // error
                }
            }
        });
    }

    public partial class EditFormVM : FormVM
    {
        public EditFormVM(Action successCB, Action cancelCB, ProductType data) : base(successCB, cancelCB)
        {
            IsBase = false;
            TitleText = "Edit Category";
            Name = data.Name;
            _id = data.Id;
        }

        public override ICommand ConfirmCommand => new DelegateCommand(async (obj) =>
        {
            if (Valid == true)
            {
                var productType = new ProductType();
                productType.Name = Name!;
                productType.Id = _id;
                var pmProductType = new PMProductType(productType);
                var result = await pmProductType.Update();
                if (result == true)
                {
                    _successCB.Invoke();
                }
                else
                {
                    // error
                }
            }
        });
    }

    public partial class DeleteFormVM : FormVM
    {
        public DeleteFormVM(Action successCB, Action cancelCB, ProductType data) : base(successCB, cancelCB)
        {
            IsBase = false;
            TitleText = "Delete Category";
            Readonly = true;
            Name = data.Name;
            _id = data.Id;
        }

        public override ICommand ConfirmCommand => new DelegateCommand(async (obj) =>
        {
            if (Valid == true)
            {
                var productType = new ProductType();
                productType.Name = Name!;
                productType.Id = _id;
                var pmProductType = new PMProductType(productType);
                var checkCascade = await pmProductType.CheckCascade();
                if (checkCascade == true)
                {
                    IPopupService warningPopup = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Warning);
                    var message = "This record is already been used by some Products. Delete all related Products (and may also Orders)?";
                    var acceptCascade = await warningPopup.ShowWindow(message);
                    // a little bit dirty here, null means decline
                    if (acceptCascade is null)
                    {
                        return;
                    }
                }
                var result = await pmProductType.RawDelete();
                if (result == true)
                {
                    _successCB.Invoke();
                }
                else
                {
                    // error
                }
            }
        });
    }
}
