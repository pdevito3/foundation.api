namespace Foundation.Api.Models.ValueToReplaces
{
    using System;

    public class ValueToReplaceDto
    {
        public int ValueToReplaceId { get; set; }
        public int? ValueToReplaceIntField1 { get; set; }
        public string ValueToReplaceTextField1 { get; set; } 
        public string ValueToReplaceTextField2 { get; set; }
        public DateTime? ValueToReplaceDateField1 { get; set; }
    }
}
