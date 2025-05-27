using System.Collections.Generic;
using CoreApp.API.Domain;

namespace CoreApp.API.Features.Comments;

public record CommentsEnvelope(List<Comment> Comments);
