﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WF2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class test1Entities : DbContext
    {
        public test1Entities()
            : base("name=test1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bab_local> bab_local { get; set; }
        public virtual DbSet<ban> ban { get; set; }
        public virtual DbSet<bar_data> bar_data { get; set; }
        public virtual DbSet<bab> bab { get; set; }
    }
}
