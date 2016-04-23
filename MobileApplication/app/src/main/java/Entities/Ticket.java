package Entities;

public class Ticket {

    private int id;
    private String title;
    private String description;
    private String openedByName;
    private String priority;
    private String state;
    private String category;
    private String projectName;
    private String userAssignedTo;
    private String teamAssignedTo;
    private String organisationAssignedTo;
    private String deadline;
    private String colour;
    private String lastMessage;
    private String lastResponse;

    public Ticket(){}

    public Ticket(Integer id, String title, String description, String openedByName, String priority,
                  String state, String category, String projectName, String userAssignedTo,
                  String teamAssignedTo, String organisationAssignedTo, String deadline, String colour,
                  String lastMessage, String lastResponse) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.openedByName = openedByName;
        this.priority = priority;
        this.state = state;
        this.category = category;
        this.projectName = projectName;
        this.userAssignedTo = userAssignedTo;
        this.teamAssignedTo = teamAssignedTo;
        this.organisationAssignedTo = organisationAssignedTo;
        this.deadline = deadline;
        this.colour = colour;
        this.lastMessage = lastMessage;
        this.lastResponse = lastResponse;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getOpenedByName() {
        return openedByName;
    }

    public void setOpenedByName(String openedByName) {
        this.openedByName = openedByName;
    }

    public String getPriority() {
        return priority;
    }

    public void setPriority(String priority) {
        this.priority = priority;
    }

    public String getState() {
        return state;
    }

    public void setState(String state) {
        this.state = state;
    }

    public String getCategory() {
        return category;
    }

    public void setCategory(String category) {
        this.category = category;
    }

    public String getProjectName() {
        return projectName;
    }

    public void setProjectName(String projectName) {
        this.projectName = projectName;
    }

    public String getUserAssignedTo() {
        return userAssignedTo;
    }

    public void setUserAssignedTo(String userAssignedTo) {
        this.userAssignedTo = userAssignedTo;
    }

    public String getTeamAssignedTo() {
        return teamAssignedTo;
    }

    public void setTeamAssignedTo(String teamAssignedTo) {
        this.teamAssignedTo = teamAssignedTo;
    }

    public String getOrganisationAssignedTo() {
        return organisationAssignedTo;
    }

    public void setOrganisationAssignedTo(String organisationAssignedTo) {
        this.organisationAssignedTo = organisationAssignedTo;
    }

    public String getDeadline() {
        return deadline;
    }

    public void setDeadline(String deadline) {
        this.deadline = deadline;
    }

    public String getColour() {
        return colour;
    }

    public void setColour(String colour) {
        this.colour = colour;
    }

    public String getLastMessage() {
        return lastMessage;
    }

    public void setLastMessage(String lastMessage) {
        this.lastMessage = lastMessage;
    }

    public String getLastResponse() {
        return lastResponse;
    }

    public void setLastResponse(String lastResponse) {
        this.lastResponse = lastResponse;
    }

}
