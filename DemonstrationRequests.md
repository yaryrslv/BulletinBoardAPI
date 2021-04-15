#### Регистрация пользователя c ролью Admin:  
`curl -X POST "https://localhost:44397/api/User/registeradmin?adminRegistrationKey=BulletinBoardApi" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"username\":\"newadminuser\",\"email\":\"newadminuser@mail.com\",\"phoneNumber\":\"88888888\",\"password\":\"Password0!\"}"`
#### Регистрация пользователя с ролью User:
