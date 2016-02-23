package Entities;

public class TicketLog {

    private int id;
    private int ticketId;
    private String submittedBy;
    private Boolean hasFile;
    private Boolean isInternal;
    private Boolean fromInternal;
    private String message;
    private String timeOfLog;

    public TicketLog(int id, int ticketId, String submittedBy, Boolean hasFile, Boolean isInternal, Boolean fromInternal, String message, String timeOfLog) {
        this.id = id;
        this.ticketId = ticketId;
        this.submittedBy = submittedBy;
        this.hasFile = hasFile;
        this.isInternal = isInternal;
        this.fromInternal = fromInternal;
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

    public Boolean getFromInternal() {
        return fromInternal;
    }

    public void setFromInternal(Boolean fromInternal) {
        this.fromInternal = fromInternal;
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
