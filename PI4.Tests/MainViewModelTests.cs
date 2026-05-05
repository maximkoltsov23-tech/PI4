        [Fact]
        public void Test_ThatAlwaysFails_ForDemonstration()
        {
            // Этот тест специально падает, чтобы показать блокировку слияния
            Assert.True(false, "Этот тест упал намеренно для демонстрации требования №4");
        }
