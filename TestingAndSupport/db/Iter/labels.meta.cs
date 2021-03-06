//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WF2.db.Iter
{
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using xwcs.core.db;
    using xwcs.core.db.binding.attributes;
    using xwcs.core.db.model.attributes;
    
    // This class is just for meta attributes editing
    // Do not write any logic here, it will be not considered
    // Copy this class somewhere else this one will be overridden!!!
    
    public class labels_meta
    {
        [Display(Name="Id")]
    	public int id { get; set;}
    
        [Display(Name="Tipolabel_tipo")]
    	public string tipolabel_tipo { get; set;}
    
        [Display(Name="Value")]
    	public string value { get; set;}
    
        [Display(Name="Main")]
    	public Nullable<bool> main { get; set;}
    
        [Display(Name="Iter_id")]
    	public int iter_id { get; set;}
    
        [Display(AutoGenerateField=false)]
    	[ConcurrencyCheck]
    	public int rowversion { get; set;}
    
    
    	[Display(AutoGenerateField=false)]
        public virtual tipolabel tipolabel { get; set; }
    }
    
    
    // This class couple its entity and it should be use
    // for extending functionalities of Entity class
    // here can be add de-serialized complex fields
    // or other necessary logic
    // copy this class to some other place
    // changes in this place will be overridden !!!
    
    [MetadataType(typeof(labels_meta))]
    public partial class labels
    {
    	// here add custom fields ...
    }
    
}
