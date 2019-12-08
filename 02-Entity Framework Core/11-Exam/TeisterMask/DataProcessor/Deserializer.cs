namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ImportDto;
    using System.Xml.Serialization;
    using System.IO;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectDto[]), new XmlRootAttribute("Projects"));
            var projectsAndTasksDtos = (ImportProjectDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var projects = new List<Project>();

            foreach (var projectAndTaskDto in projectsAndTasksDtos)
            {
                if (IsValid(projectAndTaskDto) == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var project = new Project();

                if (String.IsNullOrEmpty(projectAndTaskDto.DueDate))
                {
                    project = new Project
                    {
                        Name = projectAndTaskDto.Name,
                        OpenDate = DateTime.ParseExact(projectAndTaskDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)// moje da grumne
                    };
                }
                else
                {
                    project = new Project
                    {
                        Name = projectAndTaskDto.Name,
                        OpenDate = DateTime.ParseExact(projectAndTaskDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(projectAndTaskDto.DueDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture) // moje da grumne
                    };
                }


                foreach (var taskDto in projectAndTaskDto.Tasks)
                {
                    bool isTaskValid = IsValid(taskDto);

                    if (isTaskValid == false)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime taskDueDate = DateTime.ParseExact(taskDto.DueDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (taskOpenDate < project.OpenDate || taskDueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), taskDto.ExecutionType),
                        LabelType = (LabelType)Enum.Parse(typeof(LabelType), taskDto.LabelType)
                    };

                    project.Tasks.Add(task);

                }

                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
                projects.Add(project);
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

            var sb = new StringBuilder();

            var employees = new List<Employee>();

            foreach (var employeeDto in employeeDtos)
            {
                if (IsValid(employeeDto) == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };


                foreach (var taskIdDto in employeeDto.Tasks)
                {
                    var task = context.Tasks.Find(taskIdDto);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask
                    {
                        EmployeeId = employee.Id,
                        TaskId = task.Id,
                        Task = task
                    };


                    employee.EmployeesTasks.Add(employeeTask);

                }

                sb.AppendLine(String.Format(SuccessfullyImportedEmployee,
                    employee.Username,
                    employee.EmployeesTasks.Count));

                employees.Add(employee);
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}