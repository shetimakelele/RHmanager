
using RHmanager.Helpers;
using RHmanager.Models;

namespace RHmanager.Components.Pages
{
    public partial class Departments
    {
        // Variables d'etats
        private bool isLoading = true;
        private List<Department> departments = new();
        private MessageHelper messageHelper = new();

        // initialisation
        protected override async Task OnInitializedAsync()
        {
            await LoadDepartments();
        }

        private async Task LoadDepartments()
        {
            isLoading = true;
            try
            {
                departments = await DepartmentService.GetAllDepartmentsAsync();
            }
            catch (Exception ex)
            {
                messageHelper.ShowError($"Erreur lors du chargement : {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        // Actions
        private void OpenCreateModal()
        {
            // TODO: implémenter le modal 
            messageHelper.Show("Fonctionnalité 'Créer' à venir", false);
        }

        private void OpenEditModal(Department department)
        {
            // TODO: On implémentera le modal plus tard
            messageHelper.Show($"Modification de '{department.Name}' à venir", false);
        }

        private async Task DeleteDepartment(Department department)
        {
            if (department.Employees.Count > 0)
            {
                messageHelper.ShowError("Impossible de supprimer un département contenant des employés");
                return;
            }

            // TODO: Ajouter une confirmation
            var success = await DepartmentService.DeleteDepartmentAsync(department.Id);

            if (success)
            {
                messageHelper.ShowSuccess($"Département '{department.Name}' supprimé avec succès");
                await LoadDepartments(); // Recharger la liste
            }
            else
            {
                messageHelper.ShowError("Erreur lors de la suppression");
            }
        }
    }



}