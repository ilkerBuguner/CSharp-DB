namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string ImportedDepartment = "Imported {0} with {1} cells";
        private const string ImportedPrisoner = "Imported {0} {1} years old";
        private const string ImportedOfficer = "Imported {0} ({1} prisoners)";


        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentCellsDtos = JsonConvert.DeserializeObject<ImportDepartmentCellsDto[]>(jsonString);
           
            var sb = new StringBuilder();

            var departments = new List<Department>();

            foreach (var departmentCellsDto in departmentCellsDtos)
            {
                var department = new Department()
                {
                    Name = departmentCellsDto.Name,
                    Cells = departmentCellsDto.Cells
                };

                var isValidCells = department.Cells.Any(x => IsValid(x) == false);

                if (IsValid(department) == false || isValidCells == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                sb.AppendLine(String.Format(ImportedDepartment, department.Name, department.Cells.Count));

                departments.Add(department);
            }

            context.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersMailsDtos = JsonConvert.DeserializeObject<ImportPrisonersMailsDto[]>(jsonString);

            var sb = new StringBuilder();
            var prisoners = new List<Prisoner>();

            foreach (var prisonersMailsDto in prisonersMailsDtos)
            {
                var prisoner = new Prisoner
                {
                    FullName = prisonersMailsDto.FullName,
                    Nickname = prisonersMailsDto.Nickname,
                    Age = prisonersMailsDto.Age,
                    //IncarcerationDate = DateTime.ParseExact(prisonersMailsDto.IncarcerationDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    //ReleaseDate = DateTime.ParseExact(prisonersMailsDto.ReleaseDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = prisonersMailsDto.Bail,
                    CellId = prisonersMailsDto.CellId,
                    Mails = prisonersMailsDto.Mails
                };

                bool isPrisonerVaild = IsValid(prisoner);
                bool areMailsValid = prisoner.Mails.Any(m => IsValid(m) == false);

                if (isPrisonerVaild == false || areMailsValid == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                prisoners.Add(prisoner);
                sb.AppendLine(String.Format(ImportedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportOfficerDto[]), new XmlRootAttribute("Officers"));
            var officerDtos = (ImportOfficerDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();
            var officers = new List<Officer>();

            foreach (var officerDto in officerDtos)
            {
                bool isPositionValid = Enum.IsDefined(typeof(Position), officerDto.Position);
                bool isWeaponValid = Enum.IsDefined(typeof(Weapon), officerDto.Weapon);

                if (isPositionValid == false || isWeaponValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = (Position)Enum.Parse(typeof(Position), officerDto.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officerDto.Weapon),
                    DepartmentId = officerDto.DepartmentId
                };

                if (IsValid(officer) == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var prisonerId in officerDto.Prisoners)
                {
                    var prisoner = context.Prisoners.Find(prisonerId.Id);

                    officer.OfficerPrisoners.Add(new OfficerPrisoner { PrisonerId = prisonerId.Id, Prisoner = prisoner});
                }

                officers.Add(officer);
                sb.AppendLine(String.Format(ImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();
            Console.WriteLine(officers.Count);
            Console.WriteLine(officers.Sum(o => o.OfficerPrisoners.Count));
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