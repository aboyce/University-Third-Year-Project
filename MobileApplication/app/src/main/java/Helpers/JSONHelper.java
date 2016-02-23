package Helpers;

import android.util.Log;

import org.json.JSONObject;

import Entities.Ticket;
import Entities.TicketLog;

/**
 * Created by Adam on 22/02/2016.
 */
public class JSONHelper {

    public static Ticket getTicketFromJSONObject (JSONObject json){
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:getTicketFromJSONObject");
        try{
            return new Ticket(Integer.parseInt(json.getString("id")), json.getString("title"),
                    json.getString("description"), json.getString("openedByName"),
                    json.getString("ticketPriorityName"), json.getString("ticketStateName"),
                    json.getString("ticketCategoryName"), json.getString("projectName"),
                    json.getString("userAssignedToName"), json.getString("teamAssignedToName"),
                    json.getString("organisationAssignedToName"), json.getString("deadline"),
                    json.getString("lastMessage"), json.getString("lastResponse"));

        }catch(Exception e){
            Log.e("TICKET_MANAGEMENT", "TicketsActivity:getTicketFromJSONObject: Exception: " + e.getMessage());
            return null;
        }
    }

    public static TicketLog getTicketLogFromJSONObject (JSONObject json){
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:getTicketLogFromJSONObject");
        try{
            return new TicketLog(Integer.parseInt(json.getString("id")),
                    Integer.parseInt(json.getString("ticketId")),
                    json.getString("submittedByName"), json.getString("ticketLogTypeName"),
                    Boolean.parseBoolean(json.getString("hasFile")),
                    Boolean.parseBoolean(json.getString("isInternal")),
                    json.getString("message"), json.getString("timeOfLog"));

        }catch(Exception e){
            Log.e("TICKET_MANAGEMENT", "TicketsActivity:getTicketLogFromJSONObject: Exception: " + e.getMessage());
            return null;
        }
    }
}
