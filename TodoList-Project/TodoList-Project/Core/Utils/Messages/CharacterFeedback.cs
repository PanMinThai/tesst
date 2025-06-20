using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.Utils.Messages
{
    public class CharacterFeedbackDto
    {
        public string Message { get; set; }
        public string ImagePath { get; set; }

        public CharacterFeedbackDto(string message, string imagePath)
        {
            Message = message;
            ImagePath = imagePath;
        }
    }
}
