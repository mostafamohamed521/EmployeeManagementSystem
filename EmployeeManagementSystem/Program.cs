using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

using var context = new AppDbContext();
context.Database.EnsureCreated();

var service = new ManagementService(context);

while (true)
{
    Console.Clear();
    Console.WriteLine("Employee / Project Management System");
    Console.WriteLine();
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Edit");
    Console.WriteLine("3. Delete");
    Console.WriteLine("4. Display");
    Console.WriteLine("5. Exit");

    var choice = ReadMainChoice();

    Console.Clear();

    switch (choice)
    {
        case 1:
            AddMenu(service);
            break;
        case 2:
            EditMenu(service);
            break;
        case 3:
            DeleteMenu(service);
            break;
        case 4:
            DisplayMenu(service);
            break;
        case 5:
            return;
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static int ReadMainChoice()
{
    while (true)
    {
        Console.Write("Choice (1-5): ");
        var input = Console.ReadLine();

        if (int.TryParse(input, out var choice) && choice >= 1 && choice <= 5)
            return choice;

        Console.WriteLine("Invalid choice.");
    }
}

static void AddMenu(ManagementService service)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Add");
        Console.WriteLine();
        Console.WriteLine("1. Add Employee");
        Console.WriteLine("2. Add Department");
        Console.WriteLine("3. Add Project");
        Console.WriteLine("4. Back");

        var choice = ReadSubChoice(4);
        Console.Clear();

        switch (choice)
        {
            case 1:
                service.AddEmployee();
                break;
            case 2:
                service.AddDepartment();
                break;
            case 3:
                service.AddProject();
                break;
            case 4:
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static void EditMenu(ManagementService service)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Edit");
        Console.WriteLine();
        Console.WriteLine("1. Edit Employee Data");
        Console.WriteLine("2. Assign Employee To Department");
        Console.WriteLine("3. Assign Employee To Project");
        Console.WriteLine("4. Remove Employee From Project");
        Console.WriteLine("5. Edit Department Data");
        Console.WriteLine("6. Assign Employee To Department");
        Console.WriteLine("7. Edit Project Data");
        Console.WriteLine("8. Assign Employee To Project");
        Console.WriteLine("9. Back");

        var choice = ReadSubChoice(9);
        Console.Clear();

        switch (choice)
        {
            case 1:
                service.EditEmployeeData();
                break;
            case 2:
                service.AssignEmployeeToDepartment();
                break;
            case 3:
                service.AssignEmployeeToProject();
                break;
            case 4:
                service.RemoveEmployeeFromProject();
                break;
            case 5:
                service.EditDepartmentData();
                break;
            case 6:
                service.AssignEmployeeFromDepartment();
                break;
            case 7:
                service.EditProjectData();
                break;
            case 8:
                service.AssignEmployeeFromProject();
                break;
            case 9:
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static void DeleteMenu(ManagementService service)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Delete");
        Console.WriteLine();
        Console.WriteLine("1. Delete Employee");
        Console.WriteLine("2. Delete Department");
        Console.WriteLine("3. Delete Project");
        Console.WriteLine("4. Back");

        var choice = ReadSubChoice(4);
        Console.Clear();

        switch (choice)
        {
            case 1:
                service.DeleteEmployee();
                break;
            case 2:
                service.DeleteDepartment();
                break;
            case 3:
                service.DeleteProject();
                break;
            case 4:
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static void DisplayMenu(ManagementService service)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Display");
        Console.WriteLine();
        Console.WriteLine("1. Employee Data");
        Console.WriteLine("2. Department Data");
        Console.WriteLine("3. Project Data");
        Console.WriteLine("4. Back");

        var choice = ReadSubChoice(4);
        Console.Clear();

        switch (choice)
        {
            case 1:
                service.DisplayEmployees();
                break;
            case 2:
                service.DisplayDepartments();
                break;
            case 3:
                service.DisplayProjects();
                break;
            case 4:
                return;
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static int ReadSubChoice(int max)
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
