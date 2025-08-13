using System;
using System.Collections.Generic;

namespace CourseProjectitr.Models.ViewModels
{
    public class InventoryViewModel
    {
        public Guid Id { get; set; }                 
        public string Title { get; set; }           
        public string Category { get; set; }          
        public int ItemsCount { get; set; }           
        public bool IsPublic { get; set; }            
        public List<string> Tags { get; set; }     
        public string OwnerName { get; set; }         
    }
}
