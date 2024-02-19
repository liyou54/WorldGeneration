namespace SGOAP.Examples
{
    public struct AbilityContextData : IAbilityContextData
    {
        public ICharacter Owner { get; }
        public ICharacter Receiver { get; }
        public AbilityContextData(ICharacter owner, ICharacter receiver)
        {
            Owner = owner;
            Receiver = receiver;
        }
    }
}