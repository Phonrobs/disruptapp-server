using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace disruptappService.DataObjects {
    public class FeedItem : EntityData {
        [StringLength(2000)]
        public string note { get; set; }

        [StringLength(200)]
        public string picture { get; set; }

        [StringLength(100)]
        public string postBy { get; set; }

        public DateTime postDate { get; set; }

        public int like { get; set; }

        [StringLength(2000)]
        public string sasToken { get; set; }
    }
}