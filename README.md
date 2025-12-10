# Employee API Documentation

## Descrição
A **Employee API** é uma aplicação desenvolvida em `.NET 9` para gerenciar operações relacionadas a funcionários.

---

# Employee API 
Inicialização com Docker

Este projeto utiliza **Docker + Docker Compose** para rodar:

-   API em .NET\
-   SQL Server\
-   Criação do banco via script `.sql`\

------------------------------------------------------------------------

## Estrutura de Pastas

    repository/
    │
    ├── docker-compose.yml
    ├── Dockerfile                # Dockerfile da API
    │
    ├── sql/
    │   ├── Dockerfile            # Dockerfile do SQL Server
    │   └── migration.sql
    │
    └── src/
        └── Employee.Api/

------------------------------------------------------------------------

## ✅ Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:

-   Docker Desktop\
-   Git\
-   .NET SDK (opcional)

Verifique:

``` bash
docker --version
docker compose version
```

------------------------------------------------------------------------

## Inicialização do Projeto

``` bash
docker compose up -d --build
```

Esse comando irá criar toda a infraestrutura automaticamente.

------------------------------------------------------------------------



------------------------------------------------------------------------

## Credenciais de Acesso ao Banco

-   Servidor: localhost,1433\
-   Usuário: sa\
-   Senha:  Configurada no `docker-compose.yml` através da variável de ambiente:
    ```yaml
    environment:
      SA_PASSWORD: "ConfigureSuaSenha@123"
-   Banco: EmployeeDb

------------------------------------------------------------------------

## Resetar Ambiente


``` bash
docker compose down -v
docker compose up -d --build
```

------------------------------------------------------------------------


Ambiente pronto para desenvolvimento.


## Endpoint: Authenticate

### Descrição
O endpoint `Authenticate` é utilizado para autenticar um usuário com base em suas credenciais (e-mail e senha). Caso as credenciais sejam válidas, um token de autenticação é retornado, permitindo o acesso a recursos protegidos da API.

### URL
`POST /api/auth/authenticate`

---

### Payload de Entrada
O payload de entrada deve ser enviado no formato JSON e conter os seguintes campos:

#### Campos
```json
{
  "email": "string",
  "password": "string"
}
```

---

### Payload de Saída
Se as credenciais forem válidas, o endpoint retornará um payload no formato JSON com as seguintes informações:


#### Campos
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "id": "a3f1c2b4-9d8e-4f12-b7c9-1a2b3c4d5e6f"
}
```

---

### Respostas
- **200 OK:** Retorna o token de autenticação e o ID do usuário.
- **401 Unauthorized:** Credenciais inválidas (e-mail ou senha incorretos).
- **500 Internal Server Error:** Erro interno no servidor.

---


#### Os endpoins abaixos 


## Endpoint: Criar Funcionário

### Descrição
Este endpoint é responsável por criar um novo funcionário na base de dados. Ele aceita informações detalhadas sobre o funcionário, como nome, e-mail, documentos, telefones e informações do gerente.

### URL
`POST /api/employee/create`

### Payload de Entrada
O payload de entrada deve ser enviado no formato JSON e conter os seguintes campos:


#### Campos
```json
{
  "firstName": "string", // Obrigatório
  "lastName": "string", // Obrigatório
  "email": "string", // Obrigatório
  "documentNumber": "string", // Obrigatório
  "password": "string", // Obrigatório
  "birthDate": "2025-12-10T04:36:21.538Z", // Obrigatório
  "role": 1, // Obrigatório (enum): Employee = 1, Leader = 2, Director = 3, Admin = 4
  "status": 0, // Obrigatório (enum): 0 = Active | 1 = Inactive
  "managerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Opcional (Guid)
  "phones": [ // Opcional (array)
    {
      "number": "string", // Obrigatório
      "type": 1, // Obrigatório (enum): 0 = Home | 1 = Mobile
      "isPrimary": true // Obrigatório (bool)
    }
  ]
}

```
---

### Payload de Saída
O payload de saída será retornado no formato JSON e conterá os seguintes campos:


#### Campos
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Guid — Identificador único do funcionário

  "firstName": "string", // string — Nome do funcionário
  "lastName": "string", // string — Sobrenome do funcionário
  "email": "string", // string — E-mail do funcionário
  "documentNumber": "string", // string — Documento do funcionário
  "birthDate": "2025-12-10T04:36:21.538Z", // DateTime — Data de nascimento 
  "role": 1, // enum — Cargo: 0 = Admin | 1 = Employee
  "status": 0, // enum — Status: 0 = Active | 1 = Inactive
  "createdAt": "2025-12-10T05:00:00.000Z", // DateTime — Data de criação do registro
  "updatedAt": null, // DateTime? — Data da última atualização (opcional)
  "manager": { // Objeto opcional — Dados do gerente responsável
    "id": "c1a5f42f-9f1b-4ac2-b6c3-91cdaaf1f123",
    "firstName": "string",
    "lastName": "string",
    "email": "string"
  },
  "employees": [ // Array — Funcionários subordinados (se aplicável)
    {
      "id": "b2f6d111-2d3c-47a7-b9c1-9dd1b33c4567",
      "firstName": "string",
      "lastName": "string",
      "email": "string"
    }
  ],
  "phones": [ // Array — Telefones associados ao funcionário
    {
      "number": "string", // string — Número do telefone
      "type": 1, // enum — Tipo: 0 = Home | 1 = Mobile
      "isPrimary": true // bool — Indica se é o telefone principal
    }
  ]
}

```
---

### Respostas
- **201 Created:** Funcionário criado com sucesso.
- **400 Bad Request:** Dados inválidos ou campos obrigatórios ausentes.
- **500 Internal Server Error:** Erro interno no servidor.

---

## Observações
- Certifique-se de que os campos obrigatórios estejam preenchidos corretamente.
- O campo `role` deve ser um valor válido do enum `EmployeeRole`.
- O campo `status` deve ser um valor válido do enum `EmployeeStatus`.

---



## Deletar Funcionário

### URL
`DELETE /api/Employee`

### Campos
```bash
{
  "employeeId": "f9920a52-1c07-4cf1-747a-08de37aea76c"
}
```

### Retorno
- **200 OK:** Funcionario criado com sucesso.
- **400 Bad Request:** Dados inválidos ou campos obrigatórios ausentes.
- **500 Internal Server Error:** Erro interno no servidor.

---

## Buscar Funcionário por ID (GetById)

### URL
`GET /api/Employee/{id}`

### Curl
```bash
curl --location '{{BASE_URL}}/api/Employee/6f2dff9d-51b0-433f-d6ec-08de37ef9cd1' \
--header 'accept: text/plain' \
--header 'Authorization: {{TOKEN}}'
```

### Retorno
Retorna dados completos do funcionário


### Retorno


```json
{
  "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "firstName": "Funcionario",
  "lastName": "Atualizado",
  "email": "usuario@exemplo.com",
  "documentNumber": "***********",
  "password": "string",
  "role": 1,
  "status": 0,
  "birthDate": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "createdAt": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "updatedAt": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "manager": {
    "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "firstName": "Lider",
    "lastName": "Base",
    "email": "lider@exemplo.com",
    "role": 2
  },
  "phones": [
    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "number": "***********",
      "type": 1,
      "isPrimary": true
    },
    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "number": "***********",
      "type": 1,
      "isPrimary": false
    }
  ]
}

```

---

## Atualizar Funcionário (Update)

### URL
`PUT /api/Employee`

### Entrada
```json
{
   "id":"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
   "firstName":"Funcionario",
   "lastName":"Atualizado",
   "email":"usuario@exemplo.com",
   "documentNumber":"***********",
   "birthDate":"YYYY-MM-DDTHH:MM:SS.ZZZZ",
   "role":1,
   "status":0,
   "managerId":"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
   "phones":[
      {
         "id":"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
         "number":"***********",
         "type":1,
         "isPrimary":true
      },
      {
         "number":"***********",
         "type":1,
         "isPrimary":true
      }
   ]
}
```

### Retorno
```json
{
  "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "firstName": "Funcionario",
  "lastName": "Atualizado",
  "email": "usuario@exemplo.com",
  "documentNumber": "***********",
  "password": "string",
  "role": 1,
  "status": 0,
  "birthDate": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "createdAt": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "updatedAt": "YYYY-MM-DDTHH:MM:SS.ZZZZ",
  "manager": {
    "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "firstName": "Lider",
    "lastName": "Base",
    "email": "lider@exemplo.com",
    "role": 2
  },
  "phones": [
    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "number": "***********",
      "type": 1,
      "isPrimary": true
    },
    {
      "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
      "number": "***********",
      "type": 1,
      "isPrimary": false
    }
  ]
}

```

---

## Listagem de Funcionários

### URL
`GET /api/Employee`

### Parâmetros
- StartDate
- EndDate
- ManagerId
- Role
- PageNumber
- PageSize



### Retorno
```json
{
  "currentPage": 1,
  "totalPages": 1,
  "totalCount": 1,
  "data": [
    {
      "id": "guid",
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "documentNumber": "string",
      "manager": null,
      "role": 3,
      "status": 0,
      "birthDate": "2000-12-10T05:17:40.413",
      "createdAt": "2025-12-10T05:43:45.889",
      "updatedAt": "2025-12-10T05:43:45.890"
    }
  ],
  "success": true,
  "message": "",
  "errors": []
}
```

---

## Observações
- Todos os endpoints (exceto autenticação) exigem **Authorization Bearer Token**.
- Enums devem respeitar os valores configurados no back-end.

