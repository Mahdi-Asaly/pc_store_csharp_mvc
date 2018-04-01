namespace MVCTutorial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Login")]
    public partial class Login
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Login()
        //{
        //    ContactUs = new HashSet<ContactUs>();
        //    Orders = new HashSet<Orders>();
       // }

        [Key]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string UserPass { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Credits { get; set; }

        public bool Flag { get; set; }

        [Required]
        [StringLength(50)]
        public string Catagory { get; set; }

        public bool isAdmin { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ContactUs> ContactUs { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Orders> Orders { get; set; }
    }
}
