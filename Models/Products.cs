namespace MVCTutorial.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            Orders = new HashSet<Orders>();
            Warrently1 = new HashSet<Warrently>();
        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(20)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string Speed { get; set; }

        [Required]
        [StringLength(20)]
        public string RAM { get; set; }

        [Required]
        [StringLength(20)]
        public string HardDisk { get; set; }

        [Required]
        [StringLength(20)]
        public string Core { get; set; }

        //added
        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public int Price { get; set; }

        public bool Warrently { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warrently> Warrently1 { get; set; }
    }
}
