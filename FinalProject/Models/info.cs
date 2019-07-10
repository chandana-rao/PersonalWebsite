using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Education
    {
        [Key]
        public int EId { get; set; }
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Character range is 3-250 characters")]
        [Required]
        public string university { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 4, ErrorMessage = "Character range is 4-250 characters")]
        public string year { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string major { get; set; }
        public string coursework { get; set; }
        [Required]
        public TypeDegree degreeType { get; set; }

    }
    public enum TypeDegree
    {
        UnderGraduate,
        Graduate
    }
    
    public class Project
    {
        [Key]
        public int PId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string name { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 4, ErrorMessage = "Character range is 4-250 characters")]
        public string year { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string description { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Character range is 3-250 characters")]
        public string technologies { get; set; }

        [Required(ErrorMessage = "Please select")]
        public TypeProject projectType { get; set; }
        public TypeDegree? degreeType { get; set; }
    }

    public enum TypeProject
    {
        Personal,
        Academic
    }

    public class PExperience
    {
        [Key]
        public int PEId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string company { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Character range is 3-250 characters")]
        public string title { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 4, ErrorMessage = "Character range is 4-250 characters")]
        public string year { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string description { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Character range is 2-250 characters")]
        public string technologies { get; set; }
    }

    public class Recruiter
    {
        [Key]
        public int userId { get; set; }
        public string email { get; set; }
        public string comments { get; set; }
    }

    public class Files
    {
        [Key]
        public int fileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FilePath { get; set; }
    }
}
