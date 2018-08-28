using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string NameCategory { get; set; }

        public ICollection<Instruction> instruction { get; set; }
    }

    public class Instruction
    {
            [Key]
            public int InstructionId { get; set; }

            public Category Category { get; set; }

            public string ApplicationUserId { get; set; }
            public ApplicationUser ApplicationUser { get; set; }

            public string NameInstruction { get; set; }

            public string Decription { get; set; }

            public string Img { get; set; }

            public ICollection<InstructionStep> instructionSteps { get; set; }

            public  ICollection<Achievements> Achievements { get; set; }
            
            public ICollection<Comment> Comments { get; set; }
    }

    public class InstructionStep
    {
        [Key]
        public int InstructionStepId { get; set; }
        public string TitleStep { get; set; }
        public string Text { get; set; }

        public string InstuctionId { get; set; }
        public Instruction Instruction { get; set; }
    }
}
