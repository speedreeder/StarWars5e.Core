using System;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class BaseEntity : TableEntity
    {
        public ContentType ContentTypeEnum { get; set; }
        public string ContentType {
            get => ContentTypeEnum.ToString();
            set => ContentTypeEnum = Enum.Parse<ContentType>(value);
        }
        public ContentSource ContentSourceEnum { get; set; }
        public string ContentSource
        {
            get => ContentSourceEnum.ToString();
            set => ContentSourceEnum = Enum.Parse<ContentSource>(value);
        }
    }
}
