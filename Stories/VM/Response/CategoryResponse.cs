using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Response
{
    public class CategoryResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int PostCount { get; set; }
    }
}
