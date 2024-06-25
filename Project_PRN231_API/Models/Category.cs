using System;
using System.Collections.Generic;

namespace Project_PRN231_API.Models
{
    public partial class Category
    {
        public Category()
        {
            Tasks = new HashSet<Task>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
