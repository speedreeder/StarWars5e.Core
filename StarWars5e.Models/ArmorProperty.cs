﻿using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class ArmorProperty : BaseEntity
    {
        public ArmorProperty() {}
        public ArmorProperty(string name, string content, ContentType contentType)
        {
            PartitionKey = contentType.ToString();
            RowKey = name;
            Name = name;
            Content = content;
            ContentType = contentType.ToString();
        }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
