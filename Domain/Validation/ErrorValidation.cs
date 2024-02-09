using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class ErrorValidation
    {
        public ErrorValidation() { 
            validationErrors = new List<string>();
        }
        public bool isValid { get; set; }
        public List<string> validationErrors { get; set; }
    }
}
