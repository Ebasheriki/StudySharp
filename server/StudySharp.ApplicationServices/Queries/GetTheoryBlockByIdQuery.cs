using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudySharp.Domain.Constants;
using StudySharp.Domain.General;
using StudySharp.Domain.Models;
using StudySharp.DomainServices;

namespace StudySharp.ApplicationServices.Queries
{
    public sealed class GetTheoryBlockByIdQuery : IRequest<OperationResult<TheoryBlock>>
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
    }

    public sealed class GetTheoryBlockByIdQueryHandler : IRequestHandler<GetTheoryBlockByIdQuery, OperationResult<TheoryBlock>>
    {
        private readonly StudySharpDbContext _context;

        public GetTheoryBlockByIdQueryHandler(StudySharpDbContext studySharpDbContext)
        {
            _context = studySharpDbContext;
        }

        public async Task<OperationResult<TheoryBlock>> Handle(GetTheoryBlockByIdQuery request, CancellationToken cancellationToken)
        {
            var theoryBlock = await _context.TheoryBlocks.FirstOrDefaultAsync(_ => _.Id == request.Id && _.CourseId == request.CourseId, cancellationToken);
            if (theoryBlock == null)
            {
                return OperationResult.Fail<TheoryBlock>(string.Format(ErrorConstants.EntityNotFound, nameof(TheoryBlock), nameof(TheoryBlock.Id), nameof(TheoryBlock.CourseId), request.Id, request.CourseId));
            }

            return OperationResult.Ok(theoryBlock);
        }
    }
}