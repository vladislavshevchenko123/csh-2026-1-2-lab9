using lab9.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace lab9
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 1. Создаём коллекцию сервисов 
            var services = new ServiceCollection();
            // 2. Регистрируем сервисы (Lifetime) 
            // DialogService — Singleton, так как он не хранит 
            // состояние пользователя. 
            services.AddSingleton<IDialogService, DialogService>();
            // 3. ViewModel — Transient (при навигации нам будут 
            // нужны новые экземпляры) 
            services.AddTransient<MainViewModel>();
            // 4. Главное окно — Singleton с явной передачей 
            // DataContext через лямбда-выражение 
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                window.DataContext =
                sp.GetRequiredService<MainViewModel>();
                return window;
            });
            // 5. Создаём контейнер (ServiceProvider) 
            var serviceProvider =
            services.BuildServiceProvider();
            // 6. Получаем главное окно и запускаем его 
            var mainWindow =
                serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
