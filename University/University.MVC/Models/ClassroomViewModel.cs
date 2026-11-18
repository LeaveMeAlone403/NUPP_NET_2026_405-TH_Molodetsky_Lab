using System;
using System.ComponentModel.DataAnnotations;

namespace University.MVC.Models.ViewModels
{
    public class ClassroomViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Display(Name = "Has Projector")]
        public bool HasProjector { get; set; }

        public Guid? TeacherId { get; set; }
    }
}