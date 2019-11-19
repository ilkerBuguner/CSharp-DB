using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Dtos.Export;
using System.Text;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            var db = new CarDealerContext();

            //var suppliersFile = File.ReadAllText(@"./../../../Datasets/sales.xml");

            Console.WriteLine(GetSalesWithAppliedDiscount(db));


        }

        //09-Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (ImportSupplierDto[])serializer.Deserialize(new StringReader(inputXml));

            var suppliers = Mapper.Map<Supplier[]>(suppliersDto);

            context.Suppliers.AddRange(suppliers);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //10-Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            var partsDto = (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));

            var parts = new List<Part>();

            foreach (var partDto in partsDto)
            {
                var supplier = context.Suppliers.Find(partDto.SupplierId);

                if (supplier != null)
                {
                    var part = Mapper.Map<Part>(partDto);
                    parts.Add(part);
                }
            }

            context.AddRange(parts);

            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //11-Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            var carsDto = (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();
            var partCars = new List<PartCar>();
            foreach (var carDto in carsDto)
            {
                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                var parts = carDto.Parts
                    .Where(p => context.Parts.Any(pc => pc.Id == p.PartId))
                    .Select(p => p.PartId)
                    .Distinct();

                foreach (var carPart in parts)
                {
                    var partCar = new PartCar
                    {
                        PartId = carPart,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";

        }

        //12-Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])serializer.Deserialize(new StringReader(inputXml));

            var customers = Mapper.Map<Customer[]>(customersDto);

            context.Customers.AddRange(customers);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //13-Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));

            var salesDto = (ImportSaleDto[])serializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            foreach (var saleDto in salesDto)
            {
                var car = context.Cars.Find(saleDto.CarId);

                if (car != null)
                {
                    var sale = Mapper.Map<Sale>(saleDto);
                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //14-Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new ExportCarsWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //15-Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportCarsFromMakeBmwDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportCarsFromMakeBmwDto[]), new XmlRootAttribute("cars"));

            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //16-Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToArray();

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));

            serializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //17-Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new ExportCarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new ExportCarPartDto
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportCarDto[]), new XmlRootAttribute("cars"));

            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //18-Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new ExportTotalSalesByCustomerDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportTotalSalesByCustomerDto[]), new XmlRootAttribute("customers"));

            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //19-Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new ExportSaleWithAppliedDiscountDto
                {
                    Car = new ExportCarInSale
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(p => p.Part.Price) -
                        s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100
                })
                .ToArray();

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportSaleWithAppliedDiscountDto[]), new XmlRootAttribute("sales"));

            serializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}