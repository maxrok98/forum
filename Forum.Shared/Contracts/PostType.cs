using System.Runtime.Serialization;

namespace Forum.Shared.Contracts
{
    public enum PostType
    {
        [EnumMember(Value = "Event")]
        Event,
        [EnumMember(Value = "Place")]
        Place
    }
}
