using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PI4.Infrastructure;
using PI4.Models;
using PI4.Services;

namespace PI4.ViewModels
{
    public class MainViewModel : NotifyObject
    {
        private readonly BoardStorageService storageService;
        private readonly BoardState state;
        private string newColumnTitle;
        private TaskCard selectedCard;
        private TaskColumn selectedCardColumn;
        private string statusMessage;

        public MainViewModel()
        {
            storageService = new BoardStorageService();
            state = storageService.Load();
            Columns = state.Columns;
            OnlineUsers = new ObservableCollection<OnlineUser>
            {
                new OnlineUser { Name = "Алексей", Role = "Server" },
                new OnlineUser { Name = "Мария", Role = "Models" },
                new OnlineUser { Name = "Иван", Role = "ViewModel" },
                new OnlineUser { Name = "Ольга", Role = "UI/UX" }
            };

            AddColumnCommand = new RelayCommand(_ => AddColumn(), _ => !string.IsNullOrWhiteSpace(NewColumnTitle));
            DeleteColumnCommand = new RelayCommand(DeleteColumn, p => p is TaskColumn);
            AddCardCommand = new RelayCommand(AddCard, p => p is TaskColumn column && !string.IsNullOrWhiteSpace(column.NewCardTitle));
            EditCardCommand = new RelayCommand(EditCard, p => p is TaskCard);
            DeleteCardCommand = new RelayCommand(DeleteCard, p => p is TaskCard);
            MoveCardLeftCommand = new RelayCommand(p => MoveCard(p, -1), CanMoveLeft);
            MoveCardRightCommand = new RelayCommand(p => MoveCard(p, 1), CanMoveRight);
            SaveCommand = new RelayCommand(_ => SaveBoard());
            ClearSelectionCommand = new RelayCommand(_ => ClearSelection(), _ => SelectedCard != null);

            StatusMessage = "Локальная копия загружена: " + storageService.FilePath;
        }

        public ObservableCollection<TaskColumn> Columns { get; }

        public ObservableCollection<OnlineUser> OnlineUsers { get; }

        public string NewColumnTitle
        {
            get => newColumnTitle;
            set
            {
                if (SetProperty(ref newColumnTitle, value))
                {
                    RaiseCommands();
                }
            }
        }

        public TaskCard SelectedCard
        {
            get => selectedCard;
            set
            {
                if (SetProperty(ref selectedCard, value))
                {
                    RaiseCommands();
                }
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public ICommand AddColumnCommand { get; }

        public ICommand DeleteColumnCommand { get; }

        public ICommand AddCardCommand { get; }

        public ICommand EditCardCommand { get; }

        public ICommand DeleteCardCommand { get; }

        public ICommand MoveCardLeftCommand { get; }

        public ICommand MoveCardRightCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ClearSelectionCommand { get; }

        private void AddColumn()
        {
            Columns.Add(new TaskColumn { Title = NewColumnTitle.Trim() });
            NewColumnTitle = string.Empty;
            SaveBoard("Колонка добавлена и сохранена.");
        }

        private void DeleteColumn(object parameter)
        {
            var column = parameter as TaskColumn;
            if (column == null)
            {
                return;
            }

            if (column.Cards.Count > 0)
            {
                var result = MessageBox.Show(
                    "Удалить колонку вместе со всеми карточками?",
                    "Удаление колонки",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            if (selectedCardColumn == column)
            {
                ClearSelection();
            }

            Columns.Remove(column);
            SaveBoard("Колонка удалена.");
        }

        private void AddCard(object parameter)
        {
            var column = parameter as TaskColumn;
            if (column == null || string.IsNullOrWhiteSpace(column.NewCardTitle))
            {
                return;
            }

            var card = new TaskCard
            {
                Title = column.NewCardTitle.Trim(),
                Description = "Описание задачи",
                Assignee = OnlineUsers.FirstOrDefault()?.Name ?? "Не назначен"
            };

            column.Cards.Add(card);
            column.NewCardTitle = string.Empty;
            SelectedCard = card;
            selectedCardColumn = column;
            SaveBoard("Карточка добавлена.");
        }

        private void EditCard(object parameter)
        {
            SelectedCard = parameter as TaskCard;
            selectedCardColumn = FindColumn(SelectedCard);
        }

        private void DeleteCard(object parameter)
        {
            var card = parameter as TaskCard;
            var column = FindColumn(card);

            if (card == null || column == null)
            {
                return;
            }

            column.Cards.Remove(card);

            if (SelectedCard == card)
            {
                ClearSelection();
            }

            SaveBoard("Карточка удалена.");
        }

        private void MoveCard(object parameter, int direction)
        {
            var card = parameter as TaskCard;
            var sourceColumn = FindColumn(card);

            if (card == null || sourceColumn == null)
            {
                return;
            }

            var sourceIndex = Columns.IndexOf(sourceColumn);
            var targetIndex = sourceIndex + direction;

            if (targetIndex < 0 || targetIndex >= Columns.Count)
            {
                return;
            }

            sourceColumn.Cards.Remove(card);
            Columns[targetIndex].Cards.Add(card);
            selectedCardColumn = Columns[targetIndex];
            SelectedCard = card;
            SaveBoard("Карточка перемещена.");
        }

        private bool CanMoveLeft(object parameter)
        {
            var column = FindColumn(parameter as TaskCard);
            return column != null && Columns.IndexOf(column) > 0;
        }

        private bool CanMoveRight(object parameter)
        {
            var column = FindColumn(parameter as TaskCard);
            return column != null && Columns.IndexOf(column) < Columns.Count - 1;
        }

        private TaskColumn FindColumn(TaskCard card)
        {
            return card == null ? null : Columns.FirstOrDefault(column => column.Cards.Contains(card));
        }

        private void SaveBoard()
        {
            SaveBoard("Изменения сохранены в локальный JSON-кэш.");
        }

        private void SaveBoard(string message)
        {
            storageService.Save(state);
            StatusMessage = message;
            RaiseCommands();
        }

        private void ClearSelection()
        {
            SelectedCard = null;
            selectedCardColumn = null;
        }

        private void RaiseCommands()
        {
            Raise(AddColumnCommand);
            Raise(AddCardCommand);
            Raise(DeleteColumnCommand);
            Raise(DeleteCardCommand);
            Raise(MoveCardLeftCommand);
            Raise(MoveCardRightCommand);
            Raise(ClearSelectionCommand);
        }

        private static void Raise(ICommand command)
        {
            (command as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
