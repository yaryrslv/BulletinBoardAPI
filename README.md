# BulletinBoardAPI
# API для реализации Web приложения Объявления на Angular.
## Используемые технологии
 1. Платформа: ASP.NET Core
 2. База данных: MS SQL.
 3. ОRМ: EntityFramework.
 4. Аутентификация и авторизация: Identity + JTW token (Bearer)
 5. Документация: Swagger (Swashbuckle)
## Требования
1. .NET 5
2. MS SQL 2019
## Установка
1. Редактирование настроек appsettings.json:  
"DefaultConnection": "ConnectionString" Cтрока подключения к MS SQL.  
"SecretKey": "JWTTokenSectetKey" Уникальный секретный ключ, для создания токена, необходимо ввести любое уникальное значение.
"Issuer": "https://localhost:44378" Ссылка для создателя токена.  
"Audience": "http://localhost:4200" Ссылка для получателей токена.  
"AdminRegistrationKeyMD5Hash": "MD5Value" MD5 хеш ключа, для регистрации пользователей с Admin правами.  
Если используются стандартный хост и порт, лучше оставить значения по умолчанию.  
2. Миграция базы данных:  
Выполнить в .NET Core CLI:  
`dotnet ef migrations add InitialCreate`  
`dotnet ef database update`  
3. Запуск:  
`dotnet run`  
4. Публикация:  
https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli  


