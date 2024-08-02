# SmartCommerceAPI

## Visão Geral

A **SmartCommerceAPI** é uma API RESTful para gerenciar compradores. Oferece operações para criar, atualizar, listar e filtrar compradores.

## Versão

1.0

## Endpoints

### **/api/Buyers**

#### **GET** `/api/Buyers`

- **Descrição**: Obtém uma lista de todos os compradores.
- **Tags**: `Buyers`
- **Resposta de Sucesso (200)**:
  - **Descrição**: Retorna uma lista de compradores.
  - **Schema**:
    - **Tipo**: `array`
    - **Itens**: `Buyer`

#### **POST** `/api/Buyers`

- **Descrição**: Adiciona um novo comprador.
- **Tags**: `Buyers`
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, `application/*+json`
  - **Schema**: `Buyer`
  - **Exemplo**:
    ```json
    {
      "id": "string",
      "name": "string",
      "email": "string",
      "phone": "string",
      "personType": "string",
      "cpfCnpj": "string",
      "stateRegistration": "string",
      "gender": "string",
      "birthDate": "2024-08-02T00:00:00Z",
      "password": "string",
      "confirmPassword": "string",
      "createdAt": "2024-08-02T00:00:00Z",
      "blocked": true,
      "exempt": true
    }
    ```
- **Resposta de Sucesso (200)**:
  - **Descrição**: Retorna o comprador adicionado.
  - **Schema**: `Buyer`

### **/api/Buyers/filter**

#### **GET** `/api/Buyers/filter`

- **Descrição**: Filtra compradores com base nos parâmetros fornecidos.
- **Tags**: `Buyers`
- **Parâmetros de Consulta**:
  - `Name` (query) - Nome do comprador (opcional, tipo: `string`).
  - `Email` (query) - Email do comprador (opcional, tipo: `string`).
  - `Phone` (query) - Telefone do comprador (opcional, tipo: `string`).
  - `PersonType` (query) - Tipo de pessoa (opcional, tipo: `string`).
  - `Document` (query) - Documento do comprador (opcional, tipo: `string`).
  - `StateRegistration` (query) - Inscrição estadual do comprador (opcional, tipo: `string`).
  - `Blocked` (query) - Status de bloqueio do comprador (opcional, tipo: `boolean`).
- **Resposta de Sucesso (200)**:
  - **Descrição**: Retorna uma lista de compradores filtrados.
  - **Schema**:
    - **Tipo**: `array`
    - **Itens**: `Buyer`

### **/api/Buyers/{id}**

#### **PUT** `/api/Buyers/{id}`

- **Descrição**: Atualiza as informações de um comprador existente.
- **Tags**: `Buyers`
- **Parâmetros**:
  - `id` (path) - ID do comprador (formato UUID, obrigatório).
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, `application/*+json`
  - **Schema**: `Buyer`
  - **Exemplo**:
    ```json
    {
      "name": "string",
      "email": "string",
      "phone": "string",
      "personType": "string",
      "cpfCnpj": "string",
      "stateRegistration": "string",
      "gender": "string",
      "birthDate": "2024-08-02T00:00:00Z",
      "password": "string",
      "confirmPassword": "string",
      "blocked": true,
      "exempt": true
    }
    ```
- **Resposta de Sucesso (200)**:
  - **Descrição**: Retorna o comprador atualizado.
  - **Schema**: `Buyer`

### **/api/Buyers/validate-fields**

#### **POST** `/api/Buyers/validate-fields`

- **Descrição**: Valida os campos fornecidos para um comprador.
- **Tags**: `Buyers`
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, `application/*+json`
  - **Schema**: `Buyer`
  - **Exemplo**:
    ```json
    {
      "name": "string",
      "email": "string",
      "phone": "string",
      "personType": "string",
      "cpfCnpj": "string",
      "stateRegistration": "string",
      "gender": "string",
      "birthDate": "2024-08-02T00:00:00Z",
      "password": "string",
      "confirmPassword": "string",
      "blocked": true,
      "exempt": true
    }
    ```
- **Resposta de Sucesso (200)**:
  - **Descrição**: Retorna o status de validação.

## Schemas

### **Buyer**

- **id**: `string` (formato UUID)
- **name**: `string` (opcional)
- **email**: `string` (opcional)
- **phone**: `string` (opcional)
- **personType**: `string` (opcional)
- **cpfCnpj**: `string` (opcional)
- **stateRegistration**: `string` (opcional)
- **gender**: `string` (opcional)
- **birthDate**: `string` (formato date-time)
- **password**: `string` (opcional)
- **confirmPassword**: `string` (opcional)
- **createdAt**: `string` (formato date-time)
- **blocked**: `boolean` (opcional)
- **exempt**: `boolean` (opcional)

## **Execução dos Projetos**

### **Banco de Dados**
 - executar comando '**docker run -d -p 27017:27017 --name mongo mongo:latest**' para realizar o download da imagem do Mongo e rodá-la.

### **API**
 - executar o comando '**dotnet run**' dentro do diretório do projeto ou executá-la pelo próprio Visual Studio.

### **Front**
 - executar o comando '**ng serve**' dentro do diretório do projeto.
