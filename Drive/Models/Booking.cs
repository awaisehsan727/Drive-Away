//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Drive.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int BookId { get; set; }
        public int userId { get; set; }
        public int PostId { get; set; }
        public string status { get; set; }
    
        public virtual Post_Tbl Post_Tbl { get; set; }
        public virtual User User { get; set; }
    }
}
