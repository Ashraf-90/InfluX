using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MetaPages
    {
        public int Id { get; set; }
        public string Page { get; set; }
        public string EnPageTitle { get; set; }
        public string ArPageTitle { get; set; }
        public string EnMetaTitle { get; set; }
        public string ArMetaTitle { get; set; }
        public string EnMetaDescription { get; set; }
        public string ArMetaDescription { get; set; }
    }
}
