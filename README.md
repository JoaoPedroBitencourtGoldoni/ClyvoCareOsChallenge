# ClyvoCare OS API

> Sistema de Gerenciamento de Saúde Animal  
> API RESTful desenvolvida em ASP.NET Core 8 com integração Oracle + EF Core

---

## Descrição do Projeto

O ClyvoCare OS é uma plataforma digital voltada ao cuidado preventivo e rastreamento da saúde dos pets. A API centraliza o histórico de vacinas, consultas e tratamentos, organizando as informações por fase de vida do animal (filhote, adulto, idoso) e por espécie.

Funcionalidades principais:
- Cadastro de tutores e seus respectivos pets
- Registro de vacinas com alertas de próxima dose
- Registro de consultas veterinárias com diagnóstico e prescrição
- Gestão de tratamentos com controle de status
- Histórico completo de saúde por pet
- Cálculo automático de fase de vida por espécie (filhote, adulto, idoso)

---

## Estrutura do Projeto

```
ClyvoCareOSChallenge/
├── Controllers/
│   ├── TutoresController.cs
│   ├── PetsController.cs
│   ├── VacinasController.cs
│   ├── ConsultasController.cs
│   └── TratamentosController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   └── Dtos.cs
├── Migrations/
│   └── InitialCreate.cs
├── Models/
│   ├── Tutor.cs
│   ├── Pet.cs
│   ├── Vacina.cs
│   ├── Consulta.cs
│   └── Tratamento.cs
├── appsettings.json
├── Program.cs
└── ClyvoCareOSChallenge.csproj
```

---

## Pre-requisitos

- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Acesso ao banco Oracle (FIAP ou local)
- dotnet-ef para migrations

---

## Instalacao e Execucao

### 1. Clone o repositorio
```bash
git clone https://github.com/seu-usuario/ClyvoCareOSChallenge.git
cd ClyvoCareOSChallenge/ClyvoCareOSChallenge
```

### 2. Configure a connection string

Edite o arquivo `appsettings.json` com suas credenciais Oracle:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}
```

### 3. Instale as dependencias
```bash
dotnet restore
```

### 4. Instale a ferramenta EF Core
```bash
dotnet tool install --global dotnet-ef
```

### 5. Execute as Migrations (cria as tabelas no Oracle)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 6. Execute a aplicacao
```bash
dotnet run
```

### 7. Acesse o Swagger

Abra no navegador:
```
http://localhost:5159
```

O Swagger estara na raiz do projeto.

---

## Documentacao das Rotas

### Tutores — /api/tutores

| Metodo | Rota | Descricao | Status |
|--------|------|-----------|--------|
| GET | /api/tutores | Lista todos os tutores | 200 |
| GET | /api/tutores/{id} | Busca tutor por ID | 200 / 404 |
| GET | /api/tutores/cidade/{cidade} | Filtra tutores por cidade | 200 |
| GET | /api/tutores/email/{email} | Busca tutor por e-mail | 200 / 404 |
| GET | /api/tutores/{id}/pets | Lista todos os pets do tutor | 200 / 404 |
| POST | /api/tutores | Cadastra novo tutor | 201 / 400 |
| PUT | /api/tutores/{id} | Atualiza dados do tutor | 204 / 400 / 404 |
| DELETE | /api/tutores/{id} | Remove tutor | 204 / 404 |

Exemplo de body para POST/PUT:
```json
{
  "nomeCompleto": "Maria Silva",
  "email": "maria@email.com",
  "telefone": "11999999999",
  "endereco": "Rua das Flores, 100",
  "cidade": "Sao Paulo",
  "estado": "SP"
}
```

---

### Pets — /api/pets

| Metodo | Rota | Descricao | Status |
|--------|------|-----------|--------|
| GET | /api/pets | Lista todos os pets | 200 |
| GET | /api/pets/{id} | Busca pet por ID | 200 / 404 |
| GET | /api/pets/especie/{especie} | Filtra pets por especie | 200 |
| GET | /api/pets/fase/{fase} | Filtra por fase de vida (Filhote/Adulto/Idoso) | 200 |
| GET | /api/pets/buscar/{nome} | Busca pets por nome parcial | 200 |
| GET | /api/pets/{id}/historico | Historico completo de saude | 200 / 404 |
| POST | /api/pets | Cadastra novo pet | 201 / 400 |
| PUT | /api/pets/{id} | Atualiza dados do pet | 204 / 400 / 404 |
| DELETE | /api/pets/{id} | Remove pet | 204 / 404 |

Exemplo de body para POST/PUT:
```json
{
  "nome": "Rex",
  "especie": "cachorro",
  "raca": "Labrador",
  "dataNascimento": "2022-03-15T00:00:00",
  "sexo": "M",
  "pesoKg": 12.5,
  "tutorId": 1
}
```

---

### Vacinas — /api/vacinas

| Metodo | Rota | Descricao | Status |
|--------|------|-----------|--------|
| GET | /api/vacinas | Lista todas as vacinas | 200 |
| GET | /api/vacinas/{id} | Busca vacina por ID | 200 / 404 |
| GET | /api/vacinas/pet/{petId} | Lista vacinas de um pet | 200 / 404 |
| GET | /api/vacinas/proximas/{dias} | Vacinas com dose nos proximos N dias | 200 |
| POST | /api/vacinas | Registra nova vacina | 201 / 400 |
| PUT | /api/vacinas/{id} | Atualiza vacina | 204 / 400 / 404 |
| DELETE | /api/vacinas/{id} | Remove vacina | 204 / 404 |

Exemplo de body para POST/PUT:
```json
{
  "nomeVacina": "V10 Polivalente",
  "dataAplicacao": "2024-01-10T00:00:00",
  "dataProximaDose": "2025-01-10T00:00:00",
  "veterinario": "Dr. Paulo",
  "clinica": "VetCare Centro",
  "observacoes": "Sem reacoes adversas",
  "petId": 1
}
```

---

### Consultas — /api/consultas

| Metodo | Rota | Descricao | Status |
|--------|------|-----------|--------|
| GET | /api/consultas | Lista todas as consultas | 200 |
| GET | /api/consultas/{id} | Busca consulta por ID | 200 / 404 |
| GET | /api/consultas/pet/{petId} | Lista consultas de um pet | 200 / 404 |
| GET | /api/consultas/clinica/{clinica} | Filtra por clinica | 200 |
| POST | /api/consultas | Registra nova consulta | 201 / 400 |
| PUT | /api/consultas/{id} | Atualiza consulta | 204 / 400 / 404 |
| DELETE | /api/consultas/{id} | Remove consulta | 204 / 404 |

Exemplo de body para POST/PUT:
```json
{
  "dataConsulta": "2024-06-20T10:00:00",
  "motivo": "Check-up anual",
  "diagnostico": "Animal saudavel",
  "prescricao": "Manter alimentacao atual",
  "veterinario": "Dra. Ana Lima",
  "clinica": "PetClinic Paulista",
  "pesoNaConsulta": 13.2,
  "petId": 1
}
```

---

### Tratamentos — /api/tratamentos

| Metodo | Rota | Descricao | Status |
|--------|------|-----------|--------|
| GET | /api/tratamentos | Lista todos os tratamentos | 200 |
| GET | /api/tratamentos/{id} | Busca tratamento por ID | 200 / 404 |
| GET | /api/tratamentos/pet/{petId} | Lista tratamentos de um pet | 200 / 404 |
| GET | /api/tratamentos/status/{status} | Filtra por status | 200 |
| POST | /api/tratamentos | Registra novo tratamento | 201 / 400 |
| PUT | /api/tratamentos/{id} | Atualiza tratamento | 204 / 400 / 404 |
| DELETE | /api/tratamentos/{id} | Remove tratamento | 204 / 404 |

Exemplo de body para POST/PUT:
```json
{
  "nomeTratamento": "Antibioticoterapia",
  "dataInicio": "2024-06-20T00:00:00",
  "dataFim": "2024-07-04T00:00:00",
  "descricao": "Amoxicilina 250mg 2x ao dia",
  "status": "Em andamento",
  "veterinario": "Dra. Ana Lima",
  "petId": 1
}
```

---

## Banco de Dados Oracle

### Tabelas criadas pelas Migrations

| Tabela | Descricao |
|--------|-----------|
| TB_TUTORES | Donos dos pets |
| TB_PETS | Animais cadastrados |
| TB_VACINAS | Historico de vacinas |
| TB_CONSULTAS | Consultas veterinarias |
| TB_TRATAMENTOS | Tratamentos em andamento e concluidos |

### Relacionamentos

```
TB_TUTORES (1) ---- (N) TB_PETS
TB_PETS    (1) ---- (N) TB_VACINAS
TB_PETS    (1) ---- (N) TB_CONSULTAS
TB_PETS    (1) ---- (N) TB_TRATAMENTOS
```

---

## Pacotes NuGet utilizados

| Pacote | Versao | Finalidade |
|--------|--------|-----------|
| Oracle.EntityFrameworkCore | 8.21.121 | Driver Oracle para EF Core |
| Microsoft.EntityFrameworkCore | 8.0.0 | ORM principal |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | CLI para migrations |
| Swashbuckle.AspNetCore | 6.5.0 | Geracao do Swagger/OpenAPI |

---

## Integrantes

| Nome | RM |
|------|----|
| Integrante 1 | RM XXXXX |
| Integrante 2 | RM XXXXX |
| Integrante 3 | RM XXXXX |

---

## Licenca

Este projeto foi desenvolvido para fins academicos — FIAP, turma Advanced Business Development with .NET.
