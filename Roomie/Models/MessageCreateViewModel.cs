using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roomie.Models
{
    public class MessageCreateViewModel
    {
        public int LinkerId { get; set; }
        public string RecieverID { get; set; }

        public string MessageText { get; set; }
    }
}