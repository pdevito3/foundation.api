namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("ValueToReplaces")]
    public class ValueToReplace
    {
        [Key]
        [Required]
        [Column("ValueToReplaceId")]
        [Sieve(CanFilter = true, CanSort = true)]
        public int ValueToReplaceId { get; set; }

        [Column("ValueToReplaceIntField1")]
        [Sieve(CanFilter = true, CanSort = true)]
        public int? ValueToReplaceIntField1 { get; set; }

        [Column("ValueToReplaceTextField1")]
        [Sieve(CanFilter = true, CanSort = true)]
        public string ValueToReplaceTextField1 { get; set; }

        [Column("ValueToReplaceTextField2")]
        [Sieve(CanFilter = true, CanSort = true)]
        public string ValueToReplaceTextField2 { get; set; }

        [Column("ValueToReplaceDateField1")]
        public DateTime? ValueToReplaceDateField1 { get; set; }
    }
}
