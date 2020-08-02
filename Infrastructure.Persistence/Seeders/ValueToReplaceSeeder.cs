namespace Infrastructure.Persistence.Seeders
{
    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class ValueToReplaceSeeder
    {
        public static void SeedSampleValueToReplaceData(ValueToReplaceDbContext context)
        {
            if (!context.ValueToReplaces.Any())
            {
                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>()
                    .RuleFor(fake => fake.ValueToReplaceDateField1, fake => fake.Date.Past())
                    //.RuleFor(fake => fake.LastModifiedOn, fake => null)
                    //.RuleFor(fake => fake.LastModifiedBy, fake => null)
                    .RuleFor(fake => fake.ValueToReplaceIntField1, fake => fake.Random.Number()));
                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>()
                    .RuleFor(fake => fake.ValueToReplaceDateField1, fake => fake.Date.Past())
                    //.RuleFor(fake => fake.LastModifiedOn, fake => null)
                    //.RuleFor(fake => fake.LastModifiedBy, fake => null)
                    .RuleFor(fake => fake.ValueToReplaceIntField1, fake => fake.Random.Number()));
                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>()
                    .RuleFor(fake => fake.ValueToReplaceDateField1, fake => fake.Date.Past())
                    //.RuleFor(fake => fake.LastModifiedOn, fake => null)
                    //.RuleFor(fake => fake.LastModifiedBy, fake => null)
                    .RuleFor(fake => fake.ValueToReplaceIntField1, fake => fake.Random.Number()));

                context.SaveChanges();
            }
        }
    }
}
