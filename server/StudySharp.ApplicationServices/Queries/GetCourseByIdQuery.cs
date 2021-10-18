using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using StudySharp.Domain.Constants;
using StudySharp.Domain.General;
using StudySharp.Domain.Models;
using StudySharp.Domain.ValidationRules;
using StudySharp.DomainServices;

namespace StudySharp.ApplicationServices.Queries
{
    public sealed class GetCourseByIdQuery : IRequest<OperationResult<Course>>
    {
        public int Id { get; set; }
    }

    public partial class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
    {
        public GetCourseByIdQueryValidator(ICourseRules rules)
        {
            RuleFor(_ => _.Id)
                .MustAsync(rules.IsCourseIdExistAsync)
                .WithMessage(_ => string.Format(ErrorConstants.EntityNotFound, nameof(Course), nameof(Course.Id), _.Id));
        }
    }

    public sealed class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, OperationResult<Course>>
    {
        private readonly StudySharpDbContext _context;

        public GetCourseByIdQueryHandler(StudySharpDbContext studySharpDbContext)
        {
            _context = studySharpDbContext;
        }

        public async Task<OperationResult<Course>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FindAsync(request.Id);
            return OperationResult.Ok(course);
        }
    }
}