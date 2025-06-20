using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.Utils.Messages
{
    public class ShowCharacterMessage : ValueChangedMessage<CharacterFeedbackDto>
    {
        public ShowCharacterMessage(CharacterFeedbackDto feedback) : base(feedback)
        {
        }
    }

}
