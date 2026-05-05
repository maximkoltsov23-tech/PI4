using System;
using System.IO;
using Newtonsoft.Json;
using PI4.Models;

namespace PI4.Services
{
    public class BoardStorageService
    {
        private readonly string filePath;

        public BoardStorageService()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PI4TeamTaskBoard");

            Directory.CreateDirectory(folder);
            filePath = Path.Combine(folder, "board-cache.json");
        }

        public string FilePath => filePath;

        public BoardState Load()
        {
            if (!File.Exists(filePath))
            {
                return CreateDefaultState();
            }

            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<BoardState>(json) ?? CreateDefaultState();
            }
            catch
            {
                return CreateDefaultState();
            }
        }

        public void Save(BoardState state)
        {
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private static BoardState CreateDefaultState()
        {
            var state = new BoardState();

            state.Columns.Add(new TaskColumn
            {
                Title = "К выполнению",
                Cards =
                {
                    new TaskCard
                    {
                        Title = "Подготовить README",
                        Description = "Описать назначение программы, запуск и платформу.",
                        Assignee = "Алексей",
                        Priority = "Средний",
                        DueDate = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy")
                    }
                }
            });

            state.Columns.Add(new TaskColumn
            {
                Title = "В работе",
                Cards =
                {
                    new TaskCard
                    {
                        Title = "Сверстать доску",
                        Description = "Создать WPF-интерфейс с колонками и карточками.",
                        Assignee = "Иван",
                        Priority = "Высокий",
                        DueDate = DateTime.Today.AddDays(2).ToString("dd.MM.yyyy")
                    }
                }
            });

            state.Columns.Add(new TaskColumn { Title = "Готово" });

            return state;
        }
    }
}