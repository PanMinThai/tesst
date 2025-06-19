using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Features.Main.Models
{
    public class FeedbackResponseModel
    {
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public Tone Tone { get; set; }
    }

}
