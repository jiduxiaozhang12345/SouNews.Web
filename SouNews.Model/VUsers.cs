using SouNews.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouNews.Model {
    [Serializable]
    public class VUsers {
        public Users LoginUser { get; set; }
        public string Roles { get; set; }
        public List<Power> PowerList { get; set; }
    }
}
