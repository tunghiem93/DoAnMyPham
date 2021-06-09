﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Customers : CMS_EntityBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FbID { get; set; }
        public string GoogleID { get; set; }
        public string CompanyName { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public bool MaritalStatus { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int AnswerType { get; set; }
        public string AnwersSecurity { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public byte Status { get; set; }
        public int UserLogin { get; set; }
    }
}
