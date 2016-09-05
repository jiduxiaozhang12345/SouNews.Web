using SouNews.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouNews.Model {
    public class VUsers {
        public Users LoginUser { get; set; }
        public List<Power> PowerList { get; set; }
    }
}
