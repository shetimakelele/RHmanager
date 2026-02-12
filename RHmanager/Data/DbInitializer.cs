using RHmanager.Enums;
using RHmanager.Models;

namespace RHmanager.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Vérifier si la base contient déjà des données
            if (context.Departments.Any())
            {
                return; // La base est déjà seed
            }

            // 1. Créer les départements
            var departments = new Department[]
            {
                new Department
                {
                    Name = "Informatique",
                    Description = "Département IT - Développement et infrastructure"
                },
                new Department
                {
                    Name = "Ressources Humaines",
                    Description = "Département RH - Gestion du personnel"
                },
                new Department
                {
                    Name = "Commercial",
                    Description = "Département Commercial - Ventes et relations clients"
                },
                new Department
                {
                    Name = "Finance",
                    Description = "Département Finance - Comptabilité et gestion financière"
                }
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();

            // 2. Créer les types de congés
            var leaveTypes = new LeaveType[]
            {
                new LeaveType
                {
                    Name = "Congés Payés (CP)",
                    DeductFromBalance = true
                },
                new LeaveType
                {
                    Name = "RTT",
                    DeductFromBalance = true
                },
                new LeaveType
                {
                    Name = "Congé Maladie",
                    DeductFromBalance = false
                },
                new LeaveType
                {
                    Name = "Congé Sans Solde",
                    DeductFromBalance = false
                },
                new LeaveType
                {
                    Name = "Congé Parental",
                    DeductFromBalance = false
                }
            };

            context.LeaveTypes.AddRange(leaveTypes);
            context.SaveChanges();

            // 3. Créer les employés (d'abord les managers)
            var manager1 = new Employee
            {
                EmployeeNumber = "EMP001",
                FirstName = "Sophie",
                LastName = "Martin",
                Email = "sophie.martin@company.com",
                PhoneNumber = "0601020304",
                Position = "Directrice IT",
                HireDate = new DateTime(2018, 3, 15),
                DepartmentId = departments[0].Id, // Informatique
                AnnualLeaveDays = 30,
                RemainingLeaveDays = 22,
                ManagerId = null // Pas de manager (c'est une directrice)
            };

            var manager2 = new Employee
            {
                EmployeeNumber = "EMP002",
                FirstName = "Pierre",
                LastName = "Dubois",
                Email = "pierre.dubois@company.com",
                PhoneNumber = "0602030405",
                Position = "Directeur RH",
                HireDate = new DateTime(2017, 6, 1),
                DepartmentId = departments[1].Id, // RH
                AnnualLeaveDays = 30,
                RemainingLeaveDays = 25,
                ManagerId = null
            };

            context.Employees.AddRange(manager1, manager2);
            context.SaveChanges();

            // 4. Créer les employés standard
            var employees = new Employee[]
            {
                new Employee
                {
                    EmployeeNumber = "EMP003",
                    FirstName = "Jean",
                    LastName = "Dupont",
                    Email = "jean.dupont@company.com",
                    PhoneNumber = "0603040506",
                    Position = "Développeur Full Stack",
                    HireDate = new DateTime(2020, 9, 1),
                    DepartmentId = departments[0].Id, // Informatique
                    AnnualLeaveDays = 25,
                    RemainingLeaveDays = 18,
                    ManagerId = manager1.Id
                },
                new Employee
                {
                    EmployeeNumber = "EMP004",
                    FirstName = "Marie",
                    LastName = "Leroy",
                    Email = "marie.leroy@company.com",
                    PhoneNumber = "0604050607",
                    Position = "Développeuse Frontend",
                    HireDate = new DateTime(2021, 2, 15),
                    DepartmentId = departments[0].Id, // Informatique
                    AnnualLeaveDays = 25,
                    RemainingLeaveDays = 20,
                    ManagerId = manager1.Id
                },
                new Employee
                {
                    EmployeeNumber = "EMP005",
                    FirstName = "Luc",
                    LastName = "Bernard",
                    Email = "luc.bernard@company.com",
                    PhoneNumber = "0605060708",
                    Position = "Chargé de Recrutement",
                    HireDate = new DateTime(2019, 11, 10),
                    DepartmentId = departments[1].Id, // RH
                    AnnualLeaveDays = 25,
                    RemainingLeaveDays = 15,
                    ManagerId = manager2.Id
                },
                new Employee
                {
                    EmployeeNumber = "EMP006",
                    FirstName = "Claire",
                    LastName = "Moreau",
                    Email = "claire.moreau@company.com",
                    PhoneNumber = "0606070809",
                    Position = "Assistante RH",
                    HireDate = new DateTime(2022, 1, 5),
                    DepartmentId = departments[1].Id, // RH
                    AnnualLeaveDays = 25,
                    RemainingLeaveDays = 23,
                    ManagerId = manager2.Id
                },
                new Employee
                {
                    EmployeeNumber = "EMP007",
                    FirstName = "Thomas",
                    LastName = "Petit",
                    Email = "thomas.petit@company.com",
                    PhoneNumber = "0607080910",
                    Position = "Commercial Senior",
                    HireDate = new DateTime(2019, 4, 20),
                    DepartmentId = departments[2].Id, // Commercial
                    AnnualLeaveDays = 25,
                    RemainingLeaveDays = 12,
                    ManagerId = null // Pas de manager pour l'instant
                }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();

            // 5. Créer quelques demandes de congés
            var leaveRequests = new LeaveRequest[]
            {
                new LeaveRequest
                {
                    EmployeeId = employees[0].Id, // Jean Dupont
                    LeaveTypeId = leaveTypes[0].Id, // CP
                    StartDate = new DateTime(2025, 3, 10),
                    EndDate = new DateTime(2025, 3, 14),
                    NumberOfDays = 5,
                    Reason = "Vacances en famille",
                    Status = LeaveRequestStatus.Pending,
                    RequestDate = DateTime.Now.AddDays(-5)
                },
                new LeaveRequest
                {
                    EmployeeId = employees[1].Id, // Marie Leroy
                    LeaveTypeId = leaveTypes[1].Id, // RTT
                    StartDate = new DateTime(2025, 2, 20),
                    EndDate = new DateTime(2025, 2, 21),
                    NumberOfDays = 2,
                    Reason = "Week-end prolongé",
                    Status = LeaveRequestStatus.Approved,
                    RequestDate = DateTime.Now.AddDays(-10),
                    ValidatedById = manager1.Id,
                    ValidationDate = DateTime.Now.AddDays(-8),
                    ValidationComment = "Approuvé - bon travail récent"
                },
                new LeaveRequest
                {
                    EmployeeId = employees[2].Id, // Luc Bernard
                    LeaveTypeId = leaveTypes[0].Id, // CP
                    StartDate = new DateTime(2025, 4, 1),
                    EndDate = new DateTime(2025, 4, 10),
                    NumberOfDays = 10,
                    Reason = "Voyage à l'étranger",
                    Status = LeaveRequestStatus.Pending,
                    RequestDate = DateTime.Now.AddDays(-2)
                },
                new LeaveRequest
                {
                    EmployeeId = employees[3].Id, // Claire Moreau
                    LeaveTypeId = leaveTypes[2].Id, // Maladie
                    StartDate = new DateTime(2025, 2, 5),
                    EndDate = new DateTime(2025, 2, 7),
                    NumberOfDays = 3,
                    Reason = "Grippe",
                    Status = LeaveRequestStatus.Approved,
                    RequestDate = DateTime.Now.AddDays(-15),
                    ValidatedById = manager2.Id,
                    ValidationDate = DateTime.Now.AddDays(-14),
                    ValidationComment = "Bon rétablissement"
                },
                new LeaveRequest
                {
                    EmployeeId = employees[0].Id, // Jean Dupont (autre demande)
                    LeaveTypeId = leaveTypes[1].Id, // RTT
                    StartDate = new DateTime(2025, 2, 15),
                    EndDate = new DateTime(2025, 2, 15),
                    NumberOfDays = 1,
                    Reason = "Rendez-vous médical",
                    Status = LeaveRequestStatus.Rejected,
                    RequestDate = DateTime.Now.AddDays(-20),
                    ValidatedById = manager1.Id,
                    ValidationDate = DateTime.Now.AddDays(-18),
                    ValidationComment = "Période trop chargée, proposer une autre date"
                }
            };

            context.LeaveRequests.AddRange(leaveRequests);
            context.SaveChanges();
        }
    }
}