using System.Runtime.Serialization;

namespace Forum.Contracts
{
    public enum PostType
    {
        [EnumMember(Value = "Event")]
        Event,
        [EnumMember(Value = "Place")]
        Place
    }
}
