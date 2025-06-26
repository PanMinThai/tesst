using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.Utils.Exceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException() : base() { }

        public AccountLockedException(string message) : base(message) { }

        public AccountLockedException(string message, Exception inner) : base(message, inner) { }
    }

}
