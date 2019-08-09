using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DMWebApplication.Models;

namespace DMWebApplication.Models
{
    public class MovieDB
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime Date { get; set; }

    }

    public class MovieDBContext : System.Data.Entity.DbContext
    {
        public DbSet<MovieDB> Movies { get; set; }
    }
}