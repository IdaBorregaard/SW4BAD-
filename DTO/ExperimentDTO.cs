namespace assignment3.DTO;

// DTO for Experiment entity
public record ExperimentDto(
    int Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    int MissionId,
    int LeadScientistId
);

// This DTO is used for creating a new experiment, where the client must provide the MissionId and LeadScientistId
public record CreateExperimentDto(
    string Name,
    string Description,
    int MissionId,
    int LeadScientistId
);

// This DTO is used for updating an experiment, where we don't want to allow changing the MissionId or LeadScientistId
public record UpdateExperimentDto(
    string Name,
    string Description
);