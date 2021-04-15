## Работа с пользователями
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
#### Отправка запроса с токеном напрямую:  
Токен добавляется в заголовок, с ключом Authorization.  
Пример:
`"Authorization: Bearer {JWTToken}"`
#### Получение всех пользователей:
`curl -X GET "https://localhost:44397/api/UserManager/all" -H  "accept: text/plain" -H  "Authorization: Bearer {JWTToken}"`
#### Получение пользователя по Id:
`curl -X GET "https://localhost:44397/api/User/getbyid/{Id}" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
#### Получение пользователя по UserName:
`curl -X GET "https://localhost:44397/api/User/getbyid/{UserName}" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
#### Обновление почты текущего пользователя:
`curl -X PUT "https://localhost:44397/api/User/updateemail" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}" -H  "Content-Type: application/json" -d "{\"email\":\"newadminusernewmail@example.com\"}"`
#### Обновление телефона текущего пользователя:
`curl -X PUT "https://localhost:44397/api/User/updatphonenumber" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}" -H  "Content-Type: application/json" -d "{\"phoneNumber\":\"888888888888888888\"}"`
#### Обновление пароля текущего пользователя:
`curl -X PUT "https://localhost:44397/api/User/updatepassword" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}" -H  "Content-Type: application/json" -d "{\"newPassword\":\"NewPassword0!\"}"`
#### Удаление текущего пользователя:
`curl -X DELETE "https://localhost:44397/api/User/deletcurrenteuser" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
## Работа с пользователями как Администратор
#### Получение полной информации обо всех пользователях:
`curl -X GET "https://localhost:44397/api/UserManager/all" -H  "accept: text/plain" -H  "Authorization: Bearer {JWTToken}"`
#### Получение полной информации о любом пользователе по Id:
`curl -X GET "https://localhost:44397/api/UserManager/getbyid/{UserId}" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
#### Получение полной информации о любом пользователе по UserName:
`curl -X GET "https://localhost:44397/api/UserManager/getbyusername/{username}" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
#### Получение списка ролей любого пользователя по Id:
`curl -X GET "https://localhost:44397/api/UserManager/getuserrolesbyid/eb3215c7-6413-4a66-a3cd-9285eea31628" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}"`
#### Обновление почты любого пользователя по Id:
curl -X PUT "https://localhost:44397/api/UserManager/updateemailbyid/eb3215c7-6413-4a66-a3cd-9285eea31628" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}" -H  "Content-Type: application/json" -d "{\"email\":\"newusernewemail@example.com\"}"
#### Обновление телефона любого пользователя по Id:
curl -X PUT "https://localhost:44397/api/UserManager/updateemailbyid/eb3215c7-6413-4a66-a3cd-9285eea31628" -H  "accept: */*" -H  "Authorization: Bearer {JWTToken}" -H  "Content-Type: application/json" -d "{\"phoneNumber\":\"88888888\"}"

