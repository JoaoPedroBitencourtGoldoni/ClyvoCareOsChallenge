namespace ClyvoCareOSChallenge.DTOs;

public record TutorCreateDto(string NomeCompleto, string Email, string Telefone, string Endereco, string Cidade, string Estado);
public record TutorUpdateDto(string NomeCompleto, string Email, string Telefone, string Endereco, string Cidade, string Estado);
public record TutorResponseDto(int Id, string NomeCompleto, string Email, string Telefone, string Cidade, string Estado, DateTime DataCadastro);

public record PetCreateDto(string Nome, string Especie, string Raca, DateTime DataNascimento, string Sexo, decimal PesoKg, int TutorId);
public record PetUpdateDto(string Nome, string Especie, string Raca, DateTime DataNascimento, string Sexo, decimal PesoKg);
public record PetResponseDto(int Id, string Nome, string Especie, string Raca, DateTime DataNascimento, string Sexo, decimal PesoKg, string FaseDeVida, int TutorId, string NomeTutor);

public record VacinaCreateDto(string NomeVacina, DateTime DataAplicacao, DateTime? DataProximaDose, string Veterinario, string Clinica, string Observacoes, int PetId);
public record VacinaUpdateDto(string NomeVacina, DateTime DataAplicacao, DateTime? DataProximaDose, string Veterinario, string Clinica, string Observacoes);
public record VacinaResponseDto(int Id, string NomeVacina, DateTime DataAplicacao, DateTime? DataProximaDose, string Veterinario, string Clinica, string Observacoes, int PetId, string NomePet);

public record ConsultaCreateDto(DateTime DataConsulta, string Motivo, string Diagnostico, string Prescricao, string Veterinario, string Clinica, decimal PesoNaConsulta, int PetId);
public record ConsultaUpdateDto(DateTime DataConsulta, string Motivo, string Diagnostico, string Prescricao, string Veterinario, string Clinica, decimal PesoNaConsulta);
public record ConsultaResponseDto(int Id, DateTime DataConsulta, string Motivo, string Diagnostico, string Prescricao, string Veterinario, string Clinica, decimal PesoNaConsulta, int PetId, string NomePet);

public record TratamentoCreateDto(string NomeTratamento, DateTime DataInicio, DateTime? DataFim, string Descricao, string Status, string Veterinario, int PetId);
public record TratamentoUpdateDto(string NomeTratamento, DateTime DataInicio, DateTime? DataFim, string Descricao, string Status, string Veterinario);
public record TratamentoResponseDto(int Id, string NomeTratamento, DateTime DataInicio, DateTime? DataFim, string Descricao, string Status, string Veterinario, int PetId, string NomePet);
