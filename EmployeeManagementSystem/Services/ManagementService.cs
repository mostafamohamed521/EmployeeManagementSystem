using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services;

public class ManagementService
{
    private readonly AppDbContext _context;

    public ManagementService(AppDbContext context)
    {
        _context = context;
    }

    public void AddEmployee()
    {
        var departments = _context.Departments.OrderBy(d => d.Name).ToList();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found. Add a department first.");
            return;
        }

        Console.Write("Employee Name: ");
        var name = ReadRequiredString();

        Console.Write("Salary: ");
        var salary = ReadDecimal();

        Console.WriteLine();
        Console.WriteLine("Select Department:");
        var department = SelectDepartmentFromList(departments);

        if (department == null)
            return;

        var employee = new Employee
        {
            Name = name,
            Salary = salary,
            DepartmentId = department.Id
        };

        _context.Employees.Add(employee);
        _context.SaveChanges();

        Console.WriteLine("Employee added successfully.");
    }

    public void AddDepartment()
    {
        Console.Write("Department Name: ");
        var name = ReadRequiredString();

        if (_context.Departments.Any(d => d.Name.ToLower() == name.ToLower()))
        {
            Console.WriteLine("Department already exists.");
            return;
        }

        Console.Write("Description: ");
        var description = ReadRequiredString();

        _context.Departments.Add(new Department
        {
            Name = name,
            Description = description
        });

        _context.SaveChanges();
        Console.WriteLine("Department added successfully.");
    }

    public void AddProject()
    {
        Console.Write("Project Name: ");
        var name = ReadRequiredString();

        if (_context.Projects.Any(p => p.Name.ToLower() == name.ToLower()))
        {
            Console.WriteLine("Project already exists.");
            return;
        }

        Console.Write("Description: ");
        var description = ReadRequiredString();

        _context.Projects.Add(new Project
        {
            Name = name,
            Description = description
        });

        _context.SaveChanges();
        Console.WriteLine("Project added successfully.");
    }

    public void EditEmployeeData()
    {
        var employee = SelectEmployee();

        if (employee == null)
            return;

        Console.WriteLine();
        Console.WriteLine($"Current Name: {employee.Name}");
        Console.Write("New Name (press Enter to keep current): ");
        var name = Console.ReadLine();

        Console.WriteLine($"Current Salary: {employee.Salary}");
        Console.Write("New Salary (press Enter to keep current): ");
        var salaryInput = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
            employee.Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(salaryInput))
        {
            if (decimal.TryParse(salaryInput, out var salary))
                employee.Salary = salary;
            else
            {
                Console.WriteLine("Invalid salary. Old salary kept.");
            }
        }

        _context.SaveChanges();
        Console.WriteLine("Employee data updated successfully.");
    }

    public void AssignEmployeeToDepartment()
    {
        var employee = SelectEmployee();

        if (employee == null)
            return;

        var departments = _context.Departments.OrderBy(d => d.Name).ToList();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Current Department: {employee.Department.Name}");
        Console.WriteLine("Select New Department:");

        var department = SelectDepartmentFromList(departments);

        if (department == null)
            return;

        employee.DepartmentId = department.Id;
        _context.SaveChanges();

        Console.WriteLine("Employee assigned to department successfully.");
    }

    public void AssignEmployeeToProject()
    {
        var employee = SelectEmployee();

        if (employee == null)
            return;

        var projects = _context.Projects
            .Include(p => p.Employees)
            .OrderBy(p => p.Name)
            .ToList();

        var availableProjects = projects
            .Where(p => !p.Employees.Any(e => e.Id == employee.Id))
            .ToList();

        if (availableProjects.Count == 0)
        {
            Console.WriteLine("No available projects found for this employee.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Select Project:");

        var project = SelectProjectFromList(availableProjects);

        if (project == null)
            return;

        employee.Projects.Add(project);
        _context.SaveChanges();

        Console.WriteLine("Employee assigned to project successfully.");
    }

    public void RemoveEmployeeFromProject()
    {
        var employee = SelectEmployee();

        if (employee == null)
            return;

        if (employee.Projects.Count == 0)
        {
            Console.WriteLine("This employee is not assigned to any project.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Select Project To Remove:");

        var project = SelectProjectFromList(employee.Projects.OrderBy(p => p.Name).ToList());

        if (project == null)
            return;

        employee.Projects.Remove(project);
        _context.SaveChanges();

        Console.WriteLine("Employee removed from project successfully.");
    }

    public void EditDepartmentData()
    {
        var department = SelectDepartment();

        if (department == null)
            return;

        Console.WriteLine();
        Console.WriteLine($"Current Name: {department.Name}");
        Console.Write("New Name (press Enter to keep current): ");
        var name = Console.ReadLine();

        Console.WriteLine($"Current Description: {department.Description}");
        Console.Write("New Description (press Enter to keep current): ");
        var description = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
            department.Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(description))
            department.Description = description.Trim();

        _context.SaveChanges();
        Console.WriteLine("Department data updated successfully.");
    }

    public void AssignEmployeeFromDepartment()
    {
        var department = SelectDepartment();

        if (department == null)
            return;

        var employees = _context.Employees
            .Where(e => e.DepartmentId != department.Id)
            .OrderBy(e => e.Name)
            .ToList();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees available.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Search Employee By Name:");
        var employee = SelectEmployeeFromList(employees);

        if (employee == null)
            return;

        employee.DepartmentId = department.Id;
        _context.SaveChanges();

        Console.WriteLine("Employee assigned to department successfully.");
    }

    public void EditProjectData()
    {
        var project = SelectProject();

        if (project == null)
            return;

        Console.WriteLine();
        Console.WriteLine($"Current Name: {project.Name}");
        Console.Write("New Name (press Enter to keep current): ");
        var name = Console.ReadLine();

        Console.WriteLine($"Current Description: {project.Description}");
        Console.Write("New Description (press Enter to keep current): ");
        var description = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
            project.Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(description))
            project.Description = description.Trim();

        _context.SaveChanges();
        Console.WriteLine("Project data updated successfully.");
    }

    public void AssignEmployeeFromProject()
    {
        var project = SelectProject();

        if (project == null)
            return;

        var assignedIds = project.Employees.Select(e => e.Id).ToHashSet();

        var employees = _context.Employees
            .Where(e => !assignedIds.Contains(e.Id))
            .OrderBy(e => e.Name)
            .ToList();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees available.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Search Employee By Name:");
        var employee = SelectEmployeeFromList(employees);

        if (employee == null)
            return;

        project.Employees.Add(employee);
        _context.SaveChanges();

        Console.WriteLine("Employee assigned to project successfully.");
    }

    public void DeleteEmployee()
    {
        var employee = SelectEmployee();

        if (employee == null)
            return;

        Console.Write($"Delete {employee.Name}? (Y/N): ");
        var confirmation = Console.ReadLine();

        if (!string.Equals(confirmation, "Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        _context.Employees.Remove(employee);
        _context.SaveChanges();

        Console.WriteLine("Employee deleted successfully.");
    }

    public void DeleteDepartment()
    {
        var department = SelectDepartment();

        if (department == null)
            return;

        if (department.Employees.Count > 0)
        {
            Console.WriteLine("Cannot delete this department because it has employees.");
            Console.WriteLine("Assign its employees to another department first.");
            return;
        }

        Console.Write($"Delete {department.Name}? (Y/N): ");
        var confirmation = Console.ReadLine();

        if (!string.Equals(confirmation, "Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        _context.Departments.Remove(department);
        _context.SaveChanges();

        Console.WriteLine("Department deleted successfully.");
    }

    public void DeleteProject()
    {
        var project = SelectProject();

        if (project == null)
            return;

        Console.Write($"Delete {project.Name}? (Y/N): ");
        var confirmation = Console.ReadLine();

        if (!string.Equals(confirmation, "Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();

        Console.WriteLine("Project deleted successfully.");
    }

    public void DisplayEmployees()
    {
        var employees = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Projects)
            .OrderBy(e => e.Name)
            .ToList();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (var employee in employees)
        {
            Console.WriteLine();
            Console.WriteLine($"Name: {employee.Name}");
            Console.WriteLine($"Salary: {employee.Salary}");
            Console.WriteLine($"Department: {employee.Department.Name}");

            if (employee.Projects.Count == 0)
            {
                Console.WriteLine("Projects: None");
            }
            else
            {
                Console.WriteLine($"Projects: {string.Join(", ", employee.Projects.OrderBy(p => p.Name).Select(p => p.Name))}");
            }

            Console.WriteLine(new string('-', 40));
        }
    }

    public void DisplayDepartments()
    {
        var departments = _context.Departments
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToList();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return;
        }

        foreach (var department in departments)
        {
            Console.WriteLine();
            Console.WriteLine($"Name: {department.Name}");
            Console.WriteLine($"Description: {department.Description}");

            if (department.Employees.Count == 0)
            {
                Console.WriteLine("Employees: None");
            }
            else
            {
                Console.WriteLine($"Employees: {string.Join(", ", department.Employees.OrderBy(e => e.Name).Select(e => e.Name))}");
            }

            Console.WriteLine(new string('-', 40));
        }
    }

    public void DisplayProjects()
    {
        var projects = _context.Projects
            .Include(p => p.Employees)
            .OrderBy(p => p.Name)
            .ToList();

        if (projects.Count == 0)
        {
            Console.WriteLine("No projects found.");
            return;
        }

        foreach (var project in projects)
        {
            Console.WriteLine();
            Console.WriteLine($"Name: {project.Name}");
            Console.WriteLine($"Description: {project.Description}");

            if (project.Employees.Count == 0)
            {
                Console.WriteLine("Employees: None");
            }
            else
            {
                Console.WriteLine($"Employees: {string.Join(", ", project.Employees.OrderBy(e => e.Name).Select(e => e.Name))}");
            }

            Console.WriteLine(new string('-', 40));
        }
    }

    private Employee? SelectEmployee()
    {
        var employees = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Projects)
            .OrderBy(e => e.Name)
            .ToList();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return null;
        }

        return SelectEmployeeFromList(employees);
    }

    private Employee? SelectEmployeeFromList(List<Employee> employees)
    {
        Console.Write("Employee Name: ");
        var search = ReadRequiredString();

        var matches = employees
            .Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.Name)
            .ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("No matching employee found.");
            return null;
        }

        if (matches.Count == 1)
            return matches[0];

        Console.WriteLine();
        Console.WriteLine("Matching Employees:");

        for (var i = 0; i < matches.Count; i++)
            Console.WriteLine($"{i + 1}. {matches[i].Name}");

        var choice = ReadChoice(matches.Count);
        return matches[choice - 1];
    }

    private Department? SelectDepartment()
    {
        var departments = _context.Departments
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToList();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return null;
        }

        Console.WriteLine("Select Department:");
        return SelectDepartmentFromList(departments);
    }

    private Department? SelectDepartmentFromList(List<Department> departments)
    {
        for (var i = 0; i < departments.Count; i++)
            Console.WriteLine($"{i + 1}. {departments[i].Name}");

        var choice = ReadChoice(departments.Count);
        return departments[choice - 1];
    }

    private Project? SelectProject()
    {
        var projects = _context.Projects
            .Include(p => p.Employees)
            .OrderBy(p => p.Name)
            .ToList();

        if (projects.Count == 0)
        {
            Console.WriteLine("No projects found.");
            return null;
        }

        Console.WriteLine("Select Project:");
        return SelectProjectFromList(projects);
    }

    private Project? SelectProjectFromList(List<Project> projects)
    {
        for (var i = 0; i < projects.Count; i++)
            Console.WriteLine($"{i + 1}. {projects[i].Name}");

        var choice = ReadChoice(projects.Count);
        return projects[choice - 1];
    }

    private static string ReadRequiredString()
    {
        while (true)
        {
            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            Console.Write("Please enter a value: ");
        }
    }

    private static decimal ReadDecimal()
    {
        while (true)
        {
            var value = Console.ReadLine();

            if (decimal.TryParse(value, out var result) && result >= 0)
                return result;

            Console.Write("Please enter a valid salary: ");
        }
    }

    private static int ReadChoice(int max)
    {
        while (true)
        {
            Console.Write($"Choice (1-{max}): ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= max)
                return choice;

            Console.WriteLine("Invalid choice.");
        }
    }
}
