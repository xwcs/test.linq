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
    
    public partial class log : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public log()
        {
    
    	}
    
        private int _id;
    	public int id 
    	{ 
    		get { return _id; } 
    		set { SetProperty(ref _id, value); } 
    	}
    
        private string _msg;
    	public string msg 
    	{ 
    		get { return _msg; } 
    		set { SetProperty(ref _msg, value); } 
    	}
    
        private Nullable<int> _kind;
    	public Nullable<int> kind 
    	{ 
    		get { return _kind; } 
    		set { SetProperty(ref _kind, value); } 
    	}
    
        private Nullable<System.DateTime> _when;
    	public Nullable<System.DateTime> when 
    	{ 
    		get { return _when; } 
    		set { SetProperty(ref _when, value); } 
    	}
    
    
    }
}
