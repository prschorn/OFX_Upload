//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OFXUpload.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class FinancialAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinancialAccount()
        {
            this.FinancialAccountBalances = new HashSet<FinancialAccountBalance>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Agency { get; set; }
        public string AgencyDV { get; set; }
        public string Number { get; set; }
        public string NumberDV { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public int BankId { get; set; }
    
        public virtual Bank Bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FinancialAccountBalance> FinancialAccountBalances { get; set; }
    }
}