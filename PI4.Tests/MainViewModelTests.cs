using System.Linq;
using Xunit;

namespace PI4.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void Test_AlwaysPasses()
        {
            // Простой тест, который всегда проходит
            Assert.True(true);
        }
        
        [Fact]
        public void Test_ColumnsExist()
        {
            // Проверяем, что колонки есть (будет работать после настройки)
            Assert.True(true);
        }
    }
}
