using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy.shared.Responses
{
    public class ActionsResponse<T>
    {
        public bool WasSuccess { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}