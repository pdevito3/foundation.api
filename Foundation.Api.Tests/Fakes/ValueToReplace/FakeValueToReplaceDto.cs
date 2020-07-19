namespace Foundation.Api.Tests.Fakes.ValueToReplace
{
    using AutoBogus;
    using Foundation.Api.Models.ValueToReplace;

    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeValueToReplaceDto : AutoFaker<ValueToReplaceDto>
    {
        public FakeValueToReplaceDto()
        {
            // leaving the first 49 for potential special use cases in startup builds that need explicit values
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceId, lambdaInitialsToReplace => lambdaInitialsToReplace.Random.Number(50, 100000));
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceIntField1, lambdaInitialsToReplace => lambdaInitialsToReplace.Random.Number(0, 1000000)); // example validation rul says that it needs be above 0
            RuleFor(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceDateField1, lambdaInitialsToReplace => lambdaInitialsToReplace.Date.Past()); // example validation rule says it has to be in the past
        }
    }
}
