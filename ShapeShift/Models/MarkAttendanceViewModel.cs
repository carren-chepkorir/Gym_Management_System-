using ShapeShift.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ShapeShift.Models;

public class MarkAttendanceViewModel
{
    public Shift Shift { get; set; }

    public List<Member> MembersInShift { get; set; }

    [Required(ErrorMessage = "Select at least one member.")]
    public List<int> SelectedMemberIds { get; set; }
}

