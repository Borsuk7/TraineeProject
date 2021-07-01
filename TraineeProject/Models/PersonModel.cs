using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;
using CsvHelper;
using System.IO;
using System.Text;
using System.Globalization;

namespace TraineeProject.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the name")]
        [StringLength(50, ErrorMessage = "Name is not valid")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the birthday")]
        public DateTime BirthDate { get; set; }

        public bool Married { get; set; }

        [Required(ErrorMessage = "Enter the phone")]
        [Phone(ErrorMessage = "Phone is not valid")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Enter the salary")]
        [Range(0, int.MaxValue, ErrorMessage = "Don't lie")]
        public decimal Salary { get; set; }
    }

    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(x => x.Name).Name("Name");
            Map(x => x.BirthDate).Name("Date of birth");
            Map(x => x.Married).Name("Married");
            Map(x => x.Phone).Name("Phone");
            Map(x => x.Salary).Name("Salary");
        }

        public List<Person> ReadCSVFile(string location)
        {
            try
            {
                using var reader = new StreamReader(location, Encoding.Default);
                using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

                csv.Configuration.RegisterClassMap<PersonMap>();
                var records = csv.GetRecords<Person>().ToList();

                return records;
            }
            catch 
            {
                return null;
            }
        }
    }
}
