#### Регистрация пользователя c ролью Admin:  
`curl -X POST "https://localhost:44397/api/User/registeradmin?adminRegistrationKey=BulletinBoardApi" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"username\":\"newadminuser\",\"email\":\"newadminuser@mail.com\",\"phoneNumber\":\"88888888\",\"password\":\"Password0!\"}"`
#### Регистрация пользователя c ролью User:
`curl -X POST "https://localhost:44397/api/User/register" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"username\":\"newuser\",\"email\":\"newuser@mail.com\",\"phoneNumber\":\"8888\",\"password\":\"Password0!\"}"`
#### Авторизация пользователя и получение токена:
`curl -X POST "https://localhost:44397/api/User/login" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"username\":\"newadminuser\",\"password\":\"Password0!\"}"` Во всех запросах, требующих аутентификацию и ввторизацию, необходимо передавать токен в заголовке, с ключом Authorization, в виде:
`"Authorization: Bearer {JWTToken}"`
#### Добавление токена в Swagger UI:
1. Зайти на:  
https://localhost:44397/swagger/index.html  
2. Кликнуть Authorize.
3. Ввести "Bearer {JWTToken}" в поле Value.
4. Кликнуть Authorize.
#### Получение всех пользователей:
`curl -X GET "https://localhost:44397/api/UserManager/all" -H  "accept: text/plain" -H  "Authorization: Bearer {JWTToken}"`
#### Получение всех пользователей:
