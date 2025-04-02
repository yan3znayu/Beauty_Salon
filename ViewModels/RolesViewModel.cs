using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;

namespace Beauty_Salon.ViewModels
{
    public class RolesViewModel : BaseViewModel
    {
        private readonly RolesRepository _rolesRepository;
        private ObservableCollection<Roles> _roles;
        private Roles _selectedRole;
        private Roles _originalRole;

        public RolesViewModel()
        {
            _rolesRepository = new RolesRepository(new DatabaseContext());
            LoadRolesCommand = new RelayCommand(async (param) => await LoadRolesAsync());
            AddRoleCommand = new RelayCommand(async (param) => await AddRoleAsync());
            SaveRoleCommand = new RelayCommand(async (param) => await SaveRoleAsync());
            DeleteRoleCommand = new RelayCommand(async (param) => await DeleteRoleAsync());
            SelectedRole = new Roles();
            ResetSelectedRoleCommand = new RelayCommand(async (param) => await ResetSelectedRole());
        }

        public ObservableCollection<Roles> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        public Roles OriginalRole
        {
            get => _originalRole;
            set
            {
                _originalRole = value;
                OnPropertyChanged(nameof(OriginalRole));
            }
        }

        public Roles SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public ICommand LoadRolesCommand { get; set; }
        public ICommand AddRoleCommand { get; set; }
        public ICommand SaveRoleCommand { get; set; }
        public ICommand DeleteRoleCommand { get; set; }
        public ICommand ResetSelectedRoleCommand { get; set; }
        private async Task ResetSelectedRole()
        {
            if (SelectedRole != null)
            {
                using (var context = new DatabaseContext())
                {
                    _originalRole = context.Roles.FirstOrDefault(u => u.Role_ID == SelectedRole.Role_ID);
                }

                if (_originalRole != null)
                {
                    SelectedRole.Role_Name = _originalRole.Role_Name;

                    await LoadRolesAsync();
                }
            }
        }

        private async Task LoadRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();
            Roles = new ObservableCollection<Roles>(roles);
        }

        private async Task AddRoleAsync()
        {
            if (!string.IsNullOrEmpty(SelectedRole.Role_Name))
            {
                await _rolesRepository.AddAsync(SelectedRole);
                await LoadRolesAsync();
                SelectedRole = new Roles();
            }
        }

        private async Task SaveRoleAsync()
        {
            if (SelectedRole != null && SelectedRole.Role_ID > 0)
            {
                await _rolesRepository.UpdateAsync(SelectedRole);
                await LoadRolesAsync();
                SelectedRole = new Roles();
            }
        }

        private async Task DeleteRoleAsync()
        {
            if (SelectedRole != null && SelectedRole.Role_ID > 0)
            {
                await _rolesRepository.DeleteAsync(SelectedRole.Role_ID);
                await LoadRolesAsync();
                SelectedRole = new Roles();
            }
        }
    }
}
