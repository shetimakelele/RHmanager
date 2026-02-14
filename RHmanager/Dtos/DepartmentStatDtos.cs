namespace RHmanager.Dtos
{
    /// <summary>
    /// DTO pour afficher les stats par département
    /// </summary>
    public class DepartmentStatDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}
