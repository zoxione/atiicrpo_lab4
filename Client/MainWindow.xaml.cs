using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace Client
{
    public partial class MainWindow : Window
    {
        private Thread modelThread;

        public MainWindow()
        {
            InitializeComponent();

            //this.FontFamily = new FontFamily("Pixel Cyr");
            this.FontFamily = new FontFamily("Bahnschrift SemiBold");

            DataContext = Library.Instance;

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void CreateSubscriber(string name, string gender)
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 3);

            Subscriber subscriber = new Subscriber(Guid.NewGuid(), name, new ObservableCollection<CopyBook>(), $"/assets/{gender}_{random}.png", 0);

            Dispatcher.Invoke(() => {
                Library.Instance.Subscribers.Add(subscriber);
            });

            CreateChatMessage($"В библиотеку зашел новый абонент {subscriber.Name}");
        }

        private void CreateCardBook(Book book)
        {
            // Image
            Image bookImage = new Image();
            bookImage.Height = 80;
            bookImage.Width = 80;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\Users\zoxi\source\repos\atiicrpo_lab4\Client\assets\book.png");
            bitmap.EndInit();
            bookImage.Source = bitmap;

            // Title
            TextBlock titleTextBlock = new TextBlock();
            titleTextBlock.Foreground = new SolidColorBrush(Colors.White);
            titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            Binding titleBinding = new Binding("Title");
            titleBinding.Source = book;
            BindingOperations.SetBinding(titleTextBlock, TextBlock.TextProperty, titleBinding);

            TextBlock countCopiesTextBlock = new TextBlock();
            countCopiesTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Binding countCopiesBinding = new Binding("CountCopies");
            countCopiesBinding.Source = book;
            countCopiesBinding.StringFormat = " {0}:";
            BindingOperations.SetBinding(countCopiesTextBlock, TextBlock.TextProperty, countCopiesBinding);

            TextBlock countIssueCopiesTextBlock = new TextBlock();
            countIssueCopiesTextBlock.Foreground = new SolidColorBrush(Colors.White);
            Binding countIssueCopiesBinding = new Binding("CountIssueCopies");
            countIssueCopiesBinding.Source = book;
            BindingOperations.SetBinding(countIssueCopiesTextBlock, TextBlock.TextProperty, countIssueCopiesBinding);

            StackPanel titleStackPanel = new StackPanel();
            titleStackPanel.Orientation = Orientation.Horizontal;
            titleStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            titleStackPanel.Children.Add(titleTextBlock);
            titleStackPanel.Children.Add(countCopiesTextBlock);
            titleStackPanel.Children.Add(countIssueCopiesTextBlock);

            // ToolTip
            ToolTip toolTip = new ToolTip();
            StackPanel stackPanelToolTip = new StackPanel();
            foreach (var prop in typeof(Book).GetProperties())
            {
                TextBlock nameText = new TextBlock();
                nameText.Text = $"{prop.Name}: ";

                TextBlock valueText = new TextBlock();
                Binding textBlockBinding = new Binding(prop.Name);
                textBlockBinding.Source = book;
                BindingOperations.SetBinding(valueText, TextBlock.TextProperty, textBlockBinding);

                StackPanel textStackPanel = new StackPanel();
                textStackPanel.Orientation = Orientation.Horizontal;
                textStackPanel.Children.Add(nameText);
                textStackPanel.Children.Add(valueText);

                stackPanelToolTip.Children.Add(textStackPanel);
            }
            toolTip.Content = stackPanelToolTip;

            // StackPanel
            StackPanel stackPanel = new StackPanel();
            stackPanel.Name = $"book{book.Id.ToString("N")}";
            RegisterName(stackPanel.Name, stackPanel);
            stackPanel.Margin = new Thickness(10);
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(titleStackPanel);
            stackPanel.Children.Add(bookImage);
            stackPanel.ToolTip = toolTip;

            if (CardsGrid.Children.Count % CardsGrid.ColumnDefinitions.Count == 0)
            {
                RowDefinition rowDefinition = new RowDefinition();
                CardsGrid.RowDefinitions.Add(rowDefinition);
            }

            CardsGrid.Children.Add(stackPanel);

            int a = CardsGrid.RowDefinitions.Count - 1;
            int b = (CardsGrid.Children.Count - 1) % CardsGrid.ColumnDefinitions.Count;
            Grid.SetRow(stackPanel, a);
            Grid.SetColumn(stackPanel, b);
        }

        private void CreateChatMessage(string message) 
        {
            Dispatcher.Invoke(() => {
                Library.Instance.Messages.Add(message);
            });
        }

        private void UiIssueBook(Subscriber subscriber, Book book, TimeSpan period)
        {
            string message = $"Выдача книги {book.Title} абоненту {subscriber.Name}...\n";

            int res = -1;
            Dispatcher.Invoke(() => {
                Strategy strategy = new StrategyIssue();
                res = Library.Instance.Execute(strategy, subscriber.Id, book.Id, period);
            });

            switch (res)
            {
                case 0:
                    message += $"Абонент {subscriber.Name} получил книгу {book.Title}";
                    break;
                case 1:
                    message += $"Книга {book.Title} закончилась";
                    break;
                case 2:
                    message += $"Книги {book.Title} нет";
                    break;
                case 3:
                    message += $"Абонента {subscriber.Name} нет";
                    break;
                default:
                    break;
            }

            CreateChatMessage(message);
        }

        private void UiReturnBook(Subscriber subscriber, CopyBook copyBook)
        {
            Book book = Library.Instance.Books.ToList().Find(x => x.Id == copyBook.IdBook);

            if (book != null)
            {
                string message = $"Возврат книги {book.Title} абонента {subscriber.Name}...\n";

                int res = -1;
                Dispatcher.Invoke(() => {
                    Strategy strategy = new StrategyReturn();
                    res = Library.Instance.Execute(strategy, subscriber.Id, copyBook.Id, null);
                });

                switch (res)
                {
                    case 0:
                        message += $"Абонент {subscriber.Name} вернул свою книгу";
                        break;
                    case 1:
                        message += $"Абонент {subscriber.Name} несвоевременно вернул свою книгу";
                        break;
                    case 2:
                        message += $"Книги {book.Title} нет";
                        break;
                    case 3:
                        message += $"Абонента {subscriber.Name} нет";
                        break;
                    default:
                        break;
                }

                CreateChatMessage(message);
            }
            else
            {
                CreateChatMessage("Произошла ошибка");
            }
        }

        private void UiLoseBook(Subscriber subscriber, CopyBook copyBook)
        {
            Book book = Library.Instance.Books.ToList().Find(x => x.Id == copyBook.IdBook);

            if (book != null)
            {
                string message = $"Утеря книги {book.Title} абонента {subscriber.Name}...\n";

                int res = -1;
                Dispatcher.Invoke(() => {
                    Strategy strategy = new StrategyLose();
                    res = Library.Instance.Execute(strategy, subscriber.Id, copyBook.Id, null);
                });

                switch (res)
                {
                    case 0:
                        message += $"Книга {book.Title} абонента {subscriber.Name} списана и выписан штраф 1000";
                        break;
                    case 2:
                        message += $"Книги {book.Title} нет";
                        break;
                    case 3:
                        message += $"Абонента {subscriber.Name} нет";
                        break;
                    default:
                        break;
                }

                CreateChatMessage(message);
            }
            else 
            {
                CreateChatMessage("Произошла ошибка");
            }
        }

        private void ModelHandler() 
        {
            try
            {
                CreateSubscriber("Джон", "man");
                CreateSubscriber("Маша", "woman");

                Thread.Sleep(3000);

                UiIssueBook(Library.Instance.Subscribers[0], Library.Instance.Books[0], new TimeSpan(1, 0, 0, 0));
                Thread.Sleep(2000);
                UiIssueBook(Library.Instance.Subscribers[0], Library.Instance.Books[0], new TimeSpan(1, 0, 0, 0));
                Thread.Sleep(2000);
                UiIssueBook(Library.Instance.Subscribers[0], Library.Instance.Books[0], new TimeSpan(1, 0, 0, 0));
                Thread.Sleep(2000);
                UiIssueBook(Library.Instance.Subscribers[0], Library.Instance.Books[0], new TimeSpan(1, 0, 0, 0));

                Thread.Sleep(3000);

                CreateSubscriber("Себастиан", "man");
                UiIssueBook(Library.Instance.Subscribers[2], Library.Instance.Books[3], new TimeSpan(0, 0, 0, 1));
                Thread.Sleep(2000);
                UiIssueBook(Library.Instance.Subscribers[2], Library.Instance.Books[3], new TimeSpan(0, 0, 0, 1));
                Thread.Sleep(2000);

                Thread.Sleep(3000);

                UiReturnBook(Library.Instance.Subscribers[0], Library.Instance.Subscribers[0].CopyBooks[2]);
                Thread.Sleep(2000);
                UiReturnBook(Library.Instance.Subscribers[0], Library.Instance.Subscribers[0].CopyBooks[1]);
                Thread.Sleep(2000);
                UiReturnBook(Library.Instance.Subscribers[2], Library.Instance.Subscribers[2].CopyBooks[0]);
                Thread.Sleep(2000);

                Thread.Sleep(3000);

                UiLoseBook(Library.Instance.Subscribers[0], Library.Instance.Subscribers[0].CopyBooks[0]);

                CreateChatMessage("Выполнение модели завершено");
            }
            catch (ThreadInterruptedException)
            {
                CreateChatMessage("Выполнение модели прервано");
            }
            finally 
            {
                Dispatcher.Invoke(() => {
                    InitButton.IsEnabled = true;
                    StartButton.IsEnabled = true;
                    StopButton.IsEnabled = false;
                });
            }
        }

        private void ButtonInitialize_Click(object sender, RoutedEventArgs e)
        {
            CardsGrid.Children.Clear();
            CardsGrid.RowDefinitions.Clear();
            Library.Instance.Clear();

            Library.Instance.Books = new ObservableCollection<Book>()
            {
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Код да Винчи", "Дэн Браун", "Триллер", "Полка 1А", 3),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Портрет Дориана Грея", "Оскар Уайльд", "Готический роман", "Полка 1B", 2),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Над пропастью во ржи", "Дж.Д. Сэлинджер", "Роман-инициация", "Полка 1C", 5),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Великий Гэтсби", "Ф. Скотт Фицджеральд", "Трагический роман", "Полка 2A", 4),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Убить пересмешника", "Харпер Ли", "Южный готический роман", "Полка 2B", 6),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Властелин колец", "Дж.Р.Р. Толкин", "Высокое фэнтези", "Полка 3A", 3),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Хоббит", "Дж.Р.Р. Толкин", "Фэнтези", "Полка 3B", 2),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "1984", "Джордж Оруэлл", "Антиутопия", "Полка 3C", 4),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "О дивный новый мир", "Олдос Хаксли", "Антиутопия", "Полка 4A", 3),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idFictionBook, "Ферма зверей", "Джордж Оруэлл", "Сатирическая повесть", "Полка 4B", 2),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idTechnicalBook, "Чистый код", "Роберт Мартин", "Инженерия программного обеспечения", "Полка 5A", 1),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idTechnicalBook, "Полное кодирование", "Стив Макконнелл", "Инженерия программного обеспечения", "Полка 5B", 2),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idTechnicalBook, "Паттерны проектирования", "Эрих Гамма и др.", "Инженерия программного обеспечения", "Полка 6A", 3),
                FactoryBook.CreateBook(FactoryBook.IdTypeBook.idTechnicalBook, "Прагматический программист", "Эндрю Хант и Дэвид Томас", "Инженерия программного обеспечения", "Полка 6B", 4),
            };
            foreach (var cardbook in Library.Instance.Books)
            {
                CreateCardBook(cardbook);
            }

            CreateChatMessage("Начало работы");
            CreateChatMessage($"Книги в количестве {Library.Instance.Books.Count}-ти штук расставлены на полках");

            Librarian _librarian = new Librarian(Guid.NewGuid(), "Стив");
            Library.Instance.Librarian = _librarian;
            CreateChatMessage($"Библиотекарь {_librarian.Name} приступил к работе");

            StartButton.IsEnabled = true;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            InitButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            Library.Instance.Subscribers.Clear();
            // TODO Library.Instance.Messages.Clear();

            modelThread = new Thread(ModelHandler);
            modelThread.SetApartmentState(ApartmentState.STA);
            modelThread.Start();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            modelThread.Interrupt();
        }
    }
}
