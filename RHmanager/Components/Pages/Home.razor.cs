using RHmanager.Dtos;
using RHmanager.Enums;
using RHmanager.Models;


namespace RHmanager.Components.Pages
{
    public partial class Home
    {
        // Variables d'etats
        private bool isLoading = true;

        private int totalEmployees = 0;
        private int totalDepartments = 0;
        private int pendingRequests = 0;
        private int approvedThisMonth = 0;

        private List<LeaveRequest> recentRequests = new();
        private List<DepartmentStatDto> departmentStats = new();

        // initialisation
        protected override async Task OnInitializedAsync()
        {
            await LoadDashboardData();
        }

        // Chargement des données
        private async Task LoadDashboardData()
        {
            isLoading = true;

            try
            {
                // Récupérer toutes les données
                var employees = await EmployeeService.GetAllEmployeesAsync();
                var departments = await DepartmentService.GetAllDepartmentsAsync();
                var allRequests = await LeaveRequestService.GetAllLeaveRequestsAsync();

                // Calculer les statistiques principales
                totalEmployees = employees.Count;
                totalDepartments = departments.Count;

                pendingRequests = allRequests
                    .Count(r => r.Status == LeaveRequestStatus.Pending);

                // Nombre de demandes approuvées ce mois-ci
                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                approvedThisMonth = allRequests
                    .Count(r => r.Status == LeaveRequestStatus.Approved
                             && r.ValidationDate >= firstDayOfMonth);

                // Les 5 demandes les plus récentes
                recentRequests = allRequests
                    .OrderByDescending(r => r.RequestDate)
                    .Take(5)
                    .ToList();

                // Statistiques par département
                departmentStats = departments
                    .Select(d => new DepartmentStatDto
                    {
                        DepartmentName = d.Name,
                        EmployeeCount = d.Employees.Count
                    })
                    .OrderByDescending(d => d.EmployeeCount)
                    .ToList();
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}