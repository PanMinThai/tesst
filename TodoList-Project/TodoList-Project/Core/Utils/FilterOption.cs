using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.Utils
{
    public class FilterOption<T> where T : struct
    {
        public T? Value { get; set; }
        public string DisplayName { get; set; }

        public override string ToString() => DisplayName;
    }
}
