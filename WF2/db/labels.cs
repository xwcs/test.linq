//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WF2.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using xwcs.core.db;
    using xwcs.core.ui.datalayout.attributes;
    
    public partial class labels : EntityBase
    {
        private int _id;
    	public int id 
    	{ 
    		get { return _id; } 
    		set { SetProperty(ref _id, value); } 
    	}
    
        private string _tipolabel_tipo;
    	public string tipolabel_tipo 
    	{ 
    		get { return _tipolabel_tipo; } 
    		set { SetProperty(ref _tipolabel_tipo, value); } 
    	}
    
        private string _value;
    	public string value 
    	{ 
    		get { return _value; } 
    		set { SetProperty(ref _value, value); } 
    	}
    
        private Nullable<bool> _main;
    	public Nullable<bool> main 
    	{ 
    		get { return _main; } 
    		set { SetProperty(ref _main, value); } 
    	}
    
        private int _iter_id;
    	public int iter_id 
    	{ 
    		get { return _iter_id; } 
    		set { SetProperty(ref _iter_id, value); } 
    	}
    
        private int _rowversion;
    	public int rowversion 
    	{ 
    		get { return _rowversion; } 
    		set { SetProperty(ref _rowversion, value); } 
    	}
    
    
        public virtual tipolabel tipolabel { get; set; }
    }
}
