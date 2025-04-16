namespace NFW
{
    public abstract class Manager<T> where T : Manager<T>
    {
        public virtual void LogicUpdate(float deltaTime)
        { }

        public virtual void RenderUpdate(float deltaTime)
        { }
    }
}