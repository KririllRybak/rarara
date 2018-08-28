using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public Instruction Instruction { get; set; }
        public int InstructionId { get; set; }
        public string comment { get; set; }
    }
}
