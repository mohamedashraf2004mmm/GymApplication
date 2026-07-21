using GymApplication.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.ViewModels
{
    public class UpdatePlanViewModel
    {
        public string planName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be 5 to 200 chars")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Duration is required")]
        [Range(1,365,ErrorMessage = "Duration must be between 1 and 365 days")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01,10000,ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
