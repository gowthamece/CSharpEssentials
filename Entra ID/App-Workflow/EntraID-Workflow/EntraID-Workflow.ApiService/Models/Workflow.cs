﻿using System;
namespace EntraID.Workflow.ApiService.Models;


    public class Workflow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    // Add this constructor
    public Workflow(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public Workflow() { }


}


