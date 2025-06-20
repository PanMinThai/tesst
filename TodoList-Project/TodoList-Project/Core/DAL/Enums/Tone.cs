using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Enums
{
    public enum Tone
    {
        // Completed tones
        Praising,       // Khen ngợi
        Appreciative,   // Đánh giá cao
        Triumphant,     // Chúc mừng
        // Cancelled tones
        Sarcastic,      // Mỉa mai
        Critical,       // Phê bình
        Furious,
        // Creation tones
        Encouraging,    // Khuyến khích
        Inspiring,      // Truyền cảm hứng
        // Update tones
        Formal,         // Trang trọng
        Cautious,       // Thận trọng
        //Delete tones
        Stern,          // Nghiêm khắc
        Condemning,     // Lên án
        // Notification tones
        Neutral,       // Trung lập
        Alert,          // Cảnh báo
        // UndoCancel tones
        Approving,      // Chấp thuận
        Redeeming,      // Cứu vãn
        // UndoComplete tones
        Annoyed,        // Bực bội
        Bitter          // Chua chát
    }
}
