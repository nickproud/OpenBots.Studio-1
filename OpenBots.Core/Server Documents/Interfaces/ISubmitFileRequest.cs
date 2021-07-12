namespace OpenBots.Core.Server_Documents.Interfaces
{
    public interface ISubmitFileRequest
    {
        string v_AssignedTo { get; set; }
        string v_CaseNumber { get; set; }
        string v_CaseType { get; set; }
        string v_Description { get; set; }
        string v_DueDate { get; set; } //DateTime
        string v_FilePath { get; set; }
        string v_QueueName { get; set; }
        string v_Name { get; set; }
    }
}