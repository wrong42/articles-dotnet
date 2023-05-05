using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RequiredAttributeTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequiredAttributeTestController : ControllerBase
{
    #region Int
    [HttpPost("int")]
    public IActionResult PostWithInt([FromBody] TestClassWithInt request)
    {
        return Ok(request.NumberRequiredNullable);
    }

    public class TestClassWithInt
    {
        public int Number { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberRequired { get; set; }

        [Required]
        public int? NumberRequiredNullable { get; set; }
    }
    #endregion Int

    #region string
    [HttpPost("string")]
    public IActionResult PostWithString([FromBody] TestClassWithString request)
    {
        return Ok(request.NameRequired);
    }
    public class TestClassWithString
    {
        public string Name { get; set; } = string.Empty;

        [Required]
        public string NameRequired { get; set; } = string.Empty;
    }
    #endregion string

    #region guid
    [HttpPost("guid")]
    public IActionResult PostWithGuid([FromBody] TestClassWithGuid request)
    {
        return Ok(request.IdRequired);
    }

    [HttpPost("guid-validatable-object")]
    public IActionResult PostWithGuidValidatableObject([FromBody] TestClassWithGuidValidatable request)
    {
        return Ok(request.IdRequired);
    }

    public class TestClassWithGuid
    {
        public Guid Id { get; set; }

        [Required]
        public Guid IdRequired { get; set; }

        [Required]
        public Guid? IdRequiredNullable { get; set; }
    }

    public class TestClassWithGuidValidatable : IValidatableObject
    {
        public Guid Id { get; set; }

        public Guid IdRequired { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IdRequired == default)
            {
                yield return new ValidationResult("This ID is required", new[] { nameof(IdRequired) });
            }
        }
    }
    #endregion guid

    #region validatable-object-in-collection
    [HttpPost("validatable-object-collection")]
    public IActionResult PostWithValidatableObjectCollection([FromBody] TestClassWithValidatableObjectCollection request)
    {
        return Ok(request.Items.Count());
    }

    public class TestClassWithValidatableObjectCollection : IValidatableObject
    {
        public IEnumerable<TestClassWithGuid> Items { get; set; } = Enumerable.Empty<TestClassWithGuid>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Items.Any())
            {
                yield return new ValidationResult("EMPTY COLLETION NOT ALLOWED", new[] { nameof(Items) });
            }
        }
    }
    #endregion validatable-object-in-collection
}
