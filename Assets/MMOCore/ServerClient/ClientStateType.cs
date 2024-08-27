namespace MMOCore
{
    public enum ClientStateType
    { 
        none,
        blocked,
        init,
        catScene,
        inGame,
        inMenu,    
    }
    public enum SpiritStatusType
    {
        dead,
        alive,
        move,
    }

    public enum SpiritCommandType
    {
        none,
        move,
    }
}