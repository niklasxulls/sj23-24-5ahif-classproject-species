using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Validators;
using FluentValidation;
using System.Linq.Expressions;
using FluentValidation.Internal;
using Xunit;
using Castle.Core.Internal;
using System.Reflection;
using System.Collections;
using FluentAssertions;
using Xunit.Sdk;
using Moq;
using DiveSpecies.Infrastructure.Persistence;

namespace Application.UnitTests.Validators;
public static class ValidatorTestUtil
{

    public static EntityTypeBuilder<Entity> GetEntityTypeBuilderOf<Entity, EntityBuilder>() where Entity : class where EntityBuilder : IEntityTypeConfiguration<Entity>
    {
        // Construct the optionsBuilder using InMemory SqlLite
        var options = new DbContextOptionsBuilder<DiveSpeciesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        var sut = new DiveSpeciesDbContext(options);

        var conventionSet = ConventionSet.CreateConventionSet(sut);

        var modelBuilder = new ModelBuilder(conventionSet);

        // Apply entityConfiguration to entityTypeBuilder
        EntityTypeBuilder<Entity> entityTypeBuilder = modelBuilder.Entity<Entity>();
        EntityBuilder entityTypeConfiguration = (EntityBuilder)Activator.CreateInstance(typeof(EntityBuilder))!;
        entityTypeConfiguration.Configure(entityTypeBuilder);

        return entityTypeBuilder;
    }

    public static ICollection<IValidationRule>? GetValidatorsForMember<T>(this IValidator<T> validator, Func<T, PropertyInfo?> expression)
    {
        var descriptor = validator.CreateDescriptor();

        T validatorInstance = (T)Activator.CreateInstance(typeof(T))!;


        var member = expression.Invoke(validatorInstance);
        if (member == null) return null;

        return descriptor.GetRulesForMember(member.Name).ToList();
        //return descriptor.GetValidatorsForMember(member.Name).ToList();
    }

    public static void CheckValidatorWithEntityTypeBuilder<Entity, EntityBuilder, Validator, ValidatorCQRS>(Validator validator)
                where Validator : AbstractValidator<ValidatorCQRS>
                where Entity : class
                where EntityBuilder : IEntityTypeConfiguration<Entity>
    {

        var entityTypeBuilder = GetEntityTypeBuilderOf<Entity, EntityBuilder>();


        var properties = typeof(Entity).GetProperties();

        foreach (var prop in properties)
        {
            var matchingValidator = validator.GetValidatorsForMember(t =>
            {
                var validatorCQRSProperties = typeof(ValidatorCQRS).GetProperties();
                PropertyInfo? propMatch = validatorCQRSProperties.FirstOrDefault(vq => vq.Name == prop.Name);
                return propMatch;
            })?.FirstOrDefault();

            var matchingConfiguredProp = entityTypeBuilder.Metadata.FindDeclaredProperty(prop.Name);
            var matchingCQRSProp = typeof(ValidatorCQRS).GetProperty(prop.Name);
            
            if (matchingConfiguredProp == null || matchingCQRSProp == null)
            {
                continue;
            };


            if (matchingConfiguredProp.GetMaxLength() > -1)
            {
                var maxLengthValidationRules = matchingValidator != null ? matchingValidator.Components.Where(t => t.Validator.GetType().GetInterfaces().Any(i => i == typeof(IMaximumLengthValidator))) : null;
                
                if(!maxLengthValidationRules?.Any() ?? true)
                {
                    throw new ArgumentException("Validator doesn't restrict max length, while ef configuration does!");
                }
                
                IMaximumLengthValidator maxLengthValidationRule = (IMaximumLengthValidator) maxLengthValidationRules!.First().Validator;

                maxLengthValidationRule.Should().NotBeNull();
                maxLengthValidationRule!.Max.Should().Be(matchingConfiguredProp.GetMaxLength());
            }

            if (!matchingConfiguredProp.IsNullable && matchingConfiguredProp.PropertyInfo?.PropertyType == typeof(int))
            {
                if (matchingCQRSProp != null && Nullable.GetUnderlyingType(matchingCQRSProp.PropertyType) != null)
                {
                    throw new ArgumentException("Validator is  nullable, while ef configuration type is not nullable");
                }


                var greaterThanorEqualValidationRules = matchingValidator != null ? matchingValidator.Components.Where(t => t.Validator.GetType().GetInterfaces().Any(i => i == typeof(IGreaterThanOrEqualValidator))) : null;

                if (!greaterThanorEqualValidationRules?.Any() ?? true)
                {
                    throw new ArgumentException("Validator doesn't check on greather than 0, while ef configuration requires this property to be set!");
                }

                IGreaterThanOrEqualValidator greaterThanOrEqualValidationRule = (IGreaterThanOrEqualValidator) greaterThanorEqualValidationRules!.First().Validator;

                greaterThanOrEqualValidationRule.Should().NotBeNull();
                //greaterThanOrEqualValidationRule!..Should().Be(matchingConfiguredProp.GetMaxLength());
            }
            else if (matchingCQRSProp != null && matchingConfiguredProp.IsNullable && Nullable.GetUnderlyingType(matchingCQRSProp.PropertyType) == null)
            {
                throw new ArgumentException("Validator is not nullable, while ef configuration type is nullable");
            }


        }

    }
}
