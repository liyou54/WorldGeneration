namespace SGOAP.Examples
{
    public interface IAbilityContextData
    {
        ICharacter Owner { get; }
        ICharacter Receiver { get; }
    }
}