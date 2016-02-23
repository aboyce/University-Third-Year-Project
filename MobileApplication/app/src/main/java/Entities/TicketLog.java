package Entities;

public class TicketLog {

    private int id;
    private int ticketId;
    private String submittedBy;
    private String ticketLogType;
    private Boolean hasFile;
    private Boolean isInternal;
    private String message;
    private String timeOfLog;

    public TicketLog(int id, int ticketId, String submittedBy, String ticketLogType, Boolean hasFile, Boolean isInternal, String message, String timeOfLog) {
        this.id = id;
        this.ticketId = ticketId;
        this.submittedBy = submittedBy;
        this.ticketLogType = ticketLogType;
        this.hasFile = hasFile;
        this.isInternal = isInternal;
        this.message = message;
        this.timeOfLog = timeOfLog;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getTicketId() {
        return ticketId;
    }

    public void setTicketId(int ticketId) {
        this.ticketId = ticketId;
    }

    public String getSubmittedBy() {
        return submittedBy;
    }

    public void setSubmittedBy(String submittedBy) {
        this.submittedBy = submittedBy;
    }

    public String getTicketLogType() {
        return ticketLogType;
    }

    public void setTicketLogType(String ticketLogType) {
        this.ticketLogType = ticketLogType;
    }

    public Boolean getHasFile() {
        return hasFile;
    }

    public void setHasFile(Boolean hasFile) {
        this.hasFile = hasFile;
    }

    public Boolean getIsInternal() {
        return isInternal;
    }

    public void setIsInternal(Boolean isInternal) {
        this.isInternal = isInternal;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getTimeOfLog() {
        return timeOfLog;
    }

    public void setTimeOfLog(String timeOfLog) {
        this.timeOfLog = timeOfLog;
    }
}
