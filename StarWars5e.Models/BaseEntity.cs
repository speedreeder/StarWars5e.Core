using System;
using Microsoft.WindowsAzure.Storage.Table;
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
    }
}
