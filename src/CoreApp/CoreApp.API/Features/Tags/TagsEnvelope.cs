using System.Collections.Generic;

namespace CoreApp.API.Features.Tags;

public class TagsEnvelope
{
    public List<string> Tags { get; set; } = new();
}
