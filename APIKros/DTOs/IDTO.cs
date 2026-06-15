namespace APIKros.DTOs
{
   public interface IDto<TEntity, TDto>
    {
        static abstract TDto CreateInstance(TEntity entity);
    }
}