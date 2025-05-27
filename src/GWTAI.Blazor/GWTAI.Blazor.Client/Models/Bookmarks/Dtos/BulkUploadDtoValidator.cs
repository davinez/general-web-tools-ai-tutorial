using FluentValidation;

namespace GWTAI.Blazor.Client.Models.Bookmarks.Dtos;

public class BulkUploadDtoValidator : AbstractValidator<BulkUploadDto>
{
  public BulkUploadDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(upload => upload.FileName)
        .NotEmpty().WithMessage("File name is a required field.")
        .Length(5, 50).WithMessage("File name must be between 5 and 50 characters.");

    RuleFor(upload => upload.FileContent)
      .NotEmpty().WithMessage("Uploaded file is required.");
  }
}
