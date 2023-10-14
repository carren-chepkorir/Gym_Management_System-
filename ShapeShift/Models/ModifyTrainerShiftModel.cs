using System.ComponentModel.DataAnnotations;

public class ModifyTrainerShiftModel
{
    [Required(ErrorMessage = "Please select a trainer.")]
    public int TrainerId { get; set; }

    [Required(ErrorMessage = "Please select a new shift.")]
    public int NewShiftId { get; set; }
}
