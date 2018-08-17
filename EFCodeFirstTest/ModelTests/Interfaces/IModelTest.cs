namespace EFCodeFirstTest.ModelTests.Interfaces
{
    public interface IModelTest
    {
        void InitilizeOncePerRun();
        void CleanupOncePerRun();
        void InitializeBeforeEachTest();
        void CleanupAfterEachTest();
    }
}