namespace workoutapp_API.services.Catalog
{
    public interface IExerciseCatalogSeedService
    {
        Task<int> SeedFromExternalApiAsync();
    }
}
